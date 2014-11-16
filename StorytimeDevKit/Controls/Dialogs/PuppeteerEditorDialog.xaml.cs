using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ninject;
using StoryTimeDevKit.Bootstraps;
using StoryTimeDevKit.Configurations;
using System.ComponentModel;

namespace StoryTimeDevKit.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for PuppeteerEditorDialog.xaml
    /// </summary>
    public partial class PuppeteerEditorDialog : Window
    {
        private StandardKernel _puppeteerContainer;

        public PuppeteerEditorDialog()
        {
            _puppeteerContainer = new StandardKernel();
            NinjectBootstrap.PuppeteerConfigure(_puppeteerContainer);
            Application.Current.Properties[ApplicationProperties.PuppeteerDependencyInjectorKey] = _puppeteerContainer;

            Closing += ClosingHandler;

            InitializeComponent();
        }

        private void ClosingHandler(object sender, CancelEventArgs e)
        {
            _puppeteerContainer.Dispose();
        }
    }
}
