using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeodeticFunctions.Core;

public class CK63CoordinateConverter
{    
    private readonly GaussKrugerCoordinateConverter _GKconverter;
    private readonly SpatialCoordinateConverter _spatialConverter;
    private readonly CK63RegionParameters _region;
    private readonly int _zone;

    public CK63CoordinateConverter(string regionName, int zone) : base()
    {
        var region = CK63Regions.Regions.FirstOrDefault(x => x.Region == regionName);
        _region = region ?? throw new ArgumentException("Неизвестный регион");
        _zone = zone;

        var ell = new Ellipsoid(Ellipsoids.Krasovskiy);
        _GKconverter = new GaussKrugerCoordinateConverter(ell);
        _spatialConverter = new SpatialCoordinateConverter(ell);
    }


    public PointNE GeodeticToPlane(PointLatLon inputPoint)
    {
        var L0 = _region.CentralMeridian + 3 * (_zone - 1);
        L0 = L0.ToRad();

        var point = _GKconverter.GeodeticToPlane(inputPoint, L0);

        point.Northing += _region.FalseNorthing;
        point.Easting += _region.FalseEasting + 1_000_000 * _zone;

        return point;
    }

    public PointLatLon PlaneToGeodetic(PointNE inputPoint)
    {
        var L0 = _region.CentralMeridian + 3 * (_zone - 1);
        L0 = L0.ToRad();

        inputPoint.Northing -= _region.FalseNorthing;
        inputPoint.Easting -= _region.FalseEasting + 1_000_000 * _zone;

        return _GKconverter.PlaneToGeodetic(inputPoint, L0);
    }
}

public record CK63RegionParameters (string Region, double CentralMeridian, double FalseNorthing, double FalseEasting) { }

public static class CK63Regions
{
    public static readonly List<CK63RegionParameters> Regions = new()
    {
        new ("X", 23.5, -9214.69, 300_000)
    };
}
