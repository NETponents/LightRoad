using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LightRoad
{
    public static class WorldLoader
    {
        public static void LoadWorld(out World world, string roadFilename, string intersectionsFilename)
        {
            world = new World();
            StreamReader sr = File.OpenText(roadFilename);
            while(!sr.EndOfStream)
            {
                string[] data = sr.ReadLine().Split(',');
                float x1 = (float)Convert.ToDouble(data[0]);
                float y1 = (float)Convert.ToDouble(data[1]);
                float x2 = (float)Convert.ToDouble(data[2]);
                float y2 = (float)Convert.ToDouble(data[3]);
                string streetName = data[4];
                world.addRoad(new Road(new Geometry.Vector2D(x1, y1), new Geometry.Vector2D(x2, y2), streetName));
            }
            StreamReader sr2 = File.OpenText(intersectionsFilename);
            while(!sr2.EndOfStream)
            {
                string[] data = sr2.ReadLine().Split(',');
                float x = Convert.ToInt32(data[0]);
                float y = Convert.ToInt32(data[1]);
                world.addIntersection(new Intersection(new Geometry.Vector2D(x, y), ref world));
            }
        }
    }
}
