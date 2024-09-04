using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.Windows.Forms;
using System.IO;

namespace NEA.Questions.Loci
{
    public class ArgGraph : IQuestion
    {
        private string answer;
        private Complex operand;
        private Fraction argument;
        private double step;
        private bool isleft;
        private ArgandDiagram diagram;
        private (int, int)[] fractions = { (1, 6), (1, 4), (1, 3), (2, 3), (3, 4), (5, 6) };
        private double[] steps = { 0.5,  1, 2, 2, 1, 0.5 };

        public ArgGraph(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while(operand.GetComplex() == null) operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            if(rnd.Next(2) == 1)
            {
                int rand = rnd.Next(fractions.Length); ;
                argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                step = steps[rand];
                if (inanswer.GetComplex()[0] == '-')
                {
                    answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else
                {
                    answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                }
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            else
            {
                int rand = rnd.Next(fractions.Length);
                argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                step = -steps[rand];
                if (inanswer.GetComplex()[0] == '-')
                {
                    answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                }
                else
                {
                    answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                }
                if (rand > 2) isleft = true;
                else isleft = false;
            }
            
        }

        public ArgGraph(string filename, Random rnd)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                while (operand.GetComplex() == null) operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                if (rnd.Next(2) == 1)
                {
                    int rand = rnd.Next(fractions.Length); ;
                    argument = new Fraction(fractions[rand].Item1, fractions[rand].Item2);
                    step = steps[rand];
                    if (inanswer.GetComplex()[0] == '-')
                    {
                        answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    else
                    {
                        answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    if (rand > 2) isleft = true;
                    else isleft = false;
                }
                else
                {
                    int rand = rnd.Next(fractions.Length);
                    argument = new Fraction(-fractions[rand].Item1, fractions[rand].Item2);
                    step = -steps[rand];
                    if (inanswer.GetComplex()[0] == '-')
                    {
                        answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    else
                    {
                        answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
                    }
                    if (rand > 2) isleft = true;
                    else isleft = false;
                }
            }
        }

        public void Calculate()
        {
            Complex inanswer = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            int placeholder = 0;
            if (argument.GetNegative())
            {
                Fraction temp = new Fraction((int)-argument.GetTop(), (int)argument.GetBottom());
                placeholder =Array.IndexOf(fractions, (temp.GetTop(), temp.GetBottom()));
                step = steps[placeholder];
            }
            else
            {
                placeholder = Array.IndexOf(fractions, (argument.GetTop(), argument.GetBottom()));
                step = steps[placeholder];
            }
            if (inanswer.GetComplex()[0] == '-')
            {
                answer = $"arg(z{inanswer.GetComplex()})={argument.GetString()}π";
            }
            else answer = $"arg(z+{inanswer.GetComplex()})={argument.GetString()}π";
            if (placeholder > 2) isleft = true;
            else isleft = false;
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
                    if (line == "ArgGraph" && !found)
                    {
                        string number = "";
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
                        found = true;
                        string fract = sr.ReadLine();
                        string[] numbers = fract.Trim().Split('/');
                        argument = new Fraction(int.Parse(numbers[0]), int.Parse(numbers[1]));
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
            diagram.CreateLine(step, operand, isleft);
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
            return $"What is the equation for this Complex loci? (no spaces, use Pi symbol π)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("ArgGraph");
                sw.WriteLine(operand.GetComplex());
                sw.WriteLine(argument.GetString());
            }
        }
    }
}
