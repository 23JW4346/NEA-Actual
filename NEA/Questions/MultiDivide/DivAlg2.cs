﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class DivAlg2 : IQuestion
    {
        private Complex Z, Zconjagute, operand;

        private int constant;

        public DivAlg2()
        {
            Z = new Complex(false);
            Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
            operand = new Complex(false);
            Calculate();
        }

        public DivAlg2(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                Z = new Complex(false);
                Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
                operand = new Complex(false);
                Calculate();
            }
        }

        public void Calculate()
        {
            int imagpart = (int)(operand.GetRealValue() * Zconjagute.GetImaginaryValue() + operand.GetImaginaryValue() * Zconjagute.GetRealValue());
            constant = (int)(imagpart - Z.GetImaginaryValue());
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.Z.GetComplex())
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

                    if (line == "Divide2Complex" && !found)
                    {
             
                        Z = new Complex(sr.ReadLine());
                        operand = new Complex(sr.ReadLine());
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
            Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
            return found;
        }

        public string Hint()
        {
            throw new NotImplementedException();
        }

        public void LoadDiagram()
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            string output = "";
            if (correct)
            {
                output += $"Correct!\nThe Answer is {Z.GetComplex()}\n";
            }
            else output += $"Incorrect!\nThe Answer was {Z.GetComplex()}\n";
            output += $"Model answer: \n" +
                      $"This question requires you to rearange the equation to get z* on one side.\n" +
                      $"This would give you the equation of";
            if (constant < 0) output += $"(z{constant}i)/({operand.GetComplex()})";
            else output += $"(z+{constant}i)/({operand.GetComplex()})";
            output += $"With this you can now solve it, giving you an answer of {Z.GetComplex()}";
            return output;
        }

        public string PrintQuestion()
        {
            string bracket1 = "(z" + (constant < 0 ? "" : "+") + constant+"i";
            return $"The complex number z satisfies the equation {bracket1}) = ({operand.GetComplex()})z*\nDetermine z, giving your answer in the form a+bi, where a and b are real";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "MulitAlg2",
                Z.GetComplex(),
                operand.GetComplex()
            };
        }
    }
}
