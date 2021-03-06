﻿#region Using Namespace

using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Framework.Themes;
using Idealde.Modules.Compiler.Commands;
using Idealde.Modules.CodeEditor.Commands;
using Idealde.Modules.ErrorList.Commands;
using Idealde.Modules.MainMenu;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.Output.Commands;
using Idealde.Modules.ProjectExplorer.Commands;
using Idealde.Modules.Shell.Commands;
using Idealde.Modules.StatusBar;
using Idealde.Modules.ToolBarTray;
using Idealde.Modules.ToolBarTray.Models;
using Idealde.Modules.UndoRedo.Commands;
using Idealde.ProjectExplorers.Shell.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        // Dependencies
        private readonly IThemeManager _themeManager;

        // Backing fields

        #region Backing fields

        private bool _closing;
        private ILayoutItem _activeLayoutItem;

        #endregion

        // Bind models

        #region Bind models

        public IMenu MainMenu { get; }

        public IToolBarTray ToolBarTray { get; }

        public IStatusBar StatusBar { get; }

        public ILayoutItem ActiveLayoutItem
        {
            get { return _activeLayoutItem; }
            set
            {
                if (Equals(value, _activeLayoutItem)) return;
                _activeLayoutItem = value;

                if (_activeLayoutItem is IDocument)
                {
                    OpenDocument((IDocument) _activeLayoutItem);
                }
                if (_activeLayoutItem is ITool)
                {
                    ShowTool((ITool) _activeLayoutItem);
                }
            }
        }

        public IObservableCollection<IDocument> Documents => Items;

        public IObservableCollection<ITool> Tools { get; }

        #endregion

        // Initializations

        #region Initializations

        public ShellViewModel(IThemeManager themeManager, IMenu mainMenu, IToolBarTray toolBarTray, IStatusBar statusBar)
        {
            _themeManager = themeManager;

            MainMenu = mainMenu;

            StatusBar = statusBar;

            ToolBarTray = toolBarTray;

            Tools = new BindableCollection<ITool>();

            _closing = false;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            BuildMenu();

            BuildToolBarTray();

            BuildStatusBar();

            LoadDefaultDocuments();

            LoadDefaultTools();
        }

        protected override void OnViewLoaded(object view)
        {
            if (_themeManager.CurrentTheme == null)
            {
                _themeManager.SetCurrentTheme("Blue");
            }

            base.OnViewLoaded(view);
        }

        private void BuildMenu()
        {
            //< File menu
            var fileMenu = new Menu("File", Resources.FileMenuText);
            MainMenu.AddMenu(fileMenu);

            var fileNewMenu = new DisplayMenuItem("File.New", Resources.FileNewMenuText);
            var fileOpenMenu = new DisplayMenuItem("File.Open", Resources.FileOpenMenuText);
            var fileCloseMenu = new CommandMenuItem<CloseFileCommandDefinition>("File.Close");
            var fileSaveMenu = new CommandMenuItem<SaveFileCommandDefinition>("File.Save");
            var fileSaveAsMenu = new CommandMenuItem<SaveFileAsCommandDefinition>("File.SaveAs");
            var fileExitMenu = new CommandMenuItem<ExitCommandDefinition>("File.Exit");
            MainMenu.AddMenuItem(fileMenu,
                fileNewMenu, fileOpenMenu,
                new MenuItemSeparator("File.S1"),
                fileCloseMenu,
                new MenuItemSeparator("File.S2"),
                fileSaveMenu, fileSaveAsMenu,
                new MenuItemSeparator("File.S3"),
                fileExitMenu);

            //< File.New menu
            var fileNewCppProjectMenu = new CommandMenuItem<NewCppProjectCommandDefinition>("File.New.CppProject");
            var fileNewCppHeaderMenu = new CommandMenuItem<NewCppHeaderCommandDefinition>("File.New.CppHeader");
            var fileNewCppSourceMenu = new CommandMenuItem<NewCppSourceCommandDefinition>("File.New.CppSource");
            MainMenu.AddMenuItem(fileNewMenu,
                fileNewCppProjectMenu,
                new MenuItemSeparator("File.New.S1"),
                fileNewCppHeaderMenu,
                fileNewCppSourceMenu);
            //> File.New menu

            //< File.Open menu
            var fileOpenProjectMenu = new CommandMenuItem<OpenProjectCommandDefinition>("File.Open.Project");
            var fileOpenFileMenu = new CommandMenuItem<OpenFileCommandDefinition>("File.Open.File");
            MainMenu.AddMenuItem(fileOpenMenu, fileOpenFileMenu, fileOpenProjectMenu);
            //> File.Open menu

            //> File menu

            //< Edit menu
            var editMenu = new Menu("Edit", Resources.EditMenuText);
            MainMenu.AddMenu(editMenu);

            var editUndoMenu = new CommandMenuItem<UndoCommandDefinition>("Edit.Undo");
            var editRedoMenu = new CommandMenuItem<RedoCommandDefinition>("Edit.Redo");
            var editCutMenu = new DisplayMenuItem("Edit.Cut", Resources.EditCutCommandText);
            var editCopyMenu = new DisplayMenuItem("Edit.Copy", Resources.EditCopyCommandText);
            var editPasteMenu = new DisplayMenuItem("Edit.Paste", Resources.EditPasteCommandText);
            var editSelectAllMenu = new DisplayMenuItem("Edit.SelectAll", Resources.EditSelectAllCommandText);
            var editGotoMenu = new DisplayMenuItem("Edit.Goto", Resources.EditGotoCommadText);
            var editFindAndReplaceMenu = new DisplayMenuItem("Edit.FindAndReplace",
                Resources.EditFindAndReplaceCommandText);

            MainMenu.AddMenuItem(editMenu,
                editUndoMenu,
                editRedoMenu,
                new MenuItemSeparator("Edit.S1"),
                editCutMenu,
                editCopyMenu,
                editPasteMenu,
                new MenuItemSeparator("Edit.S2"),
                editSelectAllMenu,
                new MenuItemSeparator("Edit.S3"),
                editGotoMenu,
                editFindAndReplaceMenu);
            //> Edit menu

            //< View menu
            var viewMenu = new Menu("View", Resources.ViewMenuText);
            MainMenu.AddMenu(viewMenu);

            var viewProjectExplorer = new CommandMenuItem<ViewProjectExplorerCommandDefinition>("View.ProjectExplorer");
            var viewOutputMenu = new CommandMenuItem<ViewOutputCommandDefinition>("View.Output");
            var viewErrorListMenu =
                new CommandMenuItem<ViewErrorListCommandDefinition>("View.ErrorList");
            MainMenu.AddMenuItem(viewMenu, viewProjectExplorer, viewOutputMenu, viewErrorListMenu);
            //> View menu

            //< Run menu
            var runMenu = new Menu("Run", Resources.RunMenuText);
            MainMenu.AddMenu(runMenu);

            var runCompileSingleFileMenu =
                new CommandMenuItem<CompileSingleFileCommandDefinition>("Run.CompileSingleFile");
            var runRunSingleFileMenu =
                new CommandMenuItem<RunSingleFileCommandDefinition>("Run.RunSingleFile");
            var runCompileAndRunSingleFileMenu =
                new CommandMenuItem<CompileAndRunSingleFileCommandDefinition>("Run.CompileAndRunSingleFile");
            var runCompileProjectMenu = new CommandMenuItem<CompileProjectCommandDefinition>("Run.CompileProject");
            var runRunProjectMenu = new CommandMenuItem<RunProjectCommandDefinition>("Run.RunProject");

            MainMenu.AddMenuItem(runMenu,
                runCompileSingleFileMenu,
                runRunSingleFileMenu,
                runCompileAndRunSingleFileMenu,
                new MenuItemSeparator("Run.S1"),
                runCompileProjectMenu,
                runRunProjectMenu);
            //> Run menu

            //< Tools menu
            var toolsMenu = new Menu("Tool", Resources.ToolsMenuText);
            MainMenu.AddMenu(toolsMenu);

            var toolsSettingsMenu = new CommandMenuItem<OpenSettingsCommandDefinition>("Tool.Settings");
            MainMenu.AddMenuItem(toolsMenu, toolsSettingsMenu);
            //> Tools menu
        }

        private void BuildToolBarTray()
        {
            //< File tool bar
            var fileToolBar = new ToolBar("File");
            ToolBarTray.AddToolBar(fileToolBar);

            var fileNewCppProjectToolBarItem =
                new CommandToolBarItem<NewCppProjectCommandDefinition>("File.NewCppProject", true);
            var fileNewCppHeaderToolBarItem
                = new CommandToolBarItem<NewCppHeaderCommandDefinition>("File.NewCppHeader", true);
            var fileNewCppSourceToolBarItem
                = new CommandToolBarItem<NewCppSourceCommandDefinition>("File.NewCppSource", true);
            var fileOpenFileToolBarItem = new CommandToolBarItem<OpenFileCommandDefinition>("File.OpenFile");
            var fileOpenProjectToolBarItme = new CommandToolBarItem<OpenProjectCommandDefinition>("File.OpenProject");
            var fileSaveToolBarItem = new CommandToolBarItem<SaveFileCommandDefinition>("File.Save");
            var fileSaveAsToolBarItem = new CommandToolBarItem<SaveFileAsCommandDefinition>("File.SaveAs");
            ToolBarTray.AddToolBarItem(fileToolBar,
                fileNewCppProjectToolBarItem,
                fileNewCppHeaderToolBarItem,
                fileNewCppSourceToolBarItem,
                new ToolBarItemSeparator("File.S1"),
                fileOpenFileToolBarItem,
                fileOpenProjectToolBarItme,
                fileSaveToolBarItem,
                fileSaveAsToolBarItem);
            //> File tool bar

            //< Run menu
            var runToolBar = new ToolBar("Run");
            ToolBarTray.AddToolBar(runToolBar);

            var runCompileSingleFileToolBarItem =
                new CommandToolBarItem<CompileSingleFileCommandDefinition>("Run.CompileSingleFile");
            var runRunSingleFileToolBar =
                new CommandToolBarItem<RunSingleFileCommandDefinition>("Run.RunSingleFile");
            var runCompileProjectToolBarItem =
                new CommandToolBarItem<CompileProjectCommandDefinition>("Run.CompileProject");
            var runRunProjectToolBarItem =
                new CommandToolBarItem<RunProjectCommandDefinition>("Run.RunProject");
            ToolBarTray.AddToolBarItem(runToolBar,
                runCompileSingleFileToolBarItem,
                runRunSingleFileToolBar,
                new ToolBarItemSeparator("Run.S1"),
                runCompileProjectToolBarItem,
                runRunProjectToolBarItem);
            //> Run menu
        }

        private void BuildStatusBar()
        {
            StatusBar.AddItem("Ready", new GridLength(1, GridUnitType.Auto));
        }

        private void LoadDefaultDocuments()
        {
        }

        private void LoadDefaultTools()
        {
        }

        #endregion

        // Item actions

        #region Item actions

        public override void ActivateItem(IDocument item)
        {
            //bug: complex bug, temporary solution
            if (_closing) return;

            base.ActivateItem(item);
        }

        protected override void OnActivationProcessed(IDocument item, bool success)
        {
            base.OnActivationProcessed(item, success);

            if (!success) return;

            _activeLayoutItem = item;
            NotifyOfPropertyChange(() => ActiveLayoutItem);
        }

        public void OpenDocument(IDocument document)
        {
            ActivateItem(document);
        }

        public void CloseDocument(IDocument document)
        {
            DeactivateItem(document, true);
        }

        public void ShowTool(ITool tool)
        {
            if (!Tools.Contains(tool))
            {
                Tools.Add(tool);
            }
            tool.IsVisible = true;
            tool.IsSelected = true;

            _activeLayoutItem = tool;
            NotifyOfPropertyChange(() => ActiveLayoutItem);
        }

        public void ShowTool<TTool>() where TTool : ITool
        {
            ShowTool(IoC.Get<TTool>());
        }

        #endregion

        // Shell behaviors

        #region Shell behaviors

        protected override void OnDeactivate(bool close)
        {
            //bug: complex bug, temporary solution
            _closing = close;

            base.OnDeactivate(close);
        }

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }

        #endregion
    }
}