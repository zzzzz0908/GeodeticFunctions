﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions
{
    public struct PointXY
    {
        public PointXY(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y{ get; set; }
    }
}
