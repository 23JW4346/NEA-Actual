using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class DivAlg : IQuestion
    {
        private Complex operand1, operand2, answer;
        private int a;

        public DivAlg()
        {
            operand1 = new Complex(false);
            answer = new Complex(false);
            Calculate();
        }

        public DivAlg(string filename)
        {
            if(GetQuestion(filename)) Calculate();
            else
            {
                operand1 = new Complex(false);
                answer = new Complex(false);
                Calculate();
            }

        }


        public void Calculate()
        {
            Complex temp = Program.TimesComplex(operand1, operand2);
            a = 1;
            int loop = (int)temp.GetRealValue();
            if (temp.GetImaginaryValue() < temp.GetRealValue()) loop = (int)temp.GetImaginaryValue();
            for (int i = 1; i <= loop; i++)
            {
                if ((int)temp.GetRealValue() % i == 0 && (int)temp.GetImaginaryValue() % i == 0)
                {
                    a = i;
                }
            }
            Fraction real = new Fraction((int)temp.GetRealValue(), a);
            Fraction imag = new Fraction((int)temp.GetImaginaryValue(), a);
            operand2 = new Complex(real, imag);
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

                    if (line == "DivAlg" && !found)
                    {
                        operand1 = new Complex(sr.ReadLine());
                        answer = new Complex(sr.ReadLine());
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
                output +=  $"Correct!\nThe answer is {answer.GetComplex()}";
            }
            else output += $"Incorrect!\nThe answer was {answer.GetComplex()}";
            output += $"\n Model answer: \n" +
                      $"This question requires you to rearange the equation to get z on one side.\n" +
                      $"This would give you the equation of ";
            if (a != 1) output += $"z = ({a}({operand2.GetComplex()}))/({operand1.GetComplex()})\n";
            else output += $"z = ({operand2.GetComplex()})/({operand1.GetComplex()})\n";
            output += $"With this you can now solve it, giving you an answer of {answer.GetComplex()}";
            return output;
        }

        public string PrintQuestion()
        {
            if(a != 1) return $"The complex number z satisfies the equation z({operand1.GetComplex()}) = {a}({operand2.GetComplex()})\nDetermine z, giving your answer in the form a+bi, where a and b are real";
            return $"The complex number z satisfies the equation z({operand1.GetComplex()}) = ({operand2.GetComplex()})\nDetermine z, giving your answer in the form a+bi, where a and b are real";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "DivAlg",
                operand1.GetComplex(),
                answer.GetComplex()
            };
        }
    }
}
