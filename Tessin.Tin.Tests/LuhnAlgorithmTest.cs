using System;
using NUnit.Framework;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class LuhnAlgorithmTest
    {
        [Test]
        public void Calculate_DecimalString_ReturnsCorrectControl()
        {
            string result = LuhnAlgorithm.Calculate("201006458");
            Assert.AreEqual("8", result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Calculate_NonDecimalString_ThrowsException()
        {
            string result = LuhnAlgorithm.Calculate("178#A42B608");
        }

        [Test]
        public void Test_ValidLuhn_ReturnsTrue()
        {
            bool result = LuhnAlgorithm.Test("2010064588");
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_InvalidLuhn_ReturnsFalse()
        {
            bool result = LuhnAlgorithm.Test("2010064585");
            Assert.IsFalse(result);
        }

    }
}
