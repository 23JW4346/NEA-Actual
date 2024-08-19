using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.Windows.Forms;
using System.IO;

namespace NEA.Questions.Loci
{
    public class ArgGraph : IQuestion
    {
        private string answer;
        private Complex operand;
        private Fraction argument;
        private double step;
        private bool isleft;
        private ArgumentGraph diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, -2, -1, -0.5 };

        public ArgGraph(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while (operand.GetComplex() == null) operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                if (inanswer.GetComplex()[0] == '-')
                {
                    answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                step = steps[rand];
                if (argument.GetValue() >= 1 / 2)
                {
                    isleft = true;
                }
                else isleft = false;
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                if (inanswer.GetComplex()[0] == '-')
                {
                    answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                step = -steps[rand];
                if (argument.GetValue() <= -1 / 2)
                {
                    isleft = true;
                }
                else isleft = false;
            }
            Calculate();
        }

        public void Calculate()
        {
            return;
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer)
            {
                return true;
            }
            SaveQuestion("Questions.txt");
            return false;
        }

        public bool GetQuestion(string filename)
        {
            throw new NotImplementedException();
        }

        public void LoadDiagram()
        {
            diagram = new ArgumentGraph(step, operand, isleft);
            Application.Run(diagram);
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"What is the equation for this Complex loci? (no spaces, use Pi symbol π)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine();
                sw.WriteLine("ArgGraph");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(argument.GetString());
            }
        }
    }
}
