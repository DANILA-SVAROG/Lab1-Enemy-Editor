namespace Lab6Tests;

public class MathFunctions
{
    // Переводит дюймы в сантиметры.
    public double inchesToCentimeters(double inches)
    {
        if (double.IsNaN(inches) || double.IsInfinity(inches) || inches < 0)
        {
            throw new ArgumentException("Количество дюймов должно быть конечным неотрицательным числом");
        }

        double centimeters = inches * 2.54;
        if (double.IsInfinity(centimeters))
        {
            throw new ArgumentException("Результат перевода в сантиметры слишком велик");
        }

        return centimeters;
    }

    // Проверяет чётность целого числа.
    public bool isEven(int n)
    {
        return n % 2 == 0;
    }
}
