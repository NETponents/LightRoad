using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightRoad
{
    namespace AI
    {
        public class AIRandom : AI
        {
            private static Random rd;
            public AIRandom()
            {
                rd = new Random();
            }

            public override string getNextTurn(ref World world, Intersection currentIntersection, string currentStreet)
            {
                List<string> options = currentIntersection.getConnectedRoads();
                if(options.Count == 1)
                {
                    return options[0];
                }
                int choice = -1;
                do
                {
                    choice = rd.Next(0, options.Count);
                } while (options[choice] == currentStreet);
                return options[choice];
            }

            public override void initializeRoute()
            {
                throw new NotImplementedException();
            }
        }
    }
}
