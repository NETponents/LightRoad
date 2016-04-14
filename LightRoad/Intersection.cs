using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightRoad.Geometry;

namespace LightRoad
{
    public class Intersection : IWorldElement
    {
        private Vector2D position
        {
            get
            {
                return new Vector2D(center.x - (int)(size / 2), center.y - (int)(size / 2));
            }
        }
        private Vector2D center;
        private List<Road> connectors;
        private List<float> connDirection;
        private List<StopLight> connStopLights;
        private const int size = 8;

        public Intersection(Vector2D centerPosition, ref World world)
        {
            connDirection = new List<float>();
            connectors = new List<Road>();
            connStopLights = new List<StopLight>();
            center = centerPosition;
            Line n = new Line(position, new Vector2D(position.x + size, position.y));
            Line e = new Line(new Vector2D(position.x + size, position.y), new Vector2D(position.x + size, position.y + size));
            Line s = new Line(new Vector2D(position.x + size, position.y + size), new Vector2D(position.x, position.y + size));
            Line w = new Line(new Vector2D(position.x, position.y + size), position);
            connStopLights.Add(new StopLight(n, 4, 1, StopLightColor.GREEN));
            connStopLights.Add(new StopLight(e, 4, 1));
            connStopLights.Add(new StopLight(s, 4, 1, StopLightColor.GREEN));
            connStopLights.Add(new StopLight(w, 4, 1));
            foreach (IWorldElement i in world.getRoads())
            {
                Road r = i as Road;
                Line rd = r.getLine();
                if(n.Intersection(rd))
                {
                    connectors.Add(r);
                    connDirection.Add(0);
                }
                if (e.Intersection(rd))
                {
                    connectors.Add(r);
                    connDirection.Add(90);
                }
                if (s.Intersection(rd))
                {
                    connectors.Add(r);
                    connDirection.Add(180);
                }
                if (w.Intersection(rd))
                {
                    connectors.Add(r);
                    connDirection.Add(270);
                }
            }
        }
        public BoundingBox2D getBoundingBox()
        {
            return new BoundingBox2D(position, size);
        }
        public void Draw(Graphics graphics, Vector2D origin, float scale)
        {
            //Rectangle rectangle = new Rectangle((int)origin.x + (int)position.x, (int)origin.y + (int)position.y, size, size);
            //graphics.DrawRectangle(Pens.Blue, rectangle);
            foreach(StopLight sl in connStopLights)
            {
                sl.Draw(graphics, origin, scale);
            }
        }
        public Vector2D getCenterPosition()
        {
            return center;
        }
        public float getRoadDirection(string streetName)
        {
            foreach(Road i in connectors)
            {
                if(i.getName() == streetName)
                {
                    int index = connectors.IndexOf(i);
                    return connDirection[index];
                }
            }
            return 0;
        }
        public List<string> getConnectedRoads()
        {
            List<string> roads = new List<string>();
            foreach(Road i in connectors)
            {
                roads.Add(i.getName());
            }
            return roads;
        }
        public StopLightColor lightColor(string currentStreet)
        {
            foreach(Road r in connectors)
            {
                if(r.getName() == currentStreet)
                {
                    return connStopLights[connectors.IndexOf(r)].getColor();
                }
            }
            return StopLightColor.RED;
        }
        public void pulseStopLights()
        {
            foreach(StopLight sl in connStopLights)
            {
                sl.pulseSecond();
            }
        }
    }
}
