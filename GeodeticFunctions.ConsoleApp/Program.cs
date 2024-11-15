using GeodeticFunctions.Core;

var ellipsoid = new Ellipsoid(Ellipsoids.Krasovskiy);
var converter = new CoordinateConverter(ellipsoid);

var deltaX = -9214.69;
var deltaY = 6_300_000;



var sourcePoint = new PointNE(5313937.778, 6295607.862);
var point = new PointNE(5313937.778 - deltaX, 6295607.862 - deltaY);
var L0 = 38.5 / 180 * Math.PI;


var geodetic = converter.PlaneToGeodetic(point, L0);
var newPlanePoint = converter.GeodeticToPlane(geodetic, L0);

geodetic.Longitude = geodetic.Longitude / Math.PI *180;
geodetic.Latitude = geodetic.Latitude / Math.PI * 180;

newPlanePoint.Northing += deltaX;
newPlanePoint.Easting += deltaY;

Console.WriteLine(sourcePoint);
Console.WriteLine(geodetic);
Console.WriteLine(newPlanePoint);