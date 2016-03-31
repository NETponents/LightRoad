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
            }
            public BoundingBox2D getBoundingBox()
            {
                return new BoundingBox2D(vPosition.x, vPosition.y, vWidth, vHeight);
            }
            public virtual void Travel()
            {

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

            public Bitmap getDrawableElement()
            {
                throw new NotImplementedException();
            }

            public Vector2D getDrawableXY()
            {
                throw new NotImplementedException();
            }
        }
    }
}
