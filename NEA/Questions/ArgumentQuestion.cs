﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions
{
    public class ArgumentQuestion : IQuestion
    {
        private Complex operand;

        private Number answer;

        public ArgumentQuestion()
        {
            operand = new Complex(false);
            Calculate();
        }
        public ArgumentQuestion(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                operand = new Complex(false);
                Calculate();
            }
        }

        public void Calculate()
        {
            answer = new Number(operand.GetArgument());
        }

        public bool CheckAnswer(string answer)
        {
            bool correct = false;
            if (answer == this.answer.GetString())
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
            bool found = false;
            string tempfile = Path.GetTempFileName();
            using (StreamReader sr = new StreamReader(filename))
            using (StreamWriter sw = new StreamWriter(tempfile))
            {
                while (!sr.EndOfStream)
                {
                    string line;
                    line = sr.ReadLine();
                    if (line == "Argument" && !found)
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
                return $"Correct!\nThe Answer is {answer.GetString()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetString()}";
        }

        public string PrintQuestion()
        {
            return $"The Complex number z is defined as {operand.GetComplex()}. Calculate Arg(z) to 3.d.p";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine();
                sw.WriteLine("Argument");
                sw.WriteLine(operand.GetComplex());
            }
        }
    }
}