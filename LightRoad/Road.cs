using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightRoad.Geometry;

namespace LightRoad
{
    public class Road : IWorldElement
    {
        Vector2D startPoint;
        Vector2D endPoint;
        string name;

        public Road(Vector2D startPos, Vector2D endPos, string streetName)
        {
            startPoint = startPos;
            endPoint = endPos;
            name = streetName;
        }

        public void Draw(Graphics graphics, Vector2D origin)
        {
            graphics.DrawLine(Pens.White,
                (float)origin.x + (float)startPoint.x,
                (float)origin.y + (float)startPoint.y,
                (float)origin.x + (float)endPoint.x,
                (float)origin.y + (float)endPoint.y);
        }
        public string getName()
        {
            return name;
        }
        public Line getLine()
        {
            return new Line(startPoint, endPoint);
        }
    }
}
