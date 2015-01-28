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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ninject;
using ParticleEngine;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Utils;

namespace StoryTimeDevKit.Controls.Editors
{
    /// <summary>
    /// Interaction logic for ParticleEmissorPropertyEditor.xaml
    /// </summary>
    public partial class ParticleEmitterPropertyEditor : UserControl, IParticleEmitterPropertyEditor
    {
        private PropertyEditorModel _model;
        private object _selected;
        private IParticleEmitterPropertyEditorController _controller;

        public object Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                _model = new PropertyEditorModel(_selected);
                propertyGrid.SelectedObject = _model;
            }
        }

        public ParticleEmitterPropertyEditor()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _controller =
                DependencyInjectorHelper
                    .ParticleEditorKernel
                    .Get<IParticleEmitterPropertyEditorController>();
            _controller.ParticleEmitterPropertyEditor = this;
        }
    }
}
