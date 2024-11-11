using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA.Questions.Loci
{
    public class ArgModIntersect : IQuestion
    {
        private string argLoci, modLoci;
        private Complex midpoint, point1, point2, argStart;
        private Fraction argument;
        private double step;
        private Number modulus;
        private bool isleft;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgModIntersect(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public ArgModIntersect(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
            }
            Calculate();
        }

        public void GenQ(Random rnd)
        {
            point1 = new Complex(rnd.Next(-2,3), rnd.Next(-2,3));
            if (rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length); ;
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step = steps[rand];
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step = -steps[rand];
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            int loop = rnd.Next(2,4);
            midpoint = GetPoint(point1, loop, -step, !isleft);
            point2 = GetPoint(midpoint, loop, -step, !isleft);
            argStart = GetPoint(point1, rnd.Next(1, 4), step, isleft);
        }

        public Complex GetPoint(Complex a, int loop, double s, bool isleft)
        {
            Complex ret;
            double xint = a.GetRealValue(), yint = a.GetImaginaryValue();
            Number real, imag;
            while (loop > 0)
            {
                if (isleft) xint++;
                else xint--;
                yint -= s;
                loop--;
            }
            if (xint.ToString().Contains('.'))
            {
                real = new Fraction((int)(xint * 2), 2);
            }
            else
            {
                real = new Number(xint);
            }
            if (yint.ToString().Contains("."))
            {
                imag = new Fraction((int)(yint * 2), 2);
            }
            else
            {
                imag = new Number(yint);
            }
            ret = new Complex(real, imag);
            if (ret.GetComplex() == a.GetComplex()) ret = GetPoint(a, loop, s, isleft);
            return ret;
        }

        public void Calculate()
        {
            double xmod = midpoint.GetRealValue() - point1.GetRealValue();
            double ymod = midpoint.GetImaginaryValue() - point1.GetImaginaryValue();
            Complex modGetter = new Complex(xmod, ymod);
            modulus = modGetter.GetModulus();
            argLoci = Program.CreateArgLine(argStart.Flip(), argument);
            modLoci = Program.CreateModCircle(midpoint.Flip(), modulus.GetString(false));
        }

        public bool CheckAnswer(string answer)
        {
            if (answer.Contains(','))
            {
                string[] answers = answer.Split(',');

                if (answers[0] == point1.GetComplex() || answers[1] == point1.GetComplex() && answers[1] != answers[0])
                {
                    if (answers[0] == point2.GetComplex() || answers[1] == point2.GetComplex())
                    {
                        return true;
                    }
                }
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
                    if (line == "ArgInt" && !found)
                    {
                        found = true;
                        midpoint = new Complex(sr.ReadLine());
                        argStart = new Complex(sr.ReadLine());
                        point1 = new Complex(sr.ReadLine());
                        point2 = new Complex(sr.ReadLine());
                        string fract = sr.ReadLine();
                        string[] numbers = fract.Trim().Split('/');
                        argument = new Fraction(int.Parse(numbers[0]), int.Parse(numbers[1]));
                        step = int.Parse(sr.ReadLine());
                        if (Array.IndexOf(steps, step) > 2) isleft= true;
                        else isleft = false;
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


        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateCircle(midpoint, modulus.GetValue(), modLoci);
            diagram.CreateLine(step, argStart, isleft, argLoci);
            Task.Run(() => Application.Run(diagram));
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {point1.GetComplex()},{point2.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {point1.GetComplex()},{point2.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"{argLoci} and {modLoci} Intersect at 2 points. find the 2 points in the form a+bi, write both out like a+bi,c+di";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>()
            {
                "ArgModInt",
                midpoint.GetComplex(),
                argStart.GetComplex(),
                point1.GetComplex(),
                point2.GetComplex(),
                argument.GetString(false),
                step.ToString(),
            };

        }

        public void CloseDiagram()
        {
            diagram.Hide();
        }
    }
}
