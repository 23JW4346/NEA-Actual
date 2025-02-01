using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Questions.Loci;
using NEA.Number_Classes;
using System.Security.Policy;
using System.IO;

namespace NEA.Questions.Complex_Basics
{
    public class REorIMQuestion : IQuestion
    {
        private Complex c;
        private int answer;
        bool rE;

        public REorIMQuestion(Random rnd)
        {
            c = new Complex(false);
            rE = rnd.Next(2) == 1;
            Calculate();
        }

        public REorIMQuestion(Random rnd, string filename) 
        {
            if (!GetQuestion(filename))
            {
                c = new Complex(false);
                rE = rnd.Next(2) == 1;
            }
            Calculate();
        }

        public void Calculate()
        {
            if (rE) answer = (int)c.GetRealValue();
            else answer = (int)c.GetImaginaryValue(); 
        }

        public bool CheckAnswer(string answer)
        {
            if(answer == this.answer.ToString()) return true;
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

                    if (line == "ReIm" && !found)
                    {
                        c = new Complex(sr.ReadLine());
                        if (sr.ReadLine() == "1") rE = true;
                        else rE= false;
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
            string line = "im(z)";
            if (rE) line = "re(z)";
            return $"The complex number z is defined as {c.GetComplex()}. find {line}";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>()
            {
                "ReIm",
                c.GetComplex(),
                rE ? "1" : "0"
            };
        }
    }
}
