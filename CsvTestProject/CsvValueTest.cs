using Microsoft.VisualStudio.TestTools.UnitTesting;
using SplitFormts.Values;

namespace CsvTestProject
{
    [TestClass]
    public class CsvValueTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var v1 = new ValueObject("3.14");
            Assert.AreEqual(v1.DoubleValue, 3.14);
            Assert.IsFalse(v1.IsInteger);
            Assert.IsTrue(v1.IsDouble);

            var v2 = new ValueObject("-3.14");
            Assert.AreEqual(v2.DoubleValue, -3.14);
            Assert.IsFalse(v2.IsInteger);
            Assert.IsTrue(v2.IsDouble);

            var v3 = new ValueObject("1.23e+12");
            Assert.AreEqual(v3.DoubleValue, 1.23e+12);
            Assert.IsFalse(v3.IsInteger);
            Assert.IsTrue(v3.IsDouble);

            var v4 = new ValueObject("2.43E-19");
            Assert.AreEqual(v4.DoubleValue, 2.43E-19);
            Assert.IsFalse(v4.IsInteger);
            Assert.IsTrue(v4.IsDouble);

            var v5 = new ValueObject("3.141592653589793");
            Assert.AreEqual(v5.DoubleValue, 3.141592653589793);
            Assert.IsFalse(v5.IsInteger);
            Assert.IsTrue(v5.IsDouble);

            var v6 = new ValueObject("-3.141592653589793");
            Assert.AreEqual(v6.DoubleValue, -3.141592653589793);
            Assert.IsFalse(v6.IsInteger);
            Assert.IsTrue(v6.IsDouble);

            var v7 = new ValueObject("2147483647");
            Assert.AreEqual(v7.IntegerValue, 2147483647);
            Assert.IsTrue(v7.IsInteger);
            Assert.IsTrue(v7.IsDouble);

            var v8 = new ValueObject("-2147483648");
            Assert.AreEqual(v8.IntegerValue, -2147483648L);
            Assert.IsTrue(v8.IsInteger);
            Assert.IsTrue(v8.IsDouble);
        }
    }
}
