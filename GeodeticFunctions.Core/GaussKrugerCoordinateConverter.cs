using static System.Math;

namespace GeodeticFunctions.Core;


/// <summary>
/// Представляет методы для преобразования координат
/// </summary>
public class GaussKrugerCoordinateConverter
{
    private readonly Ellipsoid ell;


    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GaussKrugerCoordinateConverter"/>.
    /// </summary>
    /// <param name="ellipsoid"> Эллипсоид, на котором выполняется преобразование координат. </param>
    public GaussKrugerCoordinateConverter(Ellipsoid ellipsoid)
    {
        ell = ellipsoid;
    }

    public PointNE GeodeticToPlane(PointLatLon inputPoint, double L0)
    {
        var northing = GeodeticToPlaneX(inputPoint.Latitude, inputPoint.Longitude, L0);
        var easting = GeodeticToPlaneY(inputPoint.Latitude, inputPoint.Longitude, L0);

        return new PointNE(northing, easting);
    }

    public PointLatLon PlaneToGeodetic(PointNE inputPoint, double L0)
    {
        var latitude = PlaneToGeodeticLatitude(inputPoint.Northing, inputPoint.Easting);
        var longitude = PlaneToGeodeticlongitudeDelta(inputPoint.Northing, inputPoint.Easting) + L0;

        return new PointLatLon(latitude, longitude);
    }



    /// <summary>
    /// Возвращает значение плоской координаты X (Northing).
    /// </summary>
    /// <param name="B"> Широта точки в радианах. </param>
    /// <param name="L"> Долгота точки в радианах. </param>
    /// <param name="L0"> Долгота осевого меридиана в радианах. </param>
    /// <returns></returns>
    private double GeodeticToPlaneX(double B, double L, double L0)
    {
        double eta = ell.e2 * Cos(B);  //ell.GetEta(Bx);
        double N = ell.RadiusN(B);

        double a0 = ell.MeridianArc(0, B);

        double a2 = 1.0 / 2.0 * N * Sin(B) * Cos(B);

        double a4 = 1.0 / 24.0 * N * Sin(B) * Pow(Cos(B), 3)
            * (5 - Pow(Tan(B), 2) + 9 * Pow(eta, 2) + 4 * Pow(eta, 4));

        double a6 = 1.0 / 720.0 * N * Sin(B) * Pow(Cos(B), 5)
            * (61 - 58 * Pow(Tan(B), 2) + Pow(Tan(B), 4) + 270 * Pow(eta, 2) - 330 * Pow(eta * Tan(B), 2));

        double a8 = 1.0 / 40320.0 * N * Sin(B) * Pow(Cos(B), 7) * (1385 - 3111 * Pow(Tan(B), 2) + 543 * Pow(Tan(B), 4) - Pow(Tan(B), 6));


        double l = L - L0;

        double x = a0 + a2 * Pow(l, 2) + a4 * Pow(l, 4) + a6 * Pow(l, 6) + a8 * Pow(l, 8);

        return x;
    }


    /// <summary>
    /// Возвращает значение плоской координаты Y (Easting).
    /// </summary>
    /// <param name="B"> Широта точки в радианах. </param>
    /// <param name="L"> Долгота точки в радианах. </param>
    /// <param name="L0"> Долгота осевого меридиана в радианах. </param>
    /// <returns></returns>
    private double GeodeticToPlaneY(double B, double L, double L0)
    {
        double eta = ell.e2 * Cos(B);  //ell.GetEta(Bx);
        double N = ell.RadiusN(B);

        double b1 = ell.RadiusParallel(B);

        double b3 = 1.0 / 6.0 * N * Pow(Cos(B), 3) * (1 - Pow(Tan(B), 2) + Pow(eta, 2));

        double b5 = 1.0 / 120.0 * N * Pow(Cos(B), 5)
            * (5 - 18 * Pow(Tan(B), 2) + Pow(Tan(B), 4) + 14 * Pow(eta, 2)
            - 58 * Pow(eta * Tan(B), 2));

        double b7 = 1.0 / 5040.0 * N * Pow(Cos(B), 7)
            * (61 - 479 * Pow(Tan(B), 2) + 179 * Pow(Tan(B), 4) - Pow(Tan(B), 6));

        double l = L - L0;

        double y = b1 * l + b3 * Pow(l, 3) + b5 * Pow(l, 5) + b7 * Pow(l, 7); // добавлено * l

        return y;
    }



    /// <summary>
    /// Возвращает широту точки в радианах по плоским координатам.
    /// </summary>
    /// <param name="x"> Координата X (Northing).</param>
    /// <param name="y"> Координата Y (Easting).</param>
    /// <returns></returns>
    private double PlaneToGeodeticLatitude(double x, double y)
    {
        double Bx = ell.LatitudeCalc(x);
        double Nx = ell.RadiusN(Bx);
        double t = Tan(Bx);
        double eta = ell.e2 * Cos(Bx);  //ell.GetEta(Bx);
        double V2 = 1 + Pow(ell.e2 * Cos(Bx), 2);

        double A2 = -t * V2 / (2 * Pow(Nx, 2));

        double A4 = -A2 / (12.0 * Pow(Nx, 2)) * (5.0 + 3 * Pow(t, 2) + Pow(eta, 2) - 9.0 * Pow(eta, 2) * Pow(t, 2) - 4 * Pow(eta, 4));

        double A6 = A2 / (360 * Pow(Nx, 4)) *
            (61.0 + 90 * Pow(t, 2) + 45 * Pow(t, 4)
            + 46 * Pow(eta, 2)
            - 252 * Pow(eta * t, 2)
            - 90 * Pow(eta, 4) * Pow(t, 4));

        double B = Bx + A2 * Pow(y, 2) + A4 * Pow(y, 4) + A6 * Pow(y, 6);

        return B;
    }


    /// <summary>
    /// Вычисляет разность долгот данной точки и осевого меридиана.
    /// </summary>
    /// <param name="x"> Координата X (Northing).</param>
    /// <param name="y"> Координата Y (неприведённая) (Easting).</param>
    /// <returns></returns>
    private double PlaneToGeodeticlongitudeDelta(double x, double y)
    {
        double Bx = ell.LatitudeCalc(x);
        double Nx = ell.RadiusN(Bx);
        double eta = ell.e2 * Cos(Bx);  //ell.GetEta(Bx);

        double P1 = 1.0 / (Nx * Cos(Bx));

        double P3 = -P1 / (6 * Pow(Nx, 2)) * (1 + 2 * Pow(Tan(Bx), 2) + Pow(eta, 2));

        double P5 = P1 / (120.0 * Pow(Nx, 4)) * (5.0 + 28.0 * Pow(Tan(Bx), 2) + 24.0 * Pow(Tan(Bx), 4)
            + 6.0 * Pow(eta, 2)
            + 8.0 * Pow(eta, 2) * Pow(Tan(Bx), 2));

        double l = P1 * y + P3 * Pow(y, 3) + P5 * Pow(y, 5);

        return l;
    }
}
