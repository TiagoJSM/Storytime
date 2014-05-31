using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands;
using System.Windows;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class GameObjectCategoryViewModel : TreeViewItemViewModel
    {
        public string GameObjectCategory { get ; private set; }
        public string ImagePath { get; private set; }

        public GameObjectCategoryViewModel(IGameObjectsControl gameObjects, string gameObjectName, string imgPath, string tag = null)
            : base(gameObjects, false)
        {
            GameObjectCategory = gameObjectName;
            ImagePath = imgPath;
            Tag = tag;
        }
    }
}
