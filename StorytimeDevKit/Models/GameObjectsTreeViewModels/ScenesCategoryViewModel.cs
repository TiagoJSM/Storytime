using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ScenesCategoryViewModel : GameObjectCategoryViewModel
    {
        public ScenesCategoryViewModel(INodeAddedCallback nodeAddCB)
            : base(nodeAddCB, "Scenes", "/Images/GameObjectsControl/SceneTreeViewIcon.jpg", "Scenes")
        { }
    }
}
