﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.Windows.Forms;
using System.IO;

namespace NEA.Questions.Loci
{
    public class ModToCartesian : IQuestion
    {
        private Complex operand;
        private string loci;
        private string answer;
        private int modulus;

        public ModToCartesian(Random rnd)
        {
            GenQ(rnd);
            Calculate();
        }

        public ModToCartesian(Random rnd, string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ(rnd);
            }
            Calculate();
        }

        public void GenQ(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            modulus = rnd.Next(1, 6);
        }

        public void Calculate()
        {
            Complex temp = operand.Flip();
            loci = Program.CreateModCircle(temp, modulus.ToString());
            answer = Program.CreateCartesianCircle(temp, modulus * modulus);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer)
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
                    if (line == "ModToCart" && !found)
                    {
                        operand = new Complex(sr.ReadLine());
                        modulus = int.Parse(sr.ReadLine());
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
            diagram.CreateCircle(operand, modulus, loci);
            Task.Run(() =>
            {
                Application.Run(diagram);
            });
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
            return $"Write This Loci {loci} in Cartesian Form ((x-a)²+(y-b)²=r²), type ^2 to get the squared symbol";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModToCart",
                operand.GetComplex(),
                modulus.ToString()
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            diagram.Invoke((Action)(() =>
            {
                diagram.Close();
            }));
        }
    }
}
