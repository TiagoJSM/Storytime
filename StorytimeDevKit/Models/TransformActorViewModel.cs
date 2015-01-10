using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeCore.Physics;
using StoryTimeDevKit.Entities.Renderables;

namespace StoryTimeDevKit.Models
{
    public class TransformActorViewModel : BaseViewModel
    {
        private BaseActor _ba;
        private WidgetMode _widgetMode;
        private bool _translateWidgetMode;
        private bool _rotateWidgetMode;
        private bool _enabled;

        public BaseActor Actor 
        { 
            get 
            {
                return _ba;
            } 
            set 
            {
                if (_ba == value) return;
                UnassignEvents();
                _ba = value;
                AssignEvents();
                RunEvents();
                ResolveTranformWidgetProperties();
            } 
        }
        public Vector2 Position
        {
            get
            {
                if (_ba == null) return Vector2.Zero;
                return _ba.Body.Position;
            }
            set
            {
                if (_ba == null) return;
                if (_ba.Body.Position == value) return;
                _ba.Body.Position = value;
            }
        }
        public float Rotation
        {
            get
            {
                if (_ba == null) return 0;
                return _ba.Body.Rotation;
            }
            set
            {
                if (_ba == null) return;
                if (_ba.Body.Rotation == value) return;
                _ba.Body.Rotation = value;
            }
        }
        public bool HasActor 
        { 
            get 
            {
                return _ba != null;
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
                ResolveTranformWidgetProperties();
            }
        }
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                OnPropertyChanged("Enabled");
                ResolveTranformWidgetProperties();
            }
        }

        public bool TranslateWidgetMode
        {
            get
            {
                return _translateWidgetMode;
            }
            private set
            {
                if (_translateWidgetMode == value) return;
                _translateWidgetMode = value;
                OnPropertyChanged("TranslateWidgetMode");
            }
        }

        public bool RotateWidgetMode
        {
            get
            {
                return _rotateWidgetMode;
            }
            set
            {
                if (_rotateWidgetMode == value) return;
                _rotateWidgetMode = value;
                OnPropertyChanged("RotateWidgetMode");
            }
        }

        public TransformActorViewModel()
        {
            _enabled = true;
        }

        private void OnPositionChangesHandler(IBody body)
        {
            OnPropertyChanged("Position");
        }

        private void OnRotationChangesHandler(IBody body)
        {
            OnPropertyChanged("Rotation");
        }

        private void OnScaleChangesHandler(IBody body)
        {
            OnPropertyChanged("Scale");
        }

        private void AssignEvents()
        {
            if (_ba == null) return;
            _ba.Body.OnPositionChanges += OnPositionChangesHandler;
            _ba.Body.OnRotationChanges += OnRotationChangesHandler;
        }

        private void UnassignEvents()
        {
            if (_ba == null) return;
            _ba.Body.OnPositionChanges -= OnPositionChangesHandler;
            _ba.Body.OnRotationChanges -= OnRotationChangesHandler;
        }

        private void RunEvents()
        {
            OnPositionChangesHandler(null);
            OnRotationChangesHandler(null);
            OnScaleChangesHandler(null);
            OnPropertyChanged("HasActor");
        }

        private void ResolveTranformWidgetProperties()
        {
            TranslateWidgetMode = WidgetMode == WidgetMode.Translate && HasActor && Enabled;
            RotateWidgetMode = WidgetMode == WidgetMode.Rotate && HasActor && Enabled;
        }
    }
}
