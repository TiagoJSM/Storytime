using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class RotateActorCommand : IReversibleCommand
    {
        private BaseActor _ba;
        private float _previousRotation;
        private float _currentRotation;

        public RotateActorCommand(BaseActor ba, float previousRotation, float currentRotation)
        {
            _ba = ba;
            _previousRotation = previousRotation;
            _currentRotation = currentRotation;
        }

        public void Run()
        {
            _ba.Body.Rotation = _currentRotation;
        }

        public void Rollback()
        {
            _ba.Body.Rotation = _previousRotation;
        }
    }
}
