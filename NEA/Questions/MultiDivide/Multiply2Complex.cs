using NEA.Number_Classes;
using System;
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
            if (!GetQuestion(filename))
            {
                GenQ();
            }
            Calculate();
        }

        public void GenQ()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
        }

        public void Calculate()
        {
            answer = Program.TimesComplex(operand1, operand2);
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
            else
            {
                return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
            }
        }

        public string PrintQuestion()
        {
            return $"Calculate ({operand1.GetComplex()})*({operand2.GetComplex()})";
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

        public void CloseDiagram()
        {
            throw new NotImplementedException();
        }
    }
}
