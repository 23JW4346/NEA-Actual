using System;
using System.Collections.Generic;

namespace NEA.Number_Classes
{
    public class Complex
    {
        public Number real;
        private Number imaginary;

        private static Random rnd = new Random();

        private Dictionary<int, (int, int)> modpairs = new Dictionary<int, (int, int)> { {0, (3,4) }, {1, (5,12) },
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
        //base consturctor, takes in a bool to either generate it so mod is a whole number or not. 
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
        //Gets the complex from the text file, and converts it to the object
        public Complex(string strcomp)
        {
            string temp = "";
            bool firstneg = true;
            foreach (char c in strcomp)
            {
                if (Char.IsNumber(c))
                {
                    temp += c;
                }
                else if (c == '-' && firstneg)
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

        public Complex Flip()
        {
            Number outreal, outimag;
            if (real.GetType() == typeof(Fraction))
            {
                real = (Fraction)real;
                outreal = new Fraction((int)(-real.GetTop()), (int)real.GetBottom());
            }
            else outreal = new Number(-real.GetValue());
            if (imaginary.GetType() == typeof(Fraction))
            {
                imaginary = (Fraction)imaginary;
                outimag = new Fraction((int)(-imaginary.GetTop()), (int)imaginary.GetBottom());
            }
            else outimag = new Number(-imaginary.GetValue());
            return new Complex(outreal, outimag);
        }

        public string GetReal() => real.GetString(false);

        public double GetRealValue() => real != null? real.GetValue() : 0;

        public double GetImaginaryValue() => imaginary != null? imaginary.GetValue() : 0;
        //returns the Complex number string
        public string GetComplex()
        {
            string outputreal = "";
            string outputimag = "";
            if (real.GetString(false) == "0" && imaginary.GetString(true) == "0")
            {
                return default;
            }
            if (real.GetString(false) != "0")
            {
                outputreal += real.GetString(false);
            }
            if (imaginary.GetString(false) != "0")
            {
                outputimag += "";
                if (imaginary.GetValue() == 1) outputimag += "i";
                else if (imaginary.GetValue() == -1) outputimag += "-i";
                else
                {
                    outputimag += imaginary.GetString(true);
                }
            }
            if (outputimag.Length == 0) return outputreal;
            else if (outputimag[0] == '-') return outputreal + outputimag;
            else if (outputreal.Length == 0) return outputimag;
            return outputreal + "+" + outputimag;
        }
        //Calculates the Modulus of the Complex number
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

        //calculates the Argument of the complex number
        public double GetArgument()
        {
            double tantheta = imaginary.GetValue() / real.GetValue();
            double arg = Math.Atan(Math.Abs(tantheta));
            if(imaginary.GetValue() > 0)
            {
                if(real.GetValue() < 0) arg = Math.PI - arg;
            }
            else if (imaginary.GetValue() < 0)
            {
                if (real.GetValue() < 0) arg = -Math.PI + arg;
                else arg = -arg;
            }
            return arg;
        }
    }
}

