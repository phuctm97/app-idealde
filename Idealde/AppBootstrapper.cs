#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Services;
using Idealde.Framework.Themes;
using Idealde.Modules.ErrorList;
using Idealde.Modules.ErrorList.ViewModels;
using Idealde.Modules.MainMenu;
using Idealde.Modules.MainMenu.ViewModels;
using Idealde.Modules.MainWindow.ViewModels;
using Idealde.Modules.Output;
using Idealde.Modules.Output.ViewModels;
using Idealde.Modules.Shell.ViewModels;
using Idealde.Modules.StatusBar;
using Idealde.Modules.StatusBar.ViewModels;
using Idealde.Modules.ToolBar;
using Idealde.Modules.ToolBar.ViewModels;


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

            //services
            _container.RegisterType<IThemeManager, ThemeManager>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IResourceManager, ResourceManager>(
                new ContainerControlledLifetimeManager());

            //status bar
            _container.RegisterType<IStatusBar, StatusBarViewModel>(
                new ContainerControlledLifetimeManager());

            //mennu bar
            _container.RegisterType<IMenu, MainMenuViewModel>(
                new ContainerControlledLifetimeManager());

            //tools
            _container.RegisterType<IOutput, OutputViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IErrorList, ErrorListViewModel>(
                new ContainerControlledLifetimeManager());

            //toolbar
            _container.RegisterType < IToolBar, ToolBarViewModel > (
                new ContainerControlledLifetimeManager ( ) );
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