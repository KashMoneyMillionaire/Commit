using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Infrastructure;
using Infrastructure.Domain;
using MoreLinq;

namespace ParserUtilities
{
    public static class StaarSubjectUnpivotor
    {
        /// <summary>
        /// This takes a csv file for the Staar Subject and unpivots it. It then saves it in the output folder.
        /// </summary>
        /// <param name="file">The *.csv path to read from.</param>
        /// <param name="outPath">The folder to store the output.</param>
        /// <param name="grade">The grade to be associated with the file(s).</param>
        /// <param name="language">The language the test was taken in.</param>
        public static void Unpivot(string file, string outPath, Grade grade, LanguageEnum language, int x)
        {
            //var genericHeaders = new List<string>(new[] { "Grade", "LanguageEnum" });
            
            var dataTable = new DataTable(string.Format("Parsed {0}", file));
            int j;
            var sb = new StringBuilder();


            //Validation

            if (!file.Contains(".csv"))
                throw new CustomException("This is not a csv file.");


            //Lets unpivot this shit

            Console.WriteLine("Unpivoting: {0}", file);


            //read the lines in. First row is headers. save it then remove

            var rows = File.ReadAllLines(file).ToList();
            var headers = rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            rows.RemoveAt(0); //pop the top off


            // check if second row is headers

            var possibleHeader = rows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(possibleHeader[0], out j)) //check if second row of headers
            {
                headers.AddRange(possibleHeader);
                rows.RemoveAt(0); //pop the top off
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
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Demographic");
            dataTable.Columns.Add("Value");


            //Write the column names to the stringbuilder

            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));


            //Begin adding data

            var dataRow = dataTable.NewRow();
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

                    if (new[] { "0", "", "." }.Contains(campus[i])) 
                        continue;


                    //if it is not a triple (the first x columns), skip it

                    var triple = headers[i].Split(new[] {'_'}, 3);
                    if (triple.Length != 3)
                        continue;


                    //write the remaining values, then write to stringbuilder

                    dataRow["Subject"] = triple[0];
                    dataRow["Category"] = triple[1];
                    dataRow["Demographic"] = triple[2];
                    dataRow["Value"] = campus[i];

                    sb.AppendLine(string.Join(",", dataRow.ItemArray));
                }
            }


            //write stringbuilder to file

            var fileName = Path.GetFileNameWithoutExtension(file);
            File.WriteAllText(string.Format("{0}/{1} - Parsed.csv", outPath, fileName), sb.ToString());
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
        
        private static void FillFirstX(DataRow dataRow, IList<string> row, int x)
        {
            for (var i = 0; i < x; i++)
            {
                dataRow[i] = row[i];
            }
        }
    }

    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }
}
