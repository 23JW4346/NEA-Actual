﻿using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.ModArg
{
    public class ModulusArgumentForm : IQuestion
    {
        private Complex operand;

        private string answer;

        public ModulusArgumentForm()
        {
            operand = new Complex(true);
            Calculate();
        }

        public ModulusArgumentForm(string filename)
        {
            if (!GetQuestion(filename))
            {
                operand = new Complex(true);
                Calculate();
            }
        }

        public void Calculate()
        {
            string Modulus = operand.GetModulus().GetString(false);
            string arg = Math.Round(operand.GetArgument(), 3).ToString();
            answer = $"{Modulus}(cos({arg})+isin({arg}))";
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer)
            {
                return true;
            }
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
                    if (line == "ModArg" && !found)
                    {
                        operand = new Complex(sr.ReadLine());
                        answer = sr.ReadLine();
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
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"The complex number z is denoted as {operand.GetComplex()}. Write z in modulus-argument form (arguments in 3.d.p)\nWrite in form r(cos(θ)+isin(θ)), where θ is the argument, and r is the modulus";
        }


        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModArg",
                operand.GetComplex(),
                answer
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
