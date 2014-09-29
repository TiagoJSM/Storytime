using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.SceneViewer;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Commands;
using StoryTimeDevKit.Commands.ReversibleCommands;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Models.SceneViewer;
using StoryTimeDevKit.Exceptions.Generic;
using StoryTimeDevKit.Resources.SceneViewer;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using StoryTime.Contexts;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using FarseerPhysics.Factories;
using FarseerPhysicsWrapper;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Models.SavedData;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Controllers.TemplateControllers;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeDevKit.Models;
using StoryTimeUI;
using StoryTimeDevKit.Entities.SceneWidgets;
using Ninject;
using StoryTimeDevKit.Models.MainWindow;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public class SceneViewerController :
        MultiStackedCommandsController<ISceneViewerControl, Scene>, ISceneViewerController
    {
        private ISceneViewerControl _control;
        private GameWorld _world;
        private BaseActor _intersectedActor;
        private BindingEngine<TranslateSceneWidget, TransformActorViewModel> _translateBindingEngine;
        private BindingEngine<RotateSceneWidget, TransformActorViewModel> _rotateBindingEngine;
        private TranslateSceneWidget _translateWidg;
        private RotateSceneWidget _rotateWidg;
        private TransformModeViewModel _transformModeModel;
        
        [Inject]
        public TransformModeViewModel TransformModeModel 
        {
            get
            {
                return _transformModeModel;
            }
            set
            {
                if(_transformModeModel != null)
                    UnassignTransformModeModelEvents();
                _transformModeModel = value;
                AssignTransformModeModelEvents();
                SetTransformWidgetMode();
            }
        }

        public SceneViewerController(GameWorld world/*, TransformActorViewModel model*/)
        {
            _world = world;
        }

        public void AddActor(SceneTabViewModel s, ActorViewModel actor, Vector2 position)
        {
            if(s == null) 
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddActor", "s", typeof(SceneTabViewModel), LocalizedTexts.AddingActorError); 
            if(s.Scene == null)
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddActor", "s.Scene", typeof(Scene), LocalizedTexts.AddingActorError); 
            if(actor == null)
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddActor", "actor", typeof(ActorViewModel), LocalizedTexts.AddingActorError); 
            if(actor.ActorType == null)
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddActor", "actor.ActorType", typeof(Type), LocalizedTexts.AddingActorError);

            BaseActor ba = Activator.CreateInstance(actor.ActorType) as BaseActor;
            PopulateActorWithDefaultValuesIfNeeded(ba, position, s.Scene);

            IReversibleCommand command = new AddActorCommand(s.Scene, ba);
            SelectedStack.Push(command);
        }

        /*public void SelectWidget(ISceneWidget selected, ISceneWidget toSelect)
        {
            IReversibleCommand command = new SelectActorCommand(selected, toSelect);
            Commands.Push(command);
        }*/

        public void SaveScene(SceneTabViewModel scene)
        {
            SavedSceneModel sceneSave = scene.Scene.ToSaveModel();
            ApplicationUtils.SaveScene(sceneSave);
        }

        public ISceneViewerControl Control
        {
            set 
            { 
                if (_control == value) return;
                if (_control != null)
                    UnassignControlEventHandlers();
                _control = value;
                if (value != null)
                    AssignControlEventHandlers();
            }
        }

        public void MoveActor(BaseActor actor, Vector2 fromPosition, Vector2 toPosition)
        {
            IReversibleCommand command = new MoveActorCommand(actor, fromPosition, toPosition);
            SelectedStack.Push(command);
        }

        public void RotateActor(BaseActor actor, float previousRotation, float rotation)
        {
            IReversibleCommand command = new RotateActorCommand(actor, previousRotation, rotation);
            SelectedStack.Push(command);
        }

        private void PopulateActorWithDefaultValuesIfNeeded(BaseActor ba, Vector2 position, Scene s)
        {
            if (ba.RenderableAsset == null)
            {
                ITexture2D bitmap = _world.GraphicsContext.LoadTexture2D("default");
                Static2DRenderableAsset asset = new Static2DRenderableAsset();
                asset.Texture2D = bitmap;
                ba.RenderableAsset = asset;
                string name = "one";
                ba.Body = s.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f, name);
                ba.Body.Position = position;
            }
        }

        private void AssignControlEventHandlers()
        {
            _control.OnMouseMove += OnMouseMoveHandler;
            _control.OnMouseClick += OnMouseClickHandler;
            _control.OnDropActor += OnDropActorHandler;
            _control.OnMouseDown += OnMouseDownHandler;
            _control.OnMouseUp += OnMouseUpHandler;
            _control.OnSceneAdded += OnSceneAddedHandler;
            _control.OnSceneChanged += OnSceneChangedHandler;
        }

        private void UnassignControlEventHandlers()
        {
            _control.OnMouseMove -= OnMouseMoveHandler;
            _control.OnMouseClick -= OnMouseClickHandler;
            _control.OnDropActor -= OnDropActorHandler;
            _control.OnMouseDown -= OnMouseDownHandler;
            _control.OnMouseUp -= OnMouseUpHandler;
            _control.OnSceneAdded -= OnSceneAddedHandler;
            _control.OnSceneChanged -= OnSceneChangedHandler;
        }

        private void OnDropActorHandler(ActorViewModel actorModel, SceneTabViewModel sceneTabModel, Vector2 position)
        {
            AddActor(sceneTabModel, actorModel, position);
        }

        private void OnMouseMoveHandler(SceneTabViewModel model, Vector2 position)
        {
            model.Scene.GUI.MouseMove(position);
        }

        private void OnMouseClickHandler(SceneTabViewModel model, Vector2 position)
        {
            BaseActor newIntersectedActor = model.Scene.Intersect(position).FirstOrDefault();
            
            if (newIntersectedActor == null) return;
            if (newIntersectedActor == _intersectedActor) return;

            _intersectedActor = newIntersectedActor;

            TransformActorViewModel transformModel = new TransformActorViewModel(_intersectedActor);

            _transformModeModel.HasActor = true;
            _transformModeModel.WidgetMode = WidgetMode.Translate;
            SetTransformWidgetMode();

            _translateBindingEngine =
                new BindingEngine<TranslateSceneWidget, TransformActorViewModel>(_translateWidg, transformModel)
                    .Bind(tw => tw.Position, a => a.Position);

            _rotateBindingEngine =
                new BindingEngine<RotateSceneWidget, TransformActorViewModel>(_rotateWidg, transformModel)
                    .Bind(tw => tw.Position, a => a.Position)
                    .Bind(tw => tw.Rotation, a => a.Rotation);
        }

        private void OnMouseDownHandler(SceneTabViewModel model, Vector2 position)
        {
            model.Scene.GUI.MouseDown(position);
        }

        private void OnMouseUpHandler(SceneTabViewModel model, Vector2 position)
        {
            model.Scene.GUI.MouseUp(position);
        }

        private void OnSceneAddedHandler(SceneTabViewModel model)
        {
            _translateWidg = new TranslateSceneWidget();
            model.Scene.GUI.Children.Add(_translateWidg);
            _translateWidg.OnTranslate += OnTranslateHandler;
            _translateWidg.OnTranslationComplete += OnTranslationCompleteHandler;

            _rotateWidg = new RotateSceneWidget();
            model.Scene.GUI.Children.Add(_rotateWidg);
            _rotateWidg.OnRotation += OnRotationHandler;
            _rotateWidg.OnStopRotation += OnStopRotationHandler;

            SetTransformWidgetMode();

            AddStackFor(model.Scene);
        }

        private void OnTranslateHandler(Vector2 translation)
        {
            if (_intersectedActor == null) return;
            _intersectedActor.Body.Position += translation;
        }

        private void OnTranslationCompleteHandler(Vector2 startPosition, Vector2 currentPosition)
        {
            if (_intersectedActor == null) return;
            IReversibleCommand command = new MoveActorCommand(_intersectedActor, startPosition, currentPosition);
            SelectedStack.Push(command);
        }

        private void OnRotationHandler(float rotation)
        {
            if (_intersectedActor == null) return;
            _intersectedActor.Body.Rotation += rotation;
        }

        private void OnStopRotationHandler(float originalRotation, float finalRotation)
        {
            if (_intersectedActor == null) return;
            IReversibleCommand command = new RotateActorCommand(_intersectedActor, originalRotation, finalRotation);
            SelectedStack.Push(command);
        }

        private void SetTransformWidgetMode()
        {
            if (_rotateWidg != null)
            {
                _rotateWidg.Active = TransformModeModel.WidgetMode == WidgetMode.Rotate;
                _rotateWidg.Visible = TransformModeModel.WidgetMode == WidgetMode.Rotate;
            }

            if (_translateWidg != null)
            {
                _translateWidg.Active = TransformModeModel.WidgetMode == WidgetMode.Translate;
                _translateWidg.Visible = TransformModeModel.WidgetMode == WidgetMode.Translate;
            }
        }

        private void UnassignTransformModeModelEvents()
        {
            _transformModeModel.OnWidgetModeChanges -= OnWidgetModeChangesHandler;
        }

        private void AssignTransformModeModelEvents()
        {
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChangesHandler;
        }

        private void OnWidgetModeChangesHandler(WidgetMode mode)
        {
            SetTransformWidgetMode();
        }

        private void OnSceneChangedHandler(SceneTabViewModel model)
        {
            StackKey = model.Scene;
        }
    }
}
