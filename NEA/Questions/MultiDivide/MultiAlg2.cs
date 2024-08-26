using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class MultiAlg2 : IQuestion
    {
        private Complex Z, Zconjagute, operand;

        private int constant;

        public MultiAlg2()
        {
            Z = new Complex(false);
            Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
            operand = new Complex(false);
            Calculate();
        }

        public MultiAlg2(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                Z = new Complex(false);
                Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
                operand = new Complex(false);
            }
        }

        public void Calculate()
        {
            int realpart = (int)(operand.GetRealValue() * Zconjagute.GetRealValue() - operand.GetImaginaryValue() * Zconjagute.GetImaginaryValue());
            int imagpart = (int)(operand.GetRealValue() * Zconjagute.GetImaginaryValue() + operand.GetImaginaryValue() * Zconjagute.GetRealValue());
            constant = (int)(imagpart - Z.GetImaginaryValue());
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.Z.GetComplex())
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
                        Z = new Complex(realin, imagin);
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
                        operand = new Complex(realin, imagin);
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
            Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
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
                return $"Correct!\nThe Answer is {Z.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {Z.GetComplex()}";
        }

        public string PrintQuestion()
        {
            string bracket1 = "(Z" + (constant < 0 ? "" : "+") + constant+"i";
            return $"The complex number z satisfies the equation {bracket1}) = ({operand.GetComplex()})Z*\nDetermine z, giving your answer in the form a+bi, where a and b are real";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("MultiAlg2");
                sw.WriteLine(Z.GetComplex());
                sw.WriteLine(operand.GetComplex());
            }
        }
    }
}
