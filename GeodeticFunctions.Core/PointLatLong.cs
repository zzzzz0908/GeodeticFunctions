using System.Globalization;

namespace GeodeticFunctions.Core;

/// <summary>
/// 
/// </summary>
/// <param name="Latitude">Широта в радианах</param>
/// <param name="Longitude">Долгота в радианах</param>
public record struct PointLatLon(double Latitude, double Longitude, double Height = 0)
{
    public string ToDecimalDegreeString(int digits = 8)
    {
        var formatString = $"F{digits}";
        var degreeLat = Latitude / Math.PI * 180;
        var degreeLon = Longitude / Math.PI * 180;
        return $"{degreeLat.ToString(formatString, CultureInfo.InvariantCulture)}, {degreeLon.ToString(formatString, CultureInfo.InvariantCulture)}";
    }

    public string ToDegreeString(int secondsDigits = 3)
    {
        var lat = Latitude / Math.PI * 180;
        var lon = Longitude / Math.PI * 180;

        var formatString = $"F{secondsDigits}";

        var latSec = Math.Abs(lat) % 1.0 * 3600.0;
        var lonSec = Math.Abs(lon) % 1.0 * 3600.0;

        var formattedLat = FormattableString.Invariant($@"{Math.Abs((int)lat)}°{(int)latSec / 60:00}'{(latSec % 60).ToString(formatString, CultureInfo.InvariantCulture)}""{(lat >= 0 ? "N" : "S")}");
        var formattedLon = FormattableString.Invariant($@"{Math.Abs((int)lon)}°{(int)lonSec / 60:00}'{(lonSec % 60).ToString(formatString, CultureInfo.InvariantCulture)}""{(lon >= 0 ? "E" : "W")}");

        return $"{formattedLat}  {formattedLon}";
    }
}

//TODO: ограничение значений по градусам ?