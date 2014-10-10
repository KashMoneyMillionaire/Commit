﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Infrastructure;
using ParserUtilities;
using ParserUtilities.Helpers;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = System.IO.Path;

namespace CommitGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileDataGrid> GridValues = new ObservableCollection<FileDataGrid>();

        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
            FileGrid.ItemsSource = GridValues;
        }

        private async void UnpivotClick(object sender, RoutedEventArgs e)
        {
            StaarSubjectUnpivotor.TestAzure();

            var currentIndex = 0;
            var total = GridValues.Count();


            //Check to make sure there are any files

            if (!GridValues.Any() || OutputPath.Text.Trim() == "")
            {
                MessageBox.Text = "Either the Input File(s) or Output Folder have not been chosen.";
                return;
            }


            //Hide button, display progress bar

            UnpivotButton.IsEnabled = false;
            UnpivotProgressBar.Value = 0;
            UnpivotButton.Visibility = Visibility.Hidden;
            UnpivotProgressBar.Visibility = Visibility.Visible;
            MessageBox.Text = "Unpivoting...\r\n";

            foreach (var file in GridValues)
            {
                //Messagesss
                MessageBox.Text = string.Format("{0}\r\n{1}", MessageBox.Text,
                    Path.GetFileNameWithoutExtension(file.FileName));
                try //to get it in
                {
                    System.Windows.Forms.Application.DoEvents();
                    var file1 = file;
                    Thread.Sleep(1000);
                    //await Task.Run(() => StaarSubjectUnpivotor.Unpivot(file1.FileName, OutputPath.Text, file1.Grade, file1.FileLanguage));
                }
                catch (CustomException ex)
                {
                    MessageBox.Text = string.Format("{0} Line: {1}", ex.Message, new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
                    UnpivotProgressBar.Value = 0;
                    UnpivotButton.IsEnabled = true;
                    break;
                }
                currentIndex++;
                UnpivotProgressBar.Value = (int)Math.Round(currentIndex / (double)total * 100.0);
                MessageBox.Text = string.Format("{0}\r\nDone", MessageBox.Text);
            }

            MessageBox.Text += "\r\nDone Unpivoting";
            UnpivotButton.IsEnabled = true;
            UnpivotButton.Visibility = Visibility.Visible;
            UnpivotProgressBar.Visibility = Visibility.Hidden;
        }

        private void SelectInputFilesButtonClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = true
            };

            try
            {
                var result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    InputPath.Text = Path.GetDirectoryName(dlg.FileName);
                    var inputFiles = dlg.FileNames.ToList();
                    var gridList = (ObservableCollection<FileDataGrid>)FileGrid.ItemsSource;
                    gridList.Clear();

                    foreach (var inputFile in inputFiles)
                    {
                        var grade = Grade.EOC;
                        var gradeRegex = new Regex(@"Grade [0-9]{1,2}");
                        if (gradeRegex.IsMatch(inputFile))
                        {
                            var gradeMatch = gradeRegex.Match(inputFile);
                            grade =
                                gradeMatch.Value.Split(new[] { "Grade " }, StringSplitOptions.None)[1].ParseGrade();
                        }

                        var languageRegex = new Regex(@"[S|s]panish");
                        var languageMatch = languageRegex.IsMatch(inputFile);
                        var language = languageMatch
                            ? Infrastructure.Language.Spanish
                            : Infrastructure.Language.English;


                        gridList.Add(new FileDataGrid
                        {
                            FileName = Path.GetFileName(inputFile),
                            NumberOfColumnsAtBeginning = 6,
                            Grade = grade,
                            FileLanguage = language
                        });
                    }

                }
            }
            catch
            {
            }
        }

        private void SelectOutputFolderButtonClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputPath.Text = dlg.SelectedPath;
            }
        }

    }

    public class FileDataGrid
    {
        public string FileName { get; set; }
        public Language FileLanguage { get; set; }
        public Grade Grade { get; set; }
        public int NumberOfColumnsAtBeginning { get; set; }
    }
}