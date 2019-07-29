using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using System.Windows;

namespace WebApi_client
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static IWindsorContainer _container;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new WindsorContainer();
            _container.Install(new DependencyInstaller());

            var mainWindow = new MainWindow();
            var mvView = _container.Resolve<IViewModel>();
            mainWindow.Show();
        }
    }

    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                            .BasedOn(typeof(IWebHooker))
                            .WithServiceAllInterfaces());
            container.Register(Classes.FromThisAssembly()
                            .BasedOn(typeof(IJsonParser))
                            .WithServiceAllInterfaces());
            container.Register(Classes.FromThisAssembly()
                            .BasedOn(typeof(IViewModel))
                            .WithServiceAllInterfaces());
        }
    }

}
