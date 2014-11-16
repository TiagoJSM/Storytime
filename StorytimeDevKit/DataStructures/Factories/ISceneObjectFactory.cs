using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.SceneObjects;

namespace StoryTimeDevKit.DataStructures.Factories
{
    public interface ISceneObjectFactory
    {
        ISceneObject CreateSceneObject(object data);
    }
}
