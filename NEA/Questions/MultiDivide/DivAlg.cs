using System;
using System.Collections.Generic;
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

        public void Calculate()
        {
            double realvalue = operand1.GetRealValue() * answer.GetRealValue() - operand1.GetImaginaryValue() * answer.GetImaginaryValue();
            double imagvalue = operand1.GetRealValue() * answer.GetImaginaryValue() + operand1.GetImaginaryValue() * answer.GetRealValue();
            a = 1;
            int loop = (int)realvalue;
            if (imagvalue < realvalue) loop = (int)imagvalue;
            for (int i = 1; i <= loop; i++)
            {
                if ((int)realvalue % i == 0 && (int)imagvalue % i == 0)
                {
                    a = i;
                }
            }
            Fraction real = new Fraction((int)realvalue, a);
            Fraction imag = new Fraction((int)imagvalue, a);
            operand2 = new Complex(real, imag);
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
            if(a != 1) return $"The complex number z satisfies the equation z({operand1.GetComplex()}) = {a}({operand2.GetComplex()})\nDetermine z, giving your answer in the form a+bi, where a and b are real";
            return $"The complex number z satisfies the equation z({operand1.GetComplex()}) = ({operand2.GetComplex()})\nDetermine z, giving your answer in the form a+bi, where a and b are real";
        }

        public void SaveQuestion(string filename)
        {
            return;
        }
    }
}
