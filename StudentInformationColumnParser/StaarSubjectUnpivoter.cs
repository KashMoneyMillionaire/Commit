using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Text;
using CommitParser.Helpers;

namespace CommitParser
{
    public static class StaarSubjectUnpivotor
    {
        private const string Category = "Category";

        private static readonly List<string> Demographics =
            Enum.GetValues(typeof(StaarDemographic)).Cast<StaarDemographic>().Select(c => c.ToString()).ToList();

        //private static readonly List<string> StaticCategories =
        //    Enum.GetValues(typeof(StaarCategoryName)).Cast<StaarCategoryName>().Select(c => c.ToString()).ToList();

        private static readonly string[] ExcludedCategories = {"docs_n", "abs_n", "oth_n", "docs_r", "abs_r", "oth_r"};

        /// <summary>
        /// This takes a csv file for the Staar Subject and unpivots it. It then saves it in the output folder.
        /// </summary>
        /// <param name="file">The *.csv path to read from.</param>
        /// <param name="outPath">The folder to store the output.</param>
        /// <param name="grade">The grade to be associated with the file(s).</param>
        /// <param name="language">The language the test was taken in.</param>
        public static void Unpivot(string file, string outPath, Grade grade, Language language)
        {
            //Validation
            if(!file.Contains(".csv"))
                throw new Exception("The file is not a csv file.");


            //Lets unpivot this shit

            Console.WriteLine("Unpivoting: {0}", file);
            int j;

            var rows = File.ReadAllLines(file).ToList();
            var headers = rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            rows.RemoveAt(0); //pop the top off
            var possibleHeader = rows[0].Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
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

            const int x = 6;
            for (var i = 0; i < x; i++)
            {
                dataTable.Columns.Add(headers[i]);
            }


            //subject, grade, language, category

            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Grade");
            dataTable.Columns.Add("Language");
            dataTable.Columns.Add(Category);
            var genericHeaders = new List<string>(new[] { "Subject", "Grade", "Language" });


            //weird ones

            foreach (var cat in ExcludedCategories)
            {
                dataTable.Columns.Add(cat);
            }


            //demographics

            foreach (var demo in Demographics)
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

                FillFirstX(dataRow, campus, x);
                FillWeirdHeaders(dataRow, headers, campus);
                FillGenericData(dataRow, genericHeaders, headers[x + 1].Split('_')[0], grade.ToString(), language.ToString());

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

                    dataRow[Category] = category;
                    sb.AppendLine(string.Join(",", dataRow.ItemArray));

                    //dataTable.Rows.Add(dataRow);
                }
            }

            var fileName = Path.GetFileNameWithoutExtension(file);
            File.WriteAllText(string.Format("{0}/{1} - Parsed.csv", outPath, fileName), sb.ToString());

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

        private static void FillFirstX(DataRow dataRow, IList<string> row, int x)
        {
            for (var i = 0; i < x; i++)
            {
                dataRow[i] = row[i];
            }
        }
    }
}
