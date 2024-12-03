using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.Polynomial_Roots
{
    public class Cubic1rootgiven : IQuestion
    {
        private Complex root, conj;
        private int root2, coef;
        private string cubic;
        private bool hide1, hide2;
        public Cubic1rootgiven(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public Cubic1rootgiven(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
            }
            Calculate();
        }

        public void GenQ(Random rnd)
        {
            root = new Complex(false);
            conj = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            root2 = rnd.Next(2, 5);
            coef = rnd.Next(1, 4);
            if (rnd.Next(3) == 0) hide1 = true;
            else hide1 = false;
            if (rnd.Next(3) == 0) hide2 = true;
            else hide2 = false;
        }

        public void Calculate()
        {
            string b, c, d;
            int coef1, coef2, coef3;
            coef1 = -(int)(root.GetRealValue() + conj.GetRealValue() + root2);
            coef2 = (int)(root * conj).GetRealValue() + (int)(root * root2).GetRealValue() + (int)(conj * root2).GetRealValue();
            coef3 = -(int)(root * conj).GetRealValue() * root2;
            if (!hide1) b = (coef1 * coef).ToString();
            else b = "a";
            if (!hide2) c = (coef2 * coef).ToString();
            else if (hide2 && hide1) c = "b";
            else c = "a";
            d = (coef3 * coef).ToString();
            if (coef != 1) cubic += coef;
            cubic += "z³";
            if (b != "0")
            {
                if (!b.Contains('-')) cubic += "+";
                if (b != "a")
                {
                    if (Math.Abs(int.Parse(b)) != 1) cubic += b;
                }
                else cubic += "a";
                cubic += "z²";
            }
            if (c != "0")
            {
                if (!c.Contains('-')) cubic += "+";
                if (Char.IsNumber(c[0]))
                {
                    if (Math.Abs(int.Parse(c)) != 1) cubic += c;
                }
                else cubic += c;
                cubic += "z";
            }
            if (d != "0")
            {
                if (!d.Contains('-')) cubic += "+";
                else cubic += d; 
                cubic += "=0";
            }
        }

        public bool CheckAnswer(string answer)
        {
            if (answer.Contains(','))
            {
                string[] answers = answer.Split(',');
                if (answers[1] != answers[0])
                {
                    Complex a1;
                    Number a2;
                    if (answers[0].Contains("i"))
                    {
                        a1 = new Complex(answers[0]);
                        a2 = new Number(int.Parse(answers[1]));
                    }
                    else
                    {
                        a1 = new Complex(answers[1]);
                        a2 = new Number(int.Parse(answers[0]));
                    }
                    if (a2.GetValue() == root2)
                    {
                        if (a1 == conj)
                        {
                            return true;
                        }
                    }
                }
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
                    if (line == "Quadratic" && !found)
                    {
                        root = new Complex(sr.ReadLine());
                        conj = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
                        root2 = int.Parse(sr.ReadLine());
                        coef = int.Parse(sr.ReadLine());
                        if (sr.ReadLine() == "1") hide1 = true;
                        else hide1 = false;
                        if(sr.ReadLine() == "1") hide2 = true;
                        else hide2 = false;
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
                return $"Correct, the answer was {conj.GetComplex()},{root2}";
            }
            else
            {
                return $"Incorrect! The answer is {conj.GetComplex()},{root2}";
            }
        }

        public string PrintQuestion()
        {
            return $"{root.GetComplex()} is a root of the equation {cubic}. calculate the other 2 roots (write them out with a comma between them, for example: a+bi,c)";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Cubic1Root",
                root.GetComplex(),
                root2.ToString(),
                coef.ToString(),
                hide1? "1" : "0",
                hide2? "1" : "0",              
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
