﻿using System;
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
        private IKernel _mainWindowContainer;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindowContainer = new StandardKernel();
            NinjectBootstrap.MainWindowConfigure(_mainWindowContainer);
            Properties[ApplicationProperties.MainWindowDependencyInjectorKey] = _mainWindowContainer;
            base.OnStartup(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is ApplicationBaseException)
            {
                var ex = e.Exception as ApplicationBaseException;
                MessageBox.Show(ex.UserText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            
        }
    }
}
