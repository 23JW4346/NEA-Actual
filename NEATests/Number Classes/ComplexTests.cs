using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes.Tests
{
    [TestClass()]
    public class ComplexTests
    {
        [TestMethod()]
        public void GetModulusTest()
        {
            Complex t1 = new Complex(3, 4);
            Number t1Expected = new Number(5);
            Complex t2 = new Complex(2, -2);
            Number t2Expected = new Surd(2, 2);
            Complex t3 = new Complex(-5, 12);
            Number t3Expected = new Number(13);
            Assert.AreEqual(t1.GetModulus().GetString(false), t1Expected.GetString(false));
            Assert.AreEqual(t2.GetModulus().GetString(false), t2Expected.GetString(false));
            Assert.AreEqual(t3.GetModulus().GetString(false), t3Expected.GetString(false));

        }
        [TestMethod()]
        public void GetArgumentTest()
        {
            Complex t1 = new Complex(2,2);
            double t1Expected = Math.Atan(2/2);
            Complex t2 = new Complex(5,-3);
            double t2Expected = -Math.Atan((double)3/5);
            Complex t3 = new Complex(0,0);
            double t3Expected = 0;
            Assert.AreEqual(t1.GetArgument(), t1Expected);
            Assert.AreEqual(t2.GetArgument(), t2Expected);
            Assert.AreEqual(t3.GetArgument(), t3Expected);
        }

        [TestMethod()]
        public void MultiplyTest()
        {
            Complex c1 = new Complex(5, 3);
            Complex c2 = new Complex(5, 2);
            Complex expected = new Complex(19,25);
            Complex c3 = new Complex(2, 3);
            Assert.AreEqual((c1 * c2).GetComplex(), expected.GetComplex());
            expected = new Complex(1,21);
            Assert.AreEqual((c1 * c3).GetComplex(), expected.GetComplex());
            expected = new Complex(4,19);
            Assert.AreEqual((c2*c3).GetComplex(), expected.GetComplex());
        }
    }
}