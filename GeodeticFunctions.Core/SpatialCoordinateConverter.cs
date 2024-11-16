using static System.Math;

namespace GeodeticFunctions.Core;

public class SpatialCoordinateConverter
{
    private readonly Ellipsoid ell;

    public SpatialCoordinateConverter(Ellipsoid ellipsoid)
    {
        ell = ellipsoid;
    }

    public Point3D GeodeticToSpatial(PointLatLon inputPoint)
    {
        return GeodeticToSpatial(inputPoint.Latitude, inputPoint.Longitude, inputPoint.Height);
    }

    public PointLatLon SpatialToGeodetic(Point3D inputPoint)
    {
        var (lat, lon, h) = SpatialToGeodetic(inputPoint.X, inputPoint.Y, inputPoint.Z);

        return new PointLatLon(lat, lon, h);
    }


    private Point3D GeodeticToSpatial(double lat, double lon, double height)
    {
        double N = ell.RadiusN(lat);

        double x = (N + height) * Cos(lat) * Cos(lon);
        double y = (N + height) * Cos(lat) * Sin(lon);
        double z = ((1 - ell.e1_2) * N + height) * Sin(lat);

        return new Point3D(x, y, z);
    }

    private (double lat, double lon, double height) SpatialToGeodetic(double x, double y, double z)
    {
        var lon = Atan2(y, x);

        var Q = Sqrt(x * x + y * y);

        double lat = Atan(z / (Q * (1 - ell.e1_2)));
        double newLat;
        double delta;

        var i = 0;
        do
        {
            var N = ell.RadiusN(lat);
            var T = z + N * ell.e1_2 * Sin(lat);

            newLat = Atan(T / Q);

            delta = newLat - lat;
            lat = newLat;

            i++;
            if (i > 256) throw new ArithmeticException("Итерации не сходятся");
        }
        while (Abs(delta) > 1E-12);

        var h = Q * Cos(lat) + z * Sin(lat) - ell.RadiusN(lat) * (1 - ell.e1_2 * Pow(Sin(lat), 2));


        return (lat, lon, h);
    }
}
