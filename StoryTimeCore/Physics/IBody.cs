using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Manageables;

namespace StoryTimeCore.Physics
{
    /// <summary>
    /// The interface that represents the body of the scene components.
    /// With the methods in this interface it's possible fo the user input or the remaining Bodies to interact between each other.
    /// </summary>
    public interface IBody : ISynchronizable
    {
        event Action<IBody> OnPositionChanges;
        event Action<IBody> OnRotationChanges;
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}
