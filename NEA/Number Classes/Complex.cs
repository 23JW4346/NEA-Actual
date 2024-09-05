using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{
    public class Complex
    {
        private Number real;
        private Number imaginary;

        private static Random rnd = new Random();

        private Dictionary<int, (int ,int)> modpairs = new Dictionary<int, (int, int)> { {0, (3,4) }, {1, (5,12) },
                                                                                       {2 , (8, 15)}, {3 , (7, 24) },
                                                                                       {4 , (9, 40)}
                                                                                       };



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

        public Complex(bool modquestion)
        {
            if (modquestion)
            {
                int number = rnd.Next(0, 5);
                if (rnd.Next(0, 2) == 0)
                {
                    real = new Number(modpairs[number].Item1);
                    imaginary = new Number(modpairs[number].Item2);
                }
                else
                {
                    imaginary = new Number(modpairs[number].Item1);
                    real = new Number(modpairs[number].Item2);
                }
            }
            else
            {
                real = new Number();
                imaginary = new Number();
            }
        }

        public Complex(string strcomp)
        {
            string temp = "";
            bool firstneg = true;
            foreach(char c in strcomp)
            {
                if (Char.IsNumber(c))
                {
                    temp += c;
                }
                else if(c == '-' && firstneg)
                {
                    temp += c;
                    firstneg = false;
                }
                else if (c == '+' || c == '-')
                {
                    real = new Number(double.Parse(temp));
                    temp = "";
                }
                else if (c == 'i')
                {
                    imaginary = new Number(double.Parse(temp));
                }
            }
        }

        public string GetReal() => real.GetString();

        public double GetRealValue() => real.GetValue();

        public string GetImaginary() => imaginary.GetString();

        public double GetImaginaryValue() => imaginary.GetValue();
        
        public string GetComplex()
        {
            string outputreal = "";
            string outputimag = "";
            if (real.GetString() == "0" && imaginary.GetString() == "0")
            {
                return default;
            }
            if (real.GetString() != "0")
            {
                outputreal += real.GetString();
            }
            if (imaginary.GetString() != "0")
            {
                outputimag += "";
                if (imaginary.GetString() == "1") outputimag += "i";
                else if (imaginary.GetString() == "-1") outputimag += "-i";
                else
                {
                    outputimag += imaginary.GetString()+"i";
                }
            }
            if (outputimag.Length == 0) return outputreal;
            else if(outputimag[0] == '-') return outputreal + outputimag;
            else if (outputreal.Length == 0) return outputimag;
            return outputreal + "+" +outputimag;
            

        }

        public Number GetModulus()
        {
            int pythag = (int)Math.Pow(real.GetValue(), 2) + (int)Math.Pow(imaginary.GetValue(), 2);
            if (!Math.Sqrt(pythag).ToString().Contains("."))
            {
                return new Number(Math.Sqrt(pythag));
            }
            int coef = 1;
            for (int i = 1; i < Math.Sqrt(pythag); i++)
            {
                if (pythag % (int)Math.Pow(i, 2) == 0)
                {
                    coef = i;
                }
            }
            pythag /= (int)Math.Pow(coef, 2);
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
