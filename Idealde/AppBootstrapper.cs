#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Framework.Services;
using Idealde.Framework.Themes;
using Idealde.Modules.Compiler;
using Idealde.Modules.Compiler.Commands;
using Idealde.Modules.CodeEditor;
using Idealde.Modules.CodeEditor.Commands;
using Idealde.Modules.CodeEditor.ViewModels;
using Idealde.Modules.Compiler.ViewModels;
using Idealde.Modules.ErrorList;
using Idealde.Modules.ErrorList.Commands;
using Idealde.Modules.ErrorList.ViewModels;
using Idealde.Modules.MainMenu;
using Idealde.Modules.MainMenu.ViewModels;
using Idealde.Modules.MainWindow.ViewModels;
using Idealde.Modules.Output;
using Idealde.Modules.Output.Commands;
using Idealde.Modules.Output.ViewModels;
using Idealde.Modules.ProjectExplorer;
using Idealde.Modules.ProjectExplorer.Commands;
using Idealde.Modules.ProjectExplorer.Providers;
using Idealde.Modules.ProjectExplorer.ViewModels;
using Idealde.Modules.Settings;
using Idealde.Modules.Shell.Commands;
using Idealde.Modules.Shell.ViewModels;
using Idealde.Modules.StatusBar;
using Idealde.Modules.StatusBar.ViewModels;
using Idealde.Modules.ToolBarTray;
using Idealde.Modules.ToolBarTray.ViewModels;
using Idealde.Modules.UndoRedo;
using Idealde.ProjectExplorers.Shell.Commands;
using Microsoft.Practices.Unity;

#endregion

namespace Idealde
{
    public class AppBootstrapper : BootstrapperBase
    {
        // Dependencies container
        private readonly IUnityContainer _container;

        public AppBootstrapper()
        {
            //initialize container
            _container = new UnityContainer();

            Initialize();
        }

        protected override void Configure()
        {
            base.Configure();

            Compose();
        }

        // Resolve dependencies
        protected virtual void Compose()
        {
            //window services
            _container.RegisterType<IWindowManager, WindowManager>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IEventAggregator, EventAggregator>(
                new ContainerControlledLifetimeManager());

            //themes
            _container.RegisterType<ITheme, BlueTheme>("Blue");

            //main window
            _container.RegisterType<IMainWindow, MainWindowViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IShell, ShellViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, ExitCommandHandler>(ExitCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, OpenFileCommandHandler>(OpenFileCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, OpenSettingsCommandHandler>(
                OpenSettingsCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());

            //services
            _container.RegisterType<IThemeManager, ThemeManager>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IResourceManager, ResourceManager>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandService, CommandService>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandRouter, CommandRouter>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IFileManager, FileManager>(
                new ContainerControlledLifetimeManager());

            //status bar
            _container.RegisterType<IStatusBar, StatusBarViewModel>(
                new ContainerControlledLifetimeManager());

            //menu bar
            _container.RegisterType<IMenu, MainMenuViewModel>(
                new ContainerControlledLifetimeManager());

            //toolbar
            _container.RegisterType<IToolBarTray, ToolBarTrayViewModel>(
                new ContainerControlledLifetimeManager());

            //output
            _container.RegisterType<IOutput, OutputViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, ViewOutputCommandHandler>(ViewOutputCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());

            //code editor
            _container.RegisterType<ICodeEditor, CodeEditorViewModel>(
                new TransientLifetimeManager());
            _container.RegisterType<ILanguageDefinitionManager, LanguageDefinitionManager>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IEditorProvider, EditorProvider>(
                EditorProvider.ProviderName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, NewCppHeaderCommandHandler>(
                NewCppHeaderCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, NewCppSourceCommandHandler>(
                NewCppSourceCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());

            //error list
            _container.RegisterType<IErrorList, ErrorListViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, ViewErrorListCommandHandler>(
                ViewErrorListCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());

            //project explorer
            _container.RegisterType<IProjectService, ProjectService>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IProjectExplorer, ProjectExplorerViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, ViewProjectExplorerCommandHandler>(
                ViewProjectExplorerCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, OpenProjectCommandHandler>(
                OpenProjectCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IProjectProvider, CppProjectProvider>( "Cpp",
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, NewCppProjectCommandHandler>(
                NewCppProjectCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());

            //undo redo
            _container.RegisterType<IUndoRedoManager, UndoRedoManager>(
                new TransientLifetimeManager());

            //compiler
            _container.RegisterType<ICompiler, CppCompiler>( "Cpp",
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, CompileProjectCommandHandler>(
                CompileProjectCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandHandler, RunProjectCommandHandler>(
                RunProjectCommandDefinition.CommandName,
                new ContainerControlledLifetimeManager());

            //settings
            _container.RegisterType<ISettingsEditor, CompilerSettingsViewModel>("Compiler",
                new TransientLifetimeManager());
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.Resolve(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            DisplayRootViewFor<IMainWindow>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);

            _container.Dispose();
        }
    }
}