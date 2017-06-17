using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Idealde.Modules.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer.Views
{
    /// <summary>
    /// Interaction logic for SolutionExplorerView.xaml
    /// </summary>
    public partial class ProjectExplorerView : UserControl, IProjectExplorerView
    {
        public ProjectExplorerView()
        {
            InitializeComponent();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            var projectItem = item?.DataContext as ProjectItemBase;
            if (projectItem?.ActiveCommand == null) return;
            if (!projectItem.ActiveCommand.CanExecute(projectItem)) return;
            projectItem.ActiveCommand.Execute(projectItem);
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            item?.Focus();
        }
    }
}
