using NEA.Number_Classes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Questions.MultiDivide
{
    public class Multiply2Complex : IQuestion
    {
        private Complex operand1, operand2, answer;

        public Multiply2Complex()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
            Calculate();

        }

        public Multiply2Complex(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                operand1 = new Complex(false);
                operand2 = new Complex(false);
                Calculate();
            }
        }

        public void Calculate()
        {
            double realvalue = operand1.GetRealValue() * operand2.GetRealValue() - operand1.GetImaginaryValue() * operand2.GetImaginaryValue();
            double imagvalue = operand1.GetRealValue() * operand2.GetImaginaryValue() + operand1.GetImaginaryValue() * operand2.GetRealValue();
            answer = new Complex(realvalue, imagvalue);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer.GetComplex())
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

                    if (line == "Multiply2Complex" && !found)
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

        public string PrintAnswer(bool correct)
        {
            if(correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        } 

        public string PrintQuestion()
        {
            return $"Calculate ({operand1.GetComplex()})*({operand2.GetComplex()})";
        }

        public void SaveQuestion(string filename)
        {
            using(StreamWriter sw = new StreamWriter(filename, append: true)) 
            {
                sw.WriteLine();
                sw.WriteLine("Multiply2Complex");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(operand2.GetComplex());
            }
        }
    }
}
