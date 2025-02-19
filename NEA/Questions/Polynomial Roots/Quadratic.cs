﻿using System;
using System.Collections.Generic;
using System.Linq;
using NEA.Number_Classes;
using System.IO;
using NEA.Questions.Loci;

namespace NEA.Questions.Polynomial_Roots
{
    public class Quadratic : IQuestion
    {
        private Complex root, conjugate;
        private int coef;
        private string quadratic;
        
        public Quadratic(Random rnd)
        {
            GenQ(rnd.Next(1,4));
            Calculate();
        }

        public Quadratic(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd.Next(1,4));
            }
            Calculate();
        }

        public void GenQ(int incoef)
        {
            root = new Complex(false);
            conjugate = new Complex(root.GetRealValue(), -root.GetImaginaryValue());
            coef = incoef;
        }

        public void Calculate()
        {
            int b = -coef * (int)(root.GetRealValue() + conjugate.GetRealValue());
            int c = coef * (int)(root.GetRealValue() *  conjugate.GetRealValue() - root.GetImaginaryValue() * conjugate.GetImaginaryValue());
            if (coef != 1) quadratic += $"{coef}z²";
            else quadratic += "z²";
            if (b < 0) quadratic += $"{b}z";
            else quadratic += $"+{b}z";
            if (c < 0) quadratic += c;
            else quadratic += $"+{c}";
            quadratic += "=0";
        }

        public bool CheckAnswer(string answer)
        {
            if (answer.Contains(','))
            {
                string[] answers = answer.Split(',');
                Complex a1 = new Complex(answers[0]);
                Complex a2 = new Complex(answers[1]);
                if (a1 == root || a2 == root && a1 != a2)
                {
                    if (a1 == conjugate || a2 == conjugate)
                    {
                        return true;
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
        public void LoadDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct! The answer is {root.GetComplex()},{conjugate.GetComplex()}";
            }
            return $"Incorrect, the answer was {root.GetComplex()},{conjugate.GetComplex()}";

        }

        public string PrintQuestion()
        {
            return $"find the roots of the quadradic with the equation {quadratic}. Write the roots out next to each other, with a comma seperating them";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Quadratic",
                root.GetComplex(),
                coef.ToString()
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
