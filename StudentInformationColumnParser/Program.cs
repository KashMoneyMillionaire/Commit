using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StudentInformationColumnParser.Domain;
using StudentInformationColumnParser.Helpers;

namespace StudentInformationColumnParser
{
    class Program
    {
        private const string BaseUrl = "C:/Users/kcummings/Google Drive/Work/Commit/";
        static void Main(string[] args)
        {
            var x = ReadCampuses();
            ReadStaarData(x);
        }

        private static void ReadStaarData(IReadOnlyDictionary<long, Campus> campuses)
        {
            var rows = System.IO.File.ReadLines(
                    string.Format(BaseUrl + "2012-2013/Campus/3rd Grade STAAR Test Results.csv")).ToList();
            var headers = rows[0].Split(',');

            //for each row in the file (each campus)
            var a = new Stopwatch();
            a.Start();
            for (var x = 1; x < 100; x++)
            {
                var cells = rows[x].Split(',').ToArray();
                Campus campus;
                try
                {
                    campus = campuses[int.Parse(cells[0])];
                }
                catch (Exception)
                {
                    Console.WriteLine("No school for ID {0}", cells[0]);
                    continue;
                }

                campus.StaarStats = new List<StaarStat>();

                //for each cell in the row (each StaarStat)
                for (var i = 13; i < cells.Length; i++)
                {
                    //split the column header
                    var columnBits = headers[i].Split(new[] { "_" }, 3, StringSplitOptions.None);
                    var staar = new StaarStat();

                    try
                    {

                        staar.Subject = (StaarSubjectName)Enum.Parse(typeof(StaarSubjectName), columnBits[0]);
                        staar.Field = (StaarFieldName)Enum.Parse(typeof(StaarFieldName), columnBits[1]);
                        staar.Category = (StaarCategoryName)Enum.Parse(typeof(StaarCategoryName), columnBits[2]);
                    }
                    catch (Exception)
                    {
                        
                        
                    }
                    staar.Value = cells[i];
                    staar.Year = 2013;
                    staar.Grade = Grade.G3;
                    campus.StaarStats.Add(staar);
                }
                Console.WriteLine("Finished {1}\t of {2} - {0}", campus.Name, x, rows.Count);
            }
            a.Stop();
            Console.WriteLine(a.ElapsedMilliseconds);
        }

        public static Dictionary<long, Campus> ReadCampuses()
        {
            var campuses = new Dictionary<long, Campus>();

            var lines = System.IO.File.ReadLines(
                    string.Format(BaseUrl + "2012-2013/Campus/2013 Campus Reference.csv")).ToList();
            var headers = lines[0].Split(',');

            for (var x = 1; x < lines.Count; x++)
            {
                var line = lines[x].Replace("'", "").Split(',');
                var campus = new Campus();
                for (var y = 0; y < headers.Count(); y++)
                {
                    var column = headers[y];
                    switch (column.ToLower())
                    {
                        case "campus":
                            campus.CampusId = int.Parse(line[y]);
                            break;
                        case "cad_math":
                            campus.CadMath = line[y].ParseBool();
                            break;
                        case "cad_read":
                            campus.CadRead = line[y].ParseBool();
                            break;
                        case "cad_progress":
                            campus.CadProgress = line[y].ParseBool();
                            break;
                        case "campname":
                            campus.Name = line[y];
                            break;
                        case "cflalted":
                            campus.IsRatedUnderAEAProcedures = line[y].ParseBool();
                            break;
                        case "cflchart":
                            campus.IsCharterSchool = line[y].ParseBool();
                            break;
                        case "cntyname":
                            campus.CountyName = line[y];
                            break;
                        case "county":
                            campus.CountyId = int.Parse(line[y]);
                            break;
                        case "c_rating":
                            campus.AccountabilityRating = (AccountabilityRating)line[y].Parse();
                            break;
                        case "distname":
                            campus.DistrictName = line[y];
                            break;
                        case "district":
                            campus.DistrictNumber = int.Parse(line[y]);
                            break;
                        case "grdspan":
                            var grades = line[y].Split(new[] { " - " }, StringSplitOptions.None);
                            campus.StartGrade = (Grade)grades[0].Parse();
                            campus.EndGrade = (Grade)grades[1].Parse();
                            break;
                        case "grdtype":
                            campus.GradeType = (GradeType)line[y].Parse();
                            break;
                        case "region":
                            campus.RegionNumber = int.Parse(line[y]);
                            break;
                        case "cackdtl":
                            foreach (var character in line[y])
                            {
                                campus.CackDtl |= character.Parse();
                            }
                            break;

                        default:
                            Console.WriteLine(column);
                            break;
                    }
                }
                campuses.Add(campus.CampusId, campus);
            }
            return campuses;
        }

        public void ColumnUniqueness()
        {
            var dict = new Dictionary<string, List<int>>();
            var years = new[] { 2011, 2013 };
            foreach (var year in years)
            {
                var text = System.IO.File.ReadAllText(string.Format("C:/Users/kcummings/Google Drive/Work/Commit/StudentInformation/{0} Columns.txt", year));
                var textsplit = text.Split('\n');
                foreach (var regexed in from bit in textsplit.Select(s => s.Substring(0, s.IndexOf('-') - 1)) let pattern = "[1][0-3]" let replacement = "**" let rgx = new Regex(pattern) select rgx.Replace(bit, replacement))
                {
                    if (dict.ContainsKey(regexed))
                    {
                        dict[regexed].Add(year);
                    }
                    else
                    {
                        dict.Add(regexed, new List<int> { year });
                    }
                }
            }
            var a = dict.Where(d => d.Value.Count > 1).ToList();
            var b = dict.Where(d => d.Value.Contains(2011)).ToList();
            var b2 = b.Where(d => d.Value.Count == 1).ToList();
            var c = dict.Where(d => d.Value.Contains(2013)).ToList();
            var c2 = c.Where(d => d.Value.Count == 1).ToList();
            var e = 1;
        }

        public void StaarParser()
        {
            //var dict = new Dictionary<long, StaarStat>();

            //var text = System.IO.File.ReadLines(
            //        string.Format("C:/Users/kcummings/Google Drive/Work/Commit/StudentInformation/ Columns.txt"));
            //foreach (var year in years)
            //{
            //}
        }
    }
}


//CA0GH09N -- Campus 2009 Graduates: Recom HS Pgm All Students Count