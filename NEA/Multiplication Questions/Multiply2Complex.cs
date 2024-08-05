using NEA.Number_Classes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Multiplication_Questions
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
            if(GetQuestion(filename)) Calculate();
        }
        public void Calculate()
        {
            double realvalue = operand1.GetRealValue()*operand2.GetRealValue() - operand1.GetImaginaryValue()*operand2.GetImaginaryValue();
            double imagvalue = operand1.GetRealValue()*operand2.GetImaginaryValue() + operand1.GetImaginaryValue()*operand2.GetRealValue();
            answer = new Complex(realvalue, imagvalue);
        }

        public bool CheckAnswer(string answer)
        {
            bool correct = false;
            if(answer == this.answer.GetComplex())
            {
                correct = true;
            }
            if (correct)
            {
                return correct;
            }
            SaveQuestion("Questions.txt");
            return correct;
            
        }

        public bool GetQuestion(string filename)
        {
            using(StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    if(sr.ReadLine() == "Multiply2Complex")
                    {
                        string operand1 = sr.ReadLine();
                        string[] thing = operand1.Split('*');
                        this.operand1 = new Complex(double.Parse(thing[0]), double.Parse(thing[1][0].ToString()));

                        string operand2 = sr.ReadLine();
                        thing = operand2.Split('*');
                        this.operand2 = new Complex(double.Parse(thing[0]), double.Parse(thing[1].Remove('i')));
                        return true;
                    }
                }
                return false;
            }
        }

        public string PrintAnswer(bool correct)
        {
            if(correct)
            {
                return $"Correct!\nThe Answer is {answer.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {answer.GetComplex()}";
        }

        public string PrintQuestion()
        {
            return $"Calculate ({operand1.GetComplex()})*({operand2.GetComplex()})";
        }

        public void SaveQuestion(string filename)
        {
            using(StreamWriter sw = new StreamWriter(filename, append: true)) 
            {
                sw.WriteLine();
                sw.WriteLine("Multiply2Complex");
                sw.WriteLine(operand1.GetComplex());
                sw.WriteLine(operand2.GetComplex());
            }
        }
    }
}
