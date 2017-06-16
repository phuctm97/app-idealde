namespace Idealde.Modules.ProjectExplorer
{
    public delegate void TreeViewItemExpandedEventHandler(string path);
    internal interface IProjectExplorerView
    {
        event TreeViewItemExpandedEventHandler TreeViewItemExpanded;
    }
}
