using System;
using System.Drawing;
using Caliburn.Micro;

namespace Idealde.Modules.SolutionExplorer.Models
{
    public enum DirType
    {
        Folder = 0,
        File = 1
    }
    public class TreeViewItemModel
    {
        public string Name { get; }
        public string Path { get; }
        public int ObjectType { get; set; }
        public Uri ImageSource { get; set; }
        public IObservableCollection<TreeViewItemModel> SubItems { get; set; }

        public TreeViewItemModel(string name, string path)
        {
            SubItems = new BindableCollection<TreeViewItemModel>();
            Name = name.Substring(name.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            Path = path; 
        }
    }
}
