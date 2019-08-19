using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions
{
    public struct PointLatLong
    {
        public PointLatLong(double latitude, double longtitude)
        {
            Latitude = latitude;
            Longtitude = longtitude;
        }

        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        //TODO: ограничение значений по градусам ?
    }
}
