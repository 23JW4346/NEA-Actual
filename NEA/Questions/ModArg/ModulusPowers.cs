﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ModulusPowers : IQuestion
    {
        private Complex operand;
        private int exponent;

        private int answer;

        public ModulusPowers(int inexp)
        {
            operand = new Complex(true);
            exponent = inexp;
            Calculate();
        }

        public void Calculate()
        {
            answer = (int)Math.Pow(operand.GetModulus().GetValue(),exponent);
        }

        public bool CheckAnswer(string answer)
        {
            if(answer == this.answer.ToString())
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
                    if (line == "ModPow" && !found)
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
                        operand = new Complex(realin, imagin);
                        exponent = int.Parse(sr.ReadLine());
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
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"The complex number Z is denoted as {operand.GetComplex()}.\nWithout calculating Z^5, Find|Z^5|";
        }

        public void SaveQuestion(string filename)
        {
            using(StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine();
                sw.WriteLine("ModPow");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(exponent);
            }
        }
    }
}