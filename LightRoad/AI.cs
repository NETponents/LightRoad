using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightRoad
{
    namespace AI
    {
        public abstract class AI
        {
            public AI()
            {

            }
            public abstract void initializeRoute();
            public abstract string getNextTurn(ref World world, Intersection currentIntersection, string currentStreet);
        }
    }
}
