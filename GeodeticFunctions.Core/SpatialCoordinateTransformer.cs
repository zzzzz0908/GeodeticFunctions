using static System.Math;

namespace GeodeticFunctions.Core;

public class SpatialCoordinateTransformer
{
    public Point3D Transform(Point3D inputPoint, SpatialTransformationParameters transformationParameters)
    {
        var (x1, y1, z1) = inputPoint;
        var (dX, dY, dZ, rX, rY, rZ, m) = transformationParameters;
        var scale = 1 + m * 1E-6;

        rX = rX / (180 * 3600) * PI;
        rY = rY / (180 * 3600) * PI;
        rZ = rZ / (180 * 3600) * PI;

        var x = (x1 + rZ * y1 - rY * z1) * scale;
        var y = (-rZ * x1 + y1 + rX * z1) * scale;
        var z = (rY * x1 - rX * y1 + z1) * scale;

        x += dX;
        y += dY;
        z += dZ;

        return new Point3D(x, y, z);
    }    
}

public record SpatialTransformationParameters(double dX, double dY, double dZ, double rX, double rY, double rZ, double m) { }
