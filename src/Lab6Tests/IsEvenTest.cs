using NUnit.Framework;

namespace Lab6Tests;

[TestFixture]
public class IsEvenTest
{
    [TestCase(4)]
    [TestCase(0)]
    [TestCase(-2)]
    [TestCase(2)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue - 1)]
    public void IsEvenTest_EvenNumbers(int n)
    {
        var math = new MathFunctions();
        Assert.IsTrue(math.isEven(n));
    }

    [TestCase(3)]
    [TestCase(-7)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue + 1)]
    public void IsEvenTest_OddNumbers(int n)
    {
        var math = new MathFunctions();
        Assert.IsFalse(math.isEven(n));
    }
}
