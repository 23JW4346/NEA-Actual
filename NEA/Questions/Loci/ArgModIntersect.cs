using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NEA.Questions.Loci
{
    public class ArgModIntersect : IQuestion
    {
        private string argLoci, modLoci;
        private Complex midpoint, answer;
        private Fraction argument;
        private double step, grad;
        private int modulus;
        private bool isleft;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgModIntersect(Random rnd)
        {
            midpoint = new Complex(rnd.Next(-4, 5), rnd.Next(-4, 5));
            Complex inanswer = new Complex(-midpoint.GetRealValue(), -midpoint.GetImaginaryValue());
            modulus = rnd.Next(1, 6);
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length); ;
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step = steps[rand];
                if (argument.GetValue() < 1 / 2 && argument.GetValue() > 0) grad = step;
                else grad = -step;
                argLoci = Program.CreateArgLine(inanswer, argument);
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step = -steps[rand];
                if (argument.GetValue() > -1 && argument.GetValue() < -1 / 2) grad = step;
                else grad = -step;
                argLoci = Program.CreateArgLine(inanswer, argument);
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            modLoci = Program.CreateModCircle(inanswer, modulus);
            Calculate();
        }
        public void Calculate()
        {
            Number realpart = null, imagpart = null;
            Surd temp;
            double real = Math.Sin(argument.GetValue() * Math.PI);
            if(Math.Abs(real / Math.Sqrt(2)) == 1 / 2)
            {
                temp  = new Surd(modulus, 2);
                if(real > 0)
                {
                    realpart = new SurdFraction(temp, -2);

                }
                else realpart = new SurdFraction(temp, 2);
            }
            else if (Math.Abs(real / Math.Sqrt(3)) == 1 / 2)
            {
                temp = new Surd(modulus, 3);
                if (real > 0)
                {
                    realpart = new SurdFraction(temp, -2);

                }
                else realpart = new SurdFraction(temp, 2);
            }
            else
            {
                if (real > 0)
                {
                    realpart = new Fraction(modulus, 2);
                }
                else realpart = new Fraction(modulus, -2);
            }
            double imag = Math.Cos(argument.GetValue() * Math.PI);
            if (Math.Abs(imag / Math.Sqrt(2)) == 1 / 2)
            {
                temp = new Surd(modulus, 2);
                if (imag > 0)
                {
                    imagpart = new SurdFraction(temp, -2);

                }
                else imagpart = new SurdFraction(temp, 2);
            }
            else if (Math.Abs(imag / Math.Sqrt(3)) == 1 / 2)
            {
                temp = new Surd(modulus, 3);
                if (imag > 0)
                {
                    imagpart = new SurdFraction(temp, -2);

                }
                else imagpart = new SurdFraction(temp, 2);
            }
            else
            {
                if (imag > 0)
                {
                    imagpart = new Fraction(modulus, 2);
                }
                else imagpart = new Fraction(modulus, -2);
            }
            answer = new Complex(realpart, imagpart);
        }

        public bool CheckAnswer(string answer)
        {
            throw new NotImplementedException();
        }

        public bool GetQuestion(string filename)
        {
            throw new NotImplementedException();
        }

        public string Hint()
        {
            throw new NotImplementedException();
        }

        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateCircle(midpoint, modulus);
            diagram.CreateLine(step, midpoint, isleft);
            Task.Run(() => diagram);
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"{argLoci} and {modLoci} Intersect at the point P. Find P in the form a+bi.";
        }

        public List<string> SaveQuestion()
        {
            throw new NotImplementedException();
        }
    }
}
