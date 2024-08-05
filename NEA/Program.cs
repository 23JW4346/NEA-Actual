using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NEA.Number_Classes;
using NEA.Multiplication_Questions;

namespace NEA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IQuestion question = new Multiply2Complex();
            Console.WriteLine(question.PrintQuestion());
            Console.WriteLine(question.PrintAnswer(question.CheckAnswer(Console.ReadLine())));
            Console.ReadKey();
        }
    }
}
