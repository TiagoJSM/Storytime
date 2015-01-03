using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Controllers;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class AddActorCommand : IReversibleCommand
    {
        private Scene _scene;
        private Type _actorType;
        Action<BaseActor, Vector2> _initializer;
        private BaseActor _actor;
        private Vector2 _position;

        public AddActorCommand(Scene scene, Type actorType, Vector2 position, Action<BaseActor, Vector2> initializer)
        {
            _scene = scene;
            _actorType = actorType;
            _initializer = initializer;
            _position = position;
        }

        public void Run()
        {
            _actor = _scene.AddWorldEntity(_actorType) as BaseActor;
            _initializer(_actor, _position);
        }

        public void Rollback()
        {
            _scene.RemoveActor(_actor);
        }
    }
}
