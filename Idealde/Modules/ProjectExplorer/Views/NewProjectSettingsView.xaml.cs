#region Using Namespace

using System.Windows;
using System.Windows.Forms;

#endregion

namespace Idealde.Modules.ProjectExplorer.Views
{
    /// <summary>
    ///     Interaction logic for NewProjectSettingsView.xaml
    /// </summary>
    public partial class NewProjectSettingsView : Window
    {
        public NewProjectSettingsView()
        {
            InitializeComponent();
        }

        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog {ShowNewFolderButton = true};

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxLocation.Text = dialog.SelectedPath;
            }

            e.Handled = true;
        }
    }
}