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
using System.ComponentModel;
using StoryTime;
using StoryTimeCore.Contexts.Interfaces;

namespace StoryTimeDevKit.Controls.Animation
{
    /// <summary>
    /// Interaction logic for PupeteerEditor.xaml
    /// </summary>
    public partial class PuppeteerEditorControl : UserControl
    {
        private MyGame _game;
        private IGraphicsContext _context;

        public PuppeteerEditorControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            _game = new MyGame(PuppeteerEditor.Handle);
            _context = _game.GraphicsContext;
        }
    }
}
