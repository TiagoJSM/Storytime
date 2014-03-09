using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;

namespace StoryTimeCore.Extensions
{
    public static class RectanglefExtensions
    {
        public static Rectanglef Combine(this Rectanglef rec1, Rectanglef rec2)
        {
            float combinedTop, combinedBottom, combinedLeft, combinedRight;

            combinedTop = rec1.Top.SmallerOrEqualThan(rec2.Top);
            combinedBottom = rec1.Bottom.BiggerOrEqualThan(rec2.Bottom);
            combinedLeft = rec1.Left.SmallerOrEqualThan(rec2.Left);
            combinedRight = rec1.Right.BiggerOrEqualThan(rec2.Right);

            return new Rectanglef(
                combinedLeft, 
                combinedTop, 
                combinedBottom - combinedTop, 
                combinedRight - combinedLeft
            ); 
        }
    }
}
