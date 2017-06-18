using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Idealde.Modules.ProjectExplorer.Views
{
    /// <summary>
    /// Interaction logic for NewProjectSettingsView.xaml
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
