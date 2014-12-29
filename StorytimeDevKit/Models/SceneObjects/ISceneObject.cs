using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Delegates;

namespace StoryTimeDevKit.Models.SceneObjects
{
    public interface ISceneObject
    {
        event OnPositionChanges OnPositionChanges;
        event OnRotationChanges OnRotationChanges;

        object Object { get; }
        Vector2 Position { get; }
        float Rotation { get; }

        void Translate(Vector2 translation);
        void EndTranslation();
        void Rotate(float rotation);
        void EndRotation();
    }
}
