using System;

namespace NEA.Number_Classes
{
    public class Surd : Number
    {
        public int coefficient;
        public int root;

        public Surd(int incoef, int inroot)
        {
            coefficient = incoef;
            root = inroot;
        }

        public Surd()
        {
            coefficient = rnd.Next(-5, 6);
            int[] roots = { 2, 3, 5, 7 };
            root = roots[rnd.Next(roots.Length)];
        }

        public override string GetString(bool isimag)
        {
            if (isimag)
            {
                if (coefficient== 1) return $"√{root}i";
                else return $"{coefficient}√{root}i";
            }
            if (coefficient == 1) return $"√{root}";
            else return $"{coefficient}√{root}";
        }

        public override double GetValue()
        {
            return coefficient * Math.Sqrt(root);
        }

        public int GetCoef() => coefficient;

        public override int GetRoot() => root;

    }
}
