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

    [TestCase(new int[] { })]
    public void MaxTest_ThrowsOnEmptyArray(int[] numbers)
    {
        var math = new MathFunctions();
        var exception = Assert.Throws<ArgumentException>(() => math.max(numbers));
        Assert.That(exception!.Message, Is.EqualTo("Массив не должен быть пустым"));
    }

    [Test]
    public void MaxTest_ThrowsOnNullArray()
    {
        var math = new MathFunctions();
        var exception = Assert.Throws<ArgumentException>(() => math.max(null));
        Assert.That(exception!.Message, Is.EqualTo("Массив не должен быть null"));
    }

    [TestCase(new int[] { 7 })]
    [TestCase(new int[] { -5, -1 })]
    public void MaxTest_DoesNotThrowOnCorrectArray(int[] numbers)
    {
        var math = new MathFunctions();
        Assert.DoesNotThrow(() => math.max(numbers));
    }

    [TestCase(new int[] { 2, 5, 3 })]
    [TestCase(new int[] { -7 })]
    public void MaxTest_DoesNotChangeArray(int[] numbers)
    {
        var math = new MathFunctions();
        int[] original = (int[])numbers.Clone();
        math.max(numbers);
        CollectionAssert.AreEqual(original, numbers);
    }
}
