using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace StoryTimeDevKit.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ParticleEditorDialog.xaml
    /// </summary>
    public partial class ParticleEditorDialog : Window
    {
        private StandardKernel _particleEditorContainer;

        public ParticleEditorDialog()
        {
            _particleEditorContainer = new StandardKernel();
            NinjectBootstrap.ParticleEditorConfigure(_particleEditorContainer);
            Application.Current.Properties[ApplicationProperties.ParticleEditorInjectorKey] = _particleEditorContainer;

            Closing += ClosingHandler;

            InitializeComponent();
        }

        private void ClosingHandler(object sender, CancelEventArgs e)
        {
            _particleEditorContainer.Dispose();
        }
    }
}
