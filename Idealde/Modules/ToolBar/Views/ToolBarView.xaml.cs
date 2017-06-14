using System.Windows.Controls;

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
