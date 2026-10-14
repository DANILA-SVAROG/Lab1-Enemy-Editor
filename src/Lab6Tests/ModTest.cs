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
}
