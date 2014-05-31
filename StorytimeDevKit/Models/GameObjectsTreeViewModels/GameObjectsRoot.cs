using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class GameObjectsRoot
    {
        public GameObjectsRoot() 
        {
        }

        public List<GameObjectCategoryViewModel> GameObjectsCategories { get; set; }
    }
}
