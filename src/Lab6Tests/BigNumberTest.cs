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
}
