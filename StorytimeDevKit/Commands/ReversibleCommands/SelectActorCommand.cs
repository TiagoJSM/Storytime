using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class SelectActorCommand : IReversibleCommand
    {
        /*private ISceneWidget _previouslySelected;
        private ISceneWidget _newlySelected;

        public SelectActorCommand(ISceneWidget selected, ISceneWidget toSelect)
        {
            _previouslySelected = selected;
            _newlySelected = toSelect;
        }*/

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
