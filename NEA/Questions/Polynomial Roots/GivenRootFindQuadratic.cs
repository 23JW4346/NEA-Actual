using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.Polynomial_Roots
{
    public class GivenRootFindQuadratic : IQuestion
    {
        private Complex root;
        private string answer;

        public GivenRootFindQuadratic()
        {
            root = new Complex(false);
            Calculate();
        }

        public GivenRootFindQuadratic(string filename)
        {
            if (!GetQuestion(filename))
            {
                root = new Complex(false);
                Calculate();
            }
        }

        public void Calculate()
        {
            Complex conj = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            int coef1 , coef2 ;
            coef1 = -(int)(root.GetRealValue() + conj.GetRealValue());
            coef2 = (int)(root*conj).GetRealValue();
            answer = $"z²";
            if (coef1 > 0) answer += "+";
            answer += $"{coef1}z";
            if(coef2  >  0) answer += "+";
            answer += coef2 + "=0";
        }

        public bool CheckAnswer(string answer)
        {
            if(answer == this.answer) return true;
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
                    if (line == "GetQuad" && !found)
                    {
                        root = new Complex(sr.ReadLine());
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
                return $"Correct, the answer is {answer}";
            }
            else
            {
                return $"Incorrect, the answer was {answer}";
            }
        }

        public string PrintQuestion()
        {
            return $"A quadratic equation has a root {root.GetComplex()}.\nFind the quadratic equation in the form az²+bz+c=0, where a, b and c are integers.";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "GetQuad",
                root.GetComplex(),
                answer
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
