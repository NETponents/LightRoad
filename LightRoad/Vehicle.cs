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
            protected Vector2D vRootPosition
            {
                get
                {
                    return vPosition - new Vector2D(vWidth / 2, vHeight / 2);
                }
            }
            protected double vTravelDirection;
            protected double vWidth;
            protected double vHeight;
            protected World worldRef;
            protected Queue<string> navigationWaypoints;

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
                navigationWaypoints = generateNavigationRoute(1);
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
                navigationWaypoints = generateNavigationRoute(1);
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
                navigationWaypoints = generateNavigationRoute(1);
            }
            public Vehicle(World worldPointer, Vector2D position, string name, float direction)
            {
                vName = name;
                vPosition = position;
                vTravelDirection = direction;
                engine = new Engine();
                vWidth = 5;
                vHeight = 5;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(1);
            }
            public Vehicle(World worldPointer, Vector2D position, string name, float direction, int navRouteNumber)
            {
                vName = name;
                vPosition = position;
                vTravelDirection = direction;
                engine = new Engine();
                vWidth = 5;
                vHeight = 5;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(navRouteNumber);
            }
            public Queue<string> generateNavigationRoute(int routeNumber)
            {
                Queue<string> route = new Queue<string>();
                if (routeNumber == 1)
                {
                    route.Enqueue("A1");
                    route.Enqueue("1B");
                    route.Enqueue("B2");
                    route.Enqueue("2C");
                }
                else if(routeNumber == 2)
                {
                    route.Enqueue("Turner Place");
                    route.Enqueue("Microsoft Circle");
                    route.Enqueue("Utopia Ln");
                }
                else if(routeNumber == 3)
                {
                    route.Enqueue("Microsoft Circle");
                    route.Enqueue("Utopia Ln");
                }
                return route;
            }
            public BoundingBox2D getBoundingBox()
            {
                return new BoundingBox2D(vRootPosition.x, vRootPosition.y, vWidth, vHeight);
            }
            public virtual void Travel()
            {
                foreach(IWorldElement i in worldRef.getIntersections())
                {
                    if(this.getBoundingBox().Intersects((i as Intersection).getBoundingBox()))
                    {
                        vPosition = (i as Intersection).getCenterPosition();
                        currentStreet = navigationWaypoints.Dequeue();
                        vTravelDirection = (i as Intersection).getRoadDirection(currentStreet);
                        moveInCurrentDirection((float)Math.Max(vWidth, vHeight));
                        break;
                    }
                }
                moveInCurrentDirection(1.0f);
            }
            public void moveInCurrentDirection(float amount)
            {
                if(vTravelDirection == 0)
                {
                    vPosition.y -= amount;
                }
                else if(vTravelDirection == 90)
                {
                    vPosition.x += amount;
                }
                else if(vTravelDirection == 180)
                {
                    vPosition.y += amount;
                }
                else if(vTravelDirection == 270)
                {
                    vPosition.x -= amount;
                }
                engine.setSpeed(amount);
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
                Rectangle rectangle = new Rectangle((int)origin.x + (int)vRootPosition.x, (int)origin.y + (int)vRootPosition.y, (int)vWidth, (int)vHeight);
                graphics.DrawRectangle(Pens.Red, rectangle);
                graphics.DrawString(this.getSpeed() + " MPH", SystemFonts.DefaultFont, Brushes.White, new PointF((int)origin.x + (float)vRootPosition.x, (int)origin.y + (float)vRootPosition.y - 20));
                graphics.DrawString(vName, SystemFonts.DefaultFont, Brushes.White, new PointF((int)origin.x + (float)vRootPosition.x, (int)origin.y + (float)vRootPosition.y - 40));
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
