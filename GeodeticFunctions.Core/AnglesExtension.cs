using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions.Core;
public static class AnglesExtension
{
    public static double ToDegrees(this double radians)
    {
        return radians / Math.PI * 180;
    }

    public static double ToRad(this double degrees)
    {
        return degrees / 180 * Math.PI;
    }
}
