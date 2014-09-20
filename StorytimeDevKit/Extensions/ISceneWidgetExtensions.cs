using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;

namespace StoryTimeDevKit.Extensions
{
    public static class ISceneWidgetExtensions
    {
        public static List<ISceneWidget> GetAllIntersectedLeafChildren(this ISceneWidget widget, Vector2 point)
        {
            if (widget.ChildrenCount == 0) return new List<ISceneWidget>();

            List<ISceneWidget> leafs = new List<ISceneWidget>();
            foreach (ISceneWidget childWidget in widget.Children)
            {
                if (!childWidget.Enabled) continue;
                leafs.AddRange(childWidget.GetAllIntersectedLeafs(point));
            }
            return leafs;
        }

        private static List<ISceneWidget> GetAllIntersectedLeafs(this ISceneWidget widget, Vector2 point)
        {
            if (widget.ChildrenCount == 0)
            {
                if(widget.Intersects(point))
                    return new List<ISceneWidget> { widget };
            }

            List<ISceneWidget> leafs = new List<ISceneWidget>();
            foreach (ISceneWidget childWidget in widget.Children)
            {
                if (!childWidget.Enabled) continue;
                leafs.AddRange(childWidget.GetAllIntersectedLeafs(point));
            }
            return leafs;
        }
    }
}
