using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Domain;
using Infrastructure.Services;
using ParserUtilities;
using ParserUtilities.Helpers;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Commit.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileDataGrid> GridValues = new ObservableCollection<FileDataGrid>();
        private Boolean _autoScroll = true;

        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
            FileGrid.ItemsSource = GridValues;

            var options = new List<string>();
            options.Add("Narrow");
            options.Add("Wide");
            WidthSetting.ItemsSource = options;
            WidthSetting.SelectedIndex = 0;
        }

        private async void RemoveSelectedClick(object sender, RoutedEventArgs e)
        {
            for (var i = FileGrid.SelectedItems.Count - 1; i >= 0; i--)
            {
                var part = FileGrid.SelectedItems[i] as FileDataGrid;
                if (part == null) continue;

                var x = GridValues.First(f => f.FileName == part.FileName);
                GridValues.Remove(x);
            }
        }

        private async void RemoveClick(object sender, RoutedEventArgs e)
        {
            GridValues.Clear();
        }

        private async void UnpivotSelectedClick(object sender, RoutedEventArgs e)
        {
            Upivot(true);
        }

        private async void UnpivotClick(object sender, RoutedEventArgs e)
        {
            Upivot(false);
        }

        private async void Upivot(bool onlySelected)
        {
            var currentIndex = 0;
            int total;
            IList<FileDataGrid> operatingFiles;
            var unpivotorService = ApplicationFactory.RetrieveService<UnpivotorService>() as UnpivotorService ?? new UnpivotorService();
            if (onlySelected)
            {
                total = FileGrid.SelectedItems.Count;
                operatingFiles = (from FileDataGrid item in FileGrid.SelectedItems select GridValues.First(f => f.FileName == item.FileName)).ToList();
            }
            else
            {
                total = GridValues.Count();
                operatingFiles = GridValues;
            }

            //Check to make sure there are any files

            if (operatingFiles == null || operatingFiles.Count == 0)
            {
                System.Windows.MessageBox.Show("You have not selected any files to operate on.", "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (OutputPath.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("The Output Folder has not been chosen.", "Incorrect Output", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Double check Azure export

            if (AzureCheckBox.IsChecked.HasValue && AzureCheckBox.IsChecked.Value)
            {
                var result = System.Windows.MessageBox.Show("Are you sure you want to export to Azure?", "Azure Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;
            }

            //Hide button, display progress bar

            UnpivotButton.IsEnabled = false;
            UnpivotSelectedButton.IsEnabled = false;
            UnpivotProgressBar.Value = 0;
            UnpivotButton.Visibility = Visibility.Hidden;
            UnpivotSelectedButton.Visibility = Visibility.Hidden;
            UnpivotProgressBar.Visibility = Visibility.Visible;
            MessageBox.Text = "Unpivoting...";
            var results = new List<Task<string>>();

            foreach (var file in operatingFiles)
            {

                MessageBox.Text = string.Format("{0}\r\n{1}...", MessageBox.Text,
                    Path.GetFileNameWithoutExtension(file.FullFile));

                System.Windows.Forms.Application.DoEvents();
                var closureFile = file;
                var output = OutputPath.Text;
                var isNarrow = WidthSetting.SelectedItem.ToString() == "Narrow";
                results.Add(Task.Run(() =>
                {
                    try
                    {
                        if (isNarrow)
                        {
                            unpivotorService.UnpivotStaarTestNarrow(closureFile.FullFile, output, closureFile.FileLanguageEnum);
                        }
                        else
                        {
                            unpivotorService.UnpivotStaarTestWide(closureFile.FullFile, output, closureFile.FileLanguageEnum);
                        }
                    }
                    catch (Exception ex)
                    {
                        return string.Format("The unpivoting of {0} has failed. \r\n{1}", closureFile.FileName,
                            ex.Message);
                    }

                    return string.Format("{0} has been succesfully unpivoted.", closureFile.FileName);

                }));
            }

            //wait for results
            try
            {
                Task.WaitAll(results.ToArray());
            }
            catch (Exception)
            {
                UnpivotProgressBar.Value = 0;
                UnpivotButton.IsEnabled = true;
            }

            //update stuff
            foreach (var result in results)
            {
                MessageBox.Text = MessageBox.Text.Append(string.Format("\r\n{0}", result.Result));
                currentIndex++;
                UnpivotProgressBar.Value = (int)Math.Round(currentIndex / (double)total * 100.0);
            }

            MessageBox.Text = string.Format("{0}\r\nDone Unpivoting", MessageBox.Text);
            UnpivotButton.IsEnabled = true;
            UnpivotSelectedButton.IsEnabled = true;
            UnpivotButton.Visibility = Visibility.Visible;
            UnpivotSelectedButton.Visibility = Visibility.Visible;
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

                    //Needs to add instead of replace
                    //but not add duplicates...
                    gridList.Clear();

                    foreach (var inputFile in inputFiles)
                    {
                        //var grade = Grade.EOC;
                        //var gradeRegex = new Regex(@"Grade [0-9]{1,2}");
                        //if (gradeRegex.IsMatch(inputFile))
                        //{
                        //    var gradeMatch = gradeRegex.Match(inputFile);
                        //    grade = gradeMatch.Value.Split(new[] { "Grade " }, StringSplitOptions.None)[1].ParseGrade();
                        //}

                        var languageRegex = new Regex(@"[S|s]panish");
                        var languageMatch = languageRegex.IsMatch(inputFile);
                        var language = languageMatch
                            ? Infrastructure.LanguageEnum.Spanish
                            : Infrastructure.LanguageEnum.English;

                        gridList.Add(new FileDataGrid
                        {
                            FileName = Path.GetFileName(inputFile),
                            NumberOfColumnsAtBeginning = 6,
                            //Grade = grade,
                            FileLanguageEnum = language,
                            FullFile = inputFile
                        });
                    }
                }
            }
            catch
            {
            }
        }


        private void SelectInputZipFileButtonClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Zip File|*.zip"
            };

            try
            {
                var result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    ZipBox.Text = dlg.FileName;
                    
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

        private void UnpivotScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset autoscroll mode
            if (e.ExtentHeightChange == 0.0)
            {   // Content unchanged : user scroll event
                if (UnpivotScrollViewer.VerticalOffset == UnpivotScrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set autoscroll mode
                    _autoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset autoscroll mode
                    _autoScroll = false;
                }
            }

            // Content scroll event : autoscroll eventually
            if (_autoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and autoscroll mode set
                // Autoscroll
                UnpivotScrollViewer.ScrollToVerticalOffset(UnpivotScrollViewer.ExtentHeight);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            UploadButton.IsEnabled = false;
            if (!File.Exists(ZipBox.Text))
            {
                System.Windows.Forms.MessageBox.Show("The file does not exist");
                return;
            }


            var backgroundService = ApplicationFactory.RetrieveService<BackgroundService>() as BackgroundService ?? new BackgroundService();
            var unpivotorService = ApplicationFactory.RetrieveService<UnpivotorService>() as UnpivotorService ?? new UnpivotorService();
            Directory.CreateDirectory("App_Data/uploadlogs");
            var logPath = Path.Combine("App_Data/uploadlogs", "log.txt");
            var guid = Guid.NewGuid();
            
            try
            {

                // save file to path for use later
                var outPath = Directory.CreateDirectory(Path.Combine("App_Data/uploads", guid.ToString()));
                var newPath = Path.Combine(outPath.FullName, Path.GetFileName(ZipBox.Text) ?? "sample.csv");
                File.Copy(ZipBox.Text, newPath);

                var outputPath = backgroundService.UnzipFile(newPath);

                unpivotorService.PopulateDatabaseFromUnpivotedStaarTestDirectory(outputPath, logPath);
            }
            catch (Exception ex)
            {
                unpivotorService.WriteToLogPath(logPath, ex.Message);
            }
            finally
            {
                Directory.Delete(Path.Combine("App_Data/uploads", guid.ToString()), true);
                UploadButton.IsEnabled = true;
            }
        }

        private void ViewLogFiles_ButtonClick(object sender, RoutedEventArgs e)
        {
            var logPath = Path.Combine("App_Data/uploadlogs", "log.txt");
            if (File.Exists(logPath))
            {
                Process.Start(logPath);
            }
            else
            {
                System.Windows.MessageBox.Show("No logs exist yet");
            }
        }


    }

    public class FileDataGrid
    {
        public string FullFile { get; set; }
        public string FileName { get; set; }
        public LanguageEnum FileLanguageEnum { get; set; }
        //public Grade Grade { get; set; }
        public int NumberOfColumnsAtBeginning { get; set; }
    }
}
