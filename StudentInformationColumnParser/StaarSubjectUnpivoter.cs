using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommitParser.Helpers;

namespace CommitParser
{
    public static class StaarSubjectUnpivotor
    {
        private const string Category = "Category";

        private static readonly List<string> Demographics =
            Enum.GetValues(typeof(StaarDemographic)).Cast<StaarDemographic>().Select(c => c.ToString()).ToList();

        private static readonly List<string> Categories =
            Enum.GetValues(typeof(StaarCategoryName)).Cast<StaarCategoryName>().Select(c => c.ToString()).ToList();

        private static object[] _rowTemplate;

        public static void Unpivot(string fileName, Grade grade, Language language)
        {
            Console.WriteLine("Let's unpivot this shit.");
            var rows = File.ReadAllLines(fileName).ToList();
            var headers = rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            headers.AddRange(rows[1].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            var sdc = headers.Select(h => h.Split(new[] { '_' }, 3)).ToList();
            rows.RemoveRange(0, 2);


            //begin changing stuff

            var dataTable = new DataTable(string.Format("Parsed {0}", fileName));
            var weirdHeaders = new List<string>();

            //first X

            var x = 6;
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

            foreach (var split in headers.Select(header => header.Split(new[] { '_' }, 3)))
            {
                try
                {
                    if (!Categories.Contains(split[2].Trim()))
                    {
                        weirdHeaders.Add(split[2].Trim());
                        dataTable.Columns.Add(split[2].Trim());
                    }
                }
                catch { }
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
                FillWeirdHeaders(dataRow, weirdHeaders, headers, campus);
                FillGenericData(dataRow, genericHeaders, headers[x + 1].Split('_')[0], grade.ToString(), language.ToString());

                foreach (var category in Categories)
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

            File.WriteAllText(fileName.Replace(".csv", " - Parsed.csv"), sb.ToString());

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

        private static void FillWeirdHeaders(DataRow dataRow, IEnumerable<string> weirdHeaders, List<string> headers, string[] campusData)
        {
            foreach (var we in weirdHeaders)
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
