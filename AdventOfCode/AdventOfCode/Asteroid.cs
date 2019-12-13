using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Asteroid
    {
        public int xpos;                // X coordinate of position
        public int ypos;                // Y coordinate of position
        public int xFromBase;           // Horizontal distance from the laser
        public int yFromBase;           // Vertical distance from the laser
        public double angleFromBase;    // Angle between laser's north and asteroid's position
        public int vaporizeOrder;       // Tracks when this asteroid blew up compared to the others

        public Asteroid(int x, int y, int baseX, int baseY)
        {
            xpos = x;
            ypos = y;

            xFromBase = baseX - x;
            yFromBase = baseY - y;

            // Set value for angleFromBase as a positive double between 0 and 360 degrees (0 = 360)

            if (yFromBase == 0)     // If denominator = 0, assign 90 if on right and 270 if on left
            {
                if (xFromBase < 0)
                {
                    angleFromBase = 90d;
                }
                else
                {
                    angleFromBase = 270d;
                }
            }

            else if (x == 0)        // If numerator = 0, assign 0 if above and 180 if below
            {
                if (yFromBase < 0)
                {
                    angleFromBase = 180;
                }
                else
                {
                    angleFromBase = 0;
                }
            }

            else
            {
                angleFromBase = Math.Atan(xFromBase / yFromBase);

                // In first quadrant; make positive (0 < α < 90)
                if (xFromBase < 0 && yFromBase > 0)
                {
                    angleFromBase *= -1;
                }

                // In second quadrant; do reciprocal and add 90 (90 < α < 180)
                else if ( xFromBase < 0 && yFromBase < 0)
                {
                    angleFromBase = Math.Pow(angleFromBase, -1) + 90;
                }

                // In third quadrant; make positive and add 180 (180 < α < 270)
                else if ( xFromBase > 0 && yFromBase < 0)
                {
                    angleFromBase = angleFromBase * -1 + 180;
                }

                // In fourth quadrant; do reciprocal and add 270 (270 < α < 360)
                else // ( xFromBase > 0 && yFromBase > 0)
                {
                    angleFromBase = Math.Pow(angleFromBase, -1) + 270;
                }
            }
        }
    }
}
