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
                Fraction gradient = new Fraction(-((int)operand2.GetRealValue() - (int)operand1.GetRealValue()), (int)operand2.GetImaginaryValue() - (int)operand1.GetImaginaryValue());
                double yint = (gradient.GetTop() * midpoint.Item1) - midpoint.Item2;
                double yintbottom = gradient.GetBottom();
                while (yint.ToString().Contains('.'))
                {
                    yint *= 10;
                    yintbottom *= 10;
                }
                Fraction yintf = new Fraction((int)yint, (int)yintbottom);
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

                    if (line == "ModLine" && !found)
                    {
                        string number = null;
                        bool firstneg = true;
                        double realin = 0, imagin = 0;
                        string operand1 = sr.ReadLine();
                        for (int i = 0; i < operand1.Length; i++)
                        {
                            if (Char.IsNumber(operand1[i]))
                            {
                                number += operand1[i];
                            }
                            if (operand1[i] == '-')
                            {
                                if (firstneg && number.Length < 1)
                                {
                                    number += operand1[i];
                                    firstneg = false;
                                }
                                else
                                {
                                    realin = double.Parse(number);
                                    number = "-";
                                }
                            }
                            else if (operand1[i] == 'i')
                            {
                                imagin = double.Parse(number);
                                break;
                            }
                            else if (operand1[i] == '+')
                            {
                                realin = double.Parse(number);
                                number = null;
                            }
                        }
                        this.operand1 = new Complex(realin, imagin);
                        string operand2 = sr.ReadLine();
                        for (int i = 0; i < operand2.Length; i++)
                        {
                            if (Char.IsNumber(operand2[i]))
                            {
                                number += operand2[i];
                            }
                            if (operand2[i] == '-')
                            {
                                if (firstneg && number.Length < 1)
                                {
                                    number += operand2[i];
                                    firstneg = false;
                                }
                                else
                                {
                                    realin = double.Parse(number);
                                    number = "-";
                                }
                            }
                            else if (operand2[i] == 'i')
                            {
                                imagin = double.Parse(number);
                                break;
                            }
                            else if (operand2[i] == '+')
                            {

                                realin = double.Parse(number);
                                number = null;
                            }
                        }
                        this.operand2 = new Complex(realin, imagin);
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
            Application.Run(diagram);
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

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("ModLine");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(operand2.GetComplex());
            }
        }
    }
}
