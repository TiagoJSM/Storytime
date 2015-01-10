using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Renderables;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class SelectActorCommand : IReversibleCommand
    {
        private BaseActor _previouslySelected;
        private BaseActor _newlySelected;

        public SelectActorCommand(BaseActor selected, BaseActor toSelect)
        {
            _previouslySelected = selected;
            _newlySelected = toSelect;
        }

        public void Run()
        {
            //if(_previouslySelected != null)
            //    _previouslySelected.Selected = false;
            //_newlySelected.Selected = true;
        }

        public void Rollback()
        {
            //if(_previouslySelected != null)
            //    _previouslySelected.Selected = true;
            //_newlySelected.Selected = false;
        }
    }
}
