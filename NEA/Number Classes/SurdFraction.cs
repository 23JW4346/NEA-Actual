﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{
    public class SurdFraction : Fraction
    {
        private Surd numerator = new Surd();
        private int denominator = 1;

        public SurdFraction(Surd top, int bottom)
        {
            numerator = top;
            denominator = bottom;
            Simplify();
        }

        public override string GetString(bool isimag)
        {
            if (isimag)
            {
                if (denominator != 1)
                {
                    return $"{numerator.GetString(true)}/{denominator}";
                }
                return $"{numerator.GetString(true)}";
            }
            if (denominator != 1)
            {
                return $"{numerator.GetString(false)}/{denominator}";
            }
            return $"{numerator.GetString(false)}";
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