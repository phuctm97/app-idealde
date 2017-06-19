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
using System.Windows.Shapes;

namespace Idealde.Modules.ProjectExplorer.Views
{
    /// <summary>
    /// Interaction logic for NewItemView.xaml
    /// </summary>
    public partial class NewItemView : Window
    {
        public NewItemView()
        {
            InitializeComponent();
        }

        private void OnNewItemLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Focus();
        }
    }
}
