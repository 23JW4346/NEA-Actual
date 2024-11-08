using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
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

        private void GetPoints(int space)
        {
            double xint = midpoint.GetRealValue();
            double yint = midpoint.GetImaginaryValue();
            double negRec = -1 / grad;
            for (int i = 0; i < space; i++)
            {
                    xint--;
                    yint -= negRec;
            }
            operand1 = new Complex(xint, yint);
            xint = midpoint.GetRealValue();
            yint = midpoint.GetImaginaryValue();
            for (int i = 0; i < space; i++)
            {
                    xint++;
                    yint += negRec;
            }
            operand2 = new Complex(xint, yint);
        }

        public void Calculate()
        {
            equation = Program.CreateModLine(new Complex(-operand1.GetRealValue(), -operand1.GetImaginaryValue()), 
                                             new Complex(-operand2.GetRealValue(), -operand2.GetImaginaryValue()));
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
            diagram.CreateModLine(midpoint, grad);
            Task.Run(() => Application.Run(diagram));
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
