﻿using System;
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
        private IParticleEffectTreeView _particleEffectControl;

        private AddParticleEmitterCommand _addParticleEmitterCommand;
        private SetParticleSpawnProcessorCommand _setParticleSpawnProcessorCommand;
        private SetParticleProcessorCommand _setParticleProcessorCommand;
        private RemoveParticleProcessorCommand _removeParticleProcessorCommand;
        private ReplaceParticleProcessorCommand _replaceParticleProcessorCommand;

        private GameWorld _gameWorld;
        private ParticleEffectActor _particleEffectActor;

        private ParticleEffect ParticleEffect
        {
            get { return _particleEffectActor.ParticleEffectComponent.ParticleEffect; }
        }

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
                _particleEmitterPropertyEditor.Selected = ParticleEffect;
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

        public IParticleEffectTreeView ParticleEffectControl
        {
            get { return _particleEffectControl; }
            set
            {
                if (_particleEffectControl == value) return;
                if (_particleEffectControl != null)
                    UnassignParticleEffectControlEventHandlers();
                _particleEffectControl = value;
                if (_particleEditorControl != null)
                {
                    AssignParticleEffectControlEventHandlers();
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
            _removeParticleProcessorCommand = new RemoveParticleProcessorCommand(this);
            
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

        public void AddParticleEmitterTo(ParticleEffectViewModel particleEffectViewModel)
        {
            var emitter = ParticleEffect.AddEmitter();
            var emitterViewModel = new ParticleEmitterViewModel("name", this, _setParticleSpawnProcessorCommand,
                _setParticleProcessorCommand, particleEffectViewModel, emitter);
            
            SetParticleEmitterDefaultValues(emitter, emitterViewModel);

            particleEffectViewModel.Children.Add(emitterViewModel);
        }

        public void SetParticleSpawnProcessorTo(ParticleEmitterViewModel particleEmitterViewModel)
        {
            var spawnProcessor = particleEmitterViewModel.ParticleEmitter.SetParticleSpawnProcessor<DefaultParticleSpawnProcessor>();
            particleEmitterViewModel.Children.Add(
                new ParticleSpawnProcessorViewModel(spawnProcessor, particleEmitterViewModel, this, _replaceParticleProcessorCommand));
        }

        public void AddParticleProcessorTo(ParticleEmitterViewModel particleEmitter)
        {
            var velocityProcessor =
                new VelocityParticleProcessor()
                {
                    InitialVelocity = particleEmitter.ParticleEmitter.EmissionVelocity,
                    FinalVelocity = particleEmitter.ParticleEmitter.EmissionVelocity
                };
            particleEmitter.Children.Add(
                new ParticleProcessorViewModel(velocityProcessor, particleEmitter, this, _removeParticleProcessorCommand));
        }

        public void ReplaceParticleProcessorFromEmitter(ParticleEmitter particleEmitter, Type spawnProcessorType)
        {
        }

        public void RemoveParticleProcessorFromEmitter(ParticleProcessorViewModel particleProcessor, ParticleEmitter particleEmitter)
        {
            
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

        private void AssignParticleEffectControlEventHandlers()
        {
            _particleEffectControl.OnSelectedItemChanged += OnSelectedItemChangedHandler;
        }

        private void UnassignParticleEffectControlEventHandlers()
        {
            _particleEffectControl.OnSelectedItemChanged -= OnSelectedItemChangedHandler;
        }

        private void SetParticleEmitterDefaultValues(ParticleEmitter emitter, ParticleEmitterViewModel emitterViewModel)
        {
            var spawnProcessor = emitter.SetParticleSpawnProcessor<DefaultParticleSpawnProcessor>();
            var velocityProcessor =
                new VelocityParticleProcessor()
                {
                    InitialVelocity = emitter.EmissionVelocity,
                    FinalVelocity = emitter.EmissionVelocity
                };
            var directionProcessor =
                new DirectionParticleProcessor()
                {
                    InitialDirection = emitter.EmissionDirection,
                    FinalDirection = emitter.EmissionDirection
                };

            emitter.ParticleProcessors.Add(velocityProcessor);
            emitter.ParticleProcessors.Add(directionProcessor);

            emitterViewModel.Children.Add(
                new ParticleSpawnProcessorViewModel(spawnProcessor, emitterViewModel, this, _replaceParticleProcessorCommand));
            emitterViewModel.Children.Add(
                new ParticleProcessorViewModel(velocityProcessor, emitterViewModel, this, _removeParticleProcessorCommand));
            emitterViewModel.Children.Add(
                new ParticleProcessorViewModel(directionProcessor, emitterViewModel, this, _removeParticleProcessorCommand));
        }

        private void OnSelectedItemChangedHandler(ParticleTreeViewItem item)
        {
            _particleEmitterPropertyEditor.Selected = item.EditableObject;
        }

        public void NodeAddedCallback(TreeViewItemViewModel parent, IEnumerable<TreeViewItemViewModel> newModels)
        {
            
        }
    }
}
