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
        private Complex operand1, operand2;
        private string answer, equation;

        public ModLine()
        {
            operand1 = new Complex(false);
            do
            {
                operand2 = new Complex(false);
            }
            while (operand1.GetComplex() == operand2.GetComplex());
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


            (double, double) midpoint = ((operand1.GetRealValue() + operand2.GetRealValue()) / 2, (operand1.GetImaginaryValue() + operand2.GetImaginaryValue()) / 2);
            if (operand1.GetImaginaryValue() - operand2.GetImaginaryValue() == 0)
            {
                answer = "x=" + midpoint.Item1;
            }
            else if (operand1.GetRealValue() - operand2.GetRealValue() == 0)
            {
                answer = "y=" + midpoint.Item2;
            }
            else if (operand1.GetImaginaryValue() - operand2.GetImaginaryValue() == 0 && operand1.GetRealValue() - operand2.GetRealValue() == 0)
            {
                answer = "y=x";
            }
            else
            {
                Fraction gradient = new Fraction(-((int)operand2.GetRealValue() - (int)operand1.GetRealValue()),
                    (int)operand2.GetImaginaryValue() - (int)operand1.GetImaginaryValue());
                double yintTop = (gradient.GetTop() * midpoint.Item1) - midpoint.Item2;
                double yintBottom = gradient.GetBottom();
                while (yintTop.ToString().Contains('.'))
                {
                    yintTop *= 10;
                    yintBottom *= 10;
                }
                Fraction yintf = new Fraction((int)yintTop, (int)yintBottom);
                answer += "y=";
                if (gradient.GetString()[0] == '-')
                {
                    answer += gradient.GetString() + "x";
                }
                else if (gradient.GetString() == "1") answer += "x";
                else if (gradient.GetString() == "-1") answer += "-x";
                else answer += gradient.GetString() + "x";
                if (yintf.GetString()[0] == '-')
                {
                    answer += yintf.GetString();
                }
                else answer += "+" + yintf.GetString();
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
            diagram.CreateModLine(operand1, operand2);
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
                operand2.GetComplex()
            };
        }
    }
}
