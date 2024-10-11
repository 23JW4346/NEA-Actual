using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.ModArg
{
    public class ModulusArgumentForm : IQuestion
    {
        private Complex operand;

        private string answer;

        public ModulusArgumentForm()
        {
            operand = new Complex(true);
            Calculate();
        }

        public ModulusArgumentForm(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                operand = new Complex(true);
                Calculate();
            }
        }

        public void Calculate()
        {
            string Modulus = operand.GetModulus().GetString();
            string arg = operand.GetArgument().ToString();
            answer = $"{Modulus}(cos({arg})+isin({arg}))";
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
                    if (line == "ModArg" && !found)
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
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"The complex number z is denoted as {operand.GetComplex()}. Write z in modulus-argument form (arguments in 3.d.p)";
        }


        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModArg",
                operand.GetComplex()
            };
        }
    }
}
