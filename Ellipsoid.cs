using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace GeodeticFunctions
{
    public class Ellipsoid
    {
        public Ellipsoid(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public double a { get; }
        public double b { get; }
        

        //сжатие f
        public double f => (a - b) / a;

        //Первый эксцентриситет 
        public double e1 => Sqrt((a * a - b * b)) / a;

        //Второй эксцентриситет
        public double e2 => Sqrt((a * a - b * b)) / b;

        //Вспомогательна функция эксцентриситета
        public double GetEta(double B)
        {
            return e2 * Cos(B / 180 * PI);
        }



        /// <summary>
        /// Первая сфероидическая функция.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <returns></returns> 
        public double GetW(double B)
        {
            return Sqrt(1 - Pow(e1 * Sin(B / 180 * PI), 2));
        }


        /// <summary>
        /// Вторая сфероидическая функция.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <returns></returns> 
        public double GetV(double B)
        {
            return Sqrt(1 + Pow(e2 * Cos(B / 180 * PI), 2));
        }


        /// <summary>
        /// Возвращает значение радиуса кривизны меридианного сечения.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <returns></returns>
        public double RadiusM(double B)
        {
            return a * (1 - e1 * e1) / Pow(GetW(B), 3);
        }


        /// <summary>
        /// Возвращает значение радиуса кривизны сечения первого вертикала.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <returns></returns>
        public double RadiusN(double B)
        {
            return a / GetW(B);
        }


        /// <summary>
        /// Возвращает значение среднего радиуса кривизны.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <returns></returns>
        public double RadiusR(double B)
        {
            return b / Pow(GetW(B), 2);
        }


        /// <summary>
        /// Возвращает радиус кривизны параллели.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <returns></returns>
        public double RadiusParallel(double B)
        {
            return RadiusN(B) * Cos(B / 180 * PI);
        }


        /// <summary>
        /// Возвращает радиус кривизны произвольного нормального сечения.
        /// </summary>
        /// <param name="B"> Широта точки в градусах.</param>
        /// <param name="A"> Азимут сечения в градусах.</param>
        /// <returns></returns>
        public double RadiusAzimut(double B, double A)
        {
            return RadiusN(B) * Cos(B / 180 * PI);
        }


        /// <summary>
        /// Возвращает значение длины дуги параллели.
        /// </summary>
        /// <param name="B"> Широта параллели.</param>
        /// <param name="L1"> Долгота начальной точки.</param>
        /// <param name="L2"> Долгота Конечной точки</param>
        /// <returns></returns>
        public double ParallelArc(double B, double L1, double L2)
        {
            return RadiusParallel(B) * Abs(L2 - L1) / 180 * PI;
        }


        /// <summary>
        /// Возвращает значение длины дуги меридиана.
        /// </summary>
        /// <param name="B1"> Широта начальной точки.</param>
        /// <param name="B2"> Широта конечной точки.</param>
        /// <returns></returns>
        public double MeridianArc(double B1, double B2)
        {
            double a0 = 1.0 + 3.0 / 4.0 * Pow(e1, 2) + 45.0 / 64.0 * Pow(e1, 4) + 175.0 / 256.0 * Pow(e1, 6) + 11025.0 / 16384.0 * Pow(e1, 8);
            double a2 = 3.0 / 4.0 * Pow(e1, 2) + 15.0 / 16.0 * Pow(e1, 4) + 525.0 / 512.0 * Pow(e1, 6) + 2205.0 / 2048.0 * Pow(e1, 8);
            double a4 = 15.0 / 64.0 * Pow(e1, 4) + 105.0 / 256.0 * Pow(e1, 6) + 2205.0 / 4096.0 * Pow(e1, 8);
            double a6 = 35.0 / 512.0 * Pow(e1, 6) + 315.0 / 4096.0 * Pow(e1, 8);

            double X1 = a * (1 - e1 * e1) * (a0 * B1 / 180 * PI - a2 / 2 * Sin(2 * B1 / 180 * PI) + a4 / 4 * Sin(4 * B1 / 180 * PI) - a6 / 6 * Sin(6 * B1 / 180 * PI));
            double X2 = a * (1 - e1 * e1) * (a0 * B2 / 180 * PI - a2 / 2 * Sin(2 * B2 / 180 * PI) + a4 / 4 * Sin(4 * B2 / 180 * PI) - a6 / 6 * Sin(6 * B2 / 180 * PI));

            double X = Abs(X2 - X1);
            return X;
        }


        /// <summary>
        /// Вычисляет широту по заданной длине дуги меридиана.
        /// </summary>
        /// <param name="X"> Длина меридиана, м.</param>
        /// <returns></returns>
        public double LatitudeCalc(double X)
        {
            double a0 = 1.0 + 3.0 / 4.0 * Pow(e1, 2) + 45.0 / 64.0 * Pow(e1, 4) + 175.0 / 256.0 * Pow(e1, 6) + 11025.0 / 16384.0 * Pow(e1, 8);
            double a2 = 3.0 / 4.0 * Pow(e1, 2) + 15.0 / 16.0 * Pow(e1, 4) + 525.0 / 512.0 * Pow(e1, 6) + 2205.0 / 2048.0 * Pow(e1, 8);
            double a4 = 15.0 / 64.0 * Pow(e1, 4) + 105.0 / 256.0 * Pow(e1, 6) + 2205.0 / 4096.0 * Pow(e1, 8);
            double a6 = 35.0 / 512.0 * Pow(e1, 6) + 315.0 / 4096.0 * Pow(e1, 8);

            double B = 0;
            double Bx;
            double epsilon;
            do
            {
                Bx = 1.0 / a0 * (X / (a * (1 - e1 * e1)) + a2 / 2.0 * Sin(2 * B) - a4 / 4.0 * Sin(4 * B) + a6 / 6.0 * Sin(6 * B));
                epsilon = Abs(Bx - B);
                B = Bx;
            } while (epsilon > 0.00005 / 206265);

            return B * 180 / PI;
        }



        /// <summary>
        /// Возвращает значение площади съемочной трапеции.
        /// </summary>
        /// <param name="B1"> Широта южной границы.</param>
        /// <param name="B2"> Широта северной границы.</param>
        /// <param name="L1"> Долгота западной границы.</param>
        /// <param name="L2"> Долгота восточной границы.</param>
        /// <returns></returns>
        public double AreaCalculate(double B1, double B2, double L1, double L2)
        {
            return b * b * Abs(L2 - L1) / 180.0 * PI * (Sin(B2 / 180 * PI) - Sin(B1 / 180 * PI)
                + 2.0 / 3.0 * Pow(e1, 2) * (Pow(Sin(B2 / 180 * PI), 3) - Pow(Sin(B1 / 180 * PI), 3))
                + 3.0 / 5.0 * Pow(e1, 4) * (Pow(Sin(B2 / 180 * PI), 5) - Pow(Sin(B1 / 180 * PI), 5))
                + 4.0 / 7.0 * Pow(e1, 6) * (Pow(Sin(B2 / 180 * PI), 7) - Pow(Sin(B1 / 180 * PI), 7)));

        }


        



    }
}
