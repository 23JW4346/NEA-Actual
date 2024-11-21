using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{

    //inherited from number, so i could make fractions out of complex division
    public class Fraction : Number
    {
        private int numerator = default;
        private int denominator = 1;


        public Fraction(int innum, int inden) : base(((double)innum/(double)inden))
        {
            numerator = innum;
            denominator = inden;
            if (denominator == 0) denominator = 1;
            if ((numerator < 0 && denominator > 0) ||
                (numerator > 0 && denominator < 0))
            {
                isnegative = true;
                if (numerator < 0) numerator = -numerator;
                if (denominator < 0) denominator = -denominator;
            }
            else
            {
                isnegative = false;
                if (numerator < 0) numerator = -numerator;
                if (denominator < 0) denominator = -denominator;
            }
            Simplify();

        }
        public Fraction()
        {
            while (numerator == default) numerator = rnd.Next(-10, 11);
            while (denominator == default) denominator = rnd.Next(-10, 11);
            if ((numerator < 0 && denominator > 0) ||
                (numerator > 0 && denominator < 0))
            {
                isnegative = true;
                if (numerator < 0) numerator = -numerator;
                if (denominator < 0) denominator = -denominator;
            }
            else
            {
                isnegative = false;
                if (numerator < 0) numerator = -numerator;
                if (denominator < 0) denominator = -denominator;
            }
            Simplify();
        }


        public override string GetString(bool isimag)
        {
            if (isimag)
            {
                if (isnegative)
                {
                    if (denominator == 1)
                    {
                        if (numerator != 1) return $"-{numerator}i";
                        return "-i";
                    }
                    else if (numerator == 1) return $"-i/{denominator}";
                    return $"-{numerator}i/{denominator}";
                }
                else if (denominator == 1)
                {
                    if (numerator != 1) return $"{numerator}i";
                    return "i";
                }
                else if (numerator == 1) return $"i/{denominator}";
                else if (numerator == 0) return "0";
                return $"{numerator}i/{denominator}";
            }
            if (isnegative)
            {
                if (denominator == 1) return $"-{numerator}";
                return $"-{numerator}/{denominator}";
            }
            else if (denominator == 1) return $"{numerator}";
            else if (numerator == 0) return "0";
            return $"{numerator}/{denominator}";
        }

        public override double GetTop()
        {
            if (isnegative) return -numerator;
            return numerator;
        }

        public override double GetBottom() => denominator;

        public virtual void Simplify()
        {
            bool x = true;
            if (numerator == denominator)
            {
                numerator = 1;
                denominator = 1;
                x = false;
            }
            while (x)
            {
                int hcf = HCF();
                if (hcf > 1)
                {
                    denominator = denominator / hcf;
                    numerator = numerator / hcf;
                }
                else
                {
                    x = false;
                }
            }
        }

        public int HCF()
        {
            int loop = numerator;
            int hcf = 0;
            if (denominator < numerator) loop = denominator;
            for (int i = 1; i <= loop; i++)
            {
                if (numerator % i == 0 && denominator % i == 0)
                {
                    hcf = i;
                }
            }
            return hcf;
        }
    }
}
