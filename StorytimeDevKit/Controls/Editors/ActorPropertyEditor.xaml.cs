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
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Models;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid;
using StoryTimeCore.Entities.Actors;

namespace StoryTimeDevKit.Controls.Editors
{
    /// <summary>
    /// Interaction logic for ActorPropertyEditor.xaml
    /// </summary>
    public partial class ActorPropertyEditor : UserControl
    {
        class T : INotifyPropertyChanged 
        {
            private string _title;

            /// <summary>
            /// Gets / sets the event title
            /// </summary>
            public string Title
            {
                get { return _title; }
                set
                {
                    if (value == _title)
                        return;

                    _title = value;
                    OnPropertyChanged("Title");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        private T _t;
        private BaseActor _actor;
        public ActorPropertyEditor()
        {
            InitializeComponent();
            Actor a = new Actor();
            dynamic model = new ActorPropertyEditorModel(a);
            model.lol = "lol";
            //var model = new CustomObjectType
            //{
            //    Name = "Foo",
            //    Properties =
            //    {
            //        new CustomProperty { Name = "Bar", Type = typeof(int), Desc = "I'm a bar"},
            //        new CustomProperty { Name = "When", Type = typeof(DateTime), Desc = "When it happened"},
            //    }
            //};

            propertyGrid1.SelectedObject = model;
            //_t = new T() { Title = "lol"};
            //propertyGrid1.SelectedObject = _t;
        }

        public ActorPropertyEditor(BaseActor actor)
            :this()
        {
            _actor = actor;
        }
    }
}
