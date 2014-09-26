﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace StoryTimeUI.Interfaces
{
    public interface IParent
    {
        ObservableCollection<BaseWidget> Children { get; }
    }
}