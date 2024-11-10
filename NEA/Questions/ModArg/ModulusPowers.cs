using System;
using System.Collections.Generic;
using System.IO;
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
            GenQ(inexp);
            Calculate();
        }

        public ModulusPowers(int inexp, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(inexp);
                Calculate();
            }
        }

        public void GenQ(int inexp)
        {
            operand = new Complex(true);
            exponent = inexp;
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
                        operand = new Complex(sr.ReadLine());
                        exponent = int.Parse(sr.ReadLine());
                        answer = int.Parse(sr.ReadLine());
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
            return $"The complex number z is denoted as {operand.GetComplex()}.\nWithout calculating {z}, Find|{z}|";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModPow",
                operand.GetComplex(),
                exponent.ToString(),
                answer.ToString()
            };
        }
    }
}
