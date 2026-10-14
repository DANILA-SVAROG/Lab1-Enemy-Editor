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
}
