using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;

namespace StoryTimeDevKit.Models.MainWindow
{
    public class ActorWidgetAdapterViewModel : BaseViewModel
    {
        private ActorWidgetAdapter _adapter;
        private WidgetMode _widgetMode;

        public ActorWidgetAdapter ActorWidgetAdapter
        {
            get
            {
                return _adapter;
            }
            set
            {
                if (_adapter == value) return;
                if(_adapter != null)_adapter.OnWidgetModeChange -= OnWidgetModeChangeHandler;
                
                _adapter = value;
                WidgetMode = _adapter.WidgetMode;
                _adapter.OnWidgetModeChange += OnWidgetModeChangeHandler;
                OnPropertyChanged("HasActor");
            }
        }
        public WidgetMode WidgetMode 
        {
            get
            {
                return _widgetMode;
            }
            set
            {
                if (_widgetMode == value) return;
                
                _widgetMode = value;
                OnPropertyChanged("WidgetMode");

                if (_adapter != null)
                    _adapter.WidgetMode = value;
            }
        }
        public bool HasActor
        {
            get { return _adapter != null; }
        }

        private void OnWidgetModeChangeHandler(WidgetMode mode)
        {
            WidgetMode = mode;
        }
    }
}
