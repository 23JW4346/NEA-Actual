using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.Polynomial_Roots
{
    public class Cubic1rootgiven : IQuestion
    {
        private Complex root, conjugate;
        private int root2, coef;
        private string cubic;
        private bool hide1, hide2;
        public Cubic1rootgiven(Random rnd)
        {
            root = new Complex(false);
            conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            root2 = rnd.Next(2,5);
            coef = rnd.Next(1,4);
            if(rnd.Next(3) == 0) hide1 = true;
            else hide1 = false;
            if(rnd.Next(3) == 0) hide2 = true;
            else hide2 = false;
            Calculate();
        }

        public Cubic1rootgiven(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                root = new Complex(false);
                conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
                if (rnd.Next(3) == 0) hide1 = true;
                else hide1 = false;
                if (rnd.Next(3) == 0) hide2 = true;
                else hide2 = false;
                root2 = rnd.Next(2, 9);
                coef = rnd.Next(1, 4);
            }
            Calculate();
        }

        public void Calculate()
        {
            string b, c, d;
            if (!hide1) b = (-coef * (int)(root.GetRealValue() + conjugate.GetRealValue() + root2)).ToString();
            else b = "a";
            if (!hide2) c = (coef * (int)(root.GetRealValue() * conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue() + root.GetRealValue() * root2 + conjugate.GetRealValue() * root2)).ToString();
            else if (hide2 && hide1) c = "b";
            else c = "a";
            d = (-coef * (int)((root.GetRealValue() * conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue()) * root2)).ToString();
            if (coef != 1) cubic += coef;
            cubic += "z^3";
            if (!b.Contains('-')) cubic += "+";
            cubic += b + "z^2";
            if (!c.Contains('-')) cubic += "+";
            cubic += c + "z";
            if(!d.Contains('-')) cubic += "+";
            cubic += d;
        }

        public bool CheckAnswer(string answer)
        {
            if (answer.Contains(','))
            {
                string[] answers = answer.Split(',');

                if (answers[0] == root2.ToString() || answers[1] == root2.ToString() && answers[1] != answers[0])
                {
                    if (answers[0] == conjugate.GetComplex() || answers[1] == conjugate.GetComplex())
                    {
                        return true;
                    }
                }
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
                    if (line == "Quadratic" && !found)
                    {
                        root = new Complex(sr.ReadLine());
                        conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
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

        public void LoadDiagram()
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct, the answer was {conjugate.GetComplex()},{root2}";
            }
            else
            {
                return $"Incorrect! The answer is {conjugate.GetComplex()},{root2}";
            }
        }

        public string PrintQuestion()
        {
            return $"{root.GetComplex()} is a root of the equation {cubic}. find the other 2 roots (write them out with a comma seperating them)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("Cubic1root");
                sw.WriteLine(root.GetComplex());
                sw.WriteLine(root2);
                sw.WriteLine(coef);
                if (hide1) sw.WriteLine("1");
                else sw.WriteLine("0");
                if (hide2) sw.WriteLine("1");
                else sw.WriteLine("0");
            }
        }
    }
}
