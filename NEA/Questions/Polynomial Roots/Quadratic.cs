using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.IO;
using System.Reflection;

namespace NEA.Questions.Polynomial_Roots
{
    public class Quadratic : IQuestion 
    {
        private Complex root, conjugate;
        private int coef;
        private string quadratic;
        
        public Quadratic(Random rnd)
        {
            root = new Complex(false);
            conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            coef = rnd.Next(1,4);
            Calculate();
        }

        public Quadratic(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                root = new Complex(false);
                conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
                coef = rnd.Next(1,4);
            }
            Calculate();
        }


        public void Calculate()
        {
            int b = -coef * (int)(root.GetRealValue() + conjugate.GetRealValue());
            int c = coef * (int)(root.GetRealValue() *  conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue());
            if (coef != 1) quadratic += $"{coef}z^2";
            else quadratic += "z^2";
            if (b < 0) quadratic += $"{b}z";
            else quadratic += $"+{b}z";
            if (c < 0) quadratic += c;
            else quadratic += $"+{c}";
            quadratic += "=0";
        }

        public bool CheckAnswer(string answer)
        {
            string[] answers = answer.Split(',');
            foreach(string s in answers)
            {
                Console.WriteLine(s);
            }
            if (answers[0] == root.GetComplex() || answers[1] == root.GetComplex() && answers[1] != answers[0])
            {
                if (answers[0] == conjugate.GetComplex() || answers[1] == conjugate.GetComplex())
                {
                    return true;
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
                    if (line == "Quadratic" && !found)
                    {
                        string number = "";
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
                        root = new Complex(realin, imagin);
                        conjugate = new Complex(realin, -imagin);
                        coef = int.Parse(sr.ReadLine());
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
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"incorrect, the answer was {root.GetComplex()},{conjugate.GetComplex()}";
            }
            else
            {
                return $"correct! The answer is {root.GetComplex()},{conjugate.GetComplex()}";
            }

        }

        public string PrintQuestion()
        {
            return $"find the roots of the quadradic with the equation {quadratic}. write the roots out next to each other, with a comma seperating them";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true)) 
            {
                sw.WriteLine();
                sw.WriteLine("Quadratic");
                sw.WriteLine(root.GetComplex());
                sw.WriteLine(coef);
            }
        }
    }
}
