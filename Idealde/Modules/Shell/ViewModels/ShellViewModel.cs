#region Using Namespace

using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Framework.Themes;
using Idealde.Modules.CodeEditor.ViewModels;
using Idealde.Modules.ErrorList;
using Idealde.Modules.ErrorList.Commands;
using Idealde.Modules.MainMenu;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.Output;
using Idealde.Modules.Shell.Commands;
using Idealde.Modules.StatusBar;
using Idealde.Modules.ToolBar;
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

        public IToolBar ToolBar { get; }

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
                NotifyOfPropertyChange(() => ActiveLayoutItem);
            }
        }

        public IObservableCollection<IDocument> Documents => Items;

        public IObservableCollection<ITool> Tools { get; }

        #endregion

        // Initializations

        #region Initializations

        public ShellViewModel(IThemeManager themeManager, IMenu mainMenu, IToolBar toolBar, IStatusBar statusBar)
        {
            _themeManager = themeManager;

            MainMenu = mainMenu;

            StatusBar = statusBar;

            ToolBar = toolBar;

            Tools = new BindableCollection<ITool>();

            _closing = false;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            BuildMenu();

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
            // File menu
            var fileMenu = new Menu("File", Resources.FileMenuText);
            MainMenu.AddMenu(fileMenu);

            var fileNewMenu = new DisplayMenuItem("File.New", Resources.FileNewMenuText);
            var fileOpenMenu = new DisplayMenuItem("File.Open", Resources.FileOpenMenuText);
            var fileSaveMenu = new DisplayMenuItem("File.Save", Resources.FileSaveMenuText);
            var fileSaveAsMenu = new DisplayMenuItem("File.SaveAs", Resources.FileSaveAsMenuText);
            var fileExitMenu = new CommandMenuItem<ExitCommandDefinition>("File.Exit");
            MainMenu.AddMenuItem(fileMenu, fileNewMenu, fileOpenMenu, fileSaveMenu, fileSaveAsMenu, fileExitMenu);

            // File.New menu
            var fileNewCppHeaderMenu = new DisplayMenuItem("File.New.CppHeader", Resources.FileNewCppHeaderMenuText);
            var fileNewCppSourceMenu = new DisplayMenuItem("File.New.CppSource", Resources.FileNewCppSourceMenuText);
            MainMenu.AddMenuItem(fileNewMenu, fileNewCppHeaderMenu, fileNewCppSourceMenu);

            // View menu
            var viewMenu = new Menu("View", Resources.ViewMenuText);
            MainMenu.AddMenu(viewMenu);

            var viewOutputMenu = new DisplayMenuItem("View.Output", Resources.ViewOutputMenuText);
            var viewErrorListMenu =
                new CommandMenuItem<ViewErrorListCommandDefinition>("View.ErrorList");
            MainMenu.AddMenuItem(viewMenu, viewOutputMenu, viewErrorListMenu);
        }

        private void BuildStatusBar()
        {
            StatusBar.AddItem("Ready", new GridLength(1, GridUnitType.Auto));
        }

        private void LoadDefaultDocuments()
        {
            OpenDocument(new CodeEditorViewModel());
        }

        private void LoadDefaultTools()
        {
            ShowTool(IoC.Get<IOutput>());

            ShowTool(IoC.Get<IErrorList>());
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

            if (Equals(item, _activeLayoutItem)) return;

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
            NotifyOfPropertyChange(() => Tools);

            if (Equals(tool, _activeLayoutItem)) return;
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