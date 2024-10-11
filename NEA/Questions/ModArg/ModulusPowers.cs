using System;
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

        public ModulusPowers(int inexp, string filename)
        {
            if (!GetQuestion(filename))
            {
                operand = new Complex(true);
                exponent = inexp;
            }
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

        public string Hint()
        {
            throw new NotImplementedException();
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
            return $"The complex number Z is denoted as {operand.GetComplex()}.\nWithout calculating Z^5, Find|Z^5|";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModPow",
                operand.GetComplex(),
                exponent.ToString()
            };
        }
    }
}
