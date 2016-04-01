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

        public Road()
        {
            startPoint = new Vector2D(0, 0);
            endPoint = new Vector2D(0, 20);
            name = "Example Ln";
        }

        public void Draw(ref Graphics graphics, Vector2D origin)
        {
            graphics.DrawLine(Pens.White,
                (float)origin.x + (float)startPoint.x,
                (float)origin.y + (float)startPoint.y,
                (float)origin.x + (float)endPoint.x,
                (float)origin.y + (float)endPoint.y);
        }
    }
}
