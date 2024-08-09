using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions
{
    public class Divide2Complex : IQuestion
    {
        private Complex operand1, operand2, answer;

        public Divide2Complex()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
            Calculate();
        }
        public Divide2Complex(string filename)
        {
            if (GetQuestion(filename)) Calculate();
        }

        public void Calculate()
        {
            Fraction real = new Fraction();
            Fraction imaginary = new Fraction();
            Complex conjugate = new Complex(operand2.GetRealValue(), -operand2.GetImaginaryValue());
            double realtop = operand1.GetRealValue() * conjugate.GetRealValue() - operand1.GetImaginaryValue() * conjugate.GetImaginaryValue();
            double imagtop = operand1.GetImaginaryValue() * conjugate.GetRealValue() + operand1.GetRealValue() * conjugate.GetImaginaryValue();
            double bottom = operand2.GetRealValue() * conjugate.GetRealValue() - operand2.GetImaginaryValue() * conjugate.GetImaginaryValue();
            if (!realtop.ToString().Contains('.') && !imagtop.ToString().Contains('.') && !bottom.ToString().Contains('.'))
            {
                real = new Fraction((int)realtop, (int)bottom);
                imaginary = new Fraction((int)imagtop, (int)bottom);
            }
            else
            {
                bool loop = true;
                do
                {
                    realtop *= bottom;
                    imagtop *= bottom;
                    bottom *= bottom;
                    if (!realtop.ToString().Contains('.') && !imagtop.ToString().Contains('.') && !bottom.ToString().Contains('.')) loop = false;
                } while (loop);  

               
            }
            answer = new Complex(real, imaginary);
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

                    if (line == "Divide2Complex" && !found)
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
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"Find the Value of ({operand1.GetComplex()})/({operand2.GetComplex()}) in the form a+bi, where a and b can be written in fractions if needed (x/y)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, append: true))
            {
                sw.WriteLine();
                sw.WriteLine("Divide2Complex");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(operand2.GetComplex());
            }
        }
    }
}
