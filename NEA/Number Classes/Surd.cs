﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{
    public class Surd : Number
    {
        private int coefficent, root;

        public Surd(int incoef, int inroot)
        {
            coefficent = incoef;
            root = inroot;
        }

        public Surd()
        {
            coefficent = rnd.Next(-5, 6);
            int[] roots = { 2, 3, 5, 7 };
            root = roots[rnd.Next(roots.Length)];
        }

        public override string GetString()
        {
            if (coefficent == 1) return $"√{root}";
            else return $"{coefficent}√{root}";
        }

        public override double GetValue()
        {
            return coefficent * Math.Sqrt(root);
        }
    }
}
