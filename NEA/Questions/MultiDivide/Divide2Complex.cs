using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class Divide2Complex : IQuestion
    {
        private Complex operand1, operand2, answer;

        public Divide2Complex()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
            Calculate();
        }
        public Divide2Complex(string filename)
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
            Fraction real = new Fraction();
            Fraction imaginary = new Fraction();
            Complex conjugate = new Complex(operand2.GetRealValue(), -operand2.GetImaginaryValue());
            double realtop = operand1.GetRealValue() * conjugate.GetRealValue() - operand1.GetImaginaryValue() * conjugate.GetImaginaryValue();
            double imagtop = operand1.GetImaginaryValue() * conjugate.GetRealValue() + operand1.GetRealValue() * conjugate.GetImaginaryValue();
            double bottom = operand2.GetRealValue() * conjugate.GetRealValue() - operand2.GetImaginaryValue() * conjugate.GetImaginaryValue();
            if (!realtop.ToString().Contains('.') && !imagtop.ToString().Contains('.') && !bottom.ToString().Contains('.'))
            {
                real = new Fraction((int)realtop, (int)bottom);
                imaginary = new Fraction((int)imagtop, (int)bottom);
            }
            else
            {
                bool loop = true;
                do
                {
                    realtop *= bottom;
                    imagtop *= bottom;
                    bottom *= bottom;
                    if (!realtop.ToString().Contains('.') && !imagtop.ToString().Contains('.') && !bottom.ToString().Contains('.')) loop = false;
                } while (loop);  

               
            }
            answer = new Complex(real, imaginary);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer.GetComplex())
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

                    if (line == "Divide2Complex" && !found)
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

        public void LoadDiagram()
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"Find the Value of ({operand1.GetComplex()})/({operand2.GetComplex()}) in the form a+bi, where a and b can be written in fractions if needed (x/y)";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine();
                sw.WriteLine("Divide2Complex");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(operand2.GetComplex());
            }
        }
    }
}
