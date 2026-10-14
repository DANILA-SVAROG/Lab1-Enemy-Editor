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
}
