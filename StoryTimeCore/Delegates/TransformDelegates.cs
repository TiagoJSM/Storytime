using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.General;
using StoryTimeCore.Resources.Graphic;

namespace StoryTimeCore.Delegates
{
    public delegate void OnBoundingBoxChanges(IBoundingBoxable boundingBoxable);
    public delegate void OnRotationChanges(IPositionable positionable);
    public delegate void OnPositionChanges(IPositionable positionable);
}
