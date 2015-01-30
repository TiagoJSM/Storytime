using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FarseerPhysicsWrapper;
using Microsoft.Xna.Framework;
using ParticleEngine;
using ParticleEngine.ParticleProcessors;
using ParticleEngine.ParticleProcessors.ParticleSpawnProcessors;
using StoryTimeDevKit.Commands.UICommands.ParticleEditor;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Controls.Editors;
using StoryTimeDevKit.Controls.ParticleEditor;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Models.ParticleEditor;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public class ParticleEditorController : 
        IParticleEditorController, 
        IParticleEmitterPropertyEditorController, 
        IParticleEffectController,
        IParticleEditorActionContext,
        INodeAddedCallback
    {
        private IParticleEmitterPropertyEditor _particleEmitterPropertyEditor;
        private IParticleEditorControl _particleEditorControl;

        private AddParticleEmitterCommand _addParticleEmitterCommand;
        private SetParticleSpawnProcessorCommand _setParticleSpawnProcessorCommand;

        private GameWorld _gameWorld;
        private ParticleEffectActor _particleEffectActor;
        private SetParticleProcessorCommand _setParticleProcessorCommand;

        /*private ParticleEmitter ParticleEmitter
        {
            get { return _particleEffectActor.ParticleEmitterComponent.ParticleEmitter; }
        }*/

        private ParticleEffectViewModel _effectViewModel;

        public ObservableCollection<ParticleEffectViewModel> ParticleEffectViewModel { get; private set; }

        public IParticleEmitterPropertyEditor ParticleEmitterPropertyEditor
        {
            get { return _particleEmitterPropertyEditor; }
            set
            {
                if (_particleEmitterPropertyEditor == value) return;
                if (_particleEmitterPropertyEditor != null)
                    UnassignParticleEmissorPropertyEditorEventHandlers();
                _particleEmitterPropertyEditor = value;
                //_particleEmitterPropertyEditor.Selected = ParticleEmitter;
                if (_particleEmitterPropertyEditor != null)
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
            _particleEffectActor = scene.AddWorldEntity<ParticleEffectActor>();
            
            _addParticleEmitterCommand = new AddParticleEmitterCommand(this);
            _setParticleSpawnProcessorCommand = new SetParticleSpawnProcessorCommand(this);
            _setParticleProcessorCommand = new SetParticleProcessorCommand(this);
            
            ParticleEffectViewModel = new ObservableCollection<ParticleEffectViewModel>();
            _effectViewModel = new ParticleEffectViewModel("Particle effect", this, _addParticleEmitterCommand);
            ParticleEffectViewModel.Add(_effectViewModel);
           
            //ToDo: delete these lines in the future
            var bitmap = gameWorld.GraphicsContext.LoadTexture2D("default");
            var asset = new Static2DRenderableAsset();
            asset.Texture2D = bitmap;
            _particleEffectActor.RenderableAsset = asset;
            var name = "one";
            _particleEffectActor.Body = _particleEffectActor.Scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f, name);
        }

        public void AddParticleEmitterTo(ParticleEffectViewModel particleEffect)
        {
            var emitterViewModel = new ParticleEmitterViewModel("name", this, _setParticleSpawnProcessorCommand,
                _setParticleProcessorCommand, particleEffect);
            
            var emitter = _particleEffectActor.ParticleEffectComponent.ParticleEffect.AddEmitter();
            SetParticleEmitterEditorDefaultValues(emitter);

            particleEffect.Children.Add(emitterViewModel);
        }

        public void SetParticleSpawnProcessorTo(ParticleEmitterViewModel particleEmitter)
        {
            particleEmitter.Children.Add(
                new ParticleProcessorViewModel("name", particleEmitter, this));
        }

        public void AddParticleProcessorTo(ParticleEmitterViewModel particleEmitter)
        {
            particleEmitter.Children.Add(
                new ParticleProcessorViewModel("name", particleEmitter, this));
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

        private void SetParticleEmitterEditorDefaultValues(ParticleEmitter emitter)
        {
            emitter.SetParticleSpawnProcessor<DefaultParticleSpawnProcessor>();
            emitter.ParticleProcessors.Add(
                new VelocityParticleProcessor()
                {
                    InitialVelocity = emitter.EmissionVelocity,
                    FinalVelocity = emitter.EmissionVelocity
                });
            emitter.ParticleProcessors.Add(
                new DirectionParticleProcessor()
                {
                    InitialDirection = emitter.EmissionDirection,
                    FinalDirection = emitter.EmissionDirection
                });
        }

        public void NodeAddedCallback(TreeViewItemViewModel parent, IEnumerable<TreeViewItemViewModel> newModels)
        {
            
        }
    }
}
