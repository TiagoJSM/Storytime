using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Controllers.GameObjects
{
    public interface IGameObjectsController : IController<IGameObjectsControl>
    {
        GameObjectsRoot LoadGameObjectsTree();
        //List<GameObjectsActorModel> LoadActors();
        //List<GameObjectsTextureModel> LoadTextures();
    }
}
