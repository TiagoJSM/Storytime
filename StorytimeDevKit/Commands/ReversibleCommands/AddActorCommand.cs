using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Controllers;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class AddActorCommand : IReversibleCommand
    {
        private Scene _scene;
        private BaseActor _ba;

        public AddActorCommand(Scene scene, BaseActor ba)
        {
            _scene = scene;
            _ba = ba;
        }

        public void Run()
        {
            _scene.AddActor(_ba);
        }

        public void Rollback()
        {
            _scene.RemoveActor(_ba);
        }
    }
}
