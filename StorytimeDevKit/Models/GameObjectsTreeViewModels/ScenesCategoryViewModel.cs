using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ScenesCategoryViewModel : GameObjectCategoryViewModel
    {
        public ScenesCategoryViewModel(IGameObjectsControl gameObjects)
            : base(gameObjects, "Scenes", "/Images/GameObjectsControl/SceneTreeViewIcon.jpg", "Scenes")
        { }
    }
}
