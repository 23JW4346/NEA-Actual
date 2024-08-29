using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.Polynomial_Roots
{
    public class Cubic1rootgiven : IQuestion
    {
        private Complex root, conjugate;
        private int root2, coef;
        private string cubic;

        public Cubic1rootgiven(Random rnd)
        {
            root = new Complex(false);
            conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            root2 = rnd.Next(2,5);
            coef = rnd.Next(1,4);
            Calculate();
        }

        public Cubic1rootgiven(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                root = new Complex(false);
                conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
                root2 = rnd.Next(2, 5);
                coef = rnd.Next(1, 4);
            }
            Calculate();
        }

        public void Calculate()
        {
            int b = -coef * (int)(root.GetRealValue() + conjugate.GetRealValue() + root2);
            int c = coef * (int)(root.GetRealValue() * conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue() + root.GetRealValue() * root2 + conjugate.GetRealValue() * root2);
            int d = -coef * (int)((root.GetRealValue() * conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue()) * root2);
            if(coef != 1)
            {
                cubic += coef.ToString();
            }
            cubic += "z^3";
            if(b > 0)
            {
                cubic += "+";
            }
            cubic += $"{b}z^2";
            if (c > 0)
            {
                cubic += "+";
            }
            cubic += $"{c}z";
            if (d > 0)
            {
                cubic += "+";
            }
            cubic += $"{d}=0";
        }

        public bool CheckAnswer(string answer)
        {
            string[] answers;
            try
            {
                answers = answer.Split(',');
            }
            catch 
            {
                SaveQuestion("Questions.txt");
                return false;
            }
            if (answers[0] == conjugate.GetComplex() || answers[1] == root.GetComplex() && answers[1] != answers[0])
            {
                if (answers[0] == root2.ToString() || answers[1] == root2.ToString())
                {
                    return true;
                }
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
                        root2 = int.Parse(sr.ReadLine());
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
                return $"incorrect, the answer was {conjugate.GetComplex()},{root2}";
            }
            else
            {
                return $"correct! The answer is {conjugate.GetComplex()},{root2}";
            }
        }

        public string PrintQuestion()
        {
            return $"{root} is a root of the equation {cubic}. find the other 2 roots (write them out with a comma seperating them)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("Cubic1root");
                sw.WriteLine(root.GetComplex());
                sw.WriteLine(root2);
                sw.WriteLine(coef);
            }
        }
    }
}
