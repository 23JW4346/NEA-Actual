using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NEA.Number_Classes;
using NEA.Questions.MultiDivide;
using NEA.Questions.ModArg;
using NEA.Questions.Loci;
using System.Windows.Forms;
using System.Reflection;


namespace NEA
{
    internal class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Menu();
        }

        static void Menu()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Welcome to the Complex Number Revision Tool");
                Console.WriteLine("> Choose Topic");
                Console.WriteLine("  Create Quiz");
                Console.WriteLine("  Exit");
                Console.CursorLeft = 1;
                Console.CursorTop = 1;
                bool exit = true;
                while (exit)
                {
                    ConsoleKeyInfo choice = Console.ReadKey(true);
                    if (choice.Key == ConsoleKey.Enter)
                    {
                        switch (Console.CursorTop)
                        {
                            case 1:
                                ChooseTopic();
                                break;
                            case 2:
                                CreateQuiz();
                                break;
                            case 3:
                                exit = false;
                                break;
                        }
                        Console.Clear();
                        Console.WriteLine("Welcome to the Complex Number Revision Tool");
                        Console.WriteLine("> Choose Topic");
                        Console.WriteLine("  Create Quiz");
                        Console.WriteLine("  Exit");
                        Console.CursorLeft = 1;
                        Console.CursorTop = 1;
                    }
                    else if (choice.Key == ConsoleKey.DownArrow && Console.CursorTop != 3)
                    {
                        Console.CursorLeft = 0;
                        Console.Write(" ");
                        Console.CursorTop++;
                        Console.CursorLeft = 0;
                        Console.Write(">");
                    }
                    else if (choice.Key == ConsoleKey.UpArrow && Console.CursorTop != 1)
                    {
                        Console.CursorLeft = 0;
                        Console.Write(" "); ;
                        Console.CursorTop--;
                        Console.CursorLeft = 0;
                        Console.Write(">");
                    }
                }
                loop = false;
            }
        }
        static void ChooseTopic()
        {
            Console.Clear();
            Console.WriteLine("Which Topic:");
            Console.WriteLine("> Complex Number Multiplication");
            Console.WriteLine("  Complex Number Division");
            Console.WriteLine("  Modulus Argument Form");
            Console.WriteLine("  Finding Roots of a polynomial");
            Console.WriteLine("  Complex Loci");
            Console.WriteLine("  Main Menu");
            Console.CursorLeft = 1;
            Console.CursorTop = 1;
            bool exit = true;
            while (exit)
            {
                ConsoleKeyInfo choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Enter)
                {
                    int questionSet;
                    switch (Console.CursorTop)
                    {
                        case 1:
                            questionSet = 1;
                            GenerateQs(questionSet);
                            break;
                        case 2:
                            questionSet = 2;
                            GenerateQs(questionSet);
                            break;
                        case 3:
                            questionSet = 3;
                            GenerateQs(questionSet);
                            break;
                        case 4:
                            questionSet = 4;
                            GenerateQs(questionSet);
                            break;
                        case 5:
                            questionSet = 5;
                            GenerateQs(questionSet);
                            break;
                        case 6:
                            exit = false;
                            break;

                    }
                    Console.Clear();
                    Console.WriteLine("Which Topic:");
                    Console.WriteLine("> Complex Number Multiplication");
                    Console.WriteLine("  Complex Number Division");
                    Console.WriteLine("  Modulus Argument Form");
                    Console.WriteLine("  Finding Roots of a polynomial");
                    Console.WriteLine("  Complex Loci");
                    Console.WriteLine("  Main Menu");
                    Console.CursorLeft = 1;
                    Console.CursorTop = 1;

                }
                else if (choice.Key == ConsoleKey.DownArrow && Console.CursorTop != 6)
                {
                    Console.CursorLeft = 0;
                    Console.Write(" ");
                    Console.CursorTop++;
                    Console.CursorLeft = 0;
                    Console.Write(">");
                }
                else if (choice.Key == ConsoleKey.UpArrow && Console.CursorTop != 1)
                {
                    Console.CursorLeft = 0;
                    Console.Write(" "); ;
                    Console.CursorTop--;
                    Console.CursorLeft = 0;
                    Console.Write(">");
                }
            }
        }

        static void GenerateQs(int questionSet)
        {
            IQuestion question;
            if (questionSet == 1)
            {
                bool loop = true;
                while (loop)
                {
                    switch (rnd.Next(3))
                    {
                        case 0:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new Multiply2Complex("Questions.txt");
                            }
                            else
                            {
                                question = new Multiply2Complex();
                            }
                            AskQuestion(question, ref loop);
                            break;
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new MultiAlg("Questions.txt");
                            }
                            else
                            {
                                question = new MultiAlg();
                            }
                            AskQuestion(question, ref loop);
                            break;
                        case 2:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new MultiAlg2("Questions.txt");
                            }
                            else
                            {
                                question = new MultiAlg2();
                            }
                            AskQuestion(question, ref loop);
                            break;

                    }
                }
            }
            else if (questionSet == 2)
            {
                bool loop = true;
                while (loop)
                {
                    switch (rnd.Next(1, 4))
                    {
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new Divide2Complex("Questions.txt");
                            }
                            else
                            {
                                question = new Divide2Complex();
                            }
                            AskQuestion(question, ref loop);
                            break;
                        case 2:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new DivAlg("Questions.txt");
                            }
                            else
                            {
                                question = new DivAlg();
                            }
                            AskQuestion(question, ref loop);
                            break;
                    }
                }
            }
            else if (questionSet == 3)
            {
                bool loop = true;
                while (loop)
                {
                    switch (rnd.Next(7, 10))
                    {
                        case 7:
                            if(rnd.Next(1, 16) == 1)
                            {
                                question = new ModulusQuestion("Questions.txt");
                            }
                            else
                            {
                                question = new ModulusQuestion();
                            }
                            AskQuestion(question, ref loop);

                            break;
                        case 8:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new ArgumentQuestion("Questions.txt");
                            }
                            else
                            {
                                question = new ArgumentQuestion();
                            }
                            AskQuestion(question, ref loop);
                            break;
                        case 9:

                            break;
                    }
                }
            }
            else if (questionSet == 4)
            {
                bool loop = true;
                while (loop)
                {
                    switch (rnd.Next(10, 13))
                    {
                        case 10:

                            break;

                    }
                }
            }
            else if (questionSet == 5)
            {
                bool loop = true;
                while (loop)
                {
                    switch (rnd.Next(0, 3))
                    {
                        case 0:
                            if (rnd.Next(1, 16)== 1)
                            {
                                question = new ArgtoCartesian("Questions.txt", rnd);
                            }
                            else
                            {
                                question = new ArgtoCartesian(rnd);
                            }
                            AskQuestion(question, ref loop);
                            break;
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                question = new ArgGraph("Questions.txt", rnd);
                            }
                            else
                            {
                                question = new ArgGraph(rnd);
                            }
                            AskQuestion(question, ref loop);
                            break;
                        case 2:
                            if (rnd.Next(1,16) == 1)
                            {
                                question = new ModToCartesian(rnd);
                            }
                            else
                            {
                                question = new ModToCartesian(rnd);
                            }
                            AskQuestion(question, ref loop);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        static void AskQuestion(IQuestion question, ref bool loop)
        {
            Console.Clear();
            string ans;
            Console.WriteLine(question.PrintQuestion());
            Console.WriteLine();
            Console.Write("Answer: ");
            try
            {
                question.LoadDiagram();
                ans = Console.ReadLine();
                Console.WriteLine();
            }
            catch (NotImplementedException)
            {
                ans = Console.ReadLine();
                Console.WriteLine();
            }
            if (question.CheckAnswer(ans))
            {
                Console.WriteLine(question.PrintAnswer(true));
            }
            else
            {
                Console.WriteLine(question.PrintAnswer(false));
            }
            Console.WriteLine();
            Console.WriteLine("Would you like another question?");
            Console.WriteLine("> yes");
            Console.WriteLine("  no");
            bool exit = true;
            Console.CursorLeft = 1;
            Console.CursorTop -= 2;
            int currentposition = Console.CursorTop, newposition = 0;
            while (exit)
            {
                ConsoleKeyInfo choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Enter)
                {
                    if (newposition == 1)
                    {
                        loop = false;
                    }
                    exit = false;
                }
                else if (choice.Key == ConsoleKey.DownArrow && newposition != 1)
                {
                    Console.CursorLeft = 0;
                    Console.Write(" ");
                    newposition++;
                    Console.CursorTop = currentposition + newposition;
                    Console.CursorLeft = 0;
                    Console.Write(">");
                }
                else if (choice.Key == ConsoleKey.UpArrow && newposition != 0)
                {
                    Console.CursorLeft = 0;
                    Console.Write(" ");
                    newposition--;
                    Console.CursorTop = currentposition + newposition;
                    Console.CursorLeft = 0;
                    Console.Write(">");
                }
            }
            Console.Clear();
        }

        static void CreateQuiz()
        {
            throw new NotImplementedException();
        }
    }
}
