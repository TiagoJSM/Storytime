using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using FarseerPhysicsWrapper;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeDevKit.DataStructures.Factories;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.DataStructures.BindingEngines;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Controls;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeDevKit.Entities.SceneWidgets;

namespace StoryTimeDevKit.Controllers.TemplateControllers
{
    public abstract class EditableSceneController<TControl, TWorkingMode> : StackedCommandsController<TControl>
    {
        private GameWorld _world;
        private TranslateSceneObjectBindingEngine _translateBindingEngine;
        private RotateSceneObjectBindingEngine _rotateBindingEngine;
        private WorkingMode _activeWorkingMode;
        private IMouseInteractiveControl _control;
        
        public GameWorld GameWorld
        {
            get
            {
                return _world;
            }
            private set
            {
                _world = value;

                Scene scene = new Scene();
                scene.PhysicalWorld = new FarseerPhysicalWorld(Vector2.Zero);
                _world.AddScene(scene);
                _world.SetActiveScene(scene);

                //ConfigureSceneUI();
            }
        }
        protected IGraphicsContext GraphicsContext { get { return GameWorld.GraphicsContext; } }
        public Scene Scene { get { return GameWorld.ActiveScene; } }
        public TranslateSceneWidget TranslateWidget { get; private set; }
        public RotateSceneWidget RotateWidget { get; private set; }
        public bool EnableUI
        {
            get
            {
                return SceneObjectViewModel.Enabled;
            }
            set
            {
                SceneObjectViewModel.Enabled = value;
            }
        }
        
        protected object SelectedObject
        {
            get
            {
                if (SceneObjectViewModel.SceneObject == null) return null;
                return SceneObjectViewModel.SceneObject.Object;
            }
            set
            {
                if (SceneObjectViewModel.SceneObject == value) return;
                if (value == null)
                {
                    SceneObjectViewModel.SceneObject = null;
                    return;
                }
                SceneObjectViewModel.SceneObject = SceneObjectFactory.CreateSceneObject(value);
            }
        }
        protected TransformModeViewModel TransformModeModel { get; private set; }
        protected SceneObjectViewModel SceneObjectViewModel { get; private set; }
        
        protected abstract ISceneObjectFactory SceneObjectFactory { get; }
        protected abstract Dictionary<TWorkingMode, WorkingMode> WorkingModeMapping { get; }

        public EditableSceneController(
            GameWorld gameWorld, TransformModeViewModel transformModeModel)
        {
            GameWorld = gameWorld;
            SceneObjectViewModel = new SceneObjectViewModel();
            TransformModeModel = transformModeModel;
            SceneObjectViewModel.WidgetMode = TransformModeModel.WidgetMode;
            TransformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
            
            ConfigureSceneUI();
        }

        protected void BindControlEvents(IMouseInteractiveControl control)
        {
            if (_control != null)
            {
                _control.OnMouseClick -= OnMouseClickHandler;
                _control.OnMouseDown -= OnMouseDownHandler;
                _control.OnMouseUp -= OnMouseUpHandler;
                _control.OnMouseMove -= OnMouseMoveHandler;
            }
            _control = control;
            if (_control != null)
            {
                _control.OnMouseClick += OnMouseClickHandler;
                _control.OnMouseDown += OnMouseDownHandler;
                _control.OnMouseUp += OnMouseUpHandler;
                _control.OnMouseMove += OnMouseMoveHandler;
            }
        }

        protected void SetWorkingMode(TWorkingMode mode)
        {
            if (_activeWorkingMode != null)
                _activeWorkingMode.OnLeaveMode();
            _activeWorkingMode = WorkingModeMapping[mode];
            if (_activeWorkingMode != null)
                _activeWorkingMode.OnEnterMode();
        }

        private void ConfigureSceneUI()
        {
            TranslateWidget = new TranslateSceneWidget();
            Scene.GUI.Children.Add(TranslateWidget);

            RotateWidget = new RotateSceneWidget();
            Scene.GUI.Children.Add(RotateWidget);

            _translateBindingEngine = new TranslateSceneObjectBindingEngine(TranslateWidget, SceneObjectViewModel);
            _rotateBindingEngine = new RotateSceneObjectBindingEngine(RotateWidget, SceneObjectViewModel);

            TranslateWidget.OnTranslate += OnTranslateHandler;
            TranslateWidget.OnTranslationComplete += OnTranslationComplete;
            RotateWidget.OnRotation += OnRotateHandler;
        }

        private void OnWidgetModeChanges(WidgetMode mode)
        {
            SceneObjectViewModel.WidgetMode = mode;
        }

        private void OnTranslateHandler(Vector2 translation)
        {
            SceneObjectViewModel.SceneObject.Translate(translation);
        }

        private void OnTranslationComplete(Vector2 fromPosition, Vector2 toPosition)
        {
            SceneObjectViewModel.SceneObject.EndTranslation();
        }

        private void OnRotateHandler(float rotation)
        {
            SceneObjectViewModel.SceneObject.Rotate(rotation);
        }

        private void OnMouseClickHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode == null) return;
            _activeWorkingMode.Click(position);
        }

        private void OnMouseMoveHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode != null)
            {
                _activeWorkingMode.MouseMove(position);
            }
        }

        private void OnMouseDownHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode != null)
            {
                _activeWorkingMode.MouseDown(position);
            }
        }

        private void OnMouseUpHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode != null)
            {
                _activeWorkingMode.MouseUp(position);
            }
        }
    }
}
