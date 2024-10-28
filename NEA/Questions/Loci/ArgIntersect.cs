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
        private Random rnd;
        private Complex point1, point2, answerpoint;
        private Fraction angle1, angle2;
        private double step1, step2;
        string loci1, loci2;
        private bool isleft1, isleft2;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5, 1, 2, 2, 1, 0.5 };

        public ArgIntersect(Random randy)
        {
            rnd = randy;
            Calculate();
        }

        public ArgIntersect(Random randy, string filename)
        {
            rnd = randy;
            if (GetQuestion(filename))
            {

            }
            else Calculate();
        }

        public void Calculate()
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
            (double, double) point = (answerpoint.GetRealValue(), answerpoint.GetImaginaryValue());
            do
            {
                if (isleft1) point.Item1++;
                else point.Item1--;
                point.Item2 += step1;
            } while (rnd.Next(4) != 0 && point.Item1 >= -10 && point.Item1 <= 10 && point.Item2 <= 10 && point.Item2 >= -10);
            Number real, imag;
            if (point.Item1.ToString().Contains('.')) real = new Fraction((int)(point.Item1 * 2), 2);
            else real = new Number(point.Item1);
            if (point.Item1.ToString().Contains('.')) imag = new Fraction((int)(point.Item2 * 2), 2);
            else imag = new Number(point.Item2);
            point1 = new Complex(-real.GetValue(), -imag.GetValue());
            loci1 = Program.CreateArgLine(point1, angle1);
            point1 = new Complex(real.GetValue(), imag.GetValue());
            int rand2 = rnd.Next(fractions.Length);
            while (rand2 == rand) rand2 = rnd.Next(fractions.Length);
            rand = rand2;
            if (rnd.Next(2) == 0)
            {
                angle2 = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step2 = steps[rand];
                if (rand > 2) isleft2 = true;
                else isleft2 = false;
            }
            else
            {
                angle2 = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step2 = -steps[rand];
                if (rand > 2) isleft2 = true;
                else isleft2 = false;
            }
            point = (answerpoint.GetRealValue(), answerpoint.GetImaginaryValue());
            do
            {
                if (isleft2) point.Item1++;
                else point.Item1--;
                point.Item2 += step2;
            } while (rnd.Next(3) != 0 && point.Item1 >= -10 && point.Item1 <= 10 && point.Item2 <= 10 && point.Item2 >= -10);
            if (point.Item1.ToString().Contains('.')) real = new Fraction((int)(point.Item1 * 2), 2);
            else real = new Number(point.Item1);
            if (point.Item1.ToString().Contains('.')) imag = new Fraction((int)(point.Item2 * 2), 2);
            else imag = new Number(point.Item2);
            point2 = new Complex(-real.GetValue(), -imag.GetValue());
            loci2 = Program.CreateArgLine(point2, angle2);
            point2 = new Complex(real.GetValue(), imag.GetValue());
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

        public string Hint()
        {
            throw new NotImplementedException();
        }

        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateLine(-step1, point1, isleft1);
            diagram.CreateLine(-step2, point2, isleft2);
            Task.Run(() => Application.Run(diagram));
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
                angle1.GetString(),
                angle2.GetString()
            };
        }
    }
}
