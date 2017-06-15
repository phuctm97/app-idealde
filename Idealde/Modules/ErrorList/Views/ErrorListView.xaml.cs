#region Using Namespace

using System;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace Idealde.Modules.ErrorList.Views
{
    /// <summary>
    ///     Interaction logic for ErrorListView.xaml
    /// </summary>
    public partial class ErrorListView : UserControl, IErrorListView
    {
        public ErrorListView()
        {
            InitializeComponent();
        }

        private void OnErrorRowMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row == null) return;

            OnErrorItemQuery?.Invoke(row, (ErrorListItem) row.DataContext);
        }

        public event EventHandler<ErrorListItem> OnErrorItemQuery;
    }
}