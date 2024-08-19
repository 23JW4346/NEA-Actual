using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class MultiAlg2 : IQuestion
    {
        private Complex Z, Zconjagute, operand;

        private int constant;

        public MultiAlg2()
        {
            Z = new Complex(false);
            Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
            operand = new Complex(false);
            Calculate();
        }

        public MultiAlg2(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                Z = new Complex(false);
                Zconjagute = new Complex(Z.GetRealValue(), -Z.GetImaginaryValue());
                operand = new Complex(false);
            }
        }

        public void Calculate()
        {
            int realpart = (int)(operand.GetRealValue() * Zconjagute.GetRealValue() - operand.GetImaginaryValue() * Zconjagute.GetImaginaryValue());
            int imagpart = (int)(operand.GetRealValue() * Zconjagute.GetImaginaryValue() + operand.GetImaginaryValue() * Zconjagute.GetRealValue());
            constant = (int)(imagpart - Z.GetImaginaryValue());
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.Z.GetComplex())
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

        public void LoadDiagram()
        {
            throw new NotImplementedException();
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {Z.GetComplex()}";
            }
            return $"Incorrect!\nThe Answer was {Z.GetComplex()}";
        }

        public string PrintQuestion()
        {
            string bracket1 = "(Z" + (constant < 0 ? "" : "+") + constant;
            return $"The complex number z satisfies the equation {bracket1}) = ({operand.GetComplex()})Z*\nDetermine z, giving your answer in the form a+bi, where a and b are real";
        }

        public void SaveQuestion(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
