﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;

namespace StoryTimeCore.General
{
    public interface IBoundingBoxable
    {
        AxisAlignedBoundingBox2D AABoundingBox { get; }
        BoundingBox2D BoundingBox { get; }
    }
}
