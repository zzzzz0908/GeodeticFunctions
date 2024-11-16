﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions.Core;

public record struct PointXY (double X, double Y)
{

}

public record struct PointNE (double Northing, double Easting) { }
