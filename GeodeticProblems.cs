using static System.Math;

namespace GeodeticFunctions
{
    public static class GeodeticProblems
    {

        /// <summary>
        /// Прямая геодезическая задача (метод Рунге-Кутта)
        /// </summary>
        /// <param name="B1"></param>
        /// <param name="L1"></param>
        /// <param name="A12"></param>
        /// <param name="S"></param>
        /// <param name="B2"></param>
        /// <param name="L2"></param>
        /// <param name="A21"></param>
        public static void DirectProblem(double B1, double L1, double A12, double S, Ellipsoid ellipsoid, out double B2, out double L2, out double A21)
        {
            //коэффициенты k
            double[,] k = new double[5, 4];

            k[1, 1] = S * Cos(A12 / 180 * PI) / ellipsoid.RadiusM(B1);
            k[1, 2] = S * Sin(A12 / 180 * PI) / (ellipsoid.RadiusN(B1) * Cos(B1 / 180 * PI));
            k[1, 3] = k[1, 2] * Sin(B1 / 180 * PI);

            k[2, 1] = S * Cos(A12 / 180 * PI + 0.5 * k[1, 3]) / ellipsoid.RadiusM(B1 + 0.5 * k[1, 1] / PI * 180);
            k[2, 2] = S * Sin(A12 / 180 * PI + 0.5 * k[1, 3]) / (ellipsoid.RadiusN(B1 + 0.5 * k[1, 1] / PI * 180) * Cos(B1 / 180 * PI + 0.5 * k[1, 1]));
            k[2, 3] = k[2, 2] * Sin(B1 / 180 * PI + 0.5 * k[1, 1]);

            k[3, 1] = S * Cos(A12 / 180 * PI + 0.5 * k[2, 3]) / ellipsoid.RadiusM(B1 + 0.5 * k[2, 1] / PI * 180);
            k[3, 2] = S * Sin(A12 / 180 * PI + 0.5 * k[2, 3]) / (ellipsoid.RadiusN(B1 + 0.5 * k[2, 1] / PI * 180) * Cos(B1 / 180 * PI + 0.5 * k[2, 1]));
            k[3, 3] = k[3, 2] * Sin(B1 / 180 * PI + 0.5 * k[2, 1]);

            k[4, 1] = S * Cos(A12 / 180 * PI + k[3, 3]) / ellipsoid.RadiusM(B1 + k[3, 1] / PI * 180);
            k[4, 2] = S * Sin(A12 / 180 * PI + k[3, 3]) / (ellipsoid.RadiusN(B1 + k[3, 1] / PI * 180) * Cos(B1 / 180 * PI + k[3, 1]));
            k[4, 3] = k[4, 2] * Sin(B1 / 180 * PI + k[3, 1]);

            B2 = (B1 / 180 * PI + 1.0 / 6.0 * (k[1, 1] + 2 * k[2, 1] + 2 * k[3, 1] + k[4, 1])) / PI * 180;
            L2 = (L1 / 180 * PI + 1.0 / 6.0 * (k[1, 2] + 2 * k[2, 2] + 2 * k[3, 2] + k[4, 2])) / PI * 180;

            if (A12 < 180)
            {
                A21 = (A12 / 180 * PI + 1.0 / 6.0 * (k[1, 3] + 2 * k[2, 3] + 2 * k[3, 3] + k[4, 3])) / PI * 180 + 180;
            }
            else
            // if (A12<=180)
            {
                A21 = (A12 / 180 * PI + 1.0 / 6.0 * (k[1, 3] + 2 * k[2, 3] + 2 * k[3, 3] + k[4, 3])) / PI * 180 - 180;
            }

            if (A21 < 0)
            {
                A21 += 360;
            }

            if (A21 > 360)
            {
                A21 -= 360;
            }

        }


        /// <summary>
        /// Обратная геодезическая задача (Vincenty's formula)
        /// </summary>
        /// <param name="B1"></param>
        /// <param name="L1"></param>
        /// <param name="B2"></param>
        /// <param name="L2"></param>
        /// <param name="S"></param>
        /// <param name="A12"></param>
        /// <param name="A21"></param>
        public static void InverseProblemVincenty(double B1, double L1, double B2, double L2, Ellipsoid ellipsoid, out double S, out double A12, out double A21)
        {
            //широта, долгота в радианы
            //B1 *= PI / 180;
            //L1 *= PI / 180;
            //B2 *= PI / 180;
            //L2 *= PI / 180;

            //приведенная широта
            double U1 = Atan((1 - ellipsoid.f) * Tan(B1));
            double U2 = Atan((1 - ellipsoid.f) * Tan(B2));

            double L = L2 - L1;

            double λ = L;

            double λ1;
            double sigma;
            //косинус квадрат
            double cos2_alpha;
            //cos(2*sigma_m)
            double cos_2sigma;

            do
            {
                λ1 = λ;
                double sin_sigma = Sqrt(Pow(Cos(U2) * Sin(λ), 2) + Pow(Cos(U1) * Sin(U2) - Sin(U1) * Cos(U2) * Cos(λ), 2));
                double cos_sigma = Sin(U1) * Sin(U2) + Cos(U1) * Cos(U2) * Cos(λ);
                sigma = Atan(sin_sigma / cos_sigma);

                double sin_alpha = (Cos(U1) * Cos(U2) * Sin(λ)) / sin_sigma;

                //косинус квадрат
                cos2_alpha = 1 - Pow(sin_alpha, 2);

                cos_2sigma = Cos(sigma) - (2 * Sin(U1) * Sin(U2)) / cos2_alpha;

                double C = ellipsoid.f / 16 * cos2_alpha * (4 + ellipsoid.f * (4 - 3 * cos2_alpha));

                λ = L + (1 - C) * ellipsoid.f * sin_alpha * (sigma + C * sin_sigma * (cos_2sigma + C * cos_sigma * (-1 + 2 * Pow(cos_2sigma, 2))));

            }
            while (Abs(λ - λ1) > 1E-14);


            double u2 = cos2_alpha * (Pow(ellipsoid.a, 2) - Pow(ellipsoid.b, 2)) / Pow(ellipsoid.b, 2);
            double A = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));  //было 320 * 175
            double B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));
            double deltaSigma = B * Sin(sigma) * (cos_2sigma + B / 4.0 * (
                Cos(sigma) * (-1 + 2 * Pow(cos_2sigma, 2)) -
                B / 6 * cos_2sigma * (-3 + 4 * Pow(Sin(sigma), 2)) * (-3 + 4 * Pow(cos_2sigma, 2))));

            S = ellipsoid.b * A * (sigma - deltaSigma);

            A12 = Atan((Cos(U2) * Sin(λ)) / (Cos(U1) * Sin(U2) - Sin(U1) * Cos(U2) * Cos(λ)));

            A21 = Atan((Cos(U1) * Sin(λ)) / (-Sin(U1) * Cos(U2) + Cos(U1) * Sin(U2) * Cos(λ)));

            A12 *= 180 / PI;
            A21 = A21 * 180 / PI + 180;
        }
    }
}
