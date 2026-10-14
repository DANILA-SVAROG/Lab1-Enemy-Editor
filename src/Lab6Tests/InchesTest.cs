using NUnit.Framework;

namespace Lab6Tests;

[TestFixture]
public class InchesTest
{
    [TestCase(1, 2.54)]
    [TestCase(10, 25.4)]
    [TestCase(2.5, 6.35)]
    [TestCase(0.5, 1.27)]
    [TestCase(0, 0)]
    public void InchesTest_NormalCase(double inches, double expected)
    {
        var math = new MathFunctions();
        double result = math.inchesToCentimeters(inches);
        Assert.AreEqual(expected, result, 0.000000001);
    }

    [TestCase(-1)]
    [TestCase(-0.5)]
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void InchesTest_ThrowsOnIncorrectValue(double inches)
    {
        var math = new MathFunctions();
        var exception = Assert.Throws<ArgumentException>(() => math.inchesToCentimeters(inches));
        Assert.That(exception!.Message,
            Is.EqualTo("Количество дюймов должно быть конечным неотрицательным числом"));
    }

    [TestCase(double.MaxValue)]
    public void InchesTest_ThrowsOnTooLargeResult(double inches)
    {
        var math = new MathFunctions();
        var exception = Assert.Throws<ArgumentException>(() => math.inchesToCentimeters(inches));
        Assert.That(exception!.Message, Is.EqualTo("Результат перевода в сантиметры слишком велик"));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(0.25)]
    public void InchesTest_DoesNotThrowOnCorrectValue(double inches)
    {
        var math = new MathFunctions();
        Assert.DoesNotThrow(() => math.inchesToCentimeters(inches));
    }
}
