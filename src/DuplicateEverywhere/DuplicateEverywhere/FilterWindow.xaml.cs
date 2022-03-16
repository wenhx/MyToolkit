using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DuplicateEverywhere
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();
            this.Loaded += FilterWindowLoaded;
        }

        private void FilterWindowLoaded(object sender, RoutedEventArgs e)
        {
            UI_Textbox_IgnorePaths.Text = string.Join(Environment.NewLine, Filter.Current.IgnorePaths);
            UI_Textbox_HighlightPaths.Text = string.Join(Environment.NewLine, Filter.Current.HighlightPaths);
            UI_CheckBox_OnlyShow1ResultRows.IsChecked = Filter.Current.OnlyShow1ResultRows;
            UI_CheckBox_NotShowResultsContainHighlighPaths.IsChecked = Filter.Current.NotShowResultsContainHighlightPaths;
            UI_CheckBox_SameSize.IsChecked = Filter.Current.SameSize;
        }

        private void UI_Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Filter.Current.IgnorePaths.Clear();
            Array.ForEach(UI_Textbox_IgnorePaths.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                line => Filter.Current.IgnorePaths.Add(line));

            Filter.Current.HighlightPaths.Clear();
            Array.ForEach(UI_Textbox_HighlightPaths.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                line => Filter.Current.HighlightPaths.Add(line));

            Filter.Current.OnlyShow1ResultRows = UI_CheckBox_OnlyShow1ResultRows.IsChecked == true;
            Filter.Current.NotShowResultsContainHighlightPaths = UI_CheckBox_NotShowResultsContainHighlighPaths.IsChecked == true;
            Filter.Current.SameSize = UI_CheckBox_SameSize.IsChecked == true;
            this.Close();
        }

        private void UI_Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
