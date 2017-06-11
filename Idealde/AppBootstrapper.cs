#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Services;
using Idealde.Modules.MainWindow.ViewModels;
using Idealde.Modules.Shell.ViewModels;
using Idealde.Modules.StatusBar;
using Idealde.Modules.StatusBar.ViewModels;
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

            //main window
            _container.RegisterType<IMainWindow, MainWindowViewModel>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IShell, ShellViewModel>(
                new ContainerControlledLifetimeManager());

            //status bar
            _container.RegisterType<IStatusBar, StatusBarViewModel>(
                new ContainerControlledLifetimeManager());
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