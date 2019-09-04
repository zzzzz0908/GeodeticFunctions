using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions
{
    public struct PointLatLong
    {
        public PointLatLong(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //TODO: ограничение значений по градусам ?
    }
}
