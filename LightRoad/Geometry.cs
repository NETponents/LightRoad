using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightRoad
{
    namespace Geometry
    {
        public struct Vector2D
        {
            public double x;
            public double y;

            public Vector2D(double ax, double ay)
            {
                x = ax;
                y = ay;
            }
        }
        public struct BoundingBox2D
        {
            public double x1;
            public double y1;
            public double x2
            {
                get
                {
                    return x1 + width;
                }
            }
            public double y2
            {
                get
                {
                    return y1 + height;
                }
            }
            public double width;
            public double height;

            public BoundingBox2D(double ax, double ay, double awidth, double aheight)
            {
                x1 = ax;
                y1 = ay;
                width = awidth;
                height = aheight;
            }
            /// <summary>
            /// Checks to see if the current instance of BoundingBox2D intersects another BoundingBox2D.
            /// </summary>
            /// <param name="bb2">BoundingBox2D to compare.</param>
            /// <returns>If an intersection is present.</returns>
            public bool Intersects(BoundingBox2D bb2)
            {
                if((bb2.x2 >= x1 && bb2.x1 <= x2) && (bb2.y2 >= y1 && bb2.y1 <= y2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
