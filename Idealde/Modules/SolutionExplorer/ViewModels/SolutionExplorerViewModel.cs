using System;
using System.IO;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.SolutionExplorer.Models;

namespace Idealde.Modules.SolutionExplorer.ViewModels
{
    public class SolutionExplorerViewModel : Tool, ISolutionExplorer
    {
        private string _rootPath;

        public SolutionExplorerViewModel()
        {
            RootFolder = new BindableCollection<TreeViewItemModel>();
            RootPath = @"D:\A.System";
        }

        public IObservableCollection<TreeViewItemModel> RootFolder { get; set; }

        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                if (Equals(_rootPath, value)) return;
                _rootPath = value;
                RootFolder.Clear();
                RootFolder.Add(new TreeViewItemModel(_rootPath, _rootPath)
                {
                    ObjectType = DirType.Root
                });
                InitFromRootDirectory(RootFolder[0], _rootPath);
            }
        }

        public override PaneLocation PreferredLocation => PaneLocation.Right;

        public void OnExpanded()
        {
        }

        public void InitFromRootDirectory(TreeViewItemModel tItem, string path)
        {
            var current = Directory.GetDirectories(path);
            foreach (var direct in current)
            {
                var item = new TreeViewItemModel(direct, direct)
                {
                    ObjectType = DirType.FolderClosed
                };
                InitFromRootDirectory(item, direct);
                tItem.SubItems.Add(item);
            }
            foreach (var file in Directory.GetFiles(path))
            {
                var fItem = new TreeViewItemModel(file, file)
                {
                    ObjectType = DirType.File
                };
                tItem.SubItems.Add(fItem);
            }
        }
    }
}