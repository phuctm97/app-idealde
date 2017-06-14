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

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
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
                    ImageSource =
                        new Uri(
                            "pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/solution-explorer.png",
                            UriKind.Absolute)
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
                    ObjectType = 0,
                    ImageSource =
                        new Uri(
                            "pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/folder.png",
                            UriKind.Absolute)
                };
                InitFromRootDirectory(item, direct);
                tItem.SubItems.Add(item);
            }
            foreach (var file in Directory.GetFiles(path))
            {
                var fItem = new TreeViewItemModel(file, file)
                {
                    ObjectType = 1,
                    ImageSource =
                        new Uri("pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/file.png",
                            UriKind.Absolute)
                };
                tItem.SubItems.Add(fItem);
            }
        }
    }
}