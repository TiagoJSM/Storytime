using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ActorsCategoryViewModel : GameObjectCategoryViewModel
    {
        public ActorsCategoryViewModel(IGameObjectsControl gameObjects)
            : base(gameObjects, "Actors", "/Images/GameObjectsControl/ActorTreeViewIcon.png", "Actors")
        { }
    }
}
