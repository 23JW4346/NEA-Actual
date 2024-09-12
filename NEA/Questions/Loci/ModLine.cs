using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.Windows.Forms;
using System.IO;
using System.Linq.Expressions;
using System.CodeDom.Compiler;

namespace NEA.Questions.Loci
{
    public class ModLine : IQuestion
    {
        private ArgandDiagram diagram;
        private Complex midpoint, operand1, operand2;
        private Number gradient;
        private string answer, equation;

        public ModLine(Random rnd)
        {
            int rand = rnd.Next(3);
            if(rand == 1)
            {
                gradient = new Fraction(rnd.Next(3), rnd.Next(4)); 
            }
            else if (rand == 2)
            {
                gradient = new Fraction(rnd.Next(-3), rnd.Next(4));
            }
            else 
            {
                gradient = new Number(rnd.Next(-4, 6));
            }
            midpoint = new Complex(rnd.Next(6), rnd.Next(6));
            if(gradient.GetValue() == 0)
            {
                rand = rnd.Next(5);
                operand1 = new Complex(midpoint.GetRealValue(), midpoint.GetImaginaryValue() + rand);
                operand2 = new Complex(midpoint.GetRealValue(), midpoint.GetImaginaryValue() - rand);
            }
            else if (gradient.GetValue() == 5)
            {
                rand = rnd.Next(5);
                operand1 = new Complex(midpoint.GetRealValue() + rand, midpoint.GetImaginaryValue() );
                operand2 = new Complex(midpoint.GetRealValue() - rand, midpoint.GetImaginaryValue() );
            }
            else
            {
                GetPoints(rnd.Next(5));
            }
            Calculate();
        }

        public ModLine(string filename)
        {
            if (!GetQuestion(filename))
            {
                operand1 = new Complex(false);
                do
                {
                    operand2 = new Complex(false);
                }
                while (operand1.GetComplex() == operand2.GetComplex());
            }
            Calculate();
        }

        private void GetPoints(int space)
        {
            Complex temp = midpoint;
            double negRec = Math.Pow(gradient.GetValue(), -1);
            bool isleft = false;
            if(negRec < 0) isleft = true;
            for(int i = 0; i < space; i++)
            {
                if (isleft) temp = new Complex(temp.GetRealValue() - 1, temp.GetImaginaryValue() + negRec);
                else temp = new Complex(temp.GetRealValue() + 1, temp.GetImaginaryValue() + negRec);
            }
            operand1 = temp;
            temp = midpoint;
            for (int i = 0; i < space; i++)
            {
                if (isleft) temp = new Complex(temp.GetRealValue() + 1, temp.GetImaginaryValue() - negRec);
                else temp = new Complex(temp.GetRealValue() - 1, temp.GetImaginaryValue() - negRec);
            }
            operand2 = temp;
        }


        public void Calculate()
        {
            Complex temp = new Complex(-operand1.GetRealValue(), -operand1.GetImaginaryValue());
            equation += "|z";
            if (temp.GetComplex()[0] == '-')
            {
                equation += temp.GetComplex();
            }
            else equation += "+" + temp.GetComplex();
            equation += "|=|z";
            temp = new Complex(-operand2.GetRealValue(), -operand2.GetImaginaryValue());
            if (temp.GetComplex()[0] == '-')
            {
                equation += temp.GetComplex();
            }
            else equation += "+" + temp.GetComplex();
            equation += "|";
            if (gradient.GetValue() == 0)
            {
                answer = "x=" + midpoint.GetReal();
            }
            else if (gradient.GetValue() == 5)
            {
                answer = "y=" + midpoint.GetImaginary();
            }
            else if (operand1.GetImaginaryValue() - operand2.GetImaginaryValue() == 0 && operand1.GetRealValue() - operand2.GetRealValue() == 0)
            {
                answer = "y=x";
            }
            else
            {
                answer += "y=";
                if (gradient.GetString()[0] == '-')
                {
                    answer += gradient.GetString() + "x";
                }
                else if (gradient.GetString() == "1") answer += "x";
                else if (gradient.GetString() == "-1") answer += "-x";
                else answer += gradient.GetString() + "x";
                if (midpoint.GetImaginaryValue() < 0)
                {
                    answer += midpoint.GetImaginary();
                }
                else answer += "+" + midpoint.GetImaginary();
            }
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
                            string[] fract = line.Split('/');
                            gradient = new Fraction(int.Parse(fract[0]), int.Parse(fract[1]));
                        }
                        else gradient = new Number(int.Parse(line2));
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
            diagram.CreateModLine(midpoint, gradient.GetValue());
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
                gradient.GetString(),
                equation,
                answer
            };
        }
    }
}
