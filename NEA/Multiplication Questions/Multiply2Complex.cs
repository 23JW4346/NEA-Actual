using NEA.Number_Classes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Multiplication_Questions
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
            if(GetQuestion(filename)) Calculate();
        }
        public void Calculate()
        {
            double realvalue = operand1.GetRealValue()*operand2.GetRealValue() - operand1.GetImaginaryValue()*operand2.GetImaginaryValue();
            double imagvalue = operand1.GetRealValue()*operand2.GetImaginaryValue() + operand1.GetImaginaryValue()*operand2.GetRealValue();
            answer = new Complex(realvalue, imagvalue);
        }

        public bool CheckAnswer(string answer)
        {
            bool correct = false;
            if(answer == this.answer.GetComplex())
            {
                correct = true;
            }
            if (correct)
            {
                return correct;
            }
            SaveQuestion("Questions.txt");
            return correct;
            
        }

        public bool GetQuestion(string filename)
        {
            using(StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    if(sr.ReadLine() == "Multiply2Complex")
                    {
                        string number = null;
                        bool first = true;
                        bool firstneg = true;
                        double realin=0, imagin=0;
                        string operand1 = sr.ReadLine();
                        for(int i = 0; i < operand1.Length; i++)
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
                               first = false;
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
                                if (firstneg && number.Length <1)
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
                                first = false;
                                number = null;
                            }
                        }
                        this.operand2 = new Complex(realin, imagin);

                        return true;
                    }
                }
                sr.Close();
                return false;
            }
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
