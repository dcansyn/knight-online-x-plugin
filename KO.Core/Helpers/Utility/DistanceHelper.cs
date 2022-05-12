using System;
using System.Drawing;

namespace KO.Core.Helpers.Utility
{
    public class DistanceHelper
    {
        public static int GetDistance(Point coor, Point goCoor)
        {
            return Convert.ToInt32(Math.Sqrt(Math.Pow(goCoor.X - coor.X, 2) + Math.Pow(goCoor.Y - coor.Y, 2)));
        }

        public static int GetDistance(int coorX, int coorY, int goCoorX, int goCoorY)
        {
            return Convert.ToInt32(Math.Sqrt(Math.Pow(goCoorX - coorX, 2) + Math.Pow(goCoorY - coorY, 2)));
        }
    }
}
