using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Models;

namespace StoryTimeDevKit.Controllers.GameObjects
{
    public interface IGameObjectsController : IController<IGameObjectsControl>
    {
        List<GameObjectsActorModel> LoadActors();
    }
}
