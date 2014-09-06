using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.SceneWidgets.Interfaces
{
    public interface ISceneWidget
    {
        event Action<Vector2> OnStartDrag;
        event Action<Vector2, Vector2> OnDrag;
        //Parameters: StartDrag, currentPosition
        event Action<Vector2, Vector2> OnStopDrag;

        event Action<bool> OnSelect;
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
