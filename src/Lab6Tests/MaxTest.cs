using NUnit.Framework;

namespace Lab6Tests;

[TestFixture]
public class MaxTest
{
    [TestCase(new int[] { 2, 5, 3 }, 5)]
    [TestCase(new int[] { 5, 3, 2 }, 5)]
    [TestCase(new int[] { 1, 2, 5 }, 5)]
    [TestCase(new int[] { 7 }, 7)]
    [TestCase(new int[] { -7 }, -7)]
    [TestCase(new int[] { -10, -2, -5 }, -2)]
    [TestCase(new int[] { -3, 0, 2 }, 2)]
    [TestCase(new int[] { 4, 4, 4 }, 4)]
    [TestCase(new int[] { 0, 0 }, 0)]
    [TestCase(new int[] { int.MinValue }, int.MinValue)]
    [TestCase(new int[] { int.MinValue, int.MaxValue }, int.MaxValue)]
    public void MaxTest_NormalCase(int[] numbers, int expected)
    {
        var math = new MathFunctions();
        int result = math.max(numbers);
        Assert.AreEqual(expected, result);
    }
}
