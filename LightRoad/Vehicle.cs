using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightRoad.Geometry;

namespace LightRoad
{
    namespace Vehicles
    {
        public class Vehicle : IWorldElement
        {
            protected string vName;
            protected Vector2D vPosition;
            protected double vTravelDirection;
            protected double vWidth;
            protected double vHeight;

            private Engine engine;

            public Vehicle()
            {
                vName = "Unnamed";
                vPosition = new Vector2D(0.0f, 0.0f);
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 5;
                vHeight = 5;
            }
            public BoundingBox2D getBoundingBox()
            {
                return new BoundingBox2D(vPosition.x, vPosition.y, vWidth, vHeight);
            }
            public virtual void Travel()
            {
                vPosition.y += 1;
                Console.WriteLine(vPosition.ToString());
            }
            public string getName()
            {
                return vName;
            }
            public Vector2D getPosition()
            {
                return vPosition;
            }
            public double getSpeed()
            {
                return engine.getSpeed();
            }
            public void Crash(double impactSpeed, double impactDirection)
            {
                //TODO: implement impact response code
                impactSpeed %= 360;
            }
            public void Draw(ref Graphics graphics, Vector2D origin)
            {
                Rectangle rectangle = new Rectangle((int)origin.x + (int)vPosition.x, (int)origin.y + (int)vPosition.y, (int)vWidth, (int)vHeight);
                graphics.DrawRectangle(Pens.Red, rectangle);
                graphics.DrawString(this.getSpeed() + " MPH", SystemFonts.DefaultFont, Brushes.White, new PointF((int)origin.x + (float)vPosition.x, (int)origin.y + (float)vPosition.y - 20));
            }
        }
    }
}
