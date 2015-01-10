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
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeDevKit.Models;
using StoryTimeUI;
using StoryTimeDevKit.Entities.Renderables;
using Ninject;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.DataStructures;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public class SceneViewerController :
        MultiStackedCommandsController<ISceneViewerControl, Scene>, ISceneViewerController
    {
        private ISceneViewerControl _control;
        private GameWorld _world;

        private Dictionary<Scene, SceneControlData> _scenesControlData;
        private SceneControlData CurrentSceneControlData 
        { 
            get 
            {
                if(StackKey == null) return null;
                if (!_scenesControlData.ContainsKey(StackKey)) return null;
                return _scenesControlData[StackKey]; 
            } 
        }
        
        [Inject]
        public TransformModeViewModel TransformModeModel { get; set; }

        public SceneViewerController(GameWorld world)
        {
            _world = world;
            _scenesControlData = new Dictionary<Scene, SceneControlData>();
        }

        public void AddActor(SceneTabViewModel s, ActorViewModel actor, Vector2 position)
        {
            if(s == null) 
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddWorldEntity", "s", typeof(SceneTabViewModel), LocalizedTexts.AddingActorError); 
            if(s.Scene == null)
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddWorldEntity", "s.Scene", typeof(Scene), LocalizedTexts.AddingActorError); 
            if(actor == null)
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddWorldEntity", "actor", typeof(ActorViewModel), LocalizedTexts.AddingActorError); 
            if(actor.ActorType == null)
                throw new InvalidArgumentOnControllerMethodException(
                    this, "AddWorldEntity", "actor.ActorType", typeof(Type), LocalizedTexts.AddingActorError);

            IReversibleCommand command = 
                new AddActorCommand(s.Scene, actor.ActorType, position, PopulateActorWithDefaultValuesIfNeeded);
            SelectedStack.Push(command);
        }

        private void SelectWidget(BaseActor selected, BaseActor toSelect)
        {
            IReversibleCommand command = new SelectActorCommand(selected, toSelect);
            SelectedStack.Push(command);
        }

        public void SaveScene(SceneTabViewModel scene)
        {
            var sceneSave = scene.Scene.ToSaveModel();
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

        private void PopulateActorWithDefaultValuesIfNeeded(BaseActor ba, Vector2 position)
        {
            if (ba.RenderableAsset == null)
            {
                var bitmap = _world.GraphicsContext.LoadTexture2D("default");
                var asset = new Static2DRenderableAsset();
                asset.Texture2D = bitmap;
                ba.RenderableAsset = asset;
                var name = "one";
                ba.Body = ba.Scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f, name);
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

        private void OnMouseMoveHandler(Scene scene, Vector2 position)
        {
            scene.GUI.MouseMove(position);
        }

        private void OnMouseClickHandler(Scene scene, Vector2 position)
        {
            var newIntersectedActor = scene.AxisAlignedIntersect(position).FirstOrDefault();
            if (newIntersectedActor == null) return;

            var controlData = CurrentSceneControlData;
            controlData.TransformActorModel.Actor = newIntersectedActor as BaseActor;
            
            TransformModeModel.HasActor = true;
            if (TransformModeModel.WidgetMode == WidgetMode.None)
                TransformModeModel.WidgetMode = WidgetMode.Translate;
        }

        private void OnMouseDownHandler(Scene scene, Vector2 position)
        {
            scene.GUI.MouseDown(position);
        }

        private void OnMouseUpHandler(Scene scene, Vector2 position)
        {
            scene.GUI.MouseUp(position);
        }

        private void OnSceneAddedHandler(SceneTabViewModel model)
        {
            var translateWidg = new TranslateSceneWidget();
            model.Scene.GUI.Children.Add(translateWidg);
            translateWidg.OnTranslationComplete += OnTranslationCompleteHandler;

            var rotateWidg = new RotateSceneWidget();
            model.Scene.GUI.Children.Add(rotateWidg);
            rotateWidg.OnStopRotation += OnStopRotationHandler;

            var controlData = new SceneControlData(translateWidg, rotateWidg, TransformModeModel);

            AddStackFor(model.Scene);
            _scenesControlData.Add(model.Scene, controlData);
        }

        private void OnTranslationCompleteHandler(Vector2 startPosition, Vector2 currentPosition)
        {
            var controlData = CurrentSceneControlData;

            if (!controlData.TransformActorModel.HasActor) return;
            IReversibleCommand command = new MoveActorCommand(controlData.TransformActorModel.Actor, startPosition, currentPosition);
            SelectedStack.Push(command);
        }

        private void OnStopRotationHandler(float originalRotation, float finalRotation)
        {
            var controlData = CurrentSceneControlData;

            if (!controlData.TransformActorModel.HasActor) return;
            IReversibleCommand command = new RotateActorCommand(controlData.TransformActorModel.Actor, originalRotation, finalRotation);
            SelectedStack.Push(command);
        }

        private void OnSceneChangedHandler(SceneTabViewModel model)
        {
            StackKey = model.Scene;
        }
    }
}
