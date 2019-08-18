using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace GeodeticFunctions
{
    public class CoordinateConverter
    {
        Ellipsoid ell;
        
        /// <summary>
        /// Возвращает значение плоской координаты X.
        /// </summary>
        /// <param name="B"> Широта точки.</param>
        /// <param name="L"> Долгота точки.</param>
        /// <param name="L0"> Долгота осевого меридиана.</param>
        /// <returns></returns>
        public double GeodeticToPlaneX(double B, double L, double L0)
        {
            double a0 = ell.MeridianArc(0, B);

            double a2 = 1.0 / 2.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Cos(B / 180 * PI);

            double a4 = 1.0 / 24.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Pow(Cos(B / 180 * PI), 3)
                * (5 - Pow(Tan(B / 180 * PI), 2) + 9 * Pow(ell.GetEta(B), 2) + 4 * Pow(ell.GetEta(B), 4));

            double a6 = 1.0 / 720.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Pow(Cos(B / 180 * PI), 5)
                * (61 - 58 * Pow(Tan(B / 180 * PI), 2) + Pow(Tan(B / 180 * PI), 4) + 270 * Pow(ell.GetEta(B), 2)
                - 330 * Pow(ell.GetEta(B) * Tan(B / 180 * PI), 2));

            double a8 = 1.0 / 40320.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Pow(Cos(B / 180 * PI), 7)
                * (1385 - 3111 * Pow(Tan(B / 180 * PI), 2) + 543 * Pow(Tan(B / 180 * PI), 4) - Pow(Tan(B / 180 * PI), 6));


            double l = (L - L0) / 180 * PI;

            double x = a0 + a2 * Pow(l, 2) + a4 * Pow(l, 4) + a6 * Pow(l, 6) + a8 * Pow(l, 8);

            return x;
        }


        /// <summary>
        /// Возвращает значение плоской координаты Y.
        /// </summary>
        /// <param name="B"> Широта точки.</param>
        /// <param name="L"> Долгота точки.</param>
        /// <param name="L0"> Долгота осевого меридиана.</param>
        /// <returns></returns>
        public double GeodeticToPlaneY(double B, double L, double L0)
        {
            double b1 = ell.RadiusParallel(B);

            double b3 = 1.0 / 6.0 * ell.RadiusN(B) * Pow(Cos(B / 180 * PI), 3) * (1 - Pow(Tan(B / 180 * PI), 2) + Pow(ell.GetEta(B), 2));

            double b5 = 1.0 / 120.0 * ell.RadiusN(B) * Pow(Cos(B / 180 * PI), 5)
                * (5 - 18 * Pow(Tan(B / 180 * PI), 2) + Pow(Tan(B / 180 * PI), 4) + 14 * Pow(ell.GetEta(B), 2)
                - 58 * Pow(ell.GetEta(B) * Tan(B / 180 * PI), 2));

            double b7 = 1.0 / 5040.0 * ell.RadiusN(B) * Pow(Cos(B / 180 * PI), 7)
                * (61 - 479 * Pow(Tan(B / 180 * PI), 2) + 179 * Pow(Tan(B / 180 * PI), 4) - Pow(Tan(B / 180 * PI), 6));

            double l = (L - L0) / 180 * PI;

            double y = b1 + b3 * Pow(l, 3) + b5 * Pow(l, 5) + b7 * Pow(l, 7);

            return y;
        }


        /// <summary>
        /// Преобразует геодезические координаты в плоские.
        /// </summary>
        /// <param name="B"> Широта точки.</param>
        /// <param name="L"> Долгота точки.</param>
        /// <param name="L0"> Долгота осевого меридиана.</param>
        /// <param name="x"> Плоская координата X.</param>
        /// <param name="y"> Плоская координата Y.</param>
        public void GeodeticToPlane(double B, double L, double L0, out double x, out double y)
        {
            double a0 = ell.MeridianArc(0, B);

            double a2 = 1.0 / 2.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Cos(B / 180 * PI);

            double a4 = 1.0 / 24.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Pow(Cos(B / 180 * PI), 3)
                * (5 - Pow(Tan(B / 180 * PI), 2) + 9 * Pow(ell.GetEta(B), 2) + 4 * Pow(ell.GetEta(B), 4));

            double a6 = 1.0 / 720.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Pow(Cos(B / 180 * PI), 5)
                * (61 - 58 * Pow(Tan(B / 180 * PI), 2) + Pow(Tan(B / 180 * PI), 4) + 270 * Pow(ell.GetEta(B), 2)
                - 330 * Pow(ell.GetEta(B) * Tan(B / 180 * PI), 2));

            double a8 = 1.0 / 40320.0 * ell.RadiusN(B) * Sin(B / 180 * PI) * Pow(Cos(B / 180 * PI), 7)
                * (1385 - 3111 * Pow(Tan(B / 180 * PI), 2) + 543 * Pow(Tan(B / 180 * PI), 4) - Pow(Tan(B / 180 * PI), 6));

            double b1 = ell.RadiusParallel(B);

            double b3 = 1.0 / 6.0 * ell.RadiusN(B) * Pow(Cos(B / 180 * PI), 3) * (1 - Pow(Tan(B / 180 * PI), 2) + Pow(ell.GetEta(B), 2));

            double b5 = 1.0 / 120.0 * ell.RadiusN(B) * Pow(Cos(B / 180 * PI), 5)
                * (5 - 18 * Pow(Tan(B / 180 * PI), 2) + Pow(Tan(B / 180 * PI), 4) + 14 * Pow(ell.GetEta(B), 2)
                - 58 * Pow(ell.GetEta(B) * Tan(B / 180 * PI), 2));

            double b7 = 1.0 / 5040.0 * ell.RadiusN(B) * Pow(Cos(B / 180 * PI), 7)
                * (61 - 479 * Pow(Tan(B / 180 * PI), 2) + 179 * Pow(Tan(B / 180 * PI), 4) - Pow(Tan(B / 180 * PI), 6));

            double l = (L - L0) / 180 * PI;

            x = a0 + a2 * Pow(l, 2) + a4 * Pow(l, 4) + a6 * Pow(l, 6) + a8 * Pow(l, 8);

            y = b1 * l + b3 * Pow(l, 3) + b5 * Pow(l, 5) + b7 * Pow(l, 7);
        }


        /// <summary>
        /// Вычисляет широту точки по плоским координатам.
        /// </summary>
        /// <param name="x"> Координата X.</param>
        /// <param name="y"> Координата Y.</param>
        /// <returns></returns>
        public double PlaneToGeodeticLatitude(double x, double y)
        {
            double Bx = ell.LatitudeCalc(x);
            double Nx = ell.RadiusN(Bx);
            double t = Tan(Bx / 180 * PI);
            double eta = ell.GetEta(Bx);

            double A2 = -t * Pow(ell.GetV(Bx), 2) / (2 * Pow(Nx, 2));

            double A4 = -A2 / (12.0 * Pow(Nx, 2)) * (5.0 + 3 * Pow(t, 2) + Pow(eta, 2) - 9.0 * Pow(eta, 2) * Pow(t, 2) - 4 * Pow(eta, 4));

            double A6 = A2 / (360 * Pow(Nx, 4)) * (61.0 + 90 * Pow(t, 2) + 45 * Pow(t, 4)
                + 46 * Pow(eta, 2) - 252 * Pow(eta * t, 2) - 90 * Pow(eta, 4) * Pow(t, 4));

            double B = Bx / 180 * PI + A2 * Pow(y, 2) + A4 * Pow(y, 4) + A6 * Pow(y, 6);

            return B / PI * 180;

        }



        /// <summary>
        /// Вычисляет разность долгот данной точки и осевого меридиана
        /// </summary>
        /// <param name="x"> Координата X.</param>
        /// <param name="y"> Координата Y (неприведённая)</param>
        /// <returns></returns>
        public double PlaneToGeodeticLongtitudeDelta(double x, double y)
        {
            double Bx = ell.LatitudeCalc(x);
            double Nx = ell.RadiusN(Bx);

            double P1 = 1.0 / (Nx * Cos(Bx / 180 * PI));

            double P3 = -P1 / (6 * Pow(Nx, 2)) * (1 + 2 * Pow(Tan(Bx / 180 * PI), 2) + Pow(ell.GetEta(Bx), 2));

            double P5 = P1 / (120.0 * Pow(Nx, 4)) * (5.0 + 28.0 * Pow(Tan(Bx / 180 * PI), 2) + 24.0 * Pow(Tan(Bx / 180 * PI), 4)
                + 6.0 * Pow(ell.GetEta(Bx), 2)
                + 8.0 * Pow(ell.GetEta(Bx), 2) * Pow(Tan(Bx / 180 * PI), 2));

            double l = P1 * y + P3 * Pow(y, 3) + P5 * Pow(y, 5);

            return l / PI * 180;
        }
    }
}
