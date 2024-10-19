using NEA.Number_Classes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Questions.MultiDivide
{
    public class Multiply2Complex : IQuestion
    {
        private Complex operand1, operand2, answer;

        public Multiply2Complex()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
            Calculate();

        }

        public Multiply2Complex(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                operand1 = new Complex(false);
                operand2 = new Complex(false);
                Calculate();
            }
        }

        public void Calculate()
        {
            answer = Program.TimesComplex(operand1, operand2);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer.GetComplex())
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

                    if (line == "Multiply2Complex" && !found)
                    {
                        operand1 = new Complex(sr.ReadLine());
                        operand2 = new Complex(sr.ReadLine());
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
            string output = "";
            if (correct)
            {
                output += $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            else
            {
                output += $"Incorrect!\nThe Answer was {answer.GetComplex()}";
            }
            output += "\nModel answer:\n";
            output += $"(a+bi)*(c+di) = (ac-bd) + (ad+bc)i\n" +
                      $"                 Real     Imaginary  \n" + 
                      $"So in the case of this question: ({operand1.GetComplex()})*({operand2.GetComplex()})\n" +
                      $"The real part = ({operand1.GetReal()}*{operand2.GetReal()}) - ({operand1.GetImaginary()}*{operand2.GetImaginary()}) = {answer.GetReal()}\n" +
                      $"The Imaginary part = ({operand1.GetReal()}*{operand2.GetImaginary()}) + ({operand1.GetImaginary()}*{operand2.GetReal()}) = {answer.GetImaginary()}\n" +
                      $"Which ends in the final answer, {answer.GetComplex()}\n";
            return output;
        }

        public string PrintQuestion()
        {
            return $"Calculate ({operand1.GetComplex()})*({operand2.GetComplex()})";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Multiply2Complex",
                operand1.GetComplex(),
                operand2.GetComplex()
            };
        }
    }
}
