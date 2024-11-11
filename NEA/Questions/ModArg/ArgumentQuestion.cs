using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ArgumentQuestion : IQuestion
    {
        private Complex operand;

        private Number answer;

        public ArgumentQuestion()
        {
            GenQ();
            Calculate();
        }
        public ArgumentQuestion(string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ();
            }
            Calculate();
        }

        public void GenQ()
        {
            operand = new Complex(false);
        }

        public void Calculate()
        {
            answer = new Number(Math.Round(operand.GetArgument(), 3));
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer.GetString(false))
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
                    if (line == "Argument" && !found)
                    {
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
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetString(false)}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetString(false)}";
        }

        public string PrintQuestion()
        {
            return $"The Complex number z is defined as {operand.GetComplex()}. Calculate Arg(z) to 3.d.p";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Argument",
                operand.GetComplex(),
            };
        }

        public void CloseDiagram()
        {
            throw new NotImplementedException();
        }
    }
}
