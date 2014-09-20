using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Models;

namespace StoryTimeDevKit.Controls.GameObjects
{
    public interface IGameObjectsControl
    {
        TreeView TreeView { get; }
    }
}
