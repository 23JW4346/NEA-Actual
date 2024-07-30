using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public interface IQuestion
    {
        string PrintQuestion();

        void Calculate();

        string PrintAnswer();

        void GenerateNumbers();

        bool CheckAnswer(string answer);

        void SaveQuestion();

        bool GetQuestion();
    }
}
