using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Delegates;

namespace StoryTimeDevKit.Entities.SceneWidgets.Interfaces
{
    public interface ISceneWidget
    {
        event OnStartDrag OnStartDrag;
        event OnDrag OnDrag;
        event OnStopDrag OnStopDrag;
        event Action<bool, ISceneWidget> OnSelect;
        event Action<bool> OnEnabled;

        IEnumerable<ISceneWidget> Children { get; }
        int ChildrenCount { get; }
        bool Selected { get; set; }
        bool Enabled { get; set; }

        bool Intersects(Vector2 point);
        void StartDrag(Vector2 currentPosition);
        void Drag(Vector2 dragged, Vector2 currentPosition);
        void StopDrag(Vector2 currentPosition);
    }
}
