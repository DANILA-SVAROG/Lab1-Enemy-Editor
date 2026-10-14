using EnemyEditor.Models;
using NUnit.Framework;

namespace Lab6Tests;

[TestFixture]
public class BigNumberTest
{
    // Проверяет переносы между блоками числа.
    [TestCase("3712117", "15512900", "19225017")]
    [TestCase("999", "1", "1000")]
    [TestCase("999999999", "1", "1000000000")]
    [TestCase("1000000000", "999999999", "1999999999")]
    [TestCase("999999999999999999999999999999", "1", "1000000000000000000000000000000")]
    [TestCase("0", "0", "0")]
    [TestCase("123456789", "0", "123456789")]
    [TestCase("0", "123456789", "123456789")]
    [TestCase("001", "009", "10")]
    public void AddTest(string a, string b, string expected)
    {
        var n1 = new BigNumber(a);
        var n2 = new BigNumber(b);
        var result = n1 + n2;
        Assert.AreEqual(expected, result.ToString());
    }

    // Проверяет заём из соседнего и более дальних блоков.
    [TestCase("3712117", "1911900", "1800217")]
    [TestCase("1000", "1", "999")]
    [TestCase("1000000000", "1", "999999999")]
    [TestCase("1000000000000000000000000000000", "1", "999999999999999999999999999999")]
    [TestCase("123456789", "123456789", "0")]
    [TestCase("123456789", "0", "123456789")]
    [TestCase("0", "0", "0")]
    [TestCase("1001000", "999", "1000001")]
    [TestCase("0010", "0001", "9")]
    public void SubTest(string a, string b, string expected)
    {
        var n1 = new BigNumber(a);
        var n2 = new BigNumber(b);
        var result = n1 - n2;
        Assert.AreEqual(expected, result.ToString());
    }

    [TestCase("1", "2")]
    [TestCase("0", "1")]
    [TestCase("999999999", "1000000000")]
    public void SubTest_ThrowsOnNegativeResult(string a, string b)
    {
        var n1 = new BigNumber(a);
        var n2 = new BigNumber(b);
        var exception = Assert.Throws<InvalidOperationException>(() => { var result = n1 - n2; });
        Assert.That(exception!.Message, Is.EqualTo("Результат вычитания не может быть отрицательным."));
        Assert.AreEqual(a, n1.ToString());
        Assert.AreEqual(b, n2.ToString());
    }

    // Во второй лабораторной множитель имеет тип double.
    [TestCase("7135654", 23, "164120042")]
    [TestCase("999", 999, "998001")]
    [TestCase("999999999", 1000, "999999999000")]
    [TestCase("999999999999999999999999999999", 2, "1999999999999999999999999999998")]
    [TestCase("123456789", 1, "123456789")]
    [TestCase("0", 23, "0")]
    [TestCase("7", 0.5, "3")]
    [TestCase("12345", 0.1, "1234")]
    [TestCase("2000", 0.001, "2")]
    [TestCase("1", 0.5, "0")]
    public void MulTest(string a, double b, string expected)
    {
        var n1 = new BigNumber(a);
        var result = n1 * b;
        Assert.AreEqual(expected, result.ToString());
    }

    // Проверяет отбрасывание остатка после целочисленного деления.
    [TestCase("10014217", 3, "3338072")]
    [TestCase("10", 3, "3")]
    [TestCase("7", 7, "1")]
    [TestCase("2", 5, "0")]
    [TestCase("0", 5, "0")]
    [TestCase("123456789", 1, "123456789")]
    [TestCase("1000000000", 1000, "1000000")]
    [TestCase("1000000001", 1000, "1000000")]
    [TestCase("999999999999999999999999999999", 3, "333333333333333333333333333333")]
    [TestCase("7", 0.5, "14")]
    [TestCase("7", 2.5, "2")]
    [TestCase("2000", 0.001, "2000000")]
    public void DivTest(string a, double b, string expected)
    {
        var n1 = new BigNumber(a);
        var result = n1 / b;
        Assert.AreEqual(expected, result.ToString());
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void MulTest_ThrowsOnIncorrectMultiplier(double b)
    {
        var n1 = new BigNumber("12345");
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => { var result = n1 * b; });
        var expected = new ArgumentOutOfRangeException("value", "Множитель должен быть положительным.");
        Assert.That(exception!.Message, Is.EqualTo(expected.Message));
        Assert.AreEqual("12345", n1.ToString());
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void DivTest_ThrowsOnIncorrectDivisor(double b)
    {
        var n1 = new BigNumber("12345");
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => { var result = n1 / b; });
        var expected = new ArgumentOutOfRangeException("value", "Множитель должен быть положительным.");
        Assert.That(exception!.Message, Is.EqualTo(expected.Message));
        Assert.AreEqual("12345", n1.ToString());
    }

    [TestCase(double.MaxValue, "Множитель слишком велик.")]
    [TestCase(1e20, "Множитель содержит слишком много цифр.")]
    [TestCase(0.0000000001, "Множитель содержит слишком много цифр.")]
    public void MulTest_ThrowsOnUnsupportedMultiplier(double b, string message)
    {
        var n1 = new BigNumber("12345");
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => { var result = n1 * b; });
        var expected = new ArgumentOutOfRangeException("value", message);
        Assert.That(exception!.Message, Is.EqualTo(expected.Message));
    }

    [TestCase(double.MaxValue, "Множитель слишком велик.")]
    [TestCase(1e20, "Множитель содержит слишком много цифр.")]
    [TestCase(0.0000000001, "Множитель содержит слишком много цифр.")]
    public void DivTest_ThrowsOnUnsupportedDivisor(double b, string message)
    {
        var n1 = new BigNumber("12345");
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => { var result = n1 / b; });
        var expected = new ArgumentOutOfRangeException("value", message);
        Assert.That(exception!.Message, Is.EqualTo(expected.Message));
    }

    [TestCase("0", "0")]
    [TestCase("0000", "0")]
    [TestCase("00123", "123")]
    [TestCase("1000", "1000")]
    [TestCase("1000001", "1000001")]
    [TestCase("1000000001", "1000000001")]
    [TestCase("1000000000000000000000000000000", "1000000000000000000000000000000")]
    public void ToStringTest(string value, string expected)
    {
        var number = new BigNumber(value);
        Assert.AreEqual(expected, number.ToString());
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("-1")]
    [TestCase("+1")]
    [TestCase("1.5")]
    [TestCase("1,5")]
    [TestCase("12a")]
    [TestCase("1 000")]
    public void ConstructorTest_ThrowsOnIncorrectValue(string? value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new BigNumber(value!));
        var expected = new ArgumentException("Нужно указать неотрицательное целое число.", "value");
        Assert.That(exception!.Message, Is.EqualTo(expected.Message));
    }

    [TestCase("999999999", "1")]
    [TestCase("0", "123456789")]
    public void AddTest_DoesNotChangeNumbers(string a, string b)
    {
        var n1 = new BigNumber(a);
        var n2 = new BigNumber(b);
        var result = n1 + n2;
        Assert.AreEqual(a, n1.ToString());
        Assert.AreEqual(b, n2.ToString());
        Assert.AreNotSame(n1, result);
        Assert.AreNotSame(n2, result);
    }

    [TestCase("1000000000", "1")]
    [TestCase("123456789", "123456789")]
    public void SubTest_DoesNotChangeNumbers(string a, string b)
    {
        var n1 = new BigNumber(a);
        var n2 = new BigNumber(b);
        var result = n1 - n2;
        Assert.AreEqual(a, n1.ToString());
        Assert.AreEqual(b, n2.ToString());
        Assert.AreNotSame(n1, result);
        Assert.AreNotSame(n2, result);
    }

    [TestCase("999999999", 2)]
    [TestCase("12345", 1)]
    [TestCase("12345", 0.5)]
    public void MulTest_DoesNotChangeNumber(string a, double b)
    {
        var n1 = new BigNumber(a);
        var result = n1 * b;
        Assert.AreEqual(a, n1.ToString());
        Assert.AreNotSame(n1, result);
    }

    [TestCase("10014217", 3)]
    [TestCase("12345", 1)]
    [TestCase("12345", 0.5)]
    public void DivTest_DoesNotChangeNumber(string a, double b)
    {
        var n1 = new BigNumber(a);
        var result = n1 / b;
        Assert.AreEqual(a, n1.ToString());
        Assert.AreNotSame(n1, result);
    }
}
