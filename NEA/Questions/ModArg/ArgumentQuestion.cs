using System;
using System.Collections.Generic;
using System.IO;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.ModArg
{
    public class ArgumentQuestion : IQuestion
    {
        private Complex operand;

        private string answer;

        public ArgumentQuestion()
        {
            GenQ();
            Calculate();
        }
        public ArgumentQuestion(string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ();
            }
            Calculate();
        }

        public void GenQ()
        {
            operand = new Complex(false);
        }

        public void Calculate()
        {
            answer = Math.Round(operand.GetArgument(), 3).ToString();
            if (operand.GetArgument() < Math.Round(operand.GetArgument(), 3) && ((operand.GetArgument().ToString()[4] ==0 && operand.GetArgument() > 0) || (operand.GetArgument().ToString()[5] == 0 && operand.GetArgument() < 0))) 
            {
                answer += "0";
            }
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
                    if (line == "Argument" && !found)
                    {
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
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"The Complex number z is defined as {operand.GetComplex()}. Calculate Arg(z) to 3.d.p";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Argument",
                operand.GetComplex(),
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NoDiagramException();
        }
    }
}
