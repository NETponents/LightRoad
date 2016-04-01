using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LightRoad.Geometry;

namespace LightRoad
{
    public class World
    {
        List<IWorldElement> roads = new List<IWorldElement>();
        List<IWorldElement> vehicles = new List<IWorldElement>();

        public World()
        {

        }
        public void drawWorld(ref Graphics drawSurface, Vector2D origin)
        {
            foreach(IWorldElement i in roads)
            {
                i.Draw(ref drawSurface, origin);
            }
            foreach(IWorldElement i in vehicles)
            {
                i.Draw(ref drawSurface, origin);
            }
        }
        public void addRoad(IWorldElement e)
        {
            roads.Add(e);
        }
        public void addVehicle(IWorldElement e)
        {
            vehicles.Add(e);
        }
        public void step()
        {
            foreach(Vehicles.Vehicle i in vehicles)
            {
                i.Travel();
            }
        }
    }
}
