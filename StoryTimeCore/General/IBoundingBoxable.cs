﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;

namespace StoryTimeCore.General
{
    public interface IBoundingBoxable
    {
        Rectanglef BoundingBox { get; }
    }
}
