using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA.Questions.Loci
{
    public class ArgIntersect : IQuestion
    {
        private Complex point1, point2, answerpoint;
        private Fraction angle1, angle2;
        private double step1, step2;
        string loci1, loci2;
        private bool isleft1, isleft2;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgIntersect(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public ArgIntersect(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
            }
            Calculate();
        }

        public Complex GetPoint(Random rnd, double s, bool isleft)
        {
            Complex ret;
            double xint = answerpoint.GetRealValue(), yint = answerpoint.GetImaginaryValue();
            int loop = rnd.Next(3, 6);
            Number real, imag;
            while (loop > 0 && -9 < xint && xint < 9 && -9 < yint && yint < 9)
            {
                if (isleft) xint++;
                else xint--;
                yint -= s;
                loop--;
            }
            if (xint.ToString().Contains('.'))
            {
                real = new Fraction((int)(xint*2), 2);
            }
            else
            {
                real = new Number(xint);
            }
            if (yint.ToString().Contains("."))
            {
                imag = new Fraction((int)(yint*2), 2);
            }
            else
            {
                imag = new Number(yint);
            }
            ret = new Complex(real, imag);
            if (ret.GetComplex() == answerpoint.GetComplex()) ret = GetPoint(rnd, s, isleft);
            return ret;
        }

        public void GenQ(Random rnd)
        {
            answerpoint = new Complex(rnd.Next(-5, 6), rnd.Next(-5, 6));
            int rand = rnd.Next(fractions.Length);
            if (rnd.Next(2) == 0)
            {
                angle1 = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step1 = steps[rand];
                if (rand > 2) isleft1 = true;
                else isleft1 = false;
            }
            else
            {
                angle1 = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step1 = -steps[rand];
                if (rand > 2) isleft1 = true;
                else isleft1 = false;
            }
            double xint = answerpoint.GetRealValue(), yint = answerpoint.GetImaginaryValue();
            int loop = rnd.Next(2,6);
            point1 = GetPoint(rnd, step1, isleft1);
            do
            {
                int rand2 = rnd.Next(fractions.Length);
                if (rnd.Next(2) == 0)
                {
                    angle2 = new Fraction(fractions[rand2].Item1, fractions[rand2].Item2);
                    step2 = steps[rand2];
                    if (rand2 > 2) isleft2 = true;
                    else isleft2 = false;
                }
                else
                {
                    angle2 = new Fraction(-fractions[rand2].Item1, fractions[rand2].Item2);
                    step2 = -steps[rand2];
                    if (rand2 > 2) isleft2 = true;
                    else isleft2 = false;
                }

            } while (step1 == step2 || (step1 == -step2 && isleft1 != isleft2));
            point2 = GetPoint(rnd, step2, isleft2);
        }

        public void Calculate()
        {
            Complex temp = point1.Flip();
            loci1 = Program.CreateArgLine(temp, angle1);
            temp = point2.Flip();
            loci2 = Program.CreateArgLine(temp, angle2);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == answerpoint.GetComplex())
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
                    if (line == "ArgInt" && !found)
                    {
                        found = true;
                        answerpoint = new Complex(sr.ReadLine());
                        point1 = new Complex(sr.ReadLine());
                        point2 = new Complex(sr.ReadLine());
                        string fract = sr.ReadLine();
                        string[] numbers = fract.Trim().Split('/');
                        angle1 = new Fraction(int.Parse(numbers[0]), int.Parse(numbers[1]));
                        fract = sr.ReadLine();
                        numbers = fract.Trim().Split('/');
                        angle2 = new Fraction(int.Parse(numbers[0]), int.Parse(numbers[1]));
                        loci1 = sr.ReadLine();
                        loci2 = sr.ReadLine();
                        step1 = int.Parse(sr.ReadLine());
                        if (Array.IndexOf(steps, step1) > 2) isleft1 = true;
                        else isleft1 = false;
                        step2 = int.Parse(sr.ReadLine());
                        if (Array.IndexOf(steps, step2) > 2) isleft2 = true;
                        else isleft2 = false;
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
            diagram.CreateLine(step1, point1, isleft1, loci1);
            diagram.CreateLine(step2, point2, isleft2, loci2);
            Task.Run(() => Application.Run(diagram));
        }

        public void CloseDiagram()
        {
            diagram.Close();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct, the answer is {answerpoint.GetComplex()}";
            }
            else
            {
                return $"Incorrect, the answer was {answerpoint.GetComplex()}";
            }
        }

        public string PrintQuestion()
        {
            return $"The half lines {loci1} and {loci2} intersect at a point. write the complex point in the form a+bi.";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ArgInt",
                answerpoint.GetComplex(),
                point1.GetComplex(),
                point2.GetComplex(),
                angle1.GetString(false),
                angle2.GetString(false),
                loci1,
                loci2,
                step1.ToString(),
                step2.ToString(),
            };
        }
    }
}
