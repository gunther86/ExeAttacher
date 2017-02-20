using Caliburn.Micro;
using ExeAttacher.Core.Injection;
using ExeAttacher.Core.LightInject;
using ExeAttacher.Core.UI;
using ExeAttacher.UI.ViewModels;
using ExeAttacher.UI.ViewModels.Interfaces;
using ExeAttacher.UI.Views;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace ExeAttacher.UI
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            this.Initialize();
        }

        private IInjectionContainer Container { get; set; }

        protected override void Configure()
        {
            this.Container = new LightInjectContainer();
            this.Container.RegisterInstance<IInjectionContainer>(this.Container);
            this.Container.Register("ExeAttacher.Services.AttachService.*");
            this.Container.Register("ExeAttacher.Services.FileServices.*");
            this.Container.Register<IWindowService, WindowService>(Scope.Singleton);
            this.Container.Register<IWindowManager, WindowManager>(Scope.Singleton);
            this.Container.Register<IMainViewModel, MainViewModel>(Scope.Singleton);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return this.Container.GetInstance(service);
            }
            else
            {
                return this.Container.GetInstance(service, key);
            }
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<IMainViewModel>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies = new List<Assembly>(base.SelectAssemblies());

            assemblies.Add(typeof(MainViewModel).Assembly);
            assemblies.Add(typeof(MainView).Assembly);

            return assemblies;
        }
    }
}