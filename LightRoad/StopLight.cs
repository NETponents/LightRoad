using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LightRoad.Geometry;

namespace LightRoad
{
    public class StopLight
    {
        private StopLightColor currentColor;
        int currentSecondCounter = 0;
        int switchSecond = 4;
        Line drawableElement;

        public StopLight(Line drawableLocation)
        {
            drawableElement = drawableLocation;
            currentColor = StopLightColor.RED;
        }
        public StopLight(Line drawableLocation, int switchTime)
        {
            drawableElement = drawableLocation;
            currentColor = StopLightColor.RED;
            switchSecond = switchTime;
        }
        public StopLight(Line drawableLocation, int switchTime, int currentStep)
        {
            drawableElement = drawableLocation;
            currentColor = StopLightColor.RED;
            switchSecond = switchTime;
            currentSecondCounter = currentStep;
        }
        public StopLight(Line drawableLocation, int switchTime, int currentStep, StopLightColor startColor)
        {
            drawableElement = drawableLocation;
            currentColor = startColor;
            switchSecond = switchTime;
            currentSecondCounter = currentStep;
        }
        public void Draw(Graphics graphics, Vector2D origin)
        {
            Line drawItem = drawableElement + origin;
            graphics.DrawLine(this.getDrawableColor(), drawItem.startPointF(), drawItem.endPointF());
        }
        public void pulseSecond()
        {
            currentSecondCounter++;
            if(currentSecondCounter >= switchSecond)
            {
                currentSecondCounter = 1;
                switchColor();
            }
        }
        public bool isClear()
        {
            if (currentColor == StopLightColor.GREEN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public StopLightColor getColor()
        {
            return currentColor;
        }
        private void switchColor()
        {
            if(currentColor == StopLightColor.GREEN)
            {
                currentColor = StopLightColor.RED;
            }
            else if(currentColor == StopLightColor.RED)
            {
                currentColor = StopLightColor.GREEN;
            }
        }
        private Pen getDrawableColor()
        {
            if(currentColor == StopLightColor.RED)
            {
                return Pens.Red;
            }
            else if(currentColor == StopLightColor.GREEN)
            {
                return Pens.Green;
            }
            else
            {
                return Pens.White;
            }
        }
    }
    public enum StopLightColor
    {
        RED,
        GREEN
    }
}
