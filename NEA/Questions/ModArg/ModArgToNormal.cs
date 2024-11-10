using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ModArgToNormal : IQuestion
    {
        private Complex answer;

        public ModArgToNormal()
        {
            answer = new Complex(true);
        }

        public ModArgToNormal(string filename)
        {
            if (!GetQuestion(filename)) answer = new Complex(true);
        }

        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer.GetComplex())
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
                    if (line == "ModArgToNormal" && !found)
                    {
                        answer = new Complex(sr.ReadLine());
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
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"The complex number z has a modulus of {answer.GetModulus().GetValue()} and an argument of {Math.Round(answer.GetArgument(), 3)}\n" +
                $"calculate z in the form a+bi, where a and b are intergers (rounded to 0.d.p)";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModArgToNormal",
                answer.GetComplex()
            };
        }
    }
}
