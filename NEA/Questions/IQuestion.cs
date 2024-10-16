﻿using System;
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

        string PrintAnswer(bool correct);

        bool CheckAnswer(string answer);

        List<string> SaveQuestion();

        string Hint();

        bool GetQuestion(string filename);

        void LoadDiagram();
    }
}
