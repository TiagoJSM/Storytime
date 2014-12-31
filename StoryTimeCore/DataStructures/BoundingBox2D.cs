using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeCore.DataStructures
{
    public struct BoundingBox2D
    {
        private Vector2[] _points;

        public Vector2 Point1 
        { 
            get 
            {
                return _points[0]; 
            } 
        }
        public Vector2 Point2
        {
            get
            {
                return _points[1];
            }
        }
        public Vector2 Point3
        {
            get
            {
                return _points[2]; 
            }
        }
        public Vector2 Point4
        {
            get
            {
                return _points[3]; 
            }
        }

        public Vector2 Center
        {
            get
            {
                var minX = _points.Min(p => p.X);
                var maxX = _points.Max(p => p.X);
                var minY = _points.Min(p => p.Y);
                var maxY = _points.Max(p => p.Y);

                return new Vector2((maxX + minX) / 2, (maxY + minY) / 2);
            }
        }

        public BoundingBox2D(Vector2 point)
        {
            _points = new Vector2[] { point, point, point, point };
        }

        public BoundingBox2D(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            _points = new Vector2[] { point1, point2, point3, point4 };
        }

        public void Translate(Vector2 translation)
        {
            for (var idx = 0; idx < _points.Length; idx++)
                _points[idx] += translation;
        }

        public bool Contains(Vector2 point)
        {
            var j = 0;
            var i = 1;
            var inFlag = false;
            while(i <= _points.Length)
            {
                var iAux = i;
                if (iAux == _points.Length)
                    iAux = 0;
                var pointJ = _points[j];
                var pointI = _points[iAux];

                if( 
                    (
                        (pointI.Y <= point.Y && 
                        point.Y < pointJ.Y) ||
                        (pointJ.Y <= point.Y &&
                        point.Y < pointI.Y)
                    )
                    &&
                    (
                        point.X < ((pointJ.X - pointI.X) * (point.Y - pointI.Y) / (pointJ.Y - pointI.Y) + pointI.X)
                    ))
                    inFlag = !inFlag;

                j = i;
                i = i + 1;
            }
            return inFlag;
        }

        public void Transform(Matrix matrix)
        {
            _points[0] = Vector3.Transform(Point1.ToVector3(), matrix).ToVector2();
            _points[1] = Vector3.Transform(Point2.ToVector3(), matrix).ToVector2();
            _points[2] = Vector3.Transform(Point3.ToVector3(), matrix).ToVector2();
            _points[3] = Vector3.Transform(Point4.ToVector3(), matrix).ToVector2();
        }
    }
}
