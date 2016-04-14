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
            protected AI.AI aiEngine;
            protected bool vCrashed
            {
                get
                {
                    return hasCrashed;
                }
                set
                {
                    if(value)
                    {
                        Console.WriteLine(String.Format("Vehicle {0} has crashed.", vName));
                    }
                    hasCrashed = value;
                }
            }

            private bool hasCrashed = false;
            private bool waitingOnLight = false;
            private Engine engine;
            private string currentStreet
            {
                get
                {
                    return vCurrentStreet;
                }
                set
                {
                    Console.WriteLine(String.Format("Vehicle {0} is now travelling on {1} at {2} MPH.", vName, value, engine.getSpeed()));
                    vCurrentStreet = value;
                }
            }
            private string vCurrentStreet = "EMPTY";
            private int crashDeadlockCheck = 0;

            public Vehicle(World worldPointer)
            {
                vName = "Unnamed";
                vPosition = new Vector2D(0.0f, 0.0f);
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 4;
                vHeight = 4;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(1);
                aiEngine = new AI.AIRandom();
            }
            public Vehicle(World worldPointer, Vector2D position)
            {
                vName = "Unnamed";
                vPosition = position;
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 4;
                vHeight = 4;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(1);
                aiEngine = new AI.AIRandom();
            }
            public Vehicle(World worldPointer, Vector2D position, string name)
            {
                vName = name;
                vPosition = position;
                vTravelDirection = 0.0f;
                engine = new Engine();
                vWidth = 4;
                vHeight = 4;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(1);
                aiEngine = new AI.AIRandom();
            }
            public Vehicle(World worldPointer, Vector2D position, string name, float direction)
            {
                vName = name;
                vPosition = position;
                vTravelDirection = direction;
                engine = new Engine();
                vWidth = 4;
                vHeight = 4;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(1);
                aiEngine = new AI.AIRandom();
            }
            public Vehicle(World worldPointer, Vector2D position, string name, float direction, int navRouteNumber)
            {
                vName = name;
                vPosition = position;
                vTravelDirection = direction;
                engine = new Engine();
                vWidth = 4;
                vHeight = 4;
                worldRef = worldPointer;
                navigationWaypoints = generateNavigationRoute(navRouteNumber);
                aiEngine = new AI.AIRandom();
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
            public virtual void initStep()
            {
                foreach (IWorldElement i in worldRef.getIntersections())
                {
                    if (this.getBoundingBox().Intersects((i as Intersection).getBoundingBox()))
                    {
                        currentStreet = aiEngine.getNextTurn(ref worldRef, (i as Intersection), currentStreet);
                    }
                }
            }
            public virtual void Travel()
            {
                bool moveBlocked = false;
                if (vCrashed)
                {
                    return;
                }
                if (crashDeadlockCheck >= 3)
                {
                    vCrashed = true;
                    return;
                }
                foreach (IWorldElement i in worldRef.getIntersections())
                {
                    if (this.getBoundingBox().Intersects((i as Intersection).getBoundingBox()))
                    {
                        if (!waitingOnLight || currentStreet == "")
                        {
                            currentStreet = aiEngine.getNextTurn(ref worldRef, (i as Intersection), currentStreet);
                            //vTravelDirection = (i as Intersection).getRoadDirection(currentStreet);
                        }
                        if ((i as Intersection).lightColor(currentStreet) == StopLightColor.RED)
                        {
                            waitingOnLight = true;
                            // Do nothing until light turns green.
                            engine.setSpeed(0);
                            moveBlocked = true;
                            break;
                        }
                        else if((i as Intersection).lightColor(currentStreet) == StopLightColor.GREEN)
                        {
                            waitingOnLight = false;
                            engine.setSpeed(1);
                            vTravelDirection = (i as Intersection).getRoadDirection(currentStreet);
                            vPosition = (i as Intersection).getCenterPosition();
                            moveInCurrentDirection(4 + ((float)Math.Max(vWidth, vHeight) / 2));
                            break;
                        }
                    }
                }
                if (!moveBlocked)
                {
                    moveInCurrentDirection(1.0f);
                }
            }
            public void moveInCurrentDirection(float amount)
            {
                Vector2D oldPosition = vPosition;
                amount *= (float)engine.getSpeed();
                if (vTravelDirection == 0)
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
                //engine.setSpeed(amount);
                //if(this.collidesWithVehicle())
                //{
                //    vPosition = oldPosition;
                //    engine.setSpeed(0);
                //    crashDeadlockCheck += 1;
                //}
                //else
                //{
                    crashDeadlockCheck = 0;
                //}
            }
            private bool collidesWithVehicle()
            {
                foreach (Vehicle v in worldRef.getVehicles())
                {
                    if(v.Equals(this))
                    {
                        continue;
                    }
                    if (this.getBoundingBox().Intersects(v.getBoundingBox()))
                    {
                        return true;
                    }
                }
                return false;
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
            public void Draw(Graphics graphics, Vector2D origin, float scale)
            {
                Font font = new Font(FontFamily.GenericSansSerif, 5 * scale);
                Rectangle r = Overloads.MultiplyPointsRect(new Rectangle((int)vRootPosition.x, (int)vRootPosition.y, (int)vWidth, (int)vHeight), scale);
                Rectangle rectangle = Overloads.AddVector2DRect(r, origin);
                graphics.DrawRectangle(Pens.Red, rectangle);
                graphics.DrawString(this.getSpeed() + " MPH", font, Brushes.White, new PointF((int)origin.x + (float)vRootPosition.x, (int)origin.y + (float)vRootPosition.y - 20));
                if (vCrashed)
                {
                    graphics.DrawString(vName + "(CRASHED)", font, Brushes.White, new PointF((int)origin.x + (float)vRootPosition.x, (int)origin.y + (float)vRootPosition.y - 40));
                }
                else
                {
                    graphics.DrawString(vName, font, Brushes.White, new PointF((int)origin.x + (float)vRootPosition.x, (int)origin.y + (float)vRootPosition.y - 40));
                }
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
