using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ModulusQuestion : IQuestion
    {
        private Complex operand;

        private Number answer;

        public ModulusQuestion()
        {
            operand = new Complex(true);
            Calculate();
        }

        public ModulusQuestion(string filename)
        {
            if (!GetQuestion(filename)) 
            { 
                operand = new Complex(true);
            }
            Calculate();
        }

        public void Calculate()
        {
            answer = operand.GetModulus();
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
                    if (line == "Modulus" && !found)
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
            return $"The Complex number z is defined as {operand.GetComplex()}. Calculate |z|"; 
        }


        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Modulus",
                operand.GetComplex()
            };
        }

        public void CloseDiagram()
        {
            throw new NotImplementedException();
        }
    }
}
