using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ActorsCategoryViewModel : GameObjectCategoryViewModel
    {
        public ActorsCategoryViewModel(INodeAddedCallback nodeAddCB)
            : base(nodeAddCB, "Actors", "/Images/GameObjectsControl/ActorTreeViewIcon.png", "Actors")
        { }
    }
}
