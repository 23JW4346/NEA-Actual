﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class ZSquare : IQuestion
    {

        private Complex operand, answer1, answer2;

        public ZSquare(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public ZSquare(string filename, Random rnd)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
                Calculate();
            }
        }

        public void GenQ(Random rnd)
        {
            answer1 = new Complex(false);
            answer2 = new Complex(-answer1.GetRealValue(), -answer2.GetRealValue());
        }


        public void Calculate()
        {
            operand = answer1;
            operand = Program.TimesComplex(answer1, operand);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer.Contains(','))
            {
                string[] answers = answer.Split(',');

                if (answers[0] == answer1.GetComplex() || answers[1] == answer1.GetComplex() && answers[1] != answers[0])
                {
                    if (answers[0] == answer2.GetComplex() || answers[1] == answer2.GetComplex())
                    {
                        return true;
                    }
                }
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
                    if (line == "ZSquared" && !found)
                    {
                        answer1 = new Complex(sr.ReadLine());
                        answer2 = new Complex(-answer1.GetRealValue(), -answer1.GetImaginaryValue());
                        operand = new Complex(sr.ReadLine());
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
            if (correct) return $"Correct, the answer was {answer1.GetComplex()},{answer2.GetComplex()}";
            return $"Incorrect, the answer is {answer1.GetComplex()},{answer2.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"Given that z²={operand.GetComplex()}, find 2 values for z in the form a+bi (place a comma between both values, with 0 spaces)";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ZSquared",
                answer1.GetComplex(),
                operand.GetComplex(),
            };
        }
    }
}
