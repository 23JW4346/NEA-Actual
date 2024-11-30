using System;

namespace NEA.Number_Classes
{

    //inherited from number, so i could make fractions out of complex division.
    public class Fraction : Number
    {
        public new Number top1;
        public new Surd top2;
        private int denominator = 1;


        public Fraction(int innum, int inden)
        {
            denominator = inden;
            top1 = new Number(innum);
            if (denominator == 0) denominator = 1;
            if ((innum < 0 && denominator > 0) ||
                (innum > 0 && denominator < 0))
            {
                isnegative = true;
                if (denominator < 0)
                {
                    denominator = -denominator;
                    top1 = -top1;
                }
            }
            else
            {
                isnegative = false;
            }
            top2 = new Surd(0,0);
            Simplify();

        }

        public Fraction(Number n1, Surd n2, int inden)
        {
            isnegative = false;
            denominator = inden;
            top1 = n1;
            if (n2 != null)
            {
                if (n2.GetValue() == 0)
                {
                    top1 = n1;
                    top2 = new Surd(0, 0);
                }
                else
                {
                    top1 = n1;
                    top2 = n2;
                }
                if (((top1 + top2).GetValue() < 0 && denominator > 0) || ((top1 + top2).GetValue() > 0 && denominator < 0))
                {
                    isnegative = true;
                    if (denominator < 0)
                    {
                        denominator = -denominator;
                        top1 = -top1;
                        top2 = -top2;
                    }
                }
            }
            Simplify();
        }


        public override string GetString(bool isimag)
        {
            if (isimag)
            {
                if (top2.GetValue() == 0)
                {
                    if (denominator == 1) return top1.GetString(true);
                    return $"{top1.GetString(true)}/{denominator}";
                }
                else if (top1.GetValue() == 0)
                {
                    if (denominator == 1) return top2.GetString(true);
                    return $"{top2.GetString(true)}/{denominator}";
                }
                else
                {
                    if (denominator == 1)
                    {
                        if (top2.GetNegative())
                        {
                            return "(" + top1.GetString(false) + top2.GetString(false) + ")i";
                        }
                        else
                        {
                            return "(" + top1.GetString(false) + "+" + top2.GetString(false) + ")i";
                        }
                    }
                    else
                    {
                        if (top2.GetNegative())
                        {
                            return "(" + top1.GetString(false) + top2.GetString(false) + ")i/" + denominator;
                        }
                        else
                        {
                            return "(" + top1.GetString(false) + "+" + top2.GetString(false) + ")i/" + denominator;
                        }
                    }
                }
            }
            else if ((top1 + top2).GetValue() == 0) return null;
            else
            {
                if (top2.GetType() != typeof(Surd))
                {
                    if (denominator == 1) return top1.GetString(false);
                    return $"{top1.GetString(false)}/{denominator}";
                }
                else if (top1.GetValue() == 0)
                {
                    if (denominator == 1) return top2.GetString(false);
                    return $"{top2.GetString(false)}/{denominator}";
                }
                else
                {
                    if (denominator == 1)
                    {
                        if (top2.GetNegative())
                        {
                            return "(" + top1.GetString(false) + top2.GetString(false) + ")";
                        }
                        else
                        {
                            return "(" + top1.GetString(false) + "+" + top2.GetString(false) + ")";
                        }
                    }
                    else
                    {
                        if (top2.GetNegative())
                        {
                            return "(" + top1.GetString(false) + top2.GetString(false) + ")/" + denominator;
                        }
                        else
                        {
                            return "(" + top1.GetString(false) + "+" + top2.GetString(false) + ")/" + denominator;
                        }
                    }
                }
            }
        }

        public override double GetTop()
        {
            return (top1 + top2).GetValue();
        }

        public override double GetBottom() => denominator;

        public override double GetValue()
        {
            return (double)((top1 + top2).GetValue() / denominator);
        }
        public virtual void Simplify()
        {
                bool x = true;
            if (top1 == null) top1 = new Number(0);
            if (top2 == null) top2 = new Surd(0,0);
            if(top1 != null && top2 != null)
                if ((top1 + top2).GetValue() == denominator)
                {
                    top1 = new Number(1);
                    top2 = new Surd(0, 0);
                    denominator = 1;
                    x = false;
                }
                while (x)
                {
                    int hcf = HCF();
                    if (hcf > 1)
                    {
                        denominator = denominator / hcf;
                        top1 = top1 / hcf;
                        top2 = new Surd(top2.GetCoef() / hcf, top2.GetRoot());
                    }
                    else
                    {
                        x = false;
                    }
                }
        }

        public int HCF()
        {
            int loop = (int)(top1 + top2).GetValue();
            int hcf = 0;
            if (denominator < (top1 + top2).GetValue()) loop = denominator;
            for (int i = 1; i <= loop; i++)
            {
                if ((top1 + top2).GetValue() % i == 0 && denominator % i == 0)
                {
                    hcf = i;
                }
            }
            return hcf;
        }

        public static Fraction operator +(Fraction f, double d)
        {
            return new Fraction(f.top1 + d * f.GetBottom(), f.top2, (int)f.GetBottom());
        }

        public static Fraction operator *(Fraction f, Number n)
        {
            return new Fraction(f.top1 * n, f.top2 * n, (int)f.GetBottom());
        }

        public static Fraction operator -(Fraction f)
        {
            return new Fraction(f.top1, f.top2, (int)-f.GetBottom());
        }
    }
}
