using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Questions.Loci;
using NEA.Number_Classes;
using System.IO;

namespace NEA.Questions.Complex_Basics
{
    public class Conjugate : IQuestion
    {
        private Complex c, answer;

        public Conjugate()
        {
            c = new Complex(false);
            Calculate();
        }

        public Conjugate(string filename)
        {
            if (!GetQuestion(filename))
            {
                c = new Complex(false);
            }
            Calculate();
        }

        public void Calculate()
        {
            answer = new Complex(c.GetRealValue(), -c.GetImaginaryValue());
        }

        public bool CheckAnswer(string answer)
        {
            if (new Complex(answer) == this.answer) return true;
            return false;
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
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

                    if (line == "conj" && !found)
                    {
                        c = new Complex(sr.ReadLine());
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
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"The complex number z is defined as {c.GetComplex()}. Find z* in the from a+bi";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>()
            {
                "conj",
                c.GetComplex()
            };
        }
    }
}
