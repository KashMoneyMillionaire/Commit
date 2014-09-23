using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper.Internal;
using CommitParser.Domain;
using CommitParser.Helpers;
using EntityFramework.BulkInsert.Extensions;

namespace CommitParser
{
    class Program : Form
    {
        private List<string> inputFiles = new List<string>();
        private Panel panel1;
        private Button OutputButton;
        private TextBox OutputPath;
        private ListView SelectFilesListView;
        private ColumnHeader FileName;
        private Button InputButton;
        private TextBox InputPath;
        private ProgressBar ProgressBar;
        private Button UnpivotButton;
        private TextBox MessageBox;
        private Label label1;
        private ComboBox GradeDropDown;
        private Label label2;
        private ComboBox LanguageDropDown;
        private Panel InputPanel;
        private Button OpenFolderButton;
        private bool unpivoting = false;

        //Setup Form
        public Program()
        {
            InitializeComponent();
            GradeDropDown.DataSource = Enum.GetValues(typeof (Grade));
            LanguageDropDown.DataSource = Enum.GetValues(typeof (Language));
        }

        //Input selection button
        protected void InputButton_Click(object obj, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = true
            };

            try
            {
                //string[] files = Directory.GetFiles(dlg.SelectedPath);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    InputPath.Text = Path.GetDirectoryName(dlg.FileName);
                    inputFiles = dlg.FileNames.ToList();

                    foreach (var inputFile in inputFiles)
                    {
                        SelectFilesListView.Items.Add(Path.GetFileName(inputFile));
                    }
                }
            }
            catch
            {
            }
        }

        //Output selection button
        protected void OutputButton_Click(object obj, EventArgs e)
        {
            
            var dlg = new FolderBrowserDialog();

            try
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    OutputPath.Text = dlg.SelectedPath;
            }
            catch { }
        }

        //Start button
        protected void StartButton_Click(object obj, EventArgs e)
        {
            UnpivotButton.Enabled = false;
            if (!inputFiles.Any() || OutputPath.Text.Trim() == "")
            {
                MessageBox.Text = "Either the Input File(s) or Output Folder have not been chosen.";
                return;
            }
            var currentIndex = 0;
            var selectedGrade = (Grade)Enum.Parse(typeof(Grade), GradeDropDown.SelectedIndex.ToString());
            var selectedLanguage = (Language)Enum.Parse(typeof(Language), LanguageDropDown.SelectedIndex.ToString());
            ProgressBar.Value = 0;
            MessageBox.Text = "Unpivoting...\r\n";
            var total = inputFiles.Count();

            foreach (var file in inputFiles)
            {
                //I know this is bad Kash but im not quite sure how to make it like you did it before... :l
                //Clay, this is how you do it.
                MessageBox.Text += string.Format("{0} ... ", Path.GetFileNameWithoutExtension(inputFiles[currentIndex]));
                try
                {
                    Application.DoEvents();
                    StaarSubjectUnpivotor.Unpivot(file, OutputPath.Text, selectedGrade, selectedLanguage);
                }
                catch (Exception ex)
                {
                    MessageBox.Text = ex.Message;
                    ProgressBar.Value = 0;
                    UnpivotButton.Enabled = true;
                    break;
                }
                currentIndex++;
                ProgressBar.Value = (int) Math.Round(currentIndex/(double) total*100.0);
                MessageBox.Text += "Done.\r\n";
            }
            MessageBox.Text += "\r\nDone Unpivoting";
            UnpivotButton.Enabled = true;
        }

        [STAThread]
        public static int Main(string[] args)
        {
            Application.Run(new Program());
            return 0;

            //ReadCampuses();
            //CreateSubCatFields();
            //CreateYearGradeLangs();
            //var ctx = new OperationalDataContext();
            //var x = ctx.Campuses.First();
            //var y = new StaarStat
            //{
            //    Campus_Id = x.Id,
            //    SubCatField_Id = ctx.SubCatFields.First().Id
            //};
            ////x.StaarStats = new List<StaarStat> { y };
            //ctx.BulkInsert(new List<StaarStat> { y });
            ////ctx.Campuses.AddOrUpdate(x);
            //ctx.SaveChangesAsync();

            //foreach (var file in Directory.EnumerateFiles(@"Resources\StaarData\Subject", "*.csv"))
            //{
            //    var a = new Stopwatch();
            //    a.Start();
            //    ReadStaarDataBySubject(file);
            //    a.Stop();
            //    Console.WriteLine("Time ellapsed: {0} secs", a.ElapsedMilliseconds / 1000.0);
            //}
            ////ReadStaarDataBySubject();
            //DeduplicateFile();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
            this.panel1 = new System.Windows.Forms.Panel();
            this.GradeDropDown = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LanguageDropDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MessageBox = new System.Windows.Forms.TextBox();
            this.UnpivotButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.OutputButton = new System.Windows.Forms.Button();
            this.OutputPath = new System.Windows.Forms.TextBox();
            this.SelectFilesListView = new System.Windows.Forms.ListView();
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InputButton = new System.Windows.Forms.Button();
            this.InputPath = new System.Windows.Forms.TextBox();
            this.OpenFolderButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OpenFolderButton);
            this.panel1.Controls.Add(this.GradeDropDown);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.LanguageDropDown);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.MessageBox);
            this.panel1.Controls.Add(this.UnpivotButton);
            this.panel1.Controls.Add(this.ProgressBar);
            this.panel1.Controls.Add(this.OutputButton);
            this.panel1.Controls.Add(this.OutputPath);
            this.panel1.Controls.Add(this.SelectFilesListView);
            this.panel1.Controls.Add(this.InputButton);
            this.panel1.Controls.Add(this.InputPath);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // GradeDropDown
            // 
            this.GradeDropDown.FormattingEnabled = true;
            resources.ApplyResources(this.GradeDropDown, "GradeDropDown");
            this.GradeDropDown.Name = "GradeDropDown";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // LanguageDropDown
            // 
            this.LanguageDropDown.FormattingEnabled = true;
            resources.ApplyResources(this.LanguageDropDown, "LanguageDropDown");
            this.LanguageDropDown.Name = "LanguageDropDown";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // MessageBox
            // 
            resources.ApplyResources(this.MessageBox, "MessageBox");
            this.MessageBox.Name = "MessageBox";
            // 
            // UnpivotButton
            // 
            resources.ApplyResources(this.UnpivotButton, "UnpivotButton");
            this.UnpivotButton.Name = "UnpivotButton";
            this.UnpivotButton.UseVisualStyleBackColor = true;
            this.UnpivotButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ProgressBar
            // 
            resources.ApplyResources(this.ProgressBar, "ProgressBar");
            this.ProgressBar.Name = "ProgressBar";
            // 
            // OutputButton
            // 
            resources.ApplyResources(this.OutputButton, "OutputButton");
            this.OutputButton.Name = "OutputButton";
            this.OutputButton.UseVisualStyleBackColor = true;
            this.OutputButton.Click += new System.EventHandler(this.OutputButton_Click);
            // 
            // OutputPath
            // 
            resources.ApplyResources(this.OutputPath, "OutputPath");
            this.OutputPath.Name = "OutputPath";
            // 
            // SelectFilesListView
            // 
            resources.ApplyResources(this.SelectFilesListView, "SelectFilesListView");
            this.SelectFilesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName});
            this.SelectFilesListView.Name = "SelectFilesListView";
            this.SelectFilesListView.UseCompatibleStateImageBehavior = false;
            this.SelectFilesListView.View = System.Windows.Forms.View.List;
            // 
            // FileName
            // 
            resources.ApplyResources(this.FileName, "FileName");
            // 
            // InputButton
            // 
            resources.ApplyResources(this.InputButton, "InputButton");
            this.InputButton.Name = "InputButton";
            this.InputButton.UseVisualStyleBackColor = true;
            this.InputButton.Click += new System.EventHandler(this.InputButton_Click);
            // 
            // InputPath
            // 
            resources.ApplyResources(this.InputPath, "InputPath");
            this.InputPath.Name = "InputPath";
            // 
            // OpenFolderButton
            // 
            resources.ApplyResources(this.OpenFolderButton, "OpenFolderButton");
            this.OpenFolderButton.Name = "OpenFolderButton";
            this.OpenFolderButton.UseVisualStyleBackColor = true;
            this.OpenFolderButton.Click += new System.EventHandler(this.OpenFolderButton_Click);
            // 
            // Program
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Name = "Program";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void OpenFolderButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(OutputPath.Text))
            {
                Process.Start(OutputPath.Text);
            }
            else
            {
                MessageBox.Text = "The path does not exist to open.";
            }
        }


        //private static void CreateYearGradeLangs()
        //{
        //    var ctx = new OperationalDataContext();
        //    if (!ctx.SubCatFields.Any())
        //    {
        //        var subCatField = new List<YearGradeLang>();
        //        foreach (var year in new[] { 2010, 2011, 2012, 2013, 2014 })
        //        {
        //            foreach (var grade in Enum.GetValues(typeof(Grade)).Cast<Grade>())
        //            {
        //                foreach (var language in Enum.GetValues(typeof(Language)).Cast<Language>())
        //                {
        //                    subCatField.Add(new YearGradeLang
        //                    {
        //                        Year = year,
        //                        Grade = grade,
        //                        Language = language
        //                    });
        //                }
        //            }
        //        }
        //        ctx.BulkInsert(subCatField);
        //        ctx.SaveChanges();
        //    }
        //}

        //private static void CreateSubCatFields()
        //{
        //    var ctx = new OperationalDataContext();
        //    if (!ctx.SubCatFields.Any())
        //    {
        //        var subCatField = new List<SubCatField>();
        //        foreach (var cat in Enum.GetValues(typeof(StaarCategoryName)).Cast<StaarCategoryName>())
        //        {
        //            foreach (var field in Enum.GetValues(typeof(StaarDemographic)).Cast<StaarDemographic>())
        //            {
        //                foreach (var sub in Enum.GetValues(typeof(StaarSubjectName)).Cast<StaarSubjectName>())
        //                {
        //                    subCatField.Add(new SubCatField
        //                    {
        //                        Field = field,
        //                        Category = cat,
        //                        Subject = sub
        //                    });
        //                }
        //            }
        //        }
        //        ctx.BulkInsert(subCatField);
        //        ctx.SaveChanges();
        //    }
        //}

        ////private static void ReadStaarDataByGrade(IReadOnlyDictionary<long, Campus> campuses, string filePath, bool firstTwoRowsAsHeaders = false)
        ////{
        ////    var rows = File.ReadLines(filePath).ToList();
        ////    var headers = rows[0].Split(',').ToList();
        ////    var startingRow = 1;

        ////    if (firstTwoRowsAsHeaders)
        ////    {
        ////        headers.AddRange(rows[1].Split(','));
        ////        startingRow++;
        ////    }

        ////    //for each row in the file (each campus)
        ////    var a = new Stopwatch();
        ////    a.Start();
        ////    for (var x = startingRow; x < rows.Count; x++)
        ////    {
        ////        var cells = rows[x].Split(',').ToArray();
        ////        Campus campus;
        ////        try
        ////        {
        ////            campus = campuses[int.Parse(cells[0])];
        ////        }
        ////        catch (Exception)
        ////        {
        ////            Console.WriteLine("No school for ID {0}", cells[0]);
        ////            continue;
        ////        }

        ////        campus.StaarStats = new List<StaarStat>();

        ////        //for each cell in the row (each StaarStat)
        ////        for (var i = 13; i < cells.Length; i++)
        ////        {
        ////            //split the column header
        ////            var columnBits = headers[i].Split(new[] { "_" }, 3, StringSplitOptions.None);
        ////            var staar = new StaarStat();

        ////            try
        ////            {

        ////                staar.Subject = (StaarSubjectName)Enum.Parse(typeof(StaarSubjectName), columnBits[0]);
        ////                staar.Field = (StaarDemographic)Enum.Parse(typeof(StaarDemographic), columnBits[1]);
        ////                staar.Category = (StaarCategoryName)Enum.Parse(typeof(StaarCategoryName), columnBits[2]);
        ////            }
        ////            catch (Exception)
        ////            {


        ////            }
        ////            staar.Value = cells[i];
        ////            staar.Year = int.Parse(cells[1]); //1
        ////            staar.Grade = (Grade)cells[6].Parse(); //6
        ////            campus.StaarStats.Add(staar);
        ////        }
        ////        Console.WriteLine("Finished {1}\t of {2} - {0}", campus.Name, x, rows.Count);
        ////    }
        ////    a.Stop();
        ////    Console.WriteLine(a.ElapsedMilliseconds);
        ////}

        //private static void ReadStaarDataBySubject(string file)
        //{
        //    Console.WriteLine("Starting Staar data by subject for {0}... ", file);


        //    //Check if this file has been processed

        //    var ctx = new OperationalDataContext();
        //    ctx.Configuration.ValidateOnSaveEnabled = false;
        //    if (ctx.CompletedFiles.SingleOrDefault(s => s.FileName == file && s.IsCompleted) != null)
        //    {
        //        return;
        //    }


        //    //get the rows from the file, and split the headers off

        //    var campusRows = File.ReadLines(file).ToList();
        //    var headers = campusRows[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    headers.AddRange(campusRows[1].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)); //headers span two rows for some reason
        //    campusRows.RemoveRange(0, 2);


        //    //for each row in the file

        //    var bag = new ConcurrentBag<StaarStat>();
        //    var campusCount = 0;
        //    foreach (var campusRow in campusRows)
        //    {
        //        campusCount++;
        //        var staarStats = campusRow.Split(',').ToArray();
        //        var campusNumber = int.Parse(staarStats[0]);


        //        //make sure we have a matching campus

        //        var campus = ctx.Campuses.SingleOrDefault(c => c.CampusNumber == campusNumber);
        //        if (campus == null)
        //        {
        //            var writer = File.AppendText(@"C:\Users\kcummings\Google Drive\Work\Commit\Missing School Numbers.txt");
        //            writer.WriteLine(campusNumber);
        //            writer.Close();
        //            //Console.WriteLine("\tNo school for ID {0}", staarStats[0]);
        //            continue;
        //        }


        //        //iterate through each column and collect the staarStats

        //        var containers = new List<Container>(staarStats.Length);
        //        for (var i = 11; i < staarStats.Length; i++)
        //        {
        //            containers.Add(new Container { Stat = staarStats[i], Header = headers[i] });
        //        }

        //        var closureBag = bag;
        //        Parallel.ForEach(containers,
        //            new ParallelOptions { MaxDegreeOfParallelism = 4 },
        //            container =>
        //            {
        //                //split the column header
        //                var staarStatValues = container.Header.Split(new[] { "_" }, 3, StringSplitOptions.None);
        //                var subject = Converter.GetSubject(staarStatValues[0]);
        //                var field = Converter.GetField(staarStatValues[1]);
        //                var category = Converter.GetCategory(staarStatValues[2]);
        //                var year = int.Parse(staarStats[1]);
        //                var grade = FindGradeBySubject(subject);
        //                var ygl =
        //                    ctx.YearGradeLangs.Single(
        //                        y => y.Year == year && y.Grade == grade && y.Language == Language.English);
        //                var scf =
        //                    ctx.SubCatFields.Single(
        //                        s => s.Category == category && s.Subject == subject && s.Field == field);
        //                var staar = new StaarStat
        //                {
        //                    Value = container.Stat,
        //                    Campus_Id = campus.Id,
        //                    SubCatField = scf,
        //                    YearGradeLang = ygl
        //                };
        //                closureBag.Add(staar);
        //            });


        //        //bulk insert when greater than 10,000

        //        //if (bag.Count > 100000) //100 = 169
        //        //{
        //        //    Console.Write("\tWriting ~100,000 more stats... {0} campuses so far... ", campusCount);
        //        //    ctx.BulkInsert(bag);
        //        //    bag = new ConcurrentBag<StaarStat>();
        //        //    Console.WriteLine("Done.");
        //        //}
        //        if (campusCount % 200 == 0)
        //        {
        //            Console.WriteLine("\tCompleted {0} schools", campusCount);
        //        }
        //    }


        //    //finish up the remaining, and update completed files

        //    Console.Write("\tFinishing up for this file... ");
        //    for (var i = 0; i <= bag.Count / 100000; i++)
        //    {
        //        ctx.BulkInsert(bag.Skip(i * 100000).Take(100000));
        //        ctx.SaveChangesAsync();
        //    }
        //    //ctx.BulkInsert(bag);
        //    Console.Write("Done.");
        //    UpdateFile(file, ctx);
        //}

        //private static Grade FindGradeBySubject(StaarSubjectName subject)
        //{
        //    switch (subject)
        //    {
        //        case StaarSubjectName.a1:
        //            return Grade.G12;
        //        case StaarSubjectName.a2:
        //            return Grade.G12;
        //        case StaarSubjectName.w:
        //            return Grade.G12;
        //        case StaarSubjectName.w1:
        //            return Grade.G12;
        //        case StaarSubjectName.w2:
        //            return Grade.G12;
        //        case StaarSubjectName.r:
        //            return Grade.G12;
        //        case StaarSubjectName.e1:
        //            return Grade.G12;
        //        case StaarSubjectName.e2:
        //            return Grade.G12;
        //        case StaarSubjectName.us:
        //            return Grade.G12;
        //        case StaarSubjectName.bi:
        //            return Grade.G12;
        //        case StaarSubjectName.wg:
        //            return Grade.G12;
        //        case StaarSubjectName.ch:
        //            return Grade.G12;
        //        case StaarSubjectName.m:
        //            return Grade.G12;
        //        case StaarSubjectName.s:
        //            return Grade.G12;
        //        case StaarSubjectName.h:
        //            return Grade.G12;
        //        default:
        //            throw new ArgumentOutOfRangeException("No subject match");
        //    }
        //}

        //public static void ReadCampuses()
        //{
        //    Console.Write("Starting Campuses... ");
        //    var ctx = new OperationalDataContext();
        //    ctx.Configuration.ValidateOnSaveEnabled = false;
        //    foreach (var file in Directory.EnumerateFiles(@"Resources\Campuses", "*.csv"))
        //    {
        //        if (ctx.CompletedFiles.SingleOrDefault(s => s.FileName == file && s.IsCompleted) != null)
        //        {
        //            continue;
        //        }

        //        var campusRow = File.ReadLines(file).ToList();
        //        var existingCampusNumbers = ctx.Campuses.Select(c => c.CampusNumber).ToList();
        //        var headers = campusRow[0].Split(',').ToList();
        //        campusRow.RemoveRange(0, 1);

        //        Parallel.ForEach(campusRow, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (row) =>
        //        {
        //            using (var parallelCtx = new OperationalDataContext())
        //            {
        //                var line = row.Replace("'", "").Split(',');
        //                var campus = new Campus();

        //                for (var y = 0; y < headers.Count(); y++)
        //                {
        //                    var column = headers[y];
        //                    switch (column.ToLower())
        //                    {
        //                        case "campus":
        //                            campus.CampusNumber = int.Parse(line[y]);
        //                            if (existingCampusNumbers.Any(c => c == campus.CampusNumber)) goto skipCampus;
        //                            break;
        //                        case "cad_math":
        //                            campus.CadMath = line[y].ParseBool();
        //                            break;
        //                        case "cad_read":
        //                            campus.CadRead = line[y].ParseBool();
        //                            break;
        //                        case "cad_progress":
        //                            campus.CadProgress = line[y].ParseBool();
        //                            break;
        //                        case "campname":
        //                            campus.Name = line[y];
        //                            break;
        //                        case "cflalted":
        //                            campus.IsRatedUnderAEAProcedures = line[y].ParseBool();
        //                            break;
        //                        case "cflchart":
        //                            campus.IsCharterSchool = line[y].ParseBool();
        //                            break;
        //                        case "cntyname":
        //                            campus.CountyName = line[y];
        //                            break;
        //                        case "county":
        //                            campus.CountyId = int.Parse(line[y]);
        //                            break;
        //                        case "c_rating":
        //                            campus.AccountabilityRating = (AccountabilityRating)line[y].Parse();
        //                            break;
        //                        case "distname":
        //                            campus.DistrictName = line[y];
        //                            break;
        //                        case "district":
        //                            campus.DistrictNumber = int.Parse(line[y]);
        //                            break;
        //                        case "grdspan":
        //                            var grades = line[y].Split(new[] { " - " }, StringSplitOptions.None);
        //                            campus.StartGrade = (Grade)grades[0].Parse();
        //                            campus.EndGrade = (Grade)grades[1].Parse();
        //                            break;
        //                        case "grdtype":
        //                            campus.GradeType = (GradeType)line[y].Parse();
        //                            break;
        //                        case "region":
        //                            campus.RegionNumber = int.Parse(line[y]);
        //                            break;
        //                        case "cackdtl":
        //                            foreach (var character in line[y])
        //                            {
        //                                campus.CackDtl |= character.Parse();
        //                            }
        //                            break;

        //                        default:
        //                            //Console.WriteLine(column);
        //                            break;
        //                    }
        //                }
        //                parallelCtx.Campuses.Add(campus);
        //                parallelCtx.SaveChanges();
        //            skipCampus:
        //                ;
        //            }
        //        });
        //        UpdateFile(file, ctx);
        //    }
        //    Console.WriteLine("Done.");
        //}

        //private static void UpdateFile(string file, OperationalDataContext ctx)
        //{
        //    var completedFile = ctx.CompletedFiles.SingleOrDefault(s => s.FileName == file);
        //    if (completedFile != null)
        //    {
        //        completedFile.IsCompleted = true;
        //    }
        //    else
        //    {
        //        ctx.CompletedFiles.Add(new CompletedFile
        //        {
        //            IsCompleted = true,
        //            TimeCompleted = DateTime.Now,
        //            FileName = file
        //        });
        //    }
        //    ctx.SaveChangesAsync();
        //}

        //public static void DeduplicateFile()
        //{
        //    var nums = File.ReadAllLines(@"C:\Users\kcummings\Google Drive\Work\Commit\Missing School Numbers.txt").ToList();
        //    File.WriteAllLines(@"C:\Users\kcummings\Google Drive\Work\Commit\Missing School Numbers.txt",
        //        nums.Distinct().ToArray());
        //}

        //private static OperationalDataContext Refresh(OperationalDataContext ctx)
        //{
        //    ctx.Dispose();
        //    ctx = new OperationalDataContext();
        //    ctx.Configuration.AutoDetectChangesEnabled = true;
        //    return ctx;
        //}

        //public void ColumnUniqueness()
        //{
        //    var dict = new Dictionary<string, List<int>>();
        //    var years = new[] { 2011, 2013 };
        //    foreach (var year in years)
        //    {
        //        var text = File.ReadAllText(string.Format("C:/Users/kcummings/Google Drive/Work/Commit/StudentInformation/{0} Columns.txt", year));
        //        var textsplit = text.Split('\n');
        //        foreach (var regexed in from bit in textsplit.Select(s => s.Substring(0, s.IndexOf('-') - 1)) let pattern = "[1][0-3]" let replacement = "**" let rgx = new Regex(pattern) select rgx.Replace(bit, replacement))
        //        {
        //            if (dict.ContainsKey(regexed))
        //            {
        //                dict[regexed].Add(year);
        //            }
        //            else
        //            {
        //                dict.Add(regexed, new List<int> { year });
        //            }
        //        }
        //    }
        //    var a = dict.Where(d => d.Value.Count > 1).ToList();
        //    var b = dict.Where(d => d.Value.Contains(2011)).ToList();
        //    var b2 = b.Where(d => d.Value.Count == 1).ToList();
        //    var c = dict.Where(d => d.Value.Contains(2013)).ToList();
        //    var c2 = c.Where(d => d.Value.Count == 1).ToList();
        //    var e = 1;
        //}

        //public void StaarParser()
        //{
        //    //var dict = new Dictionary<long, StaarStat>();

        //    //var text = System.IO.File.ReadLines(
        //    //        string.Format("C:/Users/kcummings/Google Drive/Work/Commit/StudentInformation/ Columns.txt"));
        //    //foreach (var year in years)
        //    //{
        //    //}
        //}
    }

    public class Container
    {
        public string Stat;
        public string Header;
    }
}


//CA0GH09N -- Campus 2009 Graduates: Recom HS Pgm All Students Count