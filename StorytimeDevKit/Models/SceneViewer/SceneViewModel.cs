using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using System.ComponentModel;

namespace StoryTimeDevKit.Models.SceneViewer
{
    public class SceneTabViewModel : INotifyPropertyChanged
    {
        public Scene Scene { get; private set; }

        public string SceneName
        {
            get { return Scene.SceneName; }
            set
            {
                Scene.SceneName = value;
                OnPropertyChanged("SceneName");
            }
        }

        public SceneTabViewModel(Scene scene)
        {
            Scene = scene;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
