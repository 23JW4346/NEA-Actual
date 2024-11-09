using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA.Questions.Loci
{
    public class ModLine : IQuestion
    {
        private ArgandDiagram diagram;
        private Complex midpoint, operand1, operand2;
        private Number gradient;
        private double grad;
        private string answer, equation;

        public ModLine(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public ModLine(string filename, Random rnd)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
            }
            Calculate();
        }

        public void GenQ(Random rnd)
        {
            int rand = rnd.Next(3);
            if (rand == 1)
            {
                gradient = new Fraction(rnd.Next(1, 3), rnd.Next(1, 4));
            }
            else if (rand == 2)
            {
                gradient = new Fraction(rnd.Next(-3, 0), rnd.Next(1, 4));
            }
            else
            {
                gradient = new Number(rnd.Next(-4, 6));
            }
            midpoint = new Complex(rnd.Next(6), rnd.Next(6));
            if (gradient.GetValue() == 0)
            {
                rand = rnd.Next(1, 5);
                operand1 = new Complex(midpoint.GetRealValue(), midpoint.GetImaginaryValue() + rand);
                operand2 = new Complex(midpoint.GetRealValue(), midpoint.GetImaginaryValue() - rand);
            }
            else if (gradient.GetValue() == 5)
            {
                rand = rnd.Next(1, 5);
                operand1 = new Complex(midpoint.GetRealValue() + rand, midpoint.GetImaginaryValue());
                operand2 = new Complex(midpoint.GetRealValue() - rand, midpoint.GetImaginaryValue());
            } 
            else
            {
                grad = gradient.GetValue();
                GetPoints(rnd.Next(1, 5));
            }
        }

        private Complex GetComp(double xint, double yint)
        {
            Number real = new Number(); 
            Number imag = new Number();
            Complex ret;
            if (xint.ToString().Contains('.'))
            {
                switch (xint.ToString()[2])
                {
                    case '5':
                        real = new Fraction((int)(xint * 2), 2);
                        break;
                    case '3':
                    case '6':
                        real = new Fraction((int)(xint * 3), 3);
                        break;
                    case '2':
                        if (xint.ToString().Length == 3) real = new Fraction((int)(xint * 5), 5);
                        else real = new Fraction((int)(xint * 4), 4);
                        break;
                }
            }
            else real = new Number(xint);
            if (yint.ToString().Contains('.'))
            {
                switch (yint.ToString()[2])
                {
                    case '5':
                        imag = new Fraction((int)(yint * 2), 2);
                        break;
                    case '3':
                    case '6':
                        imag = new Fraction((int)(yint * 3), 3);
                        break;
                    case '2':
                        if (yint.ToString().Length == 3) imag = new Fraction((int)(yint * 5), 5);
                        else imag = new Fraction((int)(yint * 4), 4);
                        break;
                }
            }
            else imag = new Number(yint);
            ret = new Complex(real, imag);
            return ret;
        }

        private void GetPoints(int space)
        {
            double xint = midpoint.GetRealValue();
            double yint = midpoint.GetImaginaryValue();
            double negRec = - (1 / grad);
            for (int i = 0; i < space; i++)
            {
                    xint++;
                    yint += negRec;
            }
            operand1 = GetComp(xint, yint);
            xint = midpoint.GetRealValue();
            yint = midpoint.GetImaginaryValue();
            for (int i = 0; i < space; i++)
            {
                    xint--;
                    yint -= negRec;
            }
            operand2 = GetComp(xint, yint);
        }

        public void Calculate()
        {
            equation = Program.CreateModLine(operand1.Flip(), operand2.Flip());
            answer = Program.CreateCartesianLine(midpoint, grad);
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

                    if (line == "ModLine" && !found)
                    {
                        operand1 = new Complex(sr.ReadLine());
                        operand2 = new Complex(sr.ReadLine());
                        string line2 = sr.ReadLine();
                        if (line2.Contains('/'))
                        {
                            string[] fract = line2.Split('/');
                            gradient = new Fraction(int.Parse(fract[0]), int.Parse(fract[1]));
                        }
                        else gradient = new Number(double.Parse(line2));
                        equation = sr.ReadLine();
                        answer = sr.ReadLine();
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

        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateModLine(midpoint, grad, equation);
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
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"Write This Loci {equation} in Cartesian Form (y=mx+c)";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModLine",
                operand1.GetComplex(),
                operand2.GetComplex(),
                gradient.GetString(false),
                equation,
                answer
            };
        }
    }
}
