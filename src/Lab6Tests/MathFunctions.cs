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

    // Находит наибольшее число в непустом массиве.
    public int max(int[]? numbers)
    {
        if (numbers is null)
        {
            throw new ArgumentException("Массив не должен быть null");
        }

        if (numbers.Length == 0)
        {
            throw new ArgumentException("Массив не должен быть пустым");
        }

        int maximum = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > maximum)
            {
                maximum = numbers[i];
            }
        }

        return maximum;
    }

    // Делитель должен быть положительным, как в методичке.
    public int mod(int a, int b)
    {
        if (b <= 0)
        {
            throw new ArgumentException("Делитель должен быть положительным числом");
        }

        return a % b;
    }
}
