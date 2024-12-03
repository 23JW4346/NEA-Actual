using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace NEA.Number_Classes
{
    public class Complex
    {
        public Number real;
        private Number imaginary;

        private static Random rnd = new Random();
        //for generating questions using modulus so that it doesn't contain surds
        private Dictionary<int, (int, int)> modpairs = new Dictionary<int, (int, int)> { {0, (3,4) }, {1, (5,12) },
                                                                                       {2 , (8, 15)}, {3 , (7, 24) },
                                                                                       {4 , (9, 40)}
                                                                                       };

        //takes in 2 doubles, meaning it could have decimals as the 2 parts
        public Complex(double inreal, double inimaginary)
        {
            real = new Number(inreal);
            imaginary = new Number(inimaginary);
        }
        //takes in 2 numbers, good for if they contain surds/fractions
        public Complex(Number inreal, Number inimaginary)
        {
            real = inreal;
            imaginary = inimaginary;
        }
        //base consturctor, takes in a bool to either generate it so mod is a whole number (not a surd) or not. 
        public Complex(bool modquestion)
        {
            if (modquestion)
            {
                int number = rnd.Next(0, 5);
                if (rnd.Next(0, 2) == 0)
                {
                    if(rnd.Next(2) == 0) real = new Number(modpairs[number].Item1);
                    else real = new Number(-modpairs[number].Item1);
                    if(rnd.Next(2) == 0) imaginary = new Number(modpairs[number].Item2);
                    else imaginary = new Number(-modpairs[number].Item2);
                }
                else
                {
                    if (rnd.Next(2) == 0) real = new Number(modpairs[number].Item2);
                    else real = new Number(-modpairs[number].Item2);
                    if (rnd.Next(2) == 0) imaginary = new Number(modpairs[number].Item1);
                    else imaginary = new Number(-modpairs[number].Item1);
                }
            }
            else
            {
                real = new Number();
                imaginary = new Number();
            }
        }
        //Gets the complex from a string, and converts it to the object
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
                    if (c == '-') temp = "-";
                    else temp = "";
                }
                else if (c == 'i')
                {
                    if (temp == "")
                    {
                        imaginary = new Number(1);
                    }
                    else if(temp == "-")
                    {
                        imaginary = new Number(-1);
                    }
                    else imaginary = new Number(double.Parse(temp));
                }
                firstneg = false;
            }
            if(real == null) real = new Number(0);
            if(imaginary == null) imaginary = new Number(0);
            firstneg = false;
        }

        //Gets the negative of the Complex
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
        //gets the real part as a string
        public string GetReal() => real.GetString(false);
        //returns the real part value
        public double GetRealValue() => real != null? real.GetValue() : 0;
        //returns the imagainary part value
        public double GetImaginaryValue() => imaginary != null? imaginary.GetValue() : 0;
        //returns the Complex number string
        public string GetComplex()
        {
            string outputreal = "";
            string outputimag = "";
            if (real.GetString(false) == "0" && imaginary.GetString(true) == "0")
            {
                return "0";
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

        //Calculates the Argument of the complex number
        public double GetArgument()
        {
            if(real.GetValue() ==0 &  imaginary.GetValue() == 0) return 0; 
            else if (real.GetValue() == 0) return imaginary.GetNegative() ? Math.PI / 2 : -Math.PI / 2;
            else if (imaginary.GetValue() == 0) return real.GetNegative() ? Math.PI : 0;
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
        //no idea
        public override bool Equals(object obj)
        {
            return obj is Complex complex &&
                   EqualityComparer<Number>.Default.Equals(real, complex.real) &&
                   EqualityComparer<Number>.Default.Equals(imaginary, complex.imaginary);
        }
        //no idea
        public override int GetHashCode()
        {
            int hashCode = -1613305685;
            hashCode = hashCode * -1521134295 + EqualityComparer<Number>.Default.GetHashCode(real);
            hashCode = hashCode * -1521134295 + EqualityComparer<Number>.Default.GetHashCode(imaginary);
            return hashCode;
        }

        //returns a*b
        public static Complex operator * (Complex a, Complex b)
        {
            return new Complex(a.GetRealValue() * b.GetRealValue() - a.GetImaginaryValue() * b.GetImaginaryValue(), a.GetImaginaryValue() * b.GetRealValue() + a.GetRealValue() * b.GetImaginaryValue());
        }
        //multiplies complex by scalar
        public static Complex operator * (double a, Complex b)
        {
            return new Complex(a * b.GetRealValue(), a*b.GetImaginaryValue());
        }

        public static Complex operator *(Complex a, double b) => b * a;
        //returns the calculation a/b
        public static Complex operator /(Complex a, Complex b)
        {
            Fraction RealPart = new Fraction((int)(a.GetRealValue() * b.GetRealValue() - a.GetImaginaryValue() * -b.GetImaginaryValue()), (int)(b.GetRealValue() * b.GetRealValue() - b.GetImaginaryValue() * -b.GetImaginaryValue()));
            Fraction ImagPart = new Fraction((int)(a.GetRealValue() * -b.GetImaginaryValue() + a.GetImaginaryValue() * b.GetRealValue()), (int)(b.GetRealValue() * b.GetRealValue() - b.GetImaginaryValue() * -b.GetImaginaryValue()));
            return new Complex(RealPart, ImagPart);
        }
        //returns true if the answer is equal to user answer
        public static bool operator ==(Complex a, Complex b)
        {
            if(a.GetRealValue() == b.GetRealValue() && a.GetImaginaryValue() == b.GetImaginaryValue()) return true;
            return false;
        }

        public static bool operator !=(Complex a, Complex b) 
        {
            return !(a == b);
        }

    }
}

