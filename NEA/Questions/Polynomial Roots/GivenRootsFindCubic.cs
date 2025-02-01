using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.Polynomial_Roots
{
    public class GivenRootsFindCubic : IQuestion
    {
        private Complex compRoot;
        private int intRoot;
        private string answer;

        public GivenRootsFindCubic(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public GivenRootsFindCubic(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
                Calculate();
            }
        }


        public void GenQ(Random rnd)
        {
            compRoot = new Complex(false);
            if (rnd.Next(2) == 0) intRoot = rnd.Next(-5, 0);
            else intRoot = rnd.Next(1, 6);
        }

        public void Calculate()
        {
            Complex conj = new Complex(compRoot.GetRealValue(), -compRoot.GetImaginaryValue());
            int coef1, coef2, coef3;
            coef1 = -(int)(compRoot.GetRealValue() + conj.GetRealValue() + intRoot);
            coef2 = (int)(compRoot*conj).GetRealValue() + (int)(compRoot*intRoot).GetRealValue() + (int)(conj*intRoot).GetRealValue(); 
            coef3 = -(int)(compRoot * conj*intRoot).GetRealValue();
            answer = "z³";
            if (coef1 > 0) answer += "+";
            else answer += "-";
            if (Math.Abs(coef1) != 1) answer += Math.Abs(coef1);
            answer += "z²";
            if (coef2 > 0) answer += "+";
            else answer += "-";
            if(Math.Abs(coef2) != 1) answer += Math.Abs(coef2);
            answer += "z";
            if(coef3 > 0) answer += "+";
            else answer += "-";
            if(Math.Abs(coef3) != 1) answer += Math.Abs(coef3);
            answer += "=0";
        }

        public bool CheckAnswer(string answer)
        {
            if(answer == this.answer)
            {
                return true;
            }
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
                    if (line == "rootC" && !found)
                    {
                        compRoot = new Complex(sr.ReadLine());
                        intRoot = int.Parse(sr.ReadLine());
                        answer = sr.ReadLine();
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
                return $"Correct, the answer was {answer}";
            }
            else
            {
                return $"Incorrect! The answer is {answer}";
            }
        }

        public string PrintQuestion()
        {
            return $"Find a cubic equation with real coefficients, two of whose roots are {compRoot.GetComplex()} and {intRoot}. \nWrite you answer in the form z³+az²+bz+c=0";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "rootC",
                compRoot.GetComplex(),
                intRoot.ToString(),
                answer
            };
        }
    }
}
