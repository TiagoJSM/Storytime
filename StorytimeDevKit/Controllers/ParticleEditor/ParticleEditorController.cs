using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysicsWrapper;
using Microsoft.Xna.Framework;
using ParticleEngine;
using ParticleEngine.ParticleProcessors;
using ParticleEngine.ParticleProcessors.ParticleSpawnProcessors;
using StoryTimeDevKit.Controls.Editors;
using StoryTimeDevKit.Controls.ParticleEditor;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public class ParticleEditorController : IParticleEditorController, IParticleEmissorPropertyEditorController
    {
        private IParticleEmissorPropertyEditor _particleEmissorPropertyEditor;
        private IParticleEditorControl _particleEditorControl;
        private GameWorld _gameWorld;
        private ParticleEmitterActor _particleEmitterActor;

        private ParticleEmitter ParticleEmitter
        {
            get { return _particleEmitterActor.ParticleEmitterComponent.ParticleEmitter; }
        }

        public IParticleEmissorPropertyEditor ParticleEmissorPropertyEditor
        {
            get { return _particleEmissorPropertyEditor; }
            set
            {
                if (_particleEmissorPropertyEditor == value) return;
                if (_particleEmissorPropertyEditor != null)
                    UnassignParticleEmissorPropertyEditorEventHandlers();
                _particleEmissorPropertyEditor = value;
                _particleEmissorPropertyEditor.Selected = ParticleEmitter;
                if (_particleEmissorPropertyEditor != null)
                {
                    AssignParticleEmissorPropertyEditorEventHandlers();
                } 
            }
        }

        public IParticleEditorControl ParticleEditorControl
        {
            get { return _particleEditorControl; }
            set
            {
                if (_particleEditorControl == value) return;
                if (_particleEditorControl != null)
                    UnassignParticleEditorControlEventHandlers();
                _particleEditorControl = value;
                if (_particleEditorControl != null)
                {
                    AssignParticleEditorControlEventHandlers();
                } 
            }
        }

        public ParticleEditorController(GameWorld gameWorld)
        {
            _gameWorld = gameWorld;
            var scene = new Scene();
            scene.PhysicalWorld = new FarseerPhysicalWorld(Vector2.Zero);
            _gameWorld.AddScene(scene);
            _gameWorld.SetActiveScene(scene);
            _particleEmitterActor = scene.AddWorldEntity<ParticleEmitterActor>();
            SetParticleEmitterEditorDefaultValues();
           
            //ToDo: delete these lines in the future
            var bitmap = gameWorld.GraphicsContext.LoadTexture2D("default");
            var asset = new Static2DRenderableAsset();
            asset.Texture2D = bitmap;
            _particleEmitterActor.RenderableAsset = asset;
            var name = "one";
            _particleEmitterActor.Body = _particleEmitterActor.Scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f, name);
        }

        private void UnassignParticleEmissorPropertyEditorEventHandlers()
        {
            
        }

        private void AssignParticleEmissorPropertyEditorEventHandlers()
        {

        }

        private void AssignParticleEditorControlEventHandlers()
        {

        }

        private void UnassignParticleEditorControlEventHandlers()
        {

        }

        private void SetParticleEmitterEditorDefaultValues()
        {
            ParticleEmitter.SetParticleSpawnProcessor<DefaultParticleSpawnProcessor>();
            ParticleEmitter.ParticleProcessors.Add(
                new VelocityParticleProcessor()
                {
                    InitialVelocity = ParticleEmitter.EmissionVelocity,
                    FinalVelocity = ParticleEmitter.EmissionVelocity
                });
            ParticleEmitter.ParticleProcessors.Add(
                new DirectionParticleProcessor()
                {
                    InitialDirection = ParticleEmitter.EmissionDirection,
                    FinalDirection = ParticleEmitter.EmissionDirection
                });
        }
    }
}
