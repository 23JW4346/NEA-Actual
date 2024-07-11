using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{
    public class Complex
    {
        private Number real;
        private Number imaginary;

        public Complex(double inreal, double inimaginary)
        {
            real = new Number(inreal);
            imaginary = new Number(inimaginary);
        }
        public Complex(Number inreal, Number inimaginary)
        {
            real = inreal;
            imaginary = inimaginary;
        }

        public Complex()
        {
            real = new Number();
            imaginary = new Number();
        }

        public string GetReal() => real.GetString();

        public double GetRealValue() => real.GetValue();

        public string GetImaginary() => imaginary.GetString();

        public double GetImaginaryValue() => imaginary.GetValue();
        
        public string GetComplex()
        {
            if (real.GetString() == null)
            {
                if (imaginary.GetString() == null) return "0";
                return imaginary.GetString() + "i";
            }
            else if (imaginary.GetString() == null) return real.GetString();
            if (imaginary.GetNegative())
            {
                if ((double)imaginary.GetValue() == -1) return $"{real.GetString()}-i";
                return $"{real.GetString()}{imaginary.GetString()}i";
            }
            else if ((double)imaginary.GetValue() == 1) return $"{real.GetString()}+i";
            return $"{real.GetString()}+{imaginary.GetString()}i";

        }

        public Number GetModulus()
        {
            int pythag = (int)Math.Pow(real.GetValue(), 2) + (int)Math.Pow(imaginary.GetValue(), 2);
            int coef = 1;
            if (pythag > 4)
            {

                for (int i = 1; i < Math.Sqrt(pythag); i++)
                {
                    if (pythag % (int)Math.Pow(i, 2) == 0)
                    {
                        coef = i;
                    }
                }
                pythag /= (int)Math.Pow(coef, 2);
            }
            if (pythag == 1) return new Number(coef);
            return new Surd(coef, pythag);
        }

        public double GetArgument()
        {
            double tantheta = imaginary.GetValue() / real.GetValue();
            if (tantheta < 0) tantheta = -tantheta;
            double arg;
            if (!imaginary.GetNegative())
            {
                arg = Math.Atan(tantheta);
                if (real.GetNegative() && arg < Math.PI / 2)
                {
                    arg = Math.PI - arg;
                }
            }
            else
            {
                arg = -Math.Atan(tantheta);
                if (real.GetNegative() && arg > -Math.PI / 2)
                {
                    arg = -Math.PI + arg;
                }
                if (arg > 0) arg -= Math.PI;
            }
            return Math.Round(arg, 2);
        }
    }
}
