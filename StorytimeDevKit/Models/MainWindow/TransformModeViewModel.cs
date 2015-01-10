using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Renderables;

namespace StoryTimeDevKit.Models.MainWindow
{
    public delegate void OnWidgetModeChanges(WidgetMode mode);
    public delegate void OnHasActorChanges(bool hasActor);

    public class TransformModeViewModel : BaseViewModel
    {
        private WidgetMode _widgetMode;
        private bool _hasActor;

        public event OnWidgetModeChanges OnWidgetModeChanges;
        public event OnHasActorChanges OnHasActorChanges;

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
                if (OnWidgetModeChanges != null)
                    OnWidgetModeChanges(value);
            }
        }

        public bool HasActor
        {
            get
            {
                return _hasActor;
            }
            set
            {
                if (_hasActor == value) return;
                _hasActor = value;
                OnPropertyChanged("HasActor");
                if (OnHasActorChanges != null)
                    OnHasActorChanges(value);
            }
        }
    }
}
