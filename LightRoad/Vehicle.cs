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
            protected World worldRef;

            private Engine engine;
            private string currentStreet = "EMPTY";

            public Vehicle(World worldPointer)
            {
                vName = "Unnamed";
                vPosition = new Vector2D(0.0f, 0.0f);
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 5;
                vHeight = 5;
                worldRef = worldPointer;
            }
            public Vehicle(World worldPointer, Vector2D position)
            {
                vName = "Unnamed";
                vPosition = position;
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 5;
                vHeight = 5;
                worldRef = worldPointer;
            }
            public Vehicle(World worldPointer, Vector2D position, string name)
            {
                vName = name;
                vPosition = position;
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 5;
                vHeight = 5;
                worldRef = worldPointer;
            }
            public BoundingBox2D getBoundingBox()
            {
                return new BoundingBox2D(vPosition.x, vPosition.y, vWidth, vHeight);
            }
            public virtual void Travel()
            {
                vPosition.y += 1;
                if(currentStreet != this.getCurrentStreetName())
                {
                    currentStreet = this.getCurrentStreetName();
                    Console.WriteLine(String.Format("Vehicle {0} is now travelling on {1} at {2} MPH.", vName, currentStreet, engine.getSpeed()));
                }
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
                impactDirection %= 360;
            }
            public void Draw(Graphics graphics, Vector2D origin)
            {
                Rectangle rectangle = new Rectangle((int)origin.x + (int)vPosition.x, (int)origin.y + (int)vPosition.y, (int)vWidth, (int)vHeight);
                graphics.DrawRectangle(Pens.Red, rectangle);
                graphics.DrawString(this.getSpeed() + " MPH", SystemFonts.DefaultFont, Brushes.White, new PointF((int)origin.x + (float)vPosition.x, (int)origin.y + (float)vPosition.y - 20));
                graphics.DrawString(vName, SystemFonts.DefaultFont, Brushes.White, new PointF((int)origin.x + (float)vPosition.x, (int)origin.y + (float)vPosition.y - 40));
            }
            public string getCurrentStreetName()
            {
                Road r = getCurrentStreetHandle();
                if(r == null)
                {
                    return "OFFROAD";
                }
                else
                {
                    return r.getName();
                }
            }
            public Road getCurrentStreetHandle()
            {
                foreach (IWorldElement i in worldRef.getRoads())
                {
                    Road r = i as Road;
                    if (this.getBoundingBox().Intersects(r.getLine()))
                    {
                        return r;
                    }
                }
                return null;
            }
        }
    }
}
