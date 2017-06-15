namespace Idealde.Modules.SolutionExplorer
{
    public delegate void TreeViewItemExpandedEventHandler(string path);
    internal interface ISolutionExplorerView
    {
        event TreeViewItemExpandedEventHandler TreeViewItemExpanded;
    }
}
