﻿#region Using Namespace

using System.IO;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer
    {
        public override PaneLocation PreferredLocation => PaneLocation.Right;

        private string _rootPath;

        public ProjectExplorerViewModel()
        {
            DisplayName = "Solution Explorer";
            RootFolder = new BindableCollection<TreeViewItemModel>();
            RootPath = @"D:\A.System";
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            //// Test settings window
            //IWindowManager manager = new WindowManager();
            //manager.ShowDialog(new SettingsViewModel(), null, null);
        }

        public IObservableCollection<TreeViewItemModel> RootFolder { get; set; }

        // Root folder ( to update: one root to many roots )
        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                if (_rootPath == string.Empty && _rootPath == null || !Directory.Exists(_rootPath)) return;
                if (Equals(_rootPath, value)) return;
                _rootPath = value;
                RootFolder.Clear();
                RootFolder.Add(new TreeViewItemModel(_rootPath, _rootPath)
                {
                    ObjectType = DirType.Root
                });
                // get all content of root folder
                InitFromRootDirectory(RootFolder[0], _rootPath);
            }
        }


        // get all folders and files in a folder
        public void InitFromRootDirectory(TreeViewItemModel tItem, string path)
        {
            var current = Directory.GetDirectories(path);
            // get all folder in path
            foreach (var direct in current)
            {
                var item = new TreeViewItemModel(direct, direct)
                {
                    ObjectType = DirType.FolderClosed
                };
                // Recursive
                InitFromRootDirectory(item, direct);
                tItem.SubItems.Add(item);
            }
            // get all file in path
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