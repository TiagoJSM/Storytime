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

namespace StoryTimeDevKit.Controls.GameObjects
{
    /// <summary>
    /// Interaction logic for ActorTreeViewItem.xaml
    /// </summary>
    public partial class ActorTreeViewItem : UserControl
    {
        private GameObjectsActorModel _model;

        public ActorTreeViewItem()
        {
            InitializeComponent();
        }

        public ActorTreeViewItem(GameObjectsActorModel model)
            :this()
        {
            _model = model;
            this.Name.Text = model.ActorName;
        }
    }
}
