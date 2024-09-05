using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ArgumentPowers : IQuestion
    {
        private Complex operand;
        private int exponent;
        private double answer;

        public ArgumentPowers(int inexp)
        {
            operand = new Complex(false);
            exponent = inexp;
            Calculate();
        }

        public ArgumentPowers(int inexp, string filename)
        {
            if (!GetQuestion(filename))
            {
                operand = new Complex(false);
                exponent = inexp;
            }
            Calculate();
        }

        public void Calculate()
        {
            double arg = operand.GetArgument();
            answer =  Math.Round(arg * exponent,2);
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
                    if (line == "ArgPow" && !found)
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
            return $"The complex number Z is denoted as {operand.GetComplex()}.\nWithout calculating Z^5, Find arg(Z^5)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("ArgPow");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(exponent);
            }
        }
    }
}
