using System;
using System.Collections.Generic;
using System.IO;
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
        private double step, m;
        private bool isleft;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgtoCartesian(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public ArgtoCartesian(string filename, Random rnd)
        {
            if (GetQuestion(filename))
            {
                Complex inanswer = operand.Flip();
                if (argument.GetNegative())
                {
                    Fraction temp = new Fraction((int)-argument.GetTop(), (int)argument.GetBottom());
                    step = -steps[Array.IndexOf(fractions, (temp.GetTop(), temp.GetBottom()))];
                }
                else
                {
                    step = steps[Array.IndexOf(fractions, (argument.GetTop(), argument.GetBottom()))];
                }
                loci = Program.CreateArgLine(inanswer, argument);
                if (Math.Abs(argument.GetValue()) >= 0.5) isleft = true;
                else isleft = false;
            }
            else
            {
                GenQ(rnd);
            }
            Calculate();
        }

        public void GenQ(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex inanswer = operand.Flip();
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length); ;
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step = steps[rand];
                loci = Program.CreateArgLine(inanswer, argument);
                if (rand > 2)
                {
                    isleft = true;
                    m = -step;
                }
                else
                {
                    isleft = false;
                    m = step;
                }
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step = -steps[rand];
                loci = Program.CreateArgLine(inanswer, argument);
                if (rand > 2)
                {
                    isleft = true;
                    m = -step;
                }
                else
                {
                    isleft = false;
                    m = step;
                }
            }
        }

        public void Calculate()
        {
            answer = Program.CreateCartesianLine(operand, m);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer)
            {
                return true;
            }
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
            return $"Write This Loci {loci} in Cartesian Form y=mx+c, where m and c are rational numbers";
        }
        public void LoadDiagram(ArgandDiagram diagram)
        {
            diagram.CreateLine(step, operand, isleft, loci);
            Task.Run(() =>
            {
                Application.Run(diagram);
            });
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ArgToCart",
                operand.GetComplex(),
                argument.GetString(false)
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            diagram.Invoke((Action)(() =>
            {
                diagram.Close();
            }));
        }
    }
}
