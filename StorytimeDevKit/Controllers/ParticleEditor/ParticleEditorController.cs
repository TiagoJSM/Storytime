using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysicsWrapper;
using Microsoft.Xna.Framework;
using ParticleEngine;
using StoryTimeDevKit.Controls.Editors;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public class ParticleEditorController : IParticleEditorController, IParticleEmissorPropertyEditorController
    {
        private IParticleEmissorPropertyEditor _particleEmissorPropertyEditor;
        private ParticleEmitter _emitter;
        private GameWorld _gameWorld;

        public IParticleEmissorPropertyEditor ParticleEmissorPropertyEditor
        {
            get { return _particleEmissorPropertyEditor; }
            set
            {
                if (_particleEmissorPropertyEditor == value) return;
                if (_particleEmissorPropertyEditor != null)
                    UnassignParticleEmissorPropertyEditorEventHandlers();
                _particleEmissorPropertyEditor = value;
                if (_particleEmissorPropertyEditor != null)
                {
                    AssignParticleEmissorPropertyEditorEventHandlers();
                    //_particleEmissorPropertyEditor.Selected = _emitter;
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
        }

        private void UnassignParticleEmissorPropertyEditorEventHandlers()
        {
            
        }

        private void AssignParticleEmissorPropertyEditorEventHandlers()
        {

        }
    }
}
