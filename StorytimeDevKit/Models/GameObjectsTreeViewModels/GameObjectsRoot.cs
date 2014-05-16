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
            GameObjectCategoryViewModel go1 = new GameObjectCategoryViewModel("lol", "/Images/GameObjectsControl/TextureTreeViewIcon.jpg");
            GameObjectCategoryViewModel go2 = new GameObjectCategoryViewModel("oops", "/Images/GameObjectsControl/TextureTreeViewIcon.jpg");

            AssemblyViewModel folder1 = new AssemblyViewModel(go1, "foldah");
            AssemblyViewModel folder2 = new AssemblyViewModel(folder1, "foldah");

            folder1.Children.Add(folder2);

            go1.Children.Add(folder1);

            GameObjectsCategories = new List<GameObjectCategoryViewModel>()
            {
                go1,
                go2
            };
        }

        public List<GameObjectCategoryViewModel> GameObjectsCategories { get; set; }
    }
}
