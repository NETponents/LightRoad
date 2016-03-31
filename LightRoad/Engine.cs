using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightRoad
{
    namespace Vehicles
    {
        public class Engine
        {
            private double ecAccelerationSpeed;
            private double ecMaxSpeed;
            private double ecBreakFactor;

            private double eCurrentSpeed;

            public Engine()
            {
                //TODO: create default constructor for Engine class.
            }
            public double getSpeed()
            {
                return eCurrentSpeed;
            }
            public void Accelerate()
            {
                if(eCurrentSpeed + ecAccelerationSpeed < ecMaxSpeed)
                {
                    eCurrentSpeed += ecAccelerationSpeed;
                }
            }
            public void Brake()
            {
                eCurrentSpeed = Math.Round(eCurrentSpeed * ecBreakFactor, 1);
            }
        }
    }
}
