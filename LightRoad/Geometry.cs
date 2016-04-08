using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
            public static Vector2D operator + (Vector2D a, Vector2D b)
            {
                return new Vector2D(a.x + b.x, a.y + b.y);
            }
            public static Vector2D operator - (Vector2D a, Vector2D b)
            {
                return new Vector2D(a.x - b.x, a.y - b.y);
            }
            public static bool operator == (Vector2D a, Vector2D b)
            {
                if(a.x == b.x && a.y == b.y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static bool operator != (Vector2D a, Vector2D b)
            {
                if(a == b)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            public Point ToPoint()
            {
                Point p = new Point((int)x, (int)y);
                return p;
            }
            public PointF ToPointF()
            {
                PointF p = new PointF((float)x, (float)y);
                return p;
            }
            public override string ToString()
            {
                return String.Format("({0}, {1})", x, y);
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
            public BoundingBox2D(Vector2D startPosition, double alength)
            {
                x1 = startPosition.x;
                y1 = startPosition.y;
                width = alength;
                height = alength;
            }
            public BoundingBox2D(Vector2D startPosition, double awidth, double aheight)
            {
                x1 = startPosition.x;
                y1 = startPosition.y;
                width = awidth;
                height = aheight;
            }
            public BoundingBox2D(Vector2D startPosition, Vector2D endPosition)
            {
                x1 = startPosition.x;
                width = endPosition.x - x1;
                y1 = startPosition.y;
                height = endPosition.y - y1;
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
            public bool Intersects(Line r)
            {
                Line l1 = new Line(x1, y1, x2, y1);
                if(r.Intersection(l1))
                {
                    return true;
                }
                l1 = new Line(x2, y1, x2, y2);
                if(r.Intersection(l1))
                {
                    return true;
                }
                l1 = new Line(x2, y2, x1, y2);
                if(r.Intersection(l1))
                {
                    return true;
                }
                l1 = new Line(x1, y2, x1, y2);
                if(r.Intersection(l1))
                {
                    return true;
                }
                return false;
            }
        }
        public struct Line
        {
            Vector2D startPos;
            Vector2D endPos;

            public Line(Vector2D start, Vector2D end)
            {
                startPos = start;
                endPos = end;
            }
            public static Line operator + (Line a, Vector2D b)
            {
                a.startPos += b;
                a.endPos += b;
                return a;
            }
            public static Line operator - (Line a, Vector2D b)
            {
                a.startPos -= b;
                a.endPos -= b;
                return a;
            }
            public Line(double x1, double y1, double x2, double y2)
            {
                startPos = new Vector2D(x1, y1);
                endPos = new Vector2D(x2, y2);
            }
            public PointF startPointF()
            {
                return new PointF((float)startPos.x, (float)startPos.y);
            }
            public PointF endPointF()
            {
                return new PointF((float)endPos.x, (float)endPos.y);
            }
            public bool Intersection(Line l)
            {
                return Collisions.LineIntersects(startPos, endPos, l.startPos, l.endPos);
            }
        }
        static class Collisions
        {
            public static bool LineIntersects(Vector2D a1, Vector2D a2, Vector2D b1, Vector2D b2/*, out Vector2D intersection*/)
            {
                //intersection = new Vector2D(0, 0);

                Vector2D b = a2 - a1;
                Vector2D d = b2 - b1;
                float bDotDPerp = (float)(b.x * d.y - b.y * d.x);

                // if b dot d == 0, it means the lines are parallel so have infinite intersection points
                if (bDotDPerp == 0)
                    return false;

                Vector2D c = b1 - a1;
                float t = (float)(c.x * d.y - c.y * d.x) / bDotDPerp;
                if (t < 0 || t > 1)
                    return false;

                float u = (float)(c.x * b.y - c.y * b.x) / bDotDPerp;
                if (u < 0 || u > 1)
                    return false;

                //intersection = a1 + t * b;

                return true;
            }
        }
    }
}
