using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace StoryTimeUI.Interfaces
{
    public interface IParent
    {
        Vector2 Position { get; set; }
        ObservableCollection<BaseWidget> Children { get; }
    }
}
