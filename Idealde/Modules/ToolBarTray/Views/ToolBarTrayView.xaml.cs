#region Using Namespace

using System.Windows.Controls;

#endregion

namespace Idealde.Modules.ToolBarTray.Views
{
    /// <summary>
    ///     Interaction logic for ToolBarView.xaml
    /// </summary>
    public partial class ToolBarTrayView : UserControl, IToolBarView
    {
        System.Windows.Controls.ToolBarTray IToolBarView.ToolBarTray
        {
            get { return ToolBarTray; }
        }

        public ToolBarTrayView()
        {
            InitializeComponent();
        }
    }
}