using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{
    public class SurdFraction : Fraction
    {
        private Surd numerator;
        private int denominator;

        public SurdFraction(Surd top, int bottom)
        {
            numerator = top;
            denominator = bottom;
            Simplify();
        }

        public override string GetString()
        {
            if (denominator != 1)
            {
                return $"{numerator.GetString()}/{denominator};";
            }
            return $"{numerator.GetString()}";
        }

        public override double GetValue()
        {
            return numerator.GetValue() / denominator;
        }

        public override double GetTop() => numerator.GetValue();

        public override double GetBottom() => denominator;

        public override void Simplify()
        {
            if (numerator.GetCoef() % denominator == 0)
            {
                numerator = new Surd(numerator.GetCoef() / denominator, numerator.GetRoot());
                denominator = 1;
            }
        }
    }
}
