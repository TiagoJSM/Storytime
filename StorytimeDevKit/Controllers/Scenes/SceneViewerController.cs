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
using StoryTimeDevKit.SceneWidgets;
using FarseerPhysicsWrapper;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Models.SavedData;
using StoryTimeDevKit.Utils;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public class SceneViewerController : ISceneViewerController
    {
        private ISceneViewerControl _control;
        private CommandStack _commands;
        private IGraphicsContext _graphicsContext;

        public SceneViewerController(IGraphicsContext graphicsContext)
        {
            _commands = new CommandStack();
            _graphicsContext = graphicsContext;
        }

        public void Undo()
        {
            _commands.Undo();
        }

        public void Redo()
        {
            _commands.Redo();
        }

        public int CommandCount
        {
            get { return _commands.CommandCount; }
        }

        public int? CommandIndex
        {
            get { return _commands.CommandIndex; }
        }

        public bool CanUndo
        {
            get { return _commands.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _commands.CanRedo; }
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
            ActorWidgetAdapter adapter = new ActorWidgetAdapter(this, ba, _graphicsContext);

            IReversibleCommand command = new AddActorCommand(s.Scene, adapter);
            _commands.Push(command);
        }

        public void MoveActor(BaseActor actor, Vector2 fromPosition, Vector2 toPosition)
        {
            IReversibleCommand command = new MoveActorCommand(actor, fromPosition, toPosition);
            _commands.Push(command);
        }

        public void SelectWidget(ISceneWidget selected, ISceneWidget toSelect)
        {
            IReversibleCommand command = new SelectActorCommand(selected, toSelect);
            _commands.Push(command);
        }

        public void SaveScene(SceneTabViewModel scene)
        {
            SavedSceneModel sceneSave = scene.Scene.ToSaveModel();
            ApplicationUtils.SaveScene(sceneSave);
        }

        public ISceneViewerControl Control
        {
            set { _control = value; }
        }

        private void PopulateActorWithDefaultValuesIfNeeded(BaseActor ba, Vector2 position, Scene s)
        {
            if (ba.RenderableAsset == null)
            {
                ITexture2D bitmap = _graphicsContext.LoadTexture2D("Bitmap1");
                Static2DRenderableAsset asset = new Static2DRenderableAsset();
                asset.SetBoundingBox(new Rectanglef(0, 0, 160));
                asset.Texture2D = bitmap;
                ba.RenderableAsset = asset;
                string name = "one";
                ba.Body = new FarseerBody(BodyFactory.CreateRectangle(s.World, 160f, 160f, 1f, name));
                ba.Body.Position = position;
            }
        }
    }
}
