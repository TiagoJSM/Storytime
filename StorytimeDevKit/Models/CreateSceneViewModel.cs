using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace StoryTimeDevKit.Models
{
    public class CreateSceneViewModel : INotifyPropertyChanged
    {
        private string _sceneName;

        public string SceneName
        { 
            get 
            {
                return _sceneName;
            }
            set 
            {
                if (_sceneName != value)
                {
                    _sceneName = value;
                    OnPropertyChanged("SceneName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
