using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions.Core;

public record struct PointLatLong(double Latitude, double Longitude) { }

//TODO: ограничение значений по градусам ?