using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.Windows.Forms;
using System.IO;
using System.CodeDom.Compiler;

namespace NEA.Questions.Loci
{
    public class ModToCartesian : IQuestion
    {
        private Complex operand;
        private string loci;
        private string answer;
        private int modulus;
        private ArgandDiagram diagram;

        public ModToCartesian(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while(operand.GetComplex() == "") operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            modulus = rnd.Next(1, 6);
            if (temp.GetComplex()[0] == '-')
            {
                loci = $"|z{temp.GetComplex()}|={modulus}";
            }
            else
            {
                loci = $"|z+{temp.GetComplex()}|={modulus}";
            }
            Calculate();
        }

        public ModToCartesian(Random rnd, string filename)
        {
            if (GetQuestion(filename))
            {
                Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                if (temp.GetComplex()[0] == '-')
                {
                    loci = $"|z{temp.GetComplex()}|={modulus}";
                }
                else
                {
                    loci = $"|z+{temp.GetComplex()}|={modulus}";
                }
            }
            else
            {
                operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                while (operand.GetComplex() == "") operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                modulus = rnd.Next(1, 6);
                if (temp.GetComplex()[0] == '-')
                {
                    loci = $"|z{temp.GetComplex()}|={modulus}";
                }
                else
                {
                    loci = $"|z+{temp.GetComplex()}|={modulus}";
                }
            }
            Calculate();
        }

        public void Calculate()
        {
            Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            string xpart, ypart;
            int radius = modulus * modulus;
            if (temp.GetRealValue() < 0)
            {
                xpart = $"(x{temp.GetReal()})^2";
            }
            else if (temp.GetRealValue() == 0)
            {
                xpart = "x^2";
            }
            else
            {
                xpart =  $"(x+{temp.GetReal()})^2";
            }
            if (temp.GetImaginaryValue() < 0)
            {
                ypart = $"(y{temp.GetImaginaryValue()})^2";
            }
            else if (temp.GetImaginaryValue() == 0)
            {
                ypart = "y^2";
            }
            else
            {
                ypart = $"(y+{-temp.GetImaginaryValue()})^2";
            }
            answer = $"{xpart}+{ypart}={radius}";
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer)
            {
                return true;
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
                    if (line == "ModToCart" && !found)
                    {
                        string number = null;
                        bool firstneg = true;
                        double realin = 0, imagin = 0;
                        string operand1 = sr.ReadLine();
                        for (int i = 0; i < operand1.Length; i++)
                        {
                            if (Char.IsNumber(operand1[i]))
                            {
                                number += operand1[i];
                            }
                            if (operand1[i] == '-')
                            {
                                if (firstneg && number.Length < 1)
                                {
                                    number += operand1[i];
                                    firstneg = false;
                                }
                                else
                                {
                                    realin = double.Parse(number);
                                    number = "-";
                                }
                            }
                            else if (operand1[i] == 'i')
                            {
                                imagin = double.Parse(number);
                                break;
                            }
                            else if (operand1[i] == '+')
                            {
                                realin = double.Parse(number);
                                number = null;
                            }
                        }
                        operand = new Complex(realin, imagin);
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

        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateCircle(operand, modulus);
            Application.Run(diagram);
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
            return $"Write This Loci {loci} in Cartesian Form (as a circle)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("ModToCart");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(modulus);

            }
        }
    }
}
