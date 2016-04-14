using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LightRoad.Geometry;

namespace LightRoad
{
    public interface IWorldElement
    {
        void Draw(Graphics graphics, Vector2D origin, float scale);
    }
}
