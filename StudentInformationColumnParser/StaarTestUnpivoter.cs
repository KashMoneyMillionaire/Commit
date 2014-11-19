using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using EntityFramework.BulkInsert.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Domain;
using MoreLinq;

namespace ParserUtilities
{
    public static class StaarTestUnpivotor
    {

        private static readonly string[] ExcludedCategories = { "docs_n", "abs_n", "oth_n", "docs_r", "abs_r", "oth_r" };

        /// <summary>
        /// This takes a csv file for the Staar Subject and unpivots it. It then saves it in the output folder.
        /// </summary>
        /// <param name="filePath">The *.csv path to read from.</param>
        /// <param name="outPath">The folder to store the output.</param>
        /// <param name="grade">The grade to be associated with the file(s).</param>
        /// <param name="language">The language the test was taken in.</param>
        /// <param name="x">The number of columns at the beginning.</param>
        public static void UnpivotNarrow(string filePath, string outPath, Grade grade, LanguageEnum language, int x)
        {
            var dataTable = new DataTable(string.Format("Parsed {0}", filePath));
            int j;
            var sb = new StringBuilder();
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var outFile = string.Format("{0}/{1} - Parsed.csv", outPath, fileName);


            //remove past file if it exists

            if (File.Exists(outFile))
                File.Delete(outFile);


            //Validation

            if (!filePath.Contains(".csv"))
                throw new CustomException("This is not a csv file.");


            //read the lines in. First row is headers. save it then remove

            var rows = File.ReadAllLines(filePath).ToList();
            var headers = rows[0].Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            rows.RemoveAt(0); //pop the top off


            // check if second row is headers

            var possibleHeader = rows[0].Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(possibleHeader[0], out j)) //check if second row of headers
            {
                headers.AddRange(possibleHeader);
                rows.RemoveAt(0); //pop the top off
            }


            //Groom the headers

            for (var i = 0; i < headers.Count; i++)
            {
                headers[i] = headers[i].Trim();
            }


            //setup the headers for the first X columns

            for (var i = 0; i < x; i++)
            {
                dataTable.Columns.Add(headers[i]);
            }


            //set up remaining headers

            dataTable.Columns.Add("Grade");
            dataTable.Columns.Add("LanguageEnum");
            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Demographic");
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Value");


            //Write the column names to the stringbuilder

            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));


            //Begin adding data

            var dataRow = dataTable.NewRow();
            var writer = File.AppendText(outFile);
            foreach (var campus in rows.Select(row => row.Split(',')))
            {
                //fill the first columns. These are the same for every campus

                FillFirstX(dataRow, campus, x); //First 6 (Campus, year, region, district, dname, cname)
                dataRow["Grade"] = grade;
                dataRow["LanguageEnum"] = language;


                //for each complex header whose category matches the current category make a demo and value

                for (var i = x; i < headers.Count; i++)
                {
                    //if the value is empty, skip it

                    if (new[] { "0", "", "." }.Contains(campus[i].Trim()))
                        continue;


                    //if it is not a triple (the first x columns), skip it

                    var triple = headers[i].Split(new[] { '_' }, 3);
                    if (triple.Length != 3)
                        continue;


                    //write the remaining values, then write to stringbuilder

                    dataRow["Subject"] = triple[0];
                    dataRow["Demographic"] = triple[1];
                    dataRow["Category"] = triple[2];
                    dataRow["Value"] = campus[i];

                    sb.AppendLine(string.Join(",", dataRow.ItemArray));
                }


                //write campus to file

                writer.Write(sb.ToString());
                sb.Clear();
            }

            writer.Flush();
            writer.Close();
        }


        /// <summary>
        /// This takes a csv file for the Staar Subject and unpivots it. It then saves it in the output folder.
        /// </summary>
        /// <param name="file">The *.csv path to read from.</param>
        /// <param name="outPath">The folder to store the output.</param>
        /// <param name="grade">The grade to be associated with the file(s).</param>
        /// <param name="language">The language the test was taken in.</param>
        /// <param name="x">The number of columns at the beginning.</param>
        public static void UnpivotWide(string file, string outPath, Grade grade, LanguageEnum language, int x)
        {
            //Validation
            if (!file.Contains(".csv"))
                throw new Exception("The file is not a csv file.");


            //Lets unpivot this shit

            Console.WriteLine("Unpivoting: {0}", file);
            int j;
            var demographics = Enum.GetValues(typeof(StaarDemographic)).Cast<StaarDemographic>().Select(c => c.ToString()).ToList();
            var rows = File.ReadAllLines(file).ToList();
            var headers = rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            rows.RemoveAt(0); //pop the top off
            var possibleHeader = rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(possibleHeader[0], out j)) //check if second row of headers
            {
                headers.AddRange(possibleHeader);
                rows.RemoveAt(0); //pop the top off
            }

            var sdc = headers.Select(h => h.Split(new[] { '_' }, 3)).ToList();
            var dynamicCategories = sdc
                .Where(s => s.Count() == 3)
                .Select(s => s[2])
                .Distinct()
                .Except(ExcludedCategories)
                .ToList();


            //begin changing stuff

            var dataTable = new DataTable(string.Format("Parsed {0}", file));


            //first X

            for (var i = 0; i < x; i++)
            {
                dataTable.Columns.Add(headers[i]);
            }


            //subject, grade, language, category

            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Grade");
            dataTable.Columns.Add("Language");
            dataTable.Columns.Add("Category");
            var genericHeaders = new List<string>(new[] { "Subject", "Grade", "Language" });


            //weird ones

            foreach (var cat in ExcludedCategories)
            {
                dataTable.Columns.Add(cat);
            }


            //demographics

            foreach (var demo in demographics)
            {
                dataTable.Columns.Add(demo);
            }


            //Write the column names to the sb

            var sb = new StringBuilder();

            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));


            //Begin adding data

            var dataRow = dataTable.NewRow();
            foreach (var campus in rows.Select(row => row.Split(',')))
            {
                //set up basic set

                string lang;
                lang = language == LanguageEnum.English ? "English" : "Spanish";

                FillFirstX(dataRow, campus, x);
                FillWeirdHeaders(dataRow, headers, campus);
                FillGenericData(dataRow, genericHeaders, headers[x + 1].Split('_')[0], grade.ToString(), lang);

                foreach (var category in dynamicCategories)
                {
                    //for each complex header whose category matches the current category

                    for (var i = 6; i < headers.Count; i++)
                    {
                        if (sdc[i].Length == 3 && sdc[i][2] == category)
                        {
                            //set the data column of the found demographic equal to the 
                            dataRow[sdc[i][1]] = campus[i];
                        }
                    }


                    //add the category, then write to sb

                    dataRow["Category"] = category;
                    sb.AppendLine(string.Join(",", dataRow.ItemArray));

                    //dataTable.Rows.Add(dataRow);
                }
            }

            var fileName = Path.GetFileNameWithoutExtension(file);
            File.WriteAllText(string.Format("{0}/{1} - Parsed.csv", outPath, fileName), sb.ToString());

        }

        public static void UnpivotAndPopulateDatabase(string parsedFilePath)
        {
            var ctx = new AzureDataContext();
            var dems = ctx.DemographicDetails.ToList();
            var cats = ctx.CategoryDetails.ToList();
            var subjs = ctx.Subjects.ToList();
            var camps = ctx.Campuses.ToList();
            var dists = ctx.Districts.ToList();
            var regs = ctx.Regions.ToList();
            var langs = ctx.Languages.ToList();
            var completed = ctx.StaarTests
                .GroupBy(s => new
                {
                    s.Year,
                    Subject = s.Subject.Name,
                    s.Grade,
                    s.Language.Name
                })
                .Select(s => new
                {
                    s.Key.Year,
                    SubjectName = s.Key.Subject,
                    s.Key.Grade,
                    Language = s.Key.Name
                }).ToList();

            string previousCampus = "",
                previousSubject = "",
                previousDemographic = "",
                previousCategory = "";
            long campusId = 0;
            var subject = new Subject();
            var dd = new DemographicDetail();
            var cd = new CategoryDetail();
            var log = File.CreateText(@"\Parse\log.txt");


            //Validation

            if (!parsedFilePath.Contains(".csv"))
                throw new CustomException("This is not a csv file.");


            //read the lines in. First row is headers. save it then remove

            var rows = File.ReadAllLines(parsedFilePath).ToList();
            rows.RemoveAt(0); //headers


            //Begin adding data

            var sortedRows = rows.Select(row => row.Split(',')).ToList();
            //    .OrderBy(c => c[0])
            //    .ThenBy(c => c[8])
            //    .ThenBy(c => c[9])
            //    .ThenBy(c => c[10]).ToList();


            //adjust year if it is off

            var year = int.Parse(sortedRows[0][1]);
            if (year >= 90 && year < 100) year += 1900;
            else if (year < 90) year += 2000;

            var language = langs.Single(l => l.Name == sortedRows[0][7]);
            var campusTests = new List<StaarTest>(90005);
            var sortedCount = sortedRows.Count;


            for (var i = 0; i < sortedCount; i++)
            {
                try
                {
                    var record = sortedRows[i];

                    if (completed.Any(c => c.SubjectName == record[8]
                        && c.Grade == record[6]
                        && c.Year == year
                        && c.Language == language.Name))
                        continue;

                    //check if we're on a new school

                    if (previousCampus != record[0])
                    {
                        //Find the school. Create if not new

                        long regionNum = Convert.ToInt32(record[2]);
                        long campusNum = Convert.ToInt32(record[0]);
                        long districtNum = Convert.ToInt32(record[1]);

                        //check region, district, and campus

                        var region = regs.FirstOrDefault(r => r.Number == regionNum);
                        if (region == null)
                        {
                            region = new Region
                            {
                                Name = string.Format("Region {0}", regionNum),
                                Number = regionNum
                            };
                            ctx.Regions.Add(region);
                        }

                        var district = dists.FirstOrDefault(r => r.Number == districtNum);
                        if (district == null)
                        {
                            district = new District
                            {
                                Name = record[4],
                                Number = districtNum,
                                Region_Id = region.Id
                            };
                            ctx.Districts.Add(district);
                        }

                        var campus = camps.FirstOrDefault(r => r.Number == campusNum);
                        if (campus == null)
                        {
                            campus = new Campus
                            {
                                Name = record[5],
                                Number = campusNum,
                                District_Id = district.Id
                            };
                            ctx.Campuses.Add(campus);
                        }

                        //TODO ctx.SaveChanges();
                        campusId = campus.Id;
                    }

                    if (previousSubject != record[8])
                    {
                        subject = subjs.Single(s => s.Name == record[8]);
                    }

                    if (previousDemographic != record[9])
                    {
                        dd = dems.Single(s => s.Detail == record[9]);
                    }

                    if (previousCategory != record[10])
                    {
                        cd = cats.Single(s => s.Detail == record[10]);
                    }


                    //for each complex header whose category matches the current category make a demo and value

                    campusTests.Add(new StaarTest
                    {
                        Campus_Id = campusId,
                        CategoryDetail_Id = cd.Id,
                        DemographicDetail_Id = dd.Id,
                        Subject_Id = subject.Id,
                        Year = year,
                        Language_Id = language.Id,
                        Value = Convert.ToDecimal(record[11]),
                        Grade = record[6]
                    });

                    if (campusTests.Count >= 90000 || i == sortedCount - 1)
                    {
                        //ctx.BulkInsert(campusTests);
                        campusTests = new List<StaarTest>(90005);
                    }

                    previousCampus = record[0];
                    previousSubject = record[8];
                    previousDemographic = record[9];
                    previousCategory = record[10];
                }
                catch (Exception ex)
                {
                    log.WriteLine("Message - {0}\r\nStackTrace - {1}", ex.Message, ex.StackTrace);
                    //throw;
                }
            }
            //TODO ctx.SaveChanges();
            log.Flush();
            log.Close();
        }

        public static void TestAzure()
        {
            //var ctx = new AzureDataContext();
            //var list = new List<YearGradeLang>();
            //var y = ctx.YearGradeLangs.ToList();
            //ctx.YearGradeLangs.RemoveRange(y);

            //for (var i = 0; i < 100; i++)
            //{

            //    list.Add(new YearGradeLang
            //    {
            //        LanguageEnum = LanguageEnum.English,
            //        Grade = Grade.EOC,
            //        Year = i
            //    });

            //}
            //ctx.YearGradeLangs.AddRange(list);
            //ctx.SaveChanges();
        }

        #region Helpers

        private static void FillFirstX(DataRow dataRow, IList<string> row, int x)
        {
            for (var i = 0; i < x; i++)
            {
                dataRow[i] = row[i];
            }
        }

        private static void FillGenericData(DataRow dataRow, IEnumerable<string> genericHeaders, string sub, string grade, string language)
        {
            foreach (var header in genericHeaders)
            {
                switch (header)
                {
                    case "Subject":
                        dataRow[header] = sub;
                        break;
                    case "Grade":
                        dataRow[header] = grade;
                        break;
                    case "Language":
                        dataRow[header] = language;
                        break;
                }
            }
        }

        private static void FillWeirdHeaders(DataRow dataRow, List<string> headers, string[] campusData)
        {
            foreach (var we in ExcludedCategories)
            {
                dataRow[we] = campusData[headers.FindIndex(c => c.Contains(we))];
            }
        }

        #endregion

    }

    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }

    class Triple
    {
        public string Demographic { get; set; }
        public string Category { get; set; }
        public string Subject { get; set; }
    }
}
