using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using StoryTimeDevKit.Exceptions;
using Ninject;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeDevKit.Bootstraps;
using StoryTimeDevKit.Configurations;

namespace StoryTimeDevKit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            _container = new StandardKernel();
            NinjectBootstrap.Configure(_container);
            Properties[ApplicationProperties.DependencyInjectorKey] = _container;
            base.OnStartup(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is ApplicationBaseException)
            {
                ApplicationBaseException ex = e.Exception as ApplicationBaseException;
                MessageBox.Show(ex.UserText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            
        }
    }
}
