﻿using NEA.Number_Classes;
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
                        operand1 = new Complex(sr.ReadLine());
                        operand2 = new Complex(sr.ReadLine());
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
            using(StreamWriter sw = new StreamWriter(filename, true)) 
            {
                sw.WriteLine();
                sw.WriteLine("Multiply2Complex");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(operand2.GetComplex());
            }
        }
    }
}
