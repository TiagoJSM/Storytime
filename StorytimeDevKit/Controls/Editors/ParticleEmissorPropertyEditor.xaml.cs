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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StoryTimeDevKit.Models;

namespace StoryTimeDevKit.Controls.Editors
{
    /// <summary>
    /// Interaction logic for ParticleEmissorPropertyEditor.xaml
    /// </summary>
    public partial class ParticleEmissorPropertyEditor : UserControl, IParticleEmissorPropertyEditor
    {
        private PropertyEditorModel _model;
        private object _selected;

        public object Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                _model.Data = _selected;
            }
        }

        public ParticleEmissorPropertyEditor()
        {
            InitializeComponent();
            _model = new PropertyEditorModel();
            propertyGrid.SelectedObject = _model;
        }
    }
}
