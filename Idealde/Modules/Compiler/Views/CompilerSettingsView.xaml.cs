using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Win32;

namespace Idealde.Modules.Compiler.Views
{
    /// <summary>
    /// Interaction logic for CompilerSettingsView.xaml
    /// </summary>
    public partial class CompilerSettingsView : UserControl
    {
        public CompilerSettingsView()
        {
            InitializeComponent();
        }

        private void OnButtonBrowseClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog {Multiselect = false};
            dialog.Filter = "BAT Files|*.bat";

            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                TextBoxPath.Text = dialog.FileName;
            }
        }
    }
}
