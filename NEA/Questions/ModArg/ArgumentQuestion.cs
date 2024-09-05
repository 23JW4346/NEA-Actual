using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ArgumentQuestion : IQuestion
    {
        private Complex operand;

        private Number answer;

        public ArgumentQuestion()
        {
            operand = new Complex(false);
            Calculate();
        }
        public ArgumentQuestion(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                operand = new Complex(false);
                Calculate();
            }
        }

        public void Calculate()
        {
            answer = new Number(operand.GetArgument());
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer.GetString())
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

        public void LoadDiagram()
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetString()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetString()}";
        }

        public string PrintQuestion()
        {
            return $"The Complex number z is defined as {operand.GetComplex()}. Calculate Arg(z) to 3.d.p";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("Argument");
                sw.WriteLine(operand.GetComplex());
            }
        }
    }
}
