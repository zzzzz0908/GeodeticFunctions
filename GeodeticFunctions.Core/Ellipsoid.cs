﻿using static System.Math;


namespace GeodeticFunctions.Core;


/// <summary>
/// Содержит названиия доступных эллипсоидов.
/// </summary>
public enum Ellipsoids
{
    /// <summary>
    /// Эллипсоид Красовского.
    /// </summary>
    Krasovskiy,

    /// <summary>
    /// Эллипсоид WGS 84.
    /// </summary>
    WGS84,

    /// <summary>
    /// Эллипсоид ПЗ-90 (также ПЗ-90.02 и ПЗ-90.11).
    /// </summary>
    PZ90
}

/// <summary>
/// Представляет модель эллипсоида.
/// </summary>
public class Ellipsoid
{

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Ellipsoid"/>.
    /// </summary>
    /// <param name="a"> Большая (экваториальная) полуось эллипсоида, м. </param>
    /// <param name="iverseFlattening"> Полярное сжатие эллипсоида 1/f. </param>
    public Ellipsoid(double a, double iverseFlattening)
    {
        this.a = a;
        f = 1 / iverseFlattening;

        e1_2 = 1 - b * b / (a * a);
        e2_2 = a * a / (b * b) - 1;

        e1 = Sqrt(e1_2);
        e2 = Sqrt(e2_2);
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Ellipsoid"/>.
    /// </summary>
    /// <param name="ell"> Имя эллипсоида из коллекции доступных эллипсоидов. </param>
    public Ellipsoid(Ellipsoids ell)
    {
        double[,] ellParams = new double[,]
        {
            // Эллипсоид Красовского 
            { 6378245.000, 298.3 }, 
            // Эллипсоид WGS 84
            { 6378137.000, 298.257223563 }, 
            // Эллипсоид ПЗ-90
            { 6378136.000, 298.25784 }
        };

        a = ellParams[(int)ell, 0];
        f = 1 / ellParams[(int)ell, 1];

        e1_2 = 1 - b * b / (a * a);
        e2_2 = a * a / (b * b) - 1;

        e1 = Sqrt(e1_2);
        e2 = Sqrt(e2_2);
    }

    /// <summary>
    /// Большая (экваториальная) полуось эллипсоида, м.
    /// </summary>
    public double a { get; }

    /// <summary>
    /// Малая (полярная) полуось эллипсоида, м.
    /// </summary>
    public double b => a * (1 - f);

    /// <summary>
    /// Квадрат первого эксцентрисистета.
    /// </summary>
    public double e1_2 { get; }

    /// <summary>
    /// Квадрат второго эксцентрисистета.
    /// </summary>
    public double e2_2 { get; }

    /// <summary>
    /// Полярное сжатие.
    /// </summary>
    public double f { get; }

    /// <summary>
    /// Первый эксцентристет.
    /// </summary>
    public double e1 { get; }

    /// <summary>
    /// Второй эксцентриситет.
    /// </summary>
    public double e2 { get; }


    /// <summary>
    /// Вспомогательна функция эксцентриситета.
    /// </summary>
    /// <param name="B"> Широта в радианах. </param>
    /// <returns></returns>
    private double GetEta(double B)
    {
        // TODO: Оптимизировать в конвертере координат 
        return e2 * Cos(B);
    }

    /// <summary>
    /// Квадрат второй сфероидической функции.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns>
    private double GetW2(double B)
    {
        return 1 - e1_2 * Pow(Sin(B), 2);
    }

    /// <summary>
    /// Первая сфероидическая функция.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns> 
    private double GetW(double B)
    {
        return Sqrt(GetW2(B));
    }


    /// <summary>
    /// Вторая сфероидическая функция.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns> 
    private double GetV(double B)
    {
        // TODO: стоит ли использовать
        return Sqrt(1 + Pow(e2 * Cos(B), 2));
    }


    /// <summary>
    /// Возвращает значение радиуса кривизны меридианного сечения.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns>
    public double RadiusM(double B)
    {
        return a * (1 - e1 * e1) / Pow(GetW(B), 3);
        //return b * b / (a * Pow(GetW2(B), 1.5));
    }


    /// <summary>
    /// Возвращает значение радиуса кривизны сечения первого вертикала.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns>
    public double RadiusN(double B)
    {
        return a / GetW(B);
    }


    /// <summary>
    /// Возвращает значение среднего радиуса кривизны.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns>
    public double RadiusR(double B)
    {
        return b / GetW2(B);
    }


    /// <summary>
    /// Возвращает радиус кривизны параллели.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <returns></returns>
    public double RadiusParallel(double B)
    {
        return RadiusN(B) * Cos(B);
    }


    /// <summary>
    /// Возвращает радиус кривизны произвольного нормального сечения.
    /// </summary>
    /// <param name="B"> Широта точки в радианах.</param>
    /// <param name="A"> Азимут сечения в радианах.</param>
    /// <returns></returns>
    public double RadiusAzimut(double B, double A)
    {
        return RadiusM(B) * RadiusN(B) / (RadiusN(B) * Pow(Cos(A), 2) + RadiusM(B) * Pow(Sin(A), 2));
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
        return RadiusParallel(B) * Abs(L2 - L1);
    }


    /// <summary>
    /// Возвращает значение длины дуги меридиана.
    /// </summary>
    /// <param name="B1"> Широта начальной точки.</param>
    /// <param name="B2"> Широта конечной точки.</param>
    /// <returns></returns>
    private double MeridianArc2(double B1, double B2)
    {
        double a0 = 1.0 + 3.0 / 4.0 * e1_2 + 45.0 / 64.0 * Pow(e1_2, 2) + 175.0 / 256.0 * Pow(e1_2, 3) + 11025.0 / 16384.0 * Pow(e1_2, 4);
        double a2 = 3.0 / 4.0 * e1_2 + 15.0 / 16.0 * Pow(e1_2, 2) + 525.0 / 512.0 * Pow(e1_2, 3) + 2205.0 / 2048.0 * Pow(e1_2, 4);
        double a4 = 15.0 / 64.0 * Pow(e1_2, 2) + 105.0 / 256.0 * Pow(e1_2, 3) + 2205.0 / 4096.0 * Pow(e1_2, 4);
        double a6 = 35.0 / 512.0 * Pow(e1_2, 3) + 315.0 / 2048.0 * Pow(e1_2, 4); //было 315 / 4096
        // добавление
        double a8 = 315.0 / 16384.0 * Pow(e1_2, 4);

        double X1 = a * (1 - e1_2) * (a0 * B1 - a2 / 2 * Sin(2 * B1) + a4 / 4 * Sin(4 * B1) - a6 / 6 * Sin(6 * B1) + a8 / 8 * Sin(B1));
        double X2 = a * (1 - e1_2) * (a0 * B2 - a2 / 2 * Sin(2 * B2) + a4 / 4 * Sin(4 * B2) - a6 / 6 * Sin(6 * B2) + a8 / 8 * Sin(B2));

        double X = Abs(X2 - X1);
        return X;
    }

    /// <summary>
    /// Возвращает значение длины дуги меридиана.
    /// </summary>
    /// <param name="B1"> Широта начальной точки.</param>
    /// <param name="B2"> Широта конечной точки.</param>
    /// <returns></returns>
    public double MeridianArc(double B1, double B2)
    {
        // производительность лучше, чем у MeridianArc2

        double n = (a - b) / (a + b);

        // в формуле коэффициенты B
        double a0 = b * (1 + n + 5.0 / 4.0 * n * n + 5.0 / 4.0 * n * n * n);
        double a2 = -b * (1.5 * n + 1.5 * n * n + 21.0 / 16.0 * n * n * n);
        double a4 = b * (15.0 / 16.0 * n * n + 15.0 / 16.0 * n * n * n);
        double a6 = -b * (35.0 / 48.0 * n * n * n);

        double X1 = a0 * B1 + a2 * Sin(2 * B1) + a4 * Sin(4 * B1) + a6 * Sin(6 * B1);
        double X2 = a0 * B2 + a2 * Sin(2 * B2) + a4 * Sin(4 * B2) + a6 * Sin(6 * B2);
        double result = Abs(X2 - X1);
        return result;
    }



    /// <summary>
    /// Возвращает широту в радианах по заданной длине дуги меридиана.
    /// </summary>
    /// <param name="X"> Длина меридиана, м.</param>
    /// <returns></returns>
    public double LatitudeCalc(double X)
    {
        double a0 = 1.0 + 3.0 / 4.0 * e1_2 + 45.0 / 64.0 * Pow(e1_2, 2) + 175.0 / 256.0 * Pow(e1_2, 3) + 11025.0 / 16384.0 * Pow(e1_2, 4);
        double a2 = 3.0 / 4.0 * e1_2 + 15.0 / 16.0 * Pow(e1_2, 2) + 525.0 / 512.0 * Pow(e1_2, 3) + 2205.0 / 2048.0 * Pow(e1_2, 4);
        double a4 = 15.0 / 64.0 * Pow(e1_2, 2) + 105.0 / 256.0 * Pow(e1_2, 3) + 2205.0 / 4096.0 * Pow(e1_2, 4);
        double a6 = 35.0 / 512.0 * Pow(e1_2, 3) + 315.0 / 2048.0 * Pow(e1_2, 4);
        // добавление
        double a8 = 315.0 / 16384.0 * Pow(e1_2, 4);

        double B = 0;
        double Bx;
        double epsilon;

        do
        {
            Bx = 1.0 / a0 * (X / (a * (1 - e1_2)) + a2 / 2.0 * Sin(2 * B) - a4 / 4.0 * Sin(4 * B) + a6 / 6.0 * Sin(6 * B) - a8 / 8.0 * Sin(B));
            epsilon = Abs(Bx - B);
            B = Bx;
        } while (epsilon > 1E-12);

        return B;
    }


    /// <summary>
    /// Возвращает значение площади съемочной трапеции в метрах квадратных.
    /// </summary>
    /// <param name="B1"> Широта южной границы в радианах.</param>
    /// <param name="B2"> Широта северной границы в радианах.</param>
    /// <param name="L1"> Долгота западной границы в радианах.</param>
    /// <param name="L2"> Долгота восточной границы в радианах.</param>
    /// <returns></returns>
    public double AreaCalculate(double B1, double B2, double L1, double L2)
    {
        return b * b * Abs(L2 - L1) * (Sin(B2) - Sin(B1)
            + 2.0 / 3.0 * e1_2 * (Pow(Sin(B2), 3) - Pow(Sin(B1), 3))
            + 3.0 / 5.0 * Pow(e1_2, 2) * (Pow(Sin(B2), 5) - Pow(Sin(B1), 5))
            + 4.0 / 7.0 * Pow(e1_2, 3) * (Pow(Sin(B2), 7) - Pow(Sin(B1), 7)));
    }
}
