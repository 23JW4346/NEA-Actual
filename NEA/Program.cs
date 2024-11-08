using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
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

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            bool loop = true;
            int score = 0;
            AskQuestion(new ArgIntersect(rnd), ref loop, ref score);
            Menu();

            //At the end when the user clicks exit, it saves all the questions that the user got wrong into a text file.
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

        //returns a*b
        public static Complex TimesComplex(Complex a, Complex b)
        {
            return new Complex(a.GetRealValue() * b.GetRealValue() - a.GetImaginaryValue() * b.GetImaginaryValue(), a.GetImaginaryValue() * b.GetRealValue() + a.GetRealValue() * b.GetImaginaryValue());
        }

        //returns the calculation a/b
        public static Complex DivideComplex(Complex a, Complex b)
        {
            Fraction RealPart = new Fraction((int)(a.GetRealValue() * b.GetRealValue() - a.GetImaginaryValue() * -b.GetImaginaryValue()), (int)(b.GetRealValue() * b.GetRealValue() - b.GetImaginaryValue() * -b.GetImaginaryValue()));
            Fraction ImagPart = new Fraction((int)(a.GetRealValue() * -b.GetImaginaryValue() + a.GetImaginaryValue() * b.GetRealValue()), (int)(b.GetRealValue() * b.GetRealValue() - b.GetImaginaryValue() * -b.GetImaginaryValue()));
            return new Complex(RealPart, ImagPart);
        }

        //returns the string for an arg line in an argand diagram (arg(z-(z1)=θ)
        public static string CreateArgLine(Complex a, Fraction b)
        {
            string loci;
            if (a.GetComplex()[0] == '-')
            {
                if (Math.Abs(b.GetTop()) != 1)
                {
                    loci = $"arg(z{a.GetComplex()})={b.GetTop()}π/{b.GetBottom()}";
                }
                else
                {
                    if(b.GetValue() <0 ) loci = $"arg(z{a.GetComplex()})=-π/{b.GetBottom()}";
                    else loci = $"arg(z{a.GetComplex()})=π/{b.GetBottom()}";
                }
            }
            else
            {
                if (Math.Abs(b.GetTop()) != 1)
                {
                    loci = $"arg(z+{a.GetComplex()})={b.GetTop()}π/{b.GetBottom()}";
                }
                else
                {
                    if(b.GetValue() < 0) loci = $"arg(z+{a.GetComplex()})=-π/{b.GetBottom()}";
                    loci = $"arg(z+{a.GetComplex()})=π/{b.GetBottom()}";
                }
            }
            return loci;
        }
        //returns the Complex Loci string for a circle (|z-z1|=r)
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

        //returns a string for a cartesian circle ((x-x1)² + (y-y1)² = r²)
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

        //returns the string for a cartesian line (y=mx+c)
        public static string CreateCartesianLine(Complex a, double grad)
        {
            string answer = "";
            double yint;
            if (grad != int.MaxValue) yint = grad * -a.GetRealValue() + a.GetImaginaryValue();
            else yint = a.GetImaginaryValue();
            double xint = a.GetRealValue();
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
                answer = $"y={xint2}";
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
                if (grad == int.MaxValue) answer = $"x={yint}";
            }
            return answer;
        }

        //returns the string for a modline loci (|z-z1|=|z-z2|)
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

        //Main Menu, can use arrow kets (up and down) to navigate
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

        //Menu for choosing topic (still uses up and down arrow keys)
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

        //Returns a question type due to which question set its in and a randomly generated question in that set.
        static IQuestion GenQ(int questionSet)
        {
            while (true)
            {
                switch (questionSet)
                {
                    case 0:
                        switch (rnd.Next(3))
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
                            case 2:
                                if (rnd.Next(1, 16) == 1)
                                {
                                    return new ZSquared(rnd);
                                }
                                else
                                {
                                    return new ZSquared("Questions.txt", rnd);
                                }
                        }
                        break;
                    case 1:
                        switch (rnd.Next(2))
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
                                    return new ModulusPowers(rnd.Next(2, 5), "Questions.txt");
                                }
                                else
                                {
                                    return new ModulusPowers(rnd.Next(2, 5));
                                }
                            case 5:
                                if (rnd.Next(1, 16) == 1)
                                {
                                    return new ArgumentPowers(rnd.Next(2, 5), "Questions.txt");
                                }
                                else
                                {
                                    return new ArgumentPowers(rnd.Next(2, 5));
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

                        switch (rnd.Next(7))
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
                                //case 6:
                                //    if(rnd.Next(1, 16) == 1)
                                //    {
                                //        return new ArgModIntersect(rnd);
                                //    }
                                //    else
                                //    {
                                //        return new ArgModIntersect(rnd);
                                //    }
                                //default:
                                //    break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //RegEx Patterns
        const string fracPat = "-?([1-9][0-9]*)(/[1-9][0-9]*)?|0";
        const string fracompPat = "("+fracPat+ "(\\+|-)([1-9][0-9]*)i(/[1-9][0-9]*)?)|([1-9][0-9]*)i?(/[1-9][0-9]*)?";
        const string realPat = "-?[1-9][0-9]*(\\.[0-9]*[1-9])?|-?0\\.[0-9]*[1-9]|0";
        const string complexPat = "(-?[1-9][0-9]*(\\.[0-9]*[1-9])?(\\+|-)[1-9][0-9]*(\\.[0-9]*[1-9])?i)|(-?[1-9][0-9]*(\\.[0-9]*[1-9])?i?)|-?i|0";
        const string modArgPat = "[1-9][0-9]\\(cos\\("+realPat+"\\)\\+isin\\("+realPat+"\\)\\)";
        const string quadraticPat = "z²(\\+|-)[1-9][0-9]*z(\\+|-)[1-9][0-9]*";
        const string argPat = "arg\\(z+?"+complexPat+ "\\)=[1-9]?π/[1-9]";
        const string modPat = "\\|z+?"+complexPat+"|=[1-9]";
        const string linePat = "(y=[2-9]?x(/[2-9])?((\\+|-)" + fracPat + "))?|(y=" + fracPat + ")|(x="+ fracPat + ")";
        const string circlePat = "((\\(x(\\+|-)[1-9]\\)²)|(x²))\\+((\\(y(\\+|-)[1-9]\\)²)|(y²))=[1-9][0-9]*";

        
        //Checks the user input with RegEx patterns defined above to see if its a answer that is allowed.
        //Checks what question type it is, then with the corresponding RegEx Pattern
        static bool ValidateInput(string answer, IQuestion question)
        {
            if (question.GetType() == typeof(Multiply2Complex)
                || question.GetType() == typeof(MultiAlg)
                || question.GetType() == typeof(ArgIntersect)
                || question.GetType() == typeof(ModArgToNormal)
                || question.GetType() == typeof(ArgModIntersect))
            {
                if (Regex.IsMatch(answer, complexPat)) return true;
            }
            else if (question.GetType() == typeof(ModulusQuestion)
                || question.GetType() == typeof(ModulusPowers)
                || question.GetType() == typeof(ArgumentQuestion)
                || question.GetType() == typeof(ArgumentPowers))
            {
                if (Regex.IsMatch(answer, realPat)) return true;
            }
            else if (question.GetType() == typeof(Divide2Complex)
                || question.GetType() == typeof(DivAlg))
            {
                if (Regex.IsMatch(answer, fracompPat)) return true;
            }
            else if (question.GetType() == typeof(ModulusArgumentForm))
            {
                if (Regex.IsMatch(answer, modArgPat)) return true;
            }
            else if (question.GetType() == typeof(GivenRootFindQuadratic))
            {
                if (Regex.IsMatch(answer, quadraticPat)) return true;
            }
            else if (question.GetType() == typeof(ArgGraph))
            {
                if (Regex.IsMatch(answer, argPat)) return true;
            }
            else if (question.GetType() == typeof(ModGraph))
            {
                if (Regex.IsMatch(answer, modPat)) return true;
            }
            else if (question.GetType() == typeof(ArgtoCartesian)
                || question.GetType() == typeof(ModLine))
            {
                if(Regex.IsMatch(answer ,linePat)) return true;
            }
            else if (question.GetType() == typeof(ModToCartesian))
            {
                if (Regex.IsMatch(answer, circlePat)) return true;
            }
            else if (question.GetType() == typeof(Quadratic)
                || question.GetType() == typeof(Cubic1rootgiven)
                || question.GetType() == typeof(ZSquared))
            {
                if (Regex.IsMatch(answer, complexPat + "," + complexPat)) return true;
            }
            return false;
        }

        //prints the question to the user, and takes the answer, checks the answer + validates it.
        static void AskQuestion(IQuestion question, ref bool loop, ref int score)
        {
            Console.Clear();
            string ans = "";
            bool thisloop = true;
            Console.WriteLine(question.PrintQuestion());
            Console.WriteLine();
            Console.Write("Answer: ");
            ConsoleKeyInfo choice;
            int currentposition = Console.CursorTop, newposition = 0;
            try
            {
                question.LoadDiagram();
            }
            catch { }
            do
            {
                choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Enter)
                {
                    Console.CursorTop += 2;
                    Console.CursorLeft = 0;
                    bool isValid = ValidateInput(ans, question);
                    if (!isValid)
                    {
                        Console.WriteLine("Invalid input, please enter a correct input");
                        Console.CursorTop = currentposition;
                        Console.CursorLeft = 8;
                        Console.CursorTop = currentposition;
                        Console.CursorLeft = 8;
                        ans = "";

                    }
                    else
                    {
                        bool iscorrect = question.CheckAnswer(ans);
                        if (!iscorrect)
                        {
                            savequests.Add(question.SaveQuestion());
                        }
                        Console.WriteLine(new string(' ', ans.Length));
                        Console.WriteLine(question.PrintAnswer(question.CheckAnswer(ans)));
                        thisloop = false;
                    }
                }
                else if (choice.Key == ConsoleKey.Backspace && Console.CursorLeft > 8)
                {
                    if (ans.Length > 0)
                    {
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
                else if (choice.Key == ConsoleKey.LeftArrow && Console.CursorLeft > 8)
                {
                    if (ans.Length > 0)
                    {
                        Console.CursorLeft--;
                    }
                    else
                    {
                        Console.CursorLeft++;
                        Console.CursorLeft--;
                    }
                }
                else if (choice.Key == ConsoleKey.RightArrow && Console.CursorLeft < 8 + ans.Length)
                {
                    Console.CursorLeft++;
                }
                else if (choice.KeyChar == '^')
                {
                    bool doloop = true;
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
                            default:
                                doloop = false;
                                break;

                        }
                    } while (!Char.IsDigit(choice.KeyChar) && doloop);
                }
                else if (choice.Key == ConsoleKey.P)
                {
                    Console.Write("π");
                    ans += "π";
                }
                else if (Regex.IsMatch(choice.KeyChar.ToString(), "[0-9]|[-\\.,\\+=\\|\\(\\)/ ]|[A-Za-z]"))
                {
                    if (Console.CursorLeft == 8 + ans.Length)
                    {
                        Console.Write(choice.KeyChar);
                        ans += choice.KeyChar;
                    }
                    else
                    {
                        ans.Trim(' ');
                        int index = Console.CursorLeft - 8;
                        string firstpart = ans.Substring(0, index);
                        string secondpart = ans.Substring(index);
                        Console.Write(new string(' ', Math.Abs(ans.Length - index)));
                        Console.CursorLeft = 8 + index;
                        Console.Write($"{choice.KeyChar}{secondpart}");
                        ans = firstpart + choice.KeyChar + secondpart;
                        Console.CursorLeft = 9 + index;
                    }
                }
            } while (thisloop);
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

        //generates the quiz, by asking the user for amount of questions, and differnt types of questions

        static void CreateQuiz()
        {
            Console.Clear();
            int quizScore = 0, scoreForQuestion = 0;
            bool loop = false;
            IQuestion[] quizQuestions = new IQuestion[1];
            int[] questionSets = new int[1];
            bool[] questionWrong = new bool[1];
            int[] numberwrong = new int[5];
            List<int> setsInUse = new List<int>();
            do
            {
                loop = false;
                Console.WriteLine("How many Questions would you like in your quiz?");
                string choice = Console.ReadLine();
                Console.Clear();
                if (Regex.IsMatch(choice.Trim(), "[1-9][0-9]*"))
                {
                    int length = int.Parse(choice);
                    quizQuestions = new IQuestion[length];
                    questionSets = new int[length];
                    questionWrong = new bool[length];
                }
                else
                {
                    Console.WriteLine("Please enter a valid number");
                    loop = true;
                }
            } while (loop);
            Console.Clear();
            Console.WriteLine("Which Topics would you like?");
            Console.WriteLine("  Complex Number Multiplication");
            Console.WriteLine("  Complex Number Division");
            Console.WriteLine("  Modulus Argument Form");
            Console.WriteLine("  Finding Roots of a polynomial");
            Console.WriteLine("  Complex Loci");
            Console.WriteLine("  Continue");
            Console.CursorLeft = 1;
            Console.CursorTop = 1;
            bool exit = true;
            while (exit)
            {
                ConsoleKeyInfo choice = Console.ReadKey(true);
                int cursorpos = Console.CursorTop;
                if (choice.Key == ConsoleKey.Enter)
                {
                    switch (Console.CursorTop)
                    {
                        case 1:
                            if (!setsInUse.Contains(1)) setsInUse.Add(1);
                            else setsInUse.Remove(1);
                            cursorpos = 1;
                            break;
                        case 2:
                            if (!setsInUse.Contains(2)) setsInUse.Add(2);
                            else setsInUse.Remove(2);
                            cursorpos = 2;
                            break;
                        case 3:
                            if (!setsInUse.Contains(3)) setsInUse.Add(3);
                            else setsInUse.Remove(3);
                            cursorpos = 3;
                            break;
                        case 4:
                            if (!setsInUse.Contains(4)) setsInUse.Add(4);
                            else setsInUse.Remove(4);
                            cursorpos = 4;
                            break;
                        case 5:
                            if (!setsInUse.Contains(5)) setsInUse.Add(5);
                            else setsInUse.Remove(5);
                            cursorpos = 5;
                            break;
                        default:
                            exit = false;
                            break;
                    }
                    Console.Clear();
                    Console.WriteLine("Which Topics would you like?");
                    if(setsInUse.Contains(1)) Console.WriteLine("> Complex Number Multiplication");
                    else Console.WriteLine("  Complex Number Multiplication");
                    if (setsInUse.Contains(2)) Console.WriteLine("> Complex Number Division");
                    else Console.WriteLine("  Complex Number Division");
                    if (setsInUse.Contains(3)) Console.WriteLine("> Modulus Argument Form");
                    else Console.WriteLine("  Modulus Argument Form");
                    if (setsInUse.Contains(4)) Console.WriteLine("> Finding Roots of a polynomial");
                    else Console.WriteLine("  Finding Roots of a polynomial");
                    if (setsInUse.Contains(5)) Console.WriteLine("> Complex Loci");
                    else Console.WriteLine("  Complex Loci");
                    Console.WriteLine("  Continue");
                    Console.CursorLeft = 1;
                    Console.CursorTop = cursorpos;
                }
                else if (choice.Key == ConsoleKey.DownArrow && Console.CursorTop != 6)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop++;
                    Console.CursorLeft = 1;
                    
                }
                else if (choice.Key == ConsoleKey.UpArrow && Console.CursorTop != 1)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop--;
                    Console.CursorLeft = 1;
                }
            }

            for (int i = 0; i < quizQuestions.Length; i++)
            {
                questionSets[i] = setsInUse[rnd.Next(setsInUse.Count)]-1;
                quizQuestions[i] = GenQ(questionSets[i]);
                questionWrong[i] = true;
            }
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
    }
}