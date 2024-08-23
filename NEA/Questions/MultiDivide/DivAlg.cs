using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class DivAlg : IQuestion
    {
        private Complex operand1, operand2, answer;
        private int a;

        public DivAlg()
        {
            operand1 = new Complex(false);
            answer = new Complex(false);
            Calculate();
        }

        public DivAlg(string filename)
        {
            if(GetQuestion(filename)) Calculate();
            else
            {
                operand1 = new Complex(false);
                answer = new Complex(false);
                Calculate();
            }

        }


        public void Calculate()
        {
            double realvalue = operand1.GetRealValue() * answer.GetRealValue() - operand1.GetImaginaryValue() * answer.GetImaginaryValue();
            double imagvalue = operand1.GetRealValue() * answer.GetImaginaryValue() + operand1.GetImaginaryValue() * answer.GetRealValue();
            a = 1;
            int loop = (int)realvalue;
            if (imagvalue < realvalue) loop = (int)imagvalue;
            for (int i = 1; i <= loop; i++)
            {
                if ((int)realvalue % i == 0 && (int)imagvalue % i == 0)
                {
                    a = i;
                }
            }
            Fraction real = new Fraction((int)realvalue, a);
            Fraction imag = new Fraction((int)imagvalue, a);
            operand2 = new Complex(real, imag);
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

                    if (line == "DivAlg" && !found)
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
                        answer = new Complex(realin, imagin);
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
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            if(a != 1) return $"The complex number z satisfies the equation z({operand1.GetComplex()}) = {a}({operand2.GetComplex()})\nDetermine z, giving your answer in the form a+bi, where a and b are real";
            return $"The complex number z satisfies the equation z({operand1.GetComplex()}) = ({operand2.GetComplex()})\nDetermine z, giving your answer in the form a+bi, where a and b are real";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("DivAlg");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(answer.GetComplex());
            }
        }
    }
}
