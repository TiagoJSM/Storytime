using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class GameObjectCategoryViewModel : TreeViewItemViewModel
    {
        public string GameObjectCategory { get ; private set; }
        public string ImagePath { get; private set; }

        public GameObjectCategoryViewModel(string gameObjectName, string imgPath)
            :base(null, false)
        {
            GameObjectCategory = gameObjectName;
            ImagePath = imgPath;
        }
    }
}
