using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class MoveActorCommand : IReversibleCommand
    {
        private BaseActor _ba;
        private Vector2 _fromPosition; 
        private Vector2 _toPosition;

        public MoveActorCommand(BaseActor ba, Vector2 fromPosition, Vector2 toPosition)
        {
            _ba = ba;
            _fromPosition = fromPosition;
            _toPosition = toPosition;
        }

        public void Run()
        {
            _ba.Body.Position = _toPosition;
        }

        public void Rollback()
        {
            _ba.Body.Position = _fromPosition;
        }
    }
}
