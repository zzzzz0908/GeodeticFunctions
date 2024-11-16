using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeodeticFunctions.Core;

/// <summary>
/// 
/// </summary>
/// <param name="Latitude">Широта в радианах</param>
/// <param name="Longitude">Долгота в радианах</param>
public record struct PointLatLong(double Latitude, double Longitude) 
{
    public string ToDecimalDegreeString(int digits = 8)
    {
        var formatString = $"F{digits}";

        return $"{Latitude.ToString(formatString, CultureInfo.InvariantCulture)}, {Longitude.ToString(formatString, CultureInfo.InvariantCulture)}";
    }

    public string ToDegreeString(int secondsDigits = 3)
    {
        var (lat, lon) = (Latitude, Longitude);

        var formatString = $"F{secondsDigits}";

        var latSec = Math.Abs(Latitude) % 1.0 * 3600.0;
        var lonSec = Math.Abs(Longitude) % 1.0 * 3600.0;

        var formattedLat = FormattableString.Invariant($@"{Math.Abs((int)lat)}°{(int)latSec / 60:00}'{(latSec % 60).ToString(formatString, CultureInfo.InvariantCulture)}""{(lat >= 0 ? "N" : "S")}");
        var formattedLon = FormattableString.Invariant($@"{Math.Abs((int)lon)}°{(int)lonSec / 60:00}'{(lonSec % 60).ToString(formatString, CultureInfo.InvariantCulture)}""{(lon >= 0 ? "E" : "W")}");
        
        return $"{formattedLat}  {formattedLon}";
    }
}

//TODO: ограничение значений по градусам ?