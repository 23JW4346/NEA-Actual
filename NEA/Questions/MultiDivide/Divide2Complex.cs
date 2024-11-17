using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using NEA.Questions.Loci;

namespace NEA.Questions.MultiDivide
{
    public class Divide2Complex : IQuestion
    {
        private Complex operand1, operand2, answer;

        public Divide2Complex()
        {
            GenQ();
            Calculate();
        }
        public Divide2Complex(string filename)
        {
            if (!GetQuestion(filename))
            {
                GenQ();
            }
            Calculate();
        }

        public void GenQ()
        {
            operand1 = new Complex(false);
            operand2 = new Complex(false);
        }

        public void Calculate()
        {
            answer = Program.DivideComplex(operand1, operand2);
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

        public void LoadDiagram(ArgandDiagram diagram)
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            else return $"Incorrect!\nThe Answer was {answer.GetComplex()}";

        }

        public string PrintQuestion()
        {
            return $"Find the Value of ({operand1.GetComplex()})/({operand2.GetComplex()}) in the form a+bi, where a and b can be written in fractions in there simplest form (a/b+ci/d)";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "Divide2Complex",
                operand1.GetComplex(),
                operand2.GetComplex()
            };
        }

        public void CloseDiagram(ArgandDiagram diagram)
        {
            throw new NotImplementedException();
        }
    }
}
