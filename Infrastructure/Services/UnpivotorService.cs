using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using EntityFramework.BulkInsert.Extensions;
using Infrastructure.Data;
using Infrastructure.Domain;

namespace Infrastructure.Services
{
    public class UnpivotorService : IService
    {
        private readonly AzureDataContext _ctx;

        private readonly string[] _excludedCategories = { "docs_n", "abs_n", "oth_n", "docs_r", "abs_r", "oth_r" };

        public UnpivotorService()
        {
            _ctx = ApplicationFactory.RetrieveContext();
        }

        /// <summary>
        /// This takes a csv file for the Staar Subject and unpivots it. It then saves it in the output folder.
        /// </summary>
        /// <param name="filePath">The *.csv path to read from.</param>
        /// <param name="outPath">The folder to store the output.</param>
        /// <param name="grade">The grade to be associated with the file(s).</param>
        /// <param name="language">The language the test was taken in.</param>
        /// <param name="x">The number of columns at the beginning.</param>
        public void UnpivotStaarTestNarrow(string filePath, string outPath, LanguageEnum language)
        {
            var dataTable = new DataTable(string.Format("Parsed {0}", filePath));
            int j;
            var sb = new StringBuilder();
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var outFile = string.Format("{0}/{1} - Parsed Narrow.csv", outPath, fileName);
            var emptyList = new[] { "0", "", "." };


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


            //find the first header that should be split. All previous are the 'first x'

            var x = headers.FindIndex(h => h.Split(new[] { '_' }).Length > 1);


            //setup the headers for the first X columns

            for (var i = 0; i < x; i++)
            {
                if(!headers[i].Equals("GRADE"))
                    dataTable.Columns.Add(headers[i]);
            }


            //set up remaining headers

            dataTable.Columns.Add("GRADE");
            dataTable.Columns.Add("LanguageEnum");
            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Demographic");
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Value");


            //Write the column names to the stringbuilder

            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));


            //Begin adding data

            var gradeIndex = headers.IndexOf("Grade");
            var dataRow = dataTable.NewRow();
            var writer = File.AppendText(outFile);


            //Get grade to use

            var rowSplit = rows.Select(row => row.Split(',')).ToList();
            var foundGrade = gradeIndex != -1 ? rowSplit[0][gradeIndex] : "EOC";


            foreach (var campus in rowSplit)
            {

                //fill the first columns. These are the same for every campus

                FillFirstX(dataRow, campus, x); //First 6 (Campus, year, region, district, dname, cname)
                dataRow["Grade"] = foundGrade;
                dataRow["LanguageEnum"] = language.ToString();

                //for each complex header whose category matches the current category make a demo and value

                for (var i = x; i < headers.Count; i++)
                {
                    //if the value is empty, skip it

                    if (emptyList.Contains(campus[i].Trim()))
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
        public void UnpivotStaarTestWide(string file, string outPath, LanguageEnum language)
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
            var gradeIndex = headers.IndexOf("GRADE");
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
                .Except(_excludedCategories)
                .ToList();


            //begin changing stuff

            var dataTable = new DataTable(string.Format("Parsed {0}", file));


            //find the first header that should be split. All previous are the 'first x'

            var x = headers.FindIndex(h => h.Split(new[] { '_' }).Length > 1);


            //first X

            for (var i = 0; i < x; i++)
            {
                if(!headers[i].Equals("GRADE"))
                    dataTable.Columns.Add(headers[i]);
            }


            //subject, grade, language, category

            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Grade");
            dataTable.Columns.Add("Language");
            dataTable.Columns.Add("Category");
            var genericHeaders = new List<string>(new[] { "Subject", "Grade", "Language" });


            //weird ones

            foreach (var cat in _excludedCategories)
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
                //find subjects

                var subjs = sdc
                    .Where(s => s.Count() == 3)
                    .Select(s => s[0])
                    .Distinct()
                    .ToList();


                //set up basic set

                var lang = language == LanguageEnum.English ? "English" : "Spanish";
                var foundGrade = gradeIndex != -1 ? campus[gradeIndex] : "EOC";

                FillFirstX(dataRow, campus, x);
                FillWeirdHeaders(dataRow, headers, campus);
                FillGenericData(dataRow, genericHeaders, foundGrade, lang);

                foreach (var subj in subjs)
                {
                    foreach (var category in dynamicCategories)
                    {
                        //for each complex header whose category matches the current category

                        for (var i = 6; i < headers.Count; i++)
                        {
                            if (sdc[i].Length == 3 && sdc[i][0] == subj && sdc[i][2] == category)
                            {
                                //set the data column of the found demographic equal to the 
                                dataRow[sdc[i][1]] = campus[i];
                            }
                        }


                        //add the category, then write to sb

                        dataRow["Category"] = category;
                        dataRow["Subject"] = subj;
                        sb.AppendLine(string.Join(",", dataRow.ItemArray));

                        //dataTable.Rows.Add(dataRow);
                    }
                }
            }

            var fileName = Path.GetFileNameWithoutExtension(file);
            File.WriteAllText(string.Format("{0}/{1} - Parsed Wide.csv", outPath, fileName), sb.ToString());

        }

        public void PopulateDatabaseFromUnpivotedStaarTestDirectory(string parsedFilesDirectory, string logPath)
        {
            var completed = _ctx.StaarTests
                        .GroupBy(s => new
                        {
                            s.Year,
                            Subject = s.Subject.Name,
                            s.Grade,
                            s.Language.Name
                        })
                        .Select(s => new Complete
                        {
                            Year = s.Key.Year,
                            SubjectName = s.Key.Subject,
                            Grade = s.Key.Grade,
                            Language = s.Key.Name
                        }).ToList();
            var firstTime = true;
            string previousSubject = "",
                previousDemographic = "",
                previousCategory = "";
            var subject = new Subject();
            var dd = new DemographicDetail();
            var cd = new CategoryDetail();
            var log = File.CreateText(logPath);
            //     0        1       2       3           4       5       6       7               8       9           10          11    
            //     CAMPUS	YEAR	REGION	DISTRICT	DNAME	CNAME	Grade	LanguageEnum	Subject	Demographic	Category	Value


            foreach (var unzippedFile in Directory.GetFiles(parsedFilesDirectory, "*.csv", SearchOption.TopDirectoryOnly))
            {
                #region initialize

                var dems = new List<DemographicDetail>();
                var cats = new List<CategoryDetail>();
                var subjs = new List<Subject>();
                var camps = new List<Campus>();
                var dists = new List<District>();
                var regs = new List<Region>();
                var availableLanguages = new List<Language>();

                #endregion
                try
                {
                    var testsToAdd = new List<StaarTest>(90005);


                    //enumerate through file and grab rows we need.

                    List<string[]> testsNotInDb;
                    try
                    {
                        testsNotInDb = File.ReadLines(unzippedFile)
                        .Select(row => row.Split(','))
                        .Where(row => completed.All(c =>
                            c.Grade != row[6] &&
                            c.SubjectName != row[8] &&
                            c.Language != row[7] &&
                            c.Year.ToString().Substring(2, 2) != row[1]))
                        .ToList();
                        testsNotInDb.RemoveAt(0);

                        if (testsNotInDb.Count == 0)
                        {
                            log.WriteLine("{0} has had all recordes uploaded already", unzippedFile);
                            continue;
                        }
                        if (testsNotInDb[0].Length != 12)
                        {
                            log.WriteLine("{0} is not in the format of a parsed file", unzippedFile);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("An error occured reading from {0}. The error is: {1}", unzippedFile, ex.Message);
                        continue;
                    }



                    //by this point, we know we have to know things

                    if (firstTime)
                    {
                        dems = _ctx.DemographicDetails.ToList();
                        cats = _ctx.CategoryDetails.ToList();
                        subjs = _ctx.Subjects.ToList();
                        camps = _ctx.Campuses.ToList();
                        dists = _ctx.Districts.ToList();
                        regs = _ctx.Regions.ToList();
                        availableLanguages = _ctx.Languages.ToList();
                        firstTime = false;
                    }


                    //get bits from file

                    var sortedCount = testsNotInDb.Count;
                    var fileLanguage = availableLanguages.Single(l => l.Name == testsNotInDb[0][7]);

                    var fileYear = int.Parse(testsNotInDb[0][1]);
                    if (fileYear >= 90 && fileYear < 100) fileYear += 1900;
                    else if (fileYear < 90) fileYear += 2000;


                    #region add all campuses first

                    var campusGroups = testsNotInDb.GroupBy(s => new
                    {
                        CampusNumber = Convert.ToInt32(s[0]),
                        Year = s[1],
                        RegionNumber = Convert.ToInt32(s[2]),
                        DistrictNumber = Convert.ToInt32(s[3]),
                        Dname = s[4],
                        Cname = s[5],

                    })
                        .Select(s => new
                        {
                            s.Key.CampusNumber,
                            s.Key.Year,
                            s.Key.RegionNumber,
                            s.Key.DistrictNumber,
                            s.Key.Dname,
                            s.Key.Cname
                        }).ToList();

                    var newRegs = new List<Region>();
                    var newDist = new List<District>();
                    var newCamps = new List<Campus>();

                    foreach (var campusGroup in campusGroups)
                    {
                        //Find the school. Create if not new

                        var campusNum = campusGroup.CampusNumber;
                        var regionNum = campusGroup.RegionNumber;
                        var districtNum = campusGroup.DistrictNumber;


                        //check region, district, and campus

                        var region = regs.FirstOrDefault(r => r.Number == regionNum);
                        if (region == null)
                        {
                            region = new Region
                            {
                                Name = string.Format("Region {0}", regionNum),
                                Number = regionNum
                            };
                            regs.Add(region);
                            newRegs.Add(region);
                        }

                        var district = dists.FirstOrDefault(r => r.Number == districtNum);
                        if (district == null)
                        {
                            district = new District
                            {
                                Name = campusGroup.Dname,
                                Number = districtNum,
                                Region = region
                            };
                            dists.Add(district);
                            newDist.Add(district);
                        }

                        var campus = camps.FirstOrDefault(r => r.Number == campusNum);
                        if (campus == null)
                        {
                            campus = new Campus
                            {
                                Name = campusGroup.Cname,
                                Number = campusNum,
                                District = district
                            };
                            camps.Add(campus);
                            newCamps.Add(campus);
                        }

                    }
                    _ctx.Regions.AddRange(newRegs);
                    _ctx.Districts.AddRange(newDist);
                    _ctx.Campuses.AddRange(newCamps);
                    _ctx.SaveChanges();

                    #endregion


                    #region go through records

                    for (var i = 0; i < sortedCount; i++)
                    {
                        var record = testsNotInDb[i];

                        var campusId = camps.First(c => c.Number == Convert.ToInt32(record[0])).Id;

                        //get new stuff

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

                        testsToAdd.Add(new StaarTest
                        {
                            Campus_Id = campusId,
                            CategoryDetail_Id = cd.Id,
                            DemographicDetail_Id = dd.Id,
                            Subject_Id = subject.Id,
                            Year = fileYear,
                            Language_Id = fileLanguage.Id,
                            Value = Convert.ToDecimal(record[11]),
                            Grade = record[6]
                        });

                        if (testsToAdd.Count >= 90000 || i == sortedCount - 1)
                        {
                            _ctx.BulkInsert(testsToAdd);
                            testsToAdd = new List<StaarTest>(90005);
                        }

                        previousSubject = record[8];
                        previousDemographic = record[9];
                        previousCategory = record[10];

                    }

                    #endregion


                    //add processed files to completed

                    var groupedImportsToComplete = testsNotInDb.GroupBy(s => new
                    {
                        Year = fileYear,
                        SubjectName = s[8],
                        Grade = s[6],
                        Langauge = fileLanguage.Name
                    }).Select(i => new Complete
                    {
                        Year = i.Key.Year,
                        SubjectName = i.Key.SubjectName,
                        Grade = i.Key.Grade,
                        Language = i.Key.Langauge
                    }).ToList();
                    completed.AddRange(groupedImportsToComplete);
                    log.WriteLine("{0} successfully uploaded", unzippedFile);


                    //memory management

                    testsNotInDb.Clear();
                    testsNotInDb = null;
                }
                catch (Exception ex)
                {
                    log.WriteLine("{0}\r\n{1}", ex.Message, ex.StackTrace);
                }
            }

            log.Flush();
            log.Close();
        }

        public void PopulateDatabaseFromUnpivotedStaarTestFile(string parsedFilePath, string logPath)
        {
            var dems = _ctx.DemographicDetails.ToList();
            var cats = _ctx.CategoryDetails.ToList();
            var subjs = _ctx.Subjects.ToList();
            var camps = _ctx.Campuses.ToList();
            var dists = _ctx.Districts.ToList();
            var regs = _ctx.Regions.ToList();
            var langs = _ctx.Languages.ToList();
            var completed = _ctx.StaarTests
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
            var log = File.CreateText(logPath);

            //Validation

            if (!parsedFilePath.Contains(".csv"))
                throw new CustomException("This is not a csv file.");


            //read the lines in. First row is headers. save it then remove

            var rows = File.ReadAllLines(parsedFilePath).ToList();
            rows.RemoveAt(0); //headers

            if (rows.Count > 12) //this is our test to make sure they are parsed
                throw new CustomException(string.Format("{0} is not in the format of a parsed file", Path.GetFileName(parsedFilePath)));

            //Begin adding data

            var splitRows = rows.Select(row => row.Split(',')).ToList();


            //adjust year if it is off

            var year = int.Parse(splitRows[0][1]);
            if (year >= 90 && year < 100) year += 1900;
            else if (year < 90) year += 2000;

            var language = langs.Single(l => l.Name == splitRows[0][7]);
            var campusTests = new List<StaarTest>(90005);
            var sortedCount = splitRows.Count;


            for (var i = 0; i < sortedCount; i++)
            {
                try
                {
                    var record = splitRows[i];

                    if (completed.Any(c => c.SubjectName == record[8]
                                           && c.Grade == record[6]
                                           && c.Year == year
                                           && c.Language == language.Name))
                        continue;

                    //check if we're on a new school

                    if (previousCampus != record[0])
                    {
                        //Find the school. Create if not new

                        long campusNum = Convert.ToInt32(record[0]);
                        long regionNum = Convert.ToInt32(record[2]);
                        long districtNum = Convert.ToInt32(record[3]);

                        //check region, district, and campus

                        var region = regs.FirstOrDefault(r => r.Number == regionNum);
                        if (region == null)
                        {
                            region = new Region
                            {
                                Name = string.Format("Region {0}", regionNum),
                                Number = regionNum
                            };
                            _ctx.Regions.Add(region);
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
                            _ctx.Districts.Add(district);
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
                            _ctx.Campuses.Add(campus);
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
                finally
                {
                    log.Flush();
                    log.Close();
                }
            }
            //TODO ctx.SaveChanges();
            log.Flush();
            log.Close();
        }


        public static void UnpivotAeisStudent(string filePath, string outPath)
        {
            var sb = new StringBuilder();
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var outFile = string.Format("{0}/{1} - Parsed.csv", outPath, fileName);
            var emptyList = new[] { "0", "", "." };
            var log = File.CreateText(@"\Parse\log.txt");
            var ctx = new AzureDataContext();
            var campuses = ctx.Campuses.ToList();


            //remove past file if it exists

            if (File.Exists(outFile))
                File.Delete(outFile);


            //read the lines in. First row is headers. save it then remove

            var rows = File.ReadAllLines(filePath).ToList();
            var headers = rows[0].Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            rows.RemoveAt(0); //pop the top off


            //Find year

            var yearIndex = headers.FindIndex(h => h.ToUpper() == "YEAR");
            var year = yearIndex == -1
                ? DateTime.Now.Year
                : Convert.ToInt32(rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)[yearIndex]);


            foreach (var row in rows.Select(row => row.Split(',')))
            {
                if (campuses.FirstOrDefault(c => c.Number == Convert.ToInt64(row[0])) == null)
                {
                    log.WriteLine("{0} - Campus not found", row[0]);
                    continue;
                }

                foreach (var column in row)
                {
                    if (column == "CAMPUS" || column == "YEAR") continue;

                    AeisType type;
                    switch (column[3])
                    {
                        case 'G':
                            type = AeisType.Graduates;
                            break;

                        case 'R':
                            type = AeisType.Retention;
                            break;

                        case 'T':
                            type = AeisType.Student;
                            break;

                        case 'M':
                            type = AeisType.Mobility;
                            break;
                    }
                }
            }


        }

        #region Helpers

        private static void FillFirstX(DataRow dataRow, IList<string> row, int x)
        {
            for (var i = 0; i < x; i++)
            {
                dataRow[i] = row[i];
            }
        }

        private static void FillGenericData(DataRow dataRow, IEnumerable<string> genericHeaders, string grade, string language)
        {
            foreach (var header in genericHeaders)
            {
                switch (header)
                {
                    case "Grade":
                        dataRow[header] = grade;
                        break;
                    case "Language":
                        dataRow[header] = language;
                        break;
                }
            }
        }

        private void FillWeirdHeaders(DataRow dataRow, List<string> headers, string[] campusData)
        {
            foreach (var we in _excludedCategories)
            {
                dataRow[we] = campusData[headers.FindIndex(c => c.Contains(we))];
            }
        }

        #endregion

        public void WriteToLogPath(string logPath, string message)
        {
            var log = File.CreateText(logPath);
            log.WriteLine(message);
            log.Flush();
            log.Close();
        }
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

    class Complete
    {
        public int Year { get; set; }
        public string SubjectName { get; set; }
        public string Grade { get; set; }
        public string Language { get; set; }
    }
}
