using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NEA.Number_Classes;
using NEA.Questions;
using NEA.Questions.Loci;
using NEA.Questions.ModArg;
using NEA.Questions.MultiDivide;
using NEA.Questions.Polynomial_Roots;

namespace NEA
{

    internal class Program
    {
        static Random rnd = new Random();
        static List<List<string>> savequests = new List<List<string>>();
        const string posintReg = "[1-9][0-9]*|0";
        const string intReg = "-?" + posintReg;
        const string fracReg = posintReg + "/" + posintReg + "|-?" + posintReg + "/" + posintReg +"|0";
        const string complexReg = "(("+ intReg + ")|(" + fracReg + "))[+-](("+ intReg +")|(" + fracReg + "))i|(("+ intReg + ")|(" + fracReg + "))i";



        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Menu();
            using (StreamWriter sw = new StreamWriter("Questions.txt", true))
            {
                foreach (List<string> strings in savequests)
                {
                    foreach (string s in strings)
                    {
                        sw.WriteLine(s);
                    }
                }
                sw.Close();
            }
        }

        public static Complex TimesComplex(Complex a, Complex b)
        {
            return new Complex(a.GetRealValue() * b.GetRealValue() - a.GetImaginaryValue() * b.GetImaginaryValue(), a.GetImaginaryValue() * b.GetRealValue() + a.GetRealValue() * b.GetImaginaryValue());
        }

        public static Complex DivideComplex(Complex a, Complex b)
        {
            Fraction RealPart = new Fraction((int)(a.GetRealValue() * b.GetRealValue() - a.GetImaginaryValue() * -b.GetImaginaryValue()), (int)(b.GetRealValue() * b.GetRealValue() - b.GetImaginaryValue() * -b.GetImaginaryValue()));
            Fraction ImagPart = new Fraction((int)(a.GetRealValue() * -b.GetImaginaryValue() + a.GetImaginaryValue() * b.GetRealValue()), (int)(b.GetRealValue() * b.GetRealValue() - b.GetImaginaryValue() * -b.GetImaginaryValue()));
            return new Complex(RealPart, ImagPart);
        }

        public static string CreateArgLine(Complex a, Fraction b)
        {
            string loci;
            if (a.GetComplex()[0] == '-')
            {
                if (b.GetTop() != 1)
                {
                    loci = $"arg(z{a.GetComplex()})={b.GetTop()}π/{b.GetBottom()}";
                }
                else
                {
                    loci = $"arg(z{a.GetComplex()})=π/{b.GetBottom()}";
                }
            }
            else
            {
                if (b.GetTop() != 1)
                {
                    loci = $"arg(z+{a.GetComplex()})={b.GetTop()}π/{b.GetBottom()}";
                }
                else
                {
                    loci = $"arg(z+{a.GetComplex()})=π/{b.GetBottom()}";
                }
            }
            return loci;
        }

        public static string CreateModCircle(Complex a, int modulus)
        {
            string loci;
            if (a.GetComplex()[0] == '-')
            {
                loci = $"|z{a.GetComplex()}|={modulus}";
            }
            else
            {
                loci = $"|z+{a.GetComplex()}|={modulus}";
            }
            return loci;
        }

        public static string CreateCartesianCircle(Complex a, int radius)
        {
            string xpart, ypart;
            if (a.GetRealValue() < 0)
            {
                xpart = $"(x{a.GetReal()})²";
            }
            else if (a.GetRealValue() == 0)
            {
                xpart = "x²";
            }
            else
            {
                xpart = $"(x+{a.GetReal()})²";
            }
            if (a.GetImaginaryValue() < 0)
            {
                ypart = $"(y{a.GetImaginaryValue()})²";
            }
            else if (a.GetImaginaryValue() == 0)
            {
                ypart = "y²";
            }
            else
            {
                ypart = $"(y+{a.GetImaginaryValue()})²";
            }
            return $"{xpart}+{ypart}={radius}";
        }

        public static string CreateCartesianLine(Complex a, double grad)
        {
            string answer = "";
            double yint = grad * -a.GetRealValue() + a.GetImaginaryValue();
            double xint = -yint / grad;
            if (grad == 0.5)
            {
                answer += "y=x/2";
            }
            else if (grad == -0.5) answer += "y=-x/2";
            else if (grad == 1) answer += "y=x";
            else if (grad == -1) answer += "y=-x";
            else if (grad == 0)
            {
                string xint2;
                if (xint.ToString().Contains('.'))
                {
                    xint2 = (xint * 2) + "/2";
                }
                else if (xint == double.NaN) xint2 = "0";
                else xint2 = xint.ToString();
                answer = $"x={xint2}";
            }
            else answer += "y=" + grad + "x";
            if (yint != 0)
            {
                string yint2;
                if (yint.ToString().Contains('.'))
                {
                    yint2 = (yint * 2) + "/2";
                }
                else yint2 = yint.ToString();
                if (yint < 0) answer += yint2;
                else answer += "+" + yint2;
                if (grad == int.MaxValue) answer = $"y={yint}";
            }
            return answer;
        }

        public static string CreateModLine(Complex a, Complex b)
        {
            string loci = "|z";
            if (a.GetComplex()[0] == '-')
            {
                loci += a.GetComplex();
            }
            else loci += "+" + a.GetComplex();
            loci += "|=|z";
            if (b.GetComplex()[0] == '-')
            {
                loci += b.GetComplex();
            }
            else loci += "+" + b.GetComplex();
            return loci + "|";
        }

        static void Menu()
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Welcome to the Complex Number Revision Tool (Use up and down arrows to navigate)");
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
            Console.WriteLine("  Roots of polynomials");
            Console.WriteLine("  Complex Loci");
            Console.WriteLine("  Random");
            Console.WriteLine("  Main Menu");
            Console.CursorLeft = 1;
            Console.CursorTop = 1;
            bool exit = true;
            while (exit)
            {
                ConsoleKeyInfo choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Enter)
                {
                    bool loop = true;
                    int placeholder = 0, questionset = Console.CursorTop;
                    while (loop)
                    {
                        switch (questionset)
                        {
                            case 1:
                                AskQuestion(GenQ(0), ref loop, ref placeholder);
                                break;
                            case 2:
                                AskQuestion(GenQ(1), ref loop, ref placeholder);
                                break;
                            case 3:
                                AskQuestion(GenQ(2), ref loop, ref placeholder);
                                break;
                            case 4:
                                AskQuestion(GenQ(3), ref loop, ref placeholder);
                                break;
                            case 5:
                                AskQuestion(GenQ(4), ref loop, ref placeholder);
                                break;
                            case 6:
                                AskQuestion(GenQ(rnd.Next(6)), ref loop, ref placeholder);
                                break;
                            case 7:
                                return;

                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Which Topic:");
                    Console.WriteLine("> Complex Number Multiplication");
                    Console.WriteLine("  Complex Number Division");
                    Console.WriteLine("  Modulus Argument Form");
                    Console.WriteLine("  Finding Roots of a polynomial");
                    Console.WriteLine("  Complex Loci");
                    Console.WriteLine("  Random");
                    Console.WriteLine("  Main Menu");
                    Console.CursorLeft = 1;
                    Console.CursorTop = 1;

                }
                else if (choice.Key == ConsoleKey.DownArrow && Console.CursorTop != 7)
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

        static IQuestion GenQ(int questionSet)
        {
            switch (questionSet)
            {
                case 0:
                    switch (rnd.Next(2))
                    {
                        case 0:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new Multiply2Complex("Questions.txt");
                            }
                            else
                            {
                                return new Multiply2Complex();
                            }
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new MultiAlg("Questions.txt");
                            }
                            else
                            {
                                return new MultiAlg();
                            }
                    }
                    break;
                case 1:
                    switch (rnd.Next(3))
                    {
                        case 0:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new Divide2Complex("Questions.txt");
                            }
                            else
                            {
                                return new Divide2Complex();
                            }
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new DivAlg("Questions.txt");
                            }
                            else
                            {
                                return new DivAlg();
                            }
                        case 2:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new DivAlg2("Questions.txt");
                            }
                            else
                            {
                                return new DivAlg2();
                            }
                    }
                    break;
                case 2:
                    switch (rnd.Next(6))
                    {
                        case 0:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModulusQuestion("Questions.txt");
                            }
                            else
                            {
                                return new ModulusQuestion();
                            }
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ArgumentQuestion("Questions.txt");
                            }
                            else
                            {
                                return new ArgumentQuestion();
                            }
                        case 2:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModulusArgumentForm("Questions.txt");
                            }
                            else
                            {
                                return new ModulusArgumentForm();
                            }
                        case 3:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModArgToNormal("Questions.txt");
                            }
                            else
                            {
                                return new ModArgToNormal();
                            }
                        case 4:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModulusPowers(rnd.Next(0, 4), "Questions.txt");
                            }
                            else
                            {
                                return new ModulusPowers(rnd.Next(0, 4));
                            }
                        case 5:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ArgumentPowers(rnd.Next(0, 4), "Questions.txt");
                            }
                            else
                            {
                                return new ArgumentPowers(rnd.Next(0, 4));
                            }
                    }
                    break;
                case 3:
                    switch (rnd.Next(3))
                    {
                        case 0:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new Quadratic(rnd.Next(1, 4), "Questions.txt");
                            }
                            else
                            {
                                return new Quadratic(rnd.Next(1, 4));
                            }
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new Cubic1rootgiven(rnd, "Questions.txt");
                            }
                            else
                            {
                                return new Cubic1rootgiven(rnd);
                            }
                    }
                    break;
                case 4:

                    switch (rnd.Next(6))
                    {
                        case 0:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ArgtoCartesian("Questions.txt", rnd);
                            }
                            else
                            {
                                return new ArgtoCartesian(rnd);
                            }
                        case 1:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ArgGraph("Questions.txt", rnd);
                            }
                            else
                            {
                                return new ArgGraph(rnd);
                            }
                        case 2:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModToCartesian(rnd, "Questions.txt");
                            }
                            else
                            {
                                return new ModToCartesian(rnd);
                            }
                        case 3:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModGraph(rnd, "Questions.txt");
                            }
                            else
                            {
                                return new ModGraph(rnd);
                            }
                        case 4:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ModLine("Questions.txt", rnd);
                            }
                            else
                            {
                                return new ModLine(rnd);
                            }
                        case 5:
                            if (rnd.Next(1, 16) == 1)
                            {
                                return new ArgIntersect(rnd, "Questions.txt");
                            }
                            else
                            {
                                return new ArgIntersect(rnd);
                            }
                        default:
                            break;
                    }
                    break;
                default:
                    break;

            }
            return new Quadratic(rnd.Next(1, 4));
        }

        static void AskQuestion(IQuestion question, ref bool loop, ref int score)
        {
            Console.Clear();
            string ans = "";
            bool cantypeans = true;
            char lastTyped = ' ';
            Console.WriteLine(question.PrintQuestion());
            Console.WriteLine();
            Console.Write("Answer: ");
            ConsoleKeyInfo choice;
            int CursorforAns = Console.CursorLeft;
            Console.Write("\n  Click for hint");
            Console.CursorTop--;
            Console.CursorLeft = CursorforAns;
            int currentposition = Console.CursorTop, newposition = 0;
            try
            {
                question.LoadDiagram();
            }
            catch { }
            do
            {
                choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Enter && cantypeans)
                {
                    Console.CursorTop += 2;
                    Console.CursorLeft = 0;
                    if (!question.CheckAnswer(ans))
                    {
                        savequests.Add(question.SaveQuestion());
                    }
                    Console.WriteLine(question.PrintAnswer(question.CheckAnswer(ans)));
                }
                else if (choice.Key == ConsoleKey.DownArrow && newposition != 1)
                {
                    Console.CursorLeft = 0;
                    Console.Write("A");
                    newposition++;
                    Console.CursorTop = currentposition + newposition;
                    Console.CursorLeft = 0;
                    Console.Write(">");
                    cantypeans = false;
                }
                else if (choice.Key == ConsoleKey.UpArrow && newposition != 0)
                {
                    Console.CursorLeft = 0;
                    Console.Write(" ");
                    newposition--;
                    Console.CursorTop = currentposition + newposition;
                    Console.CursorLeft = CursorforAns;
                    cantypeans = true;
                }
                else if (choice.Key == ConsoleKey.Backspace && cantypeans && Console.CursorLeft > 8)
                {
                    if (ans.Length > 0)
                    {
                        CursorforAns--;
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                        ans = ans.Substring(0, ans.Length - 1);
                    }
                    else
                    {
                        Console.CursorLeft++;
                        Console.CursorLeft--;
                    }
                }
                else if (cantypeans && choice.KeyChar == '^')
                {
                    do
                    {
                        choice = Console.ReadKey(true);
                        switch (choice.KeyChar)
                        {
                            case '2':
                                ans += "²";
                                Console.Write("²");
                                break;
                            case '3':
                                ans += "³";
                                Console.Write("³");
                                break;
                            case '4':
                                ans += "⁴";
                                Console.Write("⁴");
                                break;

                        }
                    } while (!Char.IsDigit(choice.KeyChar));
                }
                else if (cantypeans && choice.Key == ConsoleKey.P)
                {
                    Console.Write("π");
                    ans += "π";
                }
                else if (cantypeans && choice.Key != ConsoleKey.Backspace)
                {
                    Console.Write(choice.KeyChar);
                    lastTyped = choice.KeyChar;
                    ans += choice.KeyChar;
                    CursorforAns++;
                }
            } while (choice.Key != ConsoleKey.Enter);
            if (loop)
            {
                Console.WriteLine();
                Console.WriteLine("Would you like another question?");
                Console.WriteLine("> yes");
                Console.WriteLine("  no");
                bool exit = true;
                Console.CursorLeft = 1;
                Console.CursorTop -= 2;
                currentposition = Console.CursorTop;
                newposition = 0;
                while (exit)
                {
                    choice = Console.ReadKey(true);
                    if (choice.Key == ConsoleKey.Enter)
                    {
                        if (newposition == 1)
                        {
                            loop = false;
                        }
                        else
                        {
                            loop = true;
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
            }
            else
            {
                if (question.CheckAnswer(ans))
                {
                    score++;
                }
                Console.WriteLine();
                Console.WriteLine("Click any button for Next Question");
                Console.ReadKey();

            }
            Console.Clear();
        }

        static void CreateQuiz()
        {
            Console.Clear();
            int quizScore = 0, scoreForQuestion = 0;
            IQuestion[] quizQuestions = new IQuestion[10];
            int[] questionSets = new int[10];
            bool[] questionWrong = new bool[10];
            int[] numberwrong = new int[5];
            for (int i = 0; i < quizQuestions.Length; i++)
            {
                questionSets[i] = rnd.Next(6);
                quizQuestions[i] = GenQ(questionSets[i]);
                questionWrong[i] = true;
            }
            Shuffle(quizQuestions);
            foreach (IQuestion question in quizQuestions)
            {
                bool repeat = false;
                AskQuestion(question, ref repeat, ref scoreForQuestion);
                if (scoreForQuestion > 0)
                {
                    quizScore += scoreForQuestion;
                    scoreForQuestion = 0;
                    questionWrong[Array.IndexOf(quizQuestions, question)] = false;
                }
            }
            Console.WriteLine();
            Console.WriteLine("You have reached the end of the quiz");
            Console.WriteLine($"Your final score was {quizScore}");
            if (questionWrong.Contains(false))
            {
                for (int i = 0; i < questionWrong.Length; i++)
                {
                    if (!questionWrong[i])
                    {
                        switch (questionSets[i])
                        {
                            case 1:
                                numberwrong[0]++;
                                break;
                            case 2:
                                numberwrong[1]++;
                                break;
                            case 3:
                                numberwrong[2]++;
                                break;
                            case 4:
                                numberwrong[3]++;
                                break;
                            case 5:
                                numberwrong[4]++;
                                break;
                        }
                    }
                }
                Console.Write("due to your results, We suggest Revising: ");
                switch (Array.IndexOf(numberwrong, numberwrong.Max()))
                {
                    case 0:
                        Console.WriteLine("Complex number multiplication");
                        break;
                    case 1:
                        Console.WriteLine("Complex number division");
                        break;
                    case 2:
                        Console.WriteLine("Modulus-Argument form");
                        break;
                    case 3:
                        Console.WriteLine("Roots of polynomails");
                        break;
                    case 4:
                        Console.WriteLine("Complex Loci");
                        break;

                }
            }
            Console.WriteLine();
            Console.Write("> Main Menu");
            Console.CursorLeft = 1;
            Console.ReadKey();
        }

        static void Shuffle(IQuestion[] questions)
        {
            for (int i = 0; i < 100; i++)
            {
                int index = rnd.Next(questions.Length);
                IQuestion temp = questions[index];
                int index2 = index;
                while (index == index2) index2 = rnd.Next(questions.Length);
                questions[index] = questions[index2];
                questions[index2] = temp;
            }
        }
    }
}