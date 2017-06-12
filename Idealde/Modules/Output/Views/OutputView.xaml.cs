#region Using Namespace

using System.Windows.Controls;

#endregion

namespace Idealde.Modules.Output.Views
{
    /// <summary>
    ///     Interaction logic for OutputView.xaml
    /// </summary>
    public partial class OutputView : UserControl, IOutputView
    {
        public OutputView()
        {
            InitializeComponent();
        }

        // Output view behaviors
        #region Output view behaviors
        public void SetText(string text)
        {
            TextBox.Text = text;
            ScrollToEnd();
        }

        public void ScrollToEnd()
        {
            TextBox.ScrollToEnd();
        }

        public void Clear()
        {
            TextBox.Clear();
        } 
        #endregion
    }
}