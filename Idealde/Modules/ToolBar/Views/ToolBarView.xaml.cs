using System.Windows.Controls;

using Idealde.Modules.ToolBar.Model;
using Idealde.Modules.ToolBar.ViewModels;

namespace Idealde.Modules.ToolBar.Views
{
    /// <summary>
    /// Interaction logic for ToolBarView.xaml
    /// </summary>
    public partial class ToolBarView : UserControl,IToolBarView
    {
        ToolBarTray IToolBarView.ToolBarTray
        {
            get
            {
                return ToolBarTray;
            }
        }

        public ToolBarView()
        {
            InitializeComponent();
        }

    }
}
