using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.SceneObjects;
using StoryTimeDevKit.Entities.SceneWidgets;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class SceneObjectViewModel : BaseViewModel
    {
        private ISceneObject _sceneObject;
        private WidgetMode _widgetMode;
        private bool _translateWidgetMode;
        private bool _rotateWidgetMode;
        private bool _enabled;

        public ISceneObject SceneObject 
        { 
            get 
            {
                return _sceneObject;
            } 
            set 
            {
                if (_sceneObject == value) return;
                UnassignEvents();
                _sceneObject = value;
                AssignEvents();
                RunEvents();
                ResolveTranformWidgetProperties();
            } 
        }
        public Vector2 Position
        {
            get
            {
                if (_sceneObject == null) return Vector2.Zero;
                return _sceneObject.Position;
            }
        }
        public float Rotation
        {
            get
            {
                if (_sceneObject == null) return 0;
                return _sceneObject.Rotation;
            }
        }
        public bool HasSceneObject
        { 
            get 
            {
                return _sceneObject != null;
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

        public SceneObjectViewModel()
        {
            _enabled = true;
        }

        private void OnPositionChangesHandler(Vector2 position)
        {
            OnPositionChanges();
        }

        private void OnRotationChangesHandler(float totalRotation)
        {
            OnRotationChanges();
        }

        private void OnScaleChangesHandler()
        {
            OnScaleChanges();
        }

        private void AssignEvents()
        {
            if (_sceneObject == null) return;
            _sceneObject.OnPositionChanges += OnPositionChangesHandler;
            _sceneObject.OnRotationChanges += OnRotationChangesHandler;
        }

        private void UnassignEvents()
        {
            if (_sceneObject == null) return;
            _sceneObject.OnPositionChanges -= OnPositionChangesHandler;
            _sceneObject.OnRotationChanges -= OnRotationChangesHandler;
        }

        private void RunEvents()
        {
            OnPositionChanges();
            OnRotationChanges();
            OnScaleChangesHandler();
            OnPropertyChanged("HasSceneObject");
        }

        private void ResolveTranformWidgetProperties()
        {
            TranslateWidgetMode = WidgetMode == WidgetMode.Translate && HasSceneObject && Enabled;
            RotateWidgetMode = WidgetMode == WidgetMode.Rotate && HasSceneObject && Enabled;
        }

        private void OnPositionChanges()
        {
            OnPropertyChanged("Position");
        }

        private void OnRotationChanges()
        {
            OnPropertyChanged("Position");
        }

        private void OnScaleChanges()
        {
            OnPropertyChanged("Scale");
        }
    }
}
