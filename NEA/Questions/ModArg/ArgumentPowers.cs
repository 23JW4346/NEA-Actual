﻿using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.ModArg
{
    public class ArgumentPowers : IQuestion
    {
        private Complex operand;
        private int exponent;
        private double answer;

        public ArgumentPowers(Random rnd)
        {
            GenQ(rnd.Next(2, 5));
            Calculate();
        }

        public ArgumentPowers(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd.Next(2, 5));
                Calculate();
            }
        }

        public void GenQ(int inexp)
        {
            operand = new Complex(false);
            exponent = inexp;
        }

        public void Calculate()
        {
            double arg = operand.GetArgument();
            answer = exponent * arg;
            while(Math.Abs(answer) > Math.PI)
            {
                if (answer > Math.PI) answer -= 2*Math.PI;
                if (answer < -Math.PI) answer += 2*Math.PI;
            }
            answer = Math.Round(answer, 3);
        }

        public bool CheckAnswer(string answer)
        {
            if(answer == this.answer.ToString())
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
                    if (line == "ArgPow" && !found)
                    {
                        operand = new Complex(sr.ReadLine());
                        exponent = int.Parse(sr.ReadLine());
                        answer = double.Parse(sr.ReadLine());
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
            string z = "z";
            switch (exponent)
            {
                case 2:
                    z += "²";
                    break;
                case 3:
                    z += "³";
                    break;
                default:
                    z += "⁴";
                    break;
            }
            return $"The complex number z is denoted as {operand.GetComplex()}.\nWithout calculating {z}, Find arg({z}) to 3.d.p, within the range -π<arg({z})<π";
        }


        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ArgPow",
                operand.GetComplex(),
                exponent.ToString(),
                answer.ToString()
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
