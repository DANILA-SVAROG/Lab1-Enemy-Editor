using NUnit.Framework;

namespace Lab6Tests;

[TestFixture]
public class ModTest
{
    [TestCase(10, 3, 1)]
    [TestCase(7, 7, 0)]
    [TestCase(2, 5, 2)]
    [TestCase(0, 5, 0)]
    [TestCase(10, 1, 0)]
    [TestCase(12, 4, 0)]
    [TestCase(-10, 3, -1)]
    [TestCase(-12, 4, 0)]
    [TestCase(int.MaxValue, 2, 1)]
    [TestCase(int.MinValue, 3, -2)]
    [TestCase(int.MinValue, 1, 0)]
    [TestCase(int.MaxValue, int.MaxValue, 0)]
    public void ModTest_NormalCase(int a, int b, int expected)
    {
        var math = new MathFunctions();
        Assert.AreEqual(expected, math.mod(a, b));
    }

    [Test]
    public void ModTest_ThrowsOnZeroDivisor()
    {
        var math = new MathFunctions();
        var exception = Assert.Throws<ArgumentException>(() => math.mod(10, 0));
        Assert.That(exception!.Message, Is.EqualTo("Делитель должен быть положительным числом"));
    }

    [TestCase(-3)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    public void ModTest_ThrowsOnNegativeDivisor(int b)
    {
        var math = new MathFunctions();
        var exception = Assert.Throws<ArgumentException>(() => math.mod(10, b));
        Assert.That(exception!.Message, Is.EqualTo("Делитель должен быть положительным числом"));
    }

    [TestCase(10, 3)]
    [TestCase(0, 1)]
    [TestCase(-10, 3)]
    public void ModTest_DoesNotThrowOnPositiveDivisor(int a, int b)
    {
        var math = new MathFunctions();
        Assert.DoesNotThrow(() => math.mod(a, b));
    }
}
