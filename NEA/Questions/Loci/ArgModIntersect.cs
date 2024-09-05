using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.Loci
{
    public class ArgModIntersect : IQuestion
    {
        private string argLoci, modLoci;
        private Complex operand1, operand2;
        private Fraction argument;
        private double step;
        private int modulus;
        private bool isleft;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgModIntersect(Random rnd)
        {
            operand1 = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while (operand1.GetComplex() == null) operand1 = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex inanswer = new Complex(-operand1.GetRealValue(), -operand1.GetImaginaryValue());
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length); ;
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step = steps[rand];
                if (inanswer.GetComplex()[0] == '-')
                {
                    argLoci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else
                {
                    argLoci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                }
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step = -steps[rand];
                if (inanswer.GetComplex()[0] == '-')
                {
                    argLoci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else
                {
                    argLoci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                }
                if (rand > 2) isleft = true;
                else isleft = false;
            }

            operand2 = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while (operand2.GetComplex() == "") operand2 = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex temp = new Complex(-operand2.GetRealValue(), -operand2.GetImaginaryValue());
            modulus = rnd.Next(1, 6);
            if (temp.GetComplex()[0] == '-')
            {
                modLoci = $"|z{temp.GetComplex()}|={modulus}";
            }
            else
            {
                modLoci = $"|z+{temp.GetComplex()}|={modulus}";
            }
        }
        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public bool CheckAnswer(string answer)
        {
            throw new NotImplementedException();
        }

        public bool GetQuestion(string filename)
        {
            throw new NotImplementedException();
        }

        public void LoadDiagram()
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            throw new NotImplementedException();
        }

        public string PrintQuestion()
        {
            return $"Find if there are any intersections between {argLoci} and {modLoci}, and the points of intersections in the form a+bi.\n" +
                $"(if there are none, type 'no')";
        }

        public void SaveQuestion(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
  