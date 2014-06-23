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

namespace StoryTimeDevKit.Controllers.Scenes
{
    public class SceneViewerController : ISceneViewerController
    {
        private ISceneViewerControl _control;
        private CommandStack _commands;
        private XNAGraphicsContext _graphicsContext;

        public SceneViewerController(XNAGraphicsContext graphicsContext)
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
            PopulateActorWithDefaultValuesIfNeeded(ba, position);

            IReversibleCommand command = new AddActorCommand(s.Scene, ba);
            _commands.Push(command);
        }

        public ISceneViewerControl Control
        {
            set { _control = value; }
        }

        private void PopulateActorWithDefaultValuesIfNeeded(BaseActor ba, Vector2 position)
        {
            if (ba.RenderableActor == null)
            {
                ITexture2D bitmap = _graphicsContext.LoadTexture2D("Bitmap1");
                Static2DRenderableAsset asset = new Static2DRenderableAsset();
                asset.SetBoundingBox(new Rectanglef(position.X, position.Y, 160));
                asset.Texture2D = bitmap;
                ba.RenderableActor = asset;
            }
        }
    }
}
