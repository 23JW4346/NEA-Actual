using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NEA.Number_Classes;

namespace NEA.Questions.Loci
{
    public class ArgtoCartesian : IQuestion
    {
        private Complex operand;
        private Fraction argument;
        private string loci, answer;
        private double step;
        private bool isleft;
        private ArgumentGraph diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, -2, -1, -0.5 };

        public ArgtoCartesian(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex conj = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            while (operand.GetComplex() == null) operand = new Complex(rnd.Next(-3,4), rnd.Next(-3 ,4));
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                if (conj.GetComplex()[0] == '-')
                {
                    loci = $"arg(z{conj.GetComplex()})={argument.GetString()}π";
                }
                else loci = $"arg(z+{conj.GetComplex()})={argument.GetString()}π";
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
                if (conj.GetComplex()[0] == '-')
                {
                    loci = $"arg(z{conj.GetComplex()})={argument.GetString()}π";
                }
                else loci = $"arg(z+{conj.GetComplex()})={argument.GetString()}π";
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
            double yint = step*-operand.GetRealValue() + operand.GetImaginaryValue();
            if (step == 0.5 || step == -0.5)
            {
                answer += "y=" + step * 2 + "/2x";
            }
            else if (step == 1) answer += "y=x";
            else if (step == -1) answer += "y=-x";
            else answer += "y=" + step + "x";
            if (yint != 0)
            {
                string yint2;
                if (yint.ToString().Contains('.'))
                {
                    yint2 = (yint * 2) + "/2";
                }
                else yint2 = yint.ToString();
                if (yint < 0) answer += yint2;
                else answer += "+" + yint2;
            }
            Console.WriteLine(answer);
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
            string output = $"Write This Loci ({loci}) in Cartesian Form (y=mx+c)";
            return output;
        }
        public void LoadDiagram()
        {
            diagram = new ArgumentGraph(step, operand, isleft);
            Application.Run(diagram);
        }

        public void SaveQuestion(string filename)
        {
            using(StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine();
                sw.WriteLine("ArgToCart");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(argument.GetString());
            }
        }
    }
}
