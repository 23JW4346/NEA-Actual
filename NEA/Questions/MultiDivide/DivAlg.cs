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
            GenQ();
            Calculate();
        }

        public DivAlg(string filename)
        {
            if(!GetQuestion(filename))
            {
                GenQ();
                Calculate();
            }

        }

        public void GenQ()
        {
            operand1 = new Complex(false);
            answer = new Complex(false);
        }


        public void Calculate()
        {
            Complex temp = Program.TimesComplex(operand1, answer);
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
                        operand2 = new Complex(sr.ReadLine());
                        answer = new Complex(sr.ReadLine());
                        a = int.Parse(sr.ReadLine());
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
                return $"Correct!\nThe answer is {answer.GetComplex()}";
            }
            else return $"Incorrect!\nThe answer was {answer.GetComplex()}";
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
                operand2.GetComplex(),
                answer.GetComplex(),
                a.ToString()
            };
        }
    }
}
