using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DuplicateEverywhere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly string[] _MouseDoubleClickOpenFileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".mp4", ".mov" };
        string m_ProductName = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;
        BackgroundWorker m_Worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            this.m_Worker.WorkerSupportsCancellation = true;
            this.Closing += MainWindowClosing;
            this.Title = m_ProductName;

            Filter.Current.HighlightPaths.Add(@"\OneDrive\");

            Filter.Current.IgnorePaths.Add(@"AppData\Roaming\Microsoft\Windows\Recent");
            Filter.Current.IgnorePaths.Add(@"\$RECYCLE.BIN\");
        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            m_Worker.CancelAsync();
        }

        private void UI_Button_Browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    UI_TextBox_Path.Text = dialog.SelectedPath;
                }
            }
        }

        private void UI_MenuItem_Help_About_Click(object sender, RoutedEventArgs e)
        {
            var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var author = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            var product = m_ProductName;
            var content = @$"{product} {version}
Copyright ©2022 {author}.
All Rights Reserved.";
            MessageBox.Show(content, $"About {product}");
        }

        private void UI_MenuItem_File_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UI_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = UI_ListView.SelectedItem as EverythingSearchedResult;
            if (item == null)
                return;
            if (string.IsNullOrWhiteSpace(item.Name))
                return;
            if (_MouseDoubleClickOpenFileExtentions.Any(e => item.Name.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
            {
                var file = System.IO.Path.Combine(item.Path, item.Name);
                if (File.Exists(file))
                {
                    Process.Start(new ProcessStartInfo(file) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("file does not exist.");
                }
            }
        }

        private void UI_Button_Run_Click(object sender, RoutedEventArgs e)
        {
            if ((string)UI_Button_Run.Content == "Stop")
            {
                m_Worker.CancelAsync();
                return;
            }

            var path = UI_TextBox_Path.Text;
            var message = string.Empty;
            if (string.IsNullOrWhiteSpace(path))
            {
                message = "Please select a folder before click Run button.";
            }
            if (!Directory.Exists(path))
            {
                message = "The folder doesnot exists.";
            }
            if (message != string.Empty)
            {
                MessageBox.Show(message, $"Something wrong.");
                return;
            }
            SetProgressBarValue(0);
            UI_Button_Run.Content = "Stop";
            UI_ListView.Items.Clear();
            m_Worker.DoWork += DoSearch;
            m_Worker.RunWorkerCompleted += RunWorkerCompleted;
            m_Worker.RunWorkerAsync(path);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_Worker.DoWork -= DoSearch;
            m_Worker.RunWorkerCompleted -= RunWorkerCompleted;
            UI_Button_Run.Content = "Run";
        }

        private void DoSearch(object sender, DoWorkEventArgs e)
        {
            var path = (string)e.Argument;
            var files = Directory.GetFiles(path).ToList();
            UI_ProgressBar.Dispatcher.Invoke(() =>
            {
                UI_ProgressBar.Maximum = files.Count;
            });
            foreach (var f in files)
            {
                var file = new FileInfo(f);
                var results = Everything.Search(file.Name, file.Length);
                if (results == null) //Error
                    return;

                SetProgressBarValue();
                if (Filter.Current.OnlyShow1ResultRows && results.Count > 0 && results[0].Total > 1)
                    continue;

                UI_ListView.Dispatcher.InvokeAsync(() =>
                {
                    foreach (var result in results)
                    {
                        UI_ListView.Items.Add(result);
                    }
                });
                if (m_Worker.CancellationPending)
                    break;
            }
        }

        private void SetProgressBarValue(int? value = null)
        {
            UI_ProgressBar.Dispatcher.Invoke(() =>
            {
                if (value == null)
                {
                    UI_ProgressBar.Value++;
                }
                else
                {
                    UI_ProgressBar.Value = value.Value;
                }
            });
        }

        private void UI_Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow window = new FilterWindow { ShowInTaskbar = false };
            window.ShowDialog();
        }

        private void UI_TextBox_Path_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UI_TextBox_Path.SelectAll();
        }
    }
}
