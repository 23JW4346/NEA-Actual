﻿using NEA.Number_Classes;
using NEA.Questions.Loci;
using System.Collections.Generic;
using System.IO;

namespace NEA.Questions.MultiDivide
{
    public class Multiply2Complex : IQuestion
    {
        private Complex operand1, operand2, answer;

        public Multiply2Complex()
        {
            GenQ();
            Calculate();

        }

        public Multiply2Complex(string filename)
        {
            if (!GetQuestion(filename)) GenQ();
            Calculate();
        }

        public void GenQ()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
        }

        public void Calculate()
        {
            answer = operand1 * operand2;
        }

        public bool CheckAnswer(string answer) => answer == this.answer.GetComplex();


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

        public void LoadDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            else
            {
                return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
            }
        }

        public string PrintQuestion()
        {
            return $"Calculate ({operand1.GetComplex()})*({operand2.GetComplex()}). Put your answer in the form a+bi, where a and b are intergers.";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Multiply2Complex",
                operand1.GetComplex(),
                operand2.GetComplex()
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
