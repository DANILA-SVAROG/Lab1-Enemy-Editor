using System.Globalization;
using System.Text;

namespace EnemyEditor.Models;

public sealed class BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
{
    private const int Base = 1000;
    private readonly int[] number;

    public int ArrayLength => number.Length;

    // Разбивает строку на блоки по три цифры.
    public BigNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Any(ch => ch < '0' || ch > '9'))
        {
            throw new ArgumentException("Нужно указать неотрицательное целое число.", nameof(value));
        }

        value = value.TrimStart('0');
        if (value.Length == 0)
        {
            number = new[] { 0 };
            return;
        }

        number = new int[(value.Length + 2) / 3];
        for (int i = 0; i < number.Length; i++)
        {
            int end = value.Length - i * 3;
            int start = Math.Max(0, end - 3);
            number[i] = int.Parse(value.AsSpan(start, end - start), CultureInfo.InvariantCulture);
        }
    }

    private BigNumber(int[] blocks)
    {
        number = TrimLeadingZeros(blocks);
    }

    // Убирает нулевые старшие блоки.
    private static int[] TrimLeadingZeros(int[] blocks)
    {
        int length = blocks.Length;
        while (length > 1 && blocks[length - 1] == 0)
        {
            length--;
        }

        int[] result = new int[length];
        Array.Copy(blocks, result, length);
        return result;
    }

    public BigNumber Clone()
    {
        return new BigNumber(number);
    }

    // Выводит старший блок без нулей, остальные по три цифры.
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = number.Length - 1; i >= 0; i--)
        {
            if (i == number.Length - 1)
            {
                sb.Append(number[i].ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                sb.Append(number[i].ToString("D3", CultureInfo.InvariantCulture));
            }
        }

        return sb.ToString();
    }

    // Сравнивает числа от старшего блока к младшему.
    public int CompareTo(BigNumber? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (ArrayLength != other.ArrayLength)
        {
            return ArrayLength.CompareTo(other.ArrayLength);
        }

        for (int i = ArrayLength - 1; i >= 0; i--)
        {
            if (number[i] != other.number[i])
            {
                return number[i].CompareTo(other.number[i]);
            }
        }

        return 0;
    }

    public bool Equals(BigNumber? other)
    {
        return other is not null && CompareTo(other) == 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is BigNumber other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        foreach (int block in number)
        {
            hash.Add(block);
        }

        return hash.ToHashCode();
    }

    public static bool operator >(BigNumber a, BigNumber b) => a.CompareTo(b) > 0;
    public static bool operator <(BigNumber a, BigNumber b) => a.CompareTo(b) < 0;
    public static bool operator >=(BigNumber a, BigNumber b) => a.CompareTo(b) >= 0;
    public static bool operator <=(BigNumber a, BigNumber b) => a.CompareTo(b) <= 0;
    public static bool operator ==(BigNumber? a, BigNumber? b) => ReferenceEquals(a, b) || a is not null && a.Equals(b);
    public static bool operator !=(BigNumber? a, BigNumber? b) => !(a == b);
}
