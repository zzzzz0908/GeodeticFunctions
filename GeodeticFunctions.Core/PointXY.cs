using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions.Core;

public struct PointXY
{
    public PointXY(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }
}

public record struct PointNE (double Northing, double Easting) { }
