using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeUI.Delegates
{
    public delegate void OnStartDrag(Vector2 currentPosition);
    public delegate void OnDrag(Vector2 dragged, Vector2 currentPosition);
    public delegate void OnStopDrag(Vector2 startDrag, Vector2 currentPosition);
    public delegate void OnMouseDown(Vector2 mousePosition);
    public delegate void OnMouseUp(Vector2 mousePosition);

    //public delegate void OnTranslated(BaseActor actor, Vector2 startDragPosition, Vector2 currentPosition);
    //public delegate void OnRotated(BaseActor actor, float bodyStartRotation, float bodyRotation);
}
