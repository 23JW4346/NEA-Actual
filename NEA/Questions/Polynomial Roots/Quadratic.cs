using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.IO;
using System.Reflection;
using System.Data.SqlTypes;

namespace NEA.Questions.Polynomial_Roots
{
    public class Quadratic : IQuestion 
    {
        private Complex root, conjugate;
        private int coef;
        private string quadratic;
        
        public Quadratic(int incoef)
        {
            root = new Complex(false);
            conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            coef = incoef;
            Calculate();
        }

        public Quadratic(int incoef, string filename)
        {
            if (!GetQuestion(filename))
            {
                root = new Complex(false);
                conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
                coef = incoef;
            }
            Calculate();
        }


        public void Calculate()
        {
            int b = -coef * (int)(root.GetRealValue() + conjugate.GetRealValue());
            int c = coef * (int)(root.GetRealValue() *  conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue());
            if (coef != 1) quadratic += $"{coef}z^2";
            else quadratic += "z^2";
            if (b < 0) quadratic += $"{b}z";
            else quadratic += $"+{b}z";
            if (c < 0) quadratic += c;
            else quadratic += $"+{c}";
            quadratic += "=0";
        }

        public bool CheckAnswer(string answer)
        {
            string[] answers;
            try
            {
                answers = answer.Split(',');
            } catch
            {
                return false;
            }
            if (answers[0] == root.GetComplex() || answers[1] == root.GetComplex() && answers[1] != answers[0])
            {
                if (answers[0] == conjugate.GetComplex() || answers[1] == conjugate.GetComplex())
                {
                    return true;
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
                        coef = int.Parse(sr.ReadLine());
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
                return $"correct! The answer is {root.GetComplex()},{conjugate.GetComplex()}";
            }
            return $"incorrect, the answer was {root.GetComplex()},{conjugate.GetComplex()}";

        }

        public string PrintQuestion()
        {
            return $"find the roots of the quadradic with the equation {quadratic}. write the roots out next to each other, with a comma seperating them";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true)) 
            {
                sw.WriteLine();
                sw.WriteLine("Quadratic");
                sw.WriteLine(root.GetComplex());
                sw.WriteLine(coef);
            }
        }
    }
}
