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
        private double step, grad;
        private bool isleft;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgtoCartesian(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            while (operand.GetComplex() == null) operand = new Complex(rnd.Next(-3,4), rnd.Next(-3 ,4));
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length); ;
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step = steps[rand];
                if (argument.GetValue() < 1 / 2 && argument.GetValue() > 0 ) grad = step;
                else grad = -step;
                if (inanswer.GetComplex()[0] == '-')
                {
                    loci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else
                {
                    loci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                }
                if (rand > 2)  isleft = true;
                else isleft = false;
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step = -steps[rand];
                if (argument.GetValue() > -1 && argument.GetValue() < -1 / 2) grad = step;
                else grad = -step;
                if (inanswer.GetComplex()[0] == '-')
                {
                    loci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else
                {
                    loci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                }
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            Calculate();
        }

        public ArgtoCartesian(string filename, Random rnd)
        {
            if (GetQuestion(filename))
            {
                Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                if (argument.GetNegative())
                {
                    Fraction temp = new Fraction((int)-argument.GetTop(), (int)argument.GetBottom());
                    step = -steps[Array.IndexOf(fractions, (temp.GetTop(), temp.GetBottom()))];
                    if (argument.GetValue() < 1 / 2 && argument.GetValue() > 0 ||
                   argument.GetValue() > -1 && argument.GetValue() < -1 / 2) grad = step;
                    else grad = -step;
                }
                else
                {
                    step = steps[Array.IndexOf(fractions, (argument.GetTop(), argument.GetBottom()))];
                    if (argument.GetValue() < 1 / 2 && argument.GetValue() > 0 ||
                   argument.GetValue() > -1 && argument.GetValue() < -1 / 2) grad = step;
                    else grad = -step;
                }
                if (inanswer.GetComplex()[0] == '-')
                {
                    loci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else loci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                if (Math.Abs(argument.GetValue()) >= 0.5) isleft = true;
                else isleft = false;
            }
            else
            {
                operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                while (operand.GetComplex() == null) operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                if (rnd.Next(2) == 1)
                {
                    int rand = rnd.Next(fractions.Length); ;
                    argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                    step = steps[rand];
                    if (argument.GetValue() < 1 / 2 && argument.GetValue() > 0) grad = step;
                    else grad = -step;
                    if (inanswer.GetComplex()[0] == '-')
                    {
                        loci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    else
                    {
                        loci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                    }
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
                    if (inanswer.GetComplex()[0] == '-')
                    {
                        loci = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    else
                    {
                        loci = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    if (rand > 2) isleft = true;
                    else isleft = false;
                }
            }
            Calculate();
        }

        public void Calculate()
        {
            double yint = grad*-operand.GetRealValue() + operand.GetImaginaryValue();
            if (grad == 0.5 || grad == -0.5)
            {
                answer += "y=" + grad * 2 + "/2x";
            }
            else if (grad == 1) answer += "y=x";
            else if (grad == -1) answer += "y=-x";
            else answer += "y=" + grad + "x";
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
            bool found = false;
            string tempfile = Path.GetTempFileName();
            using (StreamReader sr = new StreamReader(filename))
            using (StreamWriter sw = new StreamWriter(tempfile))
            {
                while (!sr.EndOfStream)
                {
                    string line;
                    line = sr.ReadLine();
                    if (line == "ArgToCart" && !found)
                    {
                        operand = new Complex(sr.ReadLine());
                        string fract = sr.ReadLine();
                        string[] numbers = fract.Trim().Split('/');
                        argument = new Fraction(int.Parse(numbers[0]), int.Parse(numbers[1]));
                        found = true;
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
                sr.Close();
                sw.Close();
            }
            File.Delete(filename);
            File.Move(tempfile, filename);
            return found;
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
            return $"Write This Loci {loci} in Cartesian Form (y=mx+c)";
        }
        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateLine(step, operand, isleft);
            Application.Run(diagram);
        }

        public void SaveQuestion(string filename)
        {
            using(StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("ArgToCart");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(argument.GetString());
            }
        }
    }
}
