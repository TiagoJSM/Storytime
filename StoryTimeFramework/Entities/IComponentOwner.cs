﻿using StoryTimeCore.Physics;
using StoryTimeFramework.WorldManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeFramework.Entities
{
    public interface IComponentOwner
    {
        Scene Scene { get; }
        IBody Body { get; }
    }
}
