using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NEA.Number_Classes;
using NEA.Questions;
using NEA.Questions.Complex_Basics;
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
        static ArgandDiagram diagram;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            IQuestion q = new ArgGraph(rnd);
            diagram = new ArgandDiagram();
            q.LoadDiagram(diagram);
            System.Threading.Thread.Sleep(1000);
            q.CloseDiagram(diagram);
            Console.Clear();
            Test();
            Console.ReadKey();
            Menu();
            //At the end when the user clicks exit, it saves all the questions that the user got wrong into a text file.
            using (StreamWriter sw = new StreamWriter("Questions.txt", true))
            {
                foreach (List<string> strings in savequests)
                {
                    foreach (string s in strings) sw.WriteLine(s);
                }
                sw.Close();
            }
        }

        //returns the string for an arg line in an argand diagram (arg(z-(z1)=θ)
        public static string CreateArgLine(Complex a, Number b)
        {
            string loci;
            string comp = a.GetComplex();
            if (comp != "0")
            {
                if (a.GetComplex()[0] == '-')
                {
                    if (b.GetType() == typeof(Fraction)) loci = $"arg(z{comp})={b.GetString(true).Replace('i', 'π')}";
                    else loci = $"arg(z{comp})={Math.Round(b.GetValue(), 3)}";
                }
                else
                {
                    if (b.GetType() == typeof(Fraction)) loci = $"arg(z+{comp})={b.GetString(true).Replace('i', 'π')}";
                    else loci = $"arg(z+{comp})={Math.Round(b.GetValue(), 3)}";
                }
            }
            else
            {
                if (b.GetType() == typeof(Fraction)) loci = $"arg(z)={b.GetString(true).Replace('i', 'π')}";
                else loci = $"arg(z)={Math.Round(b.GetValue(), 3)}";
            }
            return loci;
        }
        //returns the Complex Loci string for a circle (|z-z1|=r)
        public static string CreateModCircle(Complex a, string modulus)
        {
            string loci;
            if (a.GetComplex() == "") loci = $"|z|={modulus}";
            else if (a.GetComplex()[0] == '-') loci = $"|z{a.GetComplex()}|={modulus}";
            else loci = $"|z+{a.GetComplex()}|={modulus}";
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
            if (grad.ToString().Contains('.'))
            {
                switch (Math.Abs(grad).ToString()[2])
                {
                    case '5':
                        answer += "y=" + new Fraction((int)(grad * 2), 2).GetString(true).Replace('i', 'x');
                        break;
                    case '3':
                    case '6':
                        answer += "y=" + new Fraction((int)(grad * 3), 3).GetString(true).Replace('i', 'x');
                        break;
                    case '2':
                        if (xint.ToString().Length == 3) answer += "y=" + new Fraction((int)(grad * 5), 5).GetString(true).Replace('i', 'x');
                        else answer += "y=" + new Fraction((int)(grad * 4), 4).GetString(true).Replace('i', 'x');
                        break;
                }
            }
            else if (grad == 1) answer += "y=x";
            else if (grad == -1) answer += "y=-x";
            else if (grad > 4) answer = $"x={xint}";
            else if (grad == 0)
            {
                answer = $"y={yint}";
                return answer;
            }
            else answer += "y=" + grad + "x";
            if (yint != 0)
            {
                string yint2 = "";
                if (yint.ToString().Contains('.'))
                {
                    switch (Math.Abs(yint).ToString()[2])
                    {
                        case '5':
                            yint2 += new Fraction((int)(yint * 2), 2).GetString(false);
                            break;
                        case '3':
                        case '6':
                            yint2 += new Fraction((int)(yint * 3), 3).GetString(false);
                            break;
                        case '2':
                            if (xint.ToString().Length == 3) yint2 += new Fraction((int)(yint * 5), 5).GetString(false);
                            else yint2 += new Fraction((int)(yint * 4), 4).GetString(false);
                            break;
                    }
                }
                else yint2 = yint.ToString();
                if (yint < 0) answer += yint2;
                else answer += "+" + yint2;
            }
            return answer;
        }

        //returns the string for a modline loci (|z-z1|=|z-z2|)
        public static string CreateModLine(Complex a, Complex b)
        {
            string loci = "|z";
            if (a.GetComplex() != "")
            {
                if (a.GetComplex()[0] == '-') loci += a.GetComplex();
                else loci += "+" + a.GetComplex();
            }
            loci += "|=|z";
            if (b.GetComplex() != "")
            {
                if (b.GetComplex()[0] == '-') loci += b.GetComplex();
                else loci += "+" + b.GetComplex();
            }
            return loci + "|";
        }

        //Gets user option with the arrow keys
        static int GetUserOption(int smallest, int largest, bool keeparrow)
        {
            bool exit = true;
            int topPosition = smallest;
            int newPosition = Console.CursorTop - smallest;
            while (exit)
            {
                ConsoleKeyInfo choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Enter)
                {
                    exit = false;
                }
                else if (choice.Key == ConsoleKey.DownArrow && newPosition < largest - smallest)
                {
                    if (keeparrow)
                    {
                        Console.CursorLeft = 0;
                        newPosition++;
                        Console.CursorTop = topPosition + newPosition;
                        Console.CursorLeft = 1;
                    }
                    else
                    {
                        Console.CursorLeft = 0;
                        Console.Write(" ");
                        newPosition++;
                        Console.CursorTop = topPosition + newPosition;
                        Console.CursorLeft = 0;
                        Console.Write(">");
                    }
                }
                else if (choice.Key == ConsoleKey.UpArrow && newPosition != 0)
                {
                    if (keeparrow)
                    {
                        Console.CursorLeft = 0;
                        newPosition--;
                        Console.CursorTop = topPosition + newPosition;
                        Console.CursorLeft = 1;
                    }
                    else
                    {
                        Console.CursorLeft = 0;
                        Console.Write(" ");
                        newPosition--;
                        Console.CursorTop = topPosition + newPosition;
                        Console.CursorLeft = 0;
                        Console.Write(">");
                    }
                }
            }
            return newPosition;
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
                Console.WriteLine("  Special characters");
                Console.WriteLine("  Exit");
                Console.CursorLeft = 1;
                Console.CursorTop = 1;
                bool exit = true;
                while (exit)
                {
                    int choice = GetUserOption(1, 4, false);
                    switch (choice + 1)
                    {
                        case 1:
                            ChooseTopic();
                            break;
                        case 2:
                            CreateQuiz();
                            break;
                        case 3:
                            Settings();
                            break;
                        case 4:
                            exit = false;
                            break;
                    }
                    Console.Clear();
                    Console.WriteLine("Welcome to the Complex Number Revision Tool");
                    Console.WriteLine("> Choose Topic");
                    Console.WriteLine("  Create Quiz");
                    Console.WriteLine("  Special characters");
                    Console.WriteLine("  Exit");
                    Console.CursorLeft = 1;
                    Console.CursorTop = 1;
                }
                loop = false;
            }
        }
        //shows all the special characters in this program
        static void Settings()
        {
            Console.Clear();
            Console.WriteLine("Typing these characters into the program will output other characters");
            Console.WriteLine("heres the List of character maps:");
            Console.WriteLine("p -> π");
            Console.WriteLine("x^2 -> x², works for any power 2-4, and x is any usable character");
            Console.WriteLine("q -> √");
            Console.WriteLine("Press any key to go back to the main menu");
            Console.ReadKey();
            Console.Clear();
        }


        //Menu for choosing topic (still uses up and down arrow keys)
        static void ChooseTopic()
        {
            Console.Clear();
            Console.WriteLine("Which Topic:");
            Console.WriteLine("> Complex Number Basics");
            Console.WriteLine("  Complex Number Multiplication");
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
                int questionset = GetUserOption(1, 8, false);
                bool loop = true;
                int placeholder = 0;
                while (loop)
                {
                    switch (questionset + 1)
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
                            AskQuestion(GenQ(5), ref loop, ref placeholder);
                            break;
                        case 7:
                            AskQuestion(GenQ(rnd.Next(6)), ref loop, ref placeholder);
                            break;
                        default:
                            exit = false;
                            loop = false;
                            break;
                    }
                }
                Console.Clear();
                Console.WriteLine("Which Topic:");
                Console.WriteLine("> Complex Number Basics");
                Console.WriteLine("  Complex Number Multiplication");
                Console.WriteLine("  Complex Number Division");
                Console.WriteLine("  Modulus Argument Form");
                Console.WriteLine("  Finding Roots of a polynomial");
                Console.WriteLine("  Complex Loci");
                Console.WriteLine("  Random");
                Console.WriteLine("  Main Menu");
                Console.CursorLeft = 1;
                Console.CursorTop = 1;
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
                        switch (rnd.Next(2))
                        {
                            case 0:
                                return rnd.Next(1, 16) == 1 ? new Conjugate("Questions.txt") : new Conjugate();
                            case 1:
                                return rnd.Next(1, 16) == 1 ? new REorIMQuestion(rnd, "Questions.txt") : new REorIMQuestion(rnd);
                        }
                        break;
                    case 1:
                        switch (rnd.Next(3))
                        {
                            case 0:
                                return rnd.Next(1, 16) == 1 ? new Multiply2Complex("Questions.txt") : new Multiply2Complex();
                            case 1:
                                return rnd.Next(1, 16) == 1 ? new MultiAlg("Questions.txt") : new MultiAlg();
                            case 2:
                                return rnd.Next(1, 16) == 1 ? new ZSquared(rnd) : new ZSquared("Questions.txt", rnd);
                        }
                        break;
                    case 2:
                        switch (rnd.Next(2))
                        {
                            case 0:
                                return rnd.Next(1, 16) == 1 ? new Divide2Complex("Questions.txt") : new Divide2Complex();
                            case 1:
                                return rnd.Next(1, 16) == 1 ? new DivAlg("Questions.txt") : new DivAlg();
                        }
                        break;
                    case 3:
                        switch (rnd.Next(6))
                        {
                            case 0:
                                return rnd.Next(1, 16) == 1 ? new ModulusQuestion("Questions.txt") : new ModulusQuestion();
                            case 1:
                                return rnd.Next(1, 16) == 1 ? new ArgumentQuestion("Questions.txt") : new ArgumentQuestion();
                            case 2:
                                return rnd.Next(1, 16) == 1 ? new ModulusArgumentForm("Questions.txt") : new ModulusArgumentForm();
                            case 3:
                                return rnd.Next(1, 16) == 1 ? new ModArgToNormal("Questions.txt") : new ModArgToNormal();
                            case 4:
                                return rnd.Next(1, 16) == 1 ? new ModulusPowers(rnd, "Questions.txt") : new ModulusPowers(rnd);
                            case 5:
                                return rnd.Next(1, 16) == 1 ? new ArgumentPowers(rnd, "Questions.txt") : new ArgumentPowers(rnd);
                        }
                        break;
                    case 4:
                        switch (rnd.Next(4))
                        {
                            case 0:
                                return rnd.Next(1, 16) == 1 ? new Quadratic(rnd, "Questions.txt") : new Quadratic(rnd);
                            case 1:
                                return rnd.Next(1, 16) == 1 ? new Cubic1rootgiven(rnd, "Questions.txt") : new Cubic1rootgiven(rnd);
                            case 2:
                                return rnd.Next(1, 16) == 1 ? new GivenRootFindQuadratic("Questions.txt") : new GivenRootFindQuadratic();
                            case 3:
                                return rnd.Next(1, 16) == 1 ? new GivenRootsFindCubic(rnd, "Questions.txt") : new GivenRootsFindCubic(rnd);
                        }
                        break;
                    case 5:

                        switch (rnd.Next(7))
                        {
                            case 0:
                                return rnd.Next(1, 16) == 1 ? new ArgtoCartesian("Questions.txt", rnd) : new ArgtoCartesian(rnd);
                            case 1:
                                return rnd.Next(1, 16) == 1 ? new ArgGraph("Questions.txt", rnd) : new ArgGraph(rnd);
                            case 2:
                                return rnd.Next(1, 16) == 1 ? new ModToCartesian(rnd, "Questions.txt") : new ModToCartesian(rnd);
                            case 3:
                                return rnd.Next(1, 16) == 1 ? new ModGraph(rnd, "Questions.txt") : new ModGraph(rnd);
                            case 4:
                                return rnd.Next(1, 16) == 1 ? new ModLine("Questions.txt", rnd) : new ModLine(rnd);
                            case 5:
                                return rnd.Next(1, 16) == 1 ? new ArgIntersect(rnd, "Questions.txt") : new ArgIntersect(rnd);
                            case 6:
                                return rnd.Next(1, 16) == 1 ? new ArgModIntersect(rnd) : new ArgModIntersect(rnd);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //RegEx Patterns
        const string fracompPat = @"((-?([1-9]\d*)(\/[1-9]\d*)?|0)[-\+]([1-9]\d*)?i(\/[1-9]\d*)?)|0";
        const string realPat = @"-?[1-9]\d*(\.\d*[1-9])?|-?0\.\d*[1-9]|0";
        const string complexPat = @"(-?[1-9]\d*(\.\d*[1-9])?[-\+]([1-9]\d*(\.\d*[1-9])?)?i)|(-?[1-9]\d*(\.\d*[1-9])?i?)|(-?i)|0";
        const string modArgPat = @"[1-9]\d*\(cos\((-?[0-3](\.\d*[1-9])?|0)\)\+isin\((-?[0-3](\.\d*[1-9])?|0)\)\)";
        const string quadraticPat = @"z²[-\+]([1-9]\d*)?z[-\+]([1-9]\d*)?=0";
        const string cubicPat = @"z³[-\+]([1-9]\d*)?z²[-\+]([1-9]\d*)?z[-\+]([1-9]\d*)?=0";
        const string argPat = @"arg\(z[-\+]?((-?[1-9]\d*(\.\d*[1-9])?[-\+]([1-9]\d*(\.\d*[1-9])?)?i)|(-?[1-9]\d*(\.\d*[1-9])?i?)|(-?i)|0)?\)=-?[1-9]?π(\/[1-9])?";
        const string modPat = @"\|z[-\+]?((-?[1-9]\d*(\.\d*[1-9])?[-\+]([1-9]\d*(\.\d*[1-9])?)?i)|(-?[1-9]\d*(\.\d*[1-9])?i?)|(-?i)|0)?\|=[1-9]";
        const string linePat = @"(y=-?[2-9]?x(\/[2-9])?([-\+](-?([1-9]\d*)(\/[1-9]\d*)?))?)|(y=((-?([1-9]\d*)(\/[1-9]\d*)?)|0))|(x=((-?([1-9]\d*)(\/[1-9]\d*)?)|0))";
        const string circlePat = @"((\(x[-\+][1-9]\)²)|(x²))\+((\(y[-\+][1-9]\)²)|(y²))=[1-9]\d*";

        //Checks the user input with RegEx patterns defined above to see if its a answer that is allowed.
        //Checks what question type it is, then with the corresponding RegEx Pattern
        static bool ValidateInput(string answer, IQuestion question)
        {
            Regex inputValid;
            if (question.GetType() == typeof(Multiply2Complex)
                || question.GetType() == typeof(ArgIntersect)
                || question.GetType() == typeof(ModArgToNormal)
                || question.GetType() == typeof(Conjugate))
            {
                inputValid = new Regex(complexPat);
            }
            else if (question.GetType() == typeof(ModulusQuestion)
                || question.GetType() == typeof(ModulusPowers)
                || question.GetType() == typeof(ArgumentQuestion)
                || question.GetType() == typeof(ArgumentPowers)
                || question.GetType() == typeof(REorIMQuestion))
            {
                inputValid = new Regex(realPat);
            }
            else if (question.GetType() == typeof(Divide2Complex)
                || question.GetType() == typeof(DivAlg))
            {
                inputValid = new Regex(fracompPat);
            }
            else if (question.GetType() == typeof(ModulusArgumentForm))
            {
                inputValid = new Regex(modArgPat);
            }
            else if (question.GetType() == typeof(GivenRootFindQuadratic))
            {
                inputValid = new Regex(quadraticPat);
            }
            else if (question.GetType() == typeof(ArgGraph))
            {
                inputValid = new Regex(argPat);
            }
            else if (question.GetType() == typeof(ModGraph))
            {
                inputValid = new Regex(modPat);
            }
            else if (question.GetType() == typeof(ArgtoCartesian)
                || question.GetType() == typeof(ModLine))
            {
                inputValid = new Regex(linePat);
            }
            else if (question.GetType() == typeof(ModToCartesian))
            {
                inputValid = new Regex(circlePat);
            }
            else if (question.GetType() == typeof(GivenRootsFindCubic))
            {
                inputValid = new Regex(cubicPat);
            }
            else
            {
                inputValid = new Regex("(" + complexPat + ")\\,(" + complexPat + ")");
            }
            return inputValid.IsMatch(answer);
        }

        //Gets the user input for the answer of the question
        static string GetUserAnswer()
        {
            string ans = "";
            ConsoleKeyInfo choice;
            do
            {
                choice = Console.ReadKey(true);
                if (choice.Key == ConsoleKey.Backspace && Console.CursorLeft < 8 + ans.Length & Console.CursorLeft >= 8)
                {
                    int index = Console.CursorLeft - 8;
                    string firstpart = ans.Substring(0, index - 1);
                    string secondpart = ans.Substring(index);
                    Console.CursorLeft = 8;
                    Console.Write(new string(' ', ans.Length + 1));
                    Console.CursorLeft = 8;
                    Console.Write(firstpart + secondpart);
                    ans = firstpart + secondpart;
                    Console.CursorLeft = 7 + index;
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
                else if (Regex.IsMatch(choice.KeyChar.ToString(), "[0-9]|[-\\.,\\+=\\|\\(\\)/ ]|[A-Za-z]"))
                {
                    if (Console.CursorLeft == 8 + ans.Length)
                    {
                        if (choice.Key == ConsoleKey.P)
                        {
                            Console.Write("π");
                            ans += "π";
                        }
                        else if (choice.Key == ConsoleKey.Q)
                        {
                            Console.Write("√");
                            ans += "√";
                        }
                        else
                        {
                            ans += choice.KeyChar;
                            Console.Write(choice.KeyChar);
                        }
                    }
                    else
                    {
                        ans.Trim(' ');
                        int index = Console.CursorLeft - 8;
                        string firstpart = ans.Substring(0, index);
                        string secondpart = ans.Substring(index);
                        char c;
                        if (choice.Key == ConsoleKey.P) c = 'π';
                        else if (choice.Key == ConsoleKey.Q) c = '√';
                        else c = choice.KeyChar;
                        Console.Write(new string(' ', Math.Abs(ans.Length - index)));
                        Console.CursorLeft = 8 + index;
                        Console.Write($"{c}{secondpart}");
                        ans = firstpart + c + secondpart;
                        Console.CursorLeft = 9 + index;
                    }
                }
            } while (choice.Key != ConsoleKey.Enter);
            return ans;
        }

        //prints the question to the user, and takes the answer, checks the answer + validates it.
        static void AskQuestion(IQuestion question, ref bool loop, ref int score)
        {
            Console.Clear();
            string ans = "";
            bool thisloop = true;
            Console.WriteLine(question.PrintQuestion());
            Console.WriteLine();
            int currentposition = Console.CursorTop, newposition = 0;
            try
            {
                diagram = new ArgandDiagram();
                question.LoadDiagram(diagram);
            }
            catch (NoDiagramException)
            {

            }
            Console.Write("Answer: ");
            while (thisloop)
            {
                ans = GetUserAnswer();
                Console.CursorTop += 2;
                Console.CursorLeft = 0;
                int length = ans.Length;
                if (ans.Contains(' '))
                {
                    string temp = "";
                    foreach (char c in ans)
                    {
                        if (c != ' ') temp += c;
                    }
                    ans = temp;

                }
                bool isValid = ValidateInput(ans, question);
                if (!isValid)
                {
                    Console.WriteLine("Invalid input, please enter a correct input");
                    Console.CursorTop = currentposition;
                    Console.CursorLeft = 8;
                    Console.Write(new string(' ', length));
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
                    Console.WriteLine(new string(' ', "Invalid input, please enter a correct input".Length));
                    Console.WriteLine(question.PrintAnswer(question.CheckAnswer(ans)));
                    thisloop = false;
                }
            }
            if (loop)
            {
                Console.WriteLine();
                Console.WriteLine("Would you like another question?");
                Console.WriteLine("> yes");
                Console.WriteLine("  no");
                Console.CursorLeft = 1;
                Console.CursorTop -= 2;
                currentposition = Console.CursorTop;
                newposition = GetUserOption(currentposition, currentposition + 1, false);
                if (newposition == 1)
                {
                    loop = false;

                }
                else
                {
                    loop = true;
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
            try
            {
                question.CloseDiagram(diagram);
            }
            catch (NoDiagramException)
            {
            }
            Console.Clear();
        }

        //generates the quiz, by asking the user for amount of questions, and differnt types of questions

        static void CreateQuiz()
        {
            Console.Clear();
            int quizScore = 0, scoreForQuestion = 0;
            IQuestion[] quizQuestions = new IQuestion[1];
            int[] questionSets = new int[1];
            bool[] questionWrong = new bool[1];
            int[] numberwrong = new int[6];
            List<int> setsInUse = new List<int>();
            bool loop;
            do
            {
                loop = false;
                Console.WriteLine("How many Questions would you like in your quiz? (5-50)");
                string choice = Console.ReadLine();
                Console.Clear();
                if (Regex.IsMatch(choice.Trim(), @"\A[1-9][0-9]?\z") && int.Parse(choice.Trim()) < 51 && int.Parse(choice.Trim()) > 4)
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
            Console.WriteLine("  Complex Number Basics");
            Console.WriteLine("  Complex Number Multiplication");
            Console.WriteLine("  Complex Number Division");
            Console.WriteLine("  Modulus Argument Form");
            Console.WriteLine("  Finding Roots of a polynomials");
            Console.WriteLine("  Complex Loci");
            Console.WriteLine("  Continue");
            Console.CursorLeft = 1;
            Console.CursorTop = 1;
            bool exit = true;
            while (exit)
            {
                int choice = GetUserOption(1, 7, true);
                Console.CursorLeft = 0;
                switch (choice)
                {
                    case 0:
                        if (!setsInUse.Contains(0))
                        {
                            setsInUse.Add(0);
                            Console.Write(">");
                        }
                        else
                        {
                            setsInUse.Remove(0);
                            Console.Write(" ");
                        }
                        break;
                    case 1:
                        if (!setsInUse.Contains(1))
                        {
                            setsInUse.Add(1);
                            Console.Write(">");
                        }
                        else
                        {
                            setsInUse.Remove(1);
                            Console.Write(" ");
                        }
                        Console.CursorLeft = 1;
                        break;
                    case 2:
                        if (!setsInUse.Contains(2))
                        {
                            setsInUse.Add(2);
                            Console.Write(">");
                        }
                        else
                        {
                            setsInUse.Remove(2);
                            Console.Write(" ");
                        }
                        Console.CursorLeft = 1;
                        break;
                    case 3:
                        if (!setsInUse.Contains(3))
                        {
                            setsInUse.Add(3);
                            Console.Write(">");
                        }
                        else
                        {
                            setsInUse.Remove(3);
                            Console.Write(" ");
                        }
                        Console.CursorLeft = 1;
                        break;
                    case 4:
                        if (!setsInUse.Contains(4))
                        {
                            setsInUse.Add(4);
                            Console.Write(">");
                        }
                        else
                        {
                            setsInUse.Remove(4);
                            Console.Write(" ");
                        }
                        Console.CursorLeft = 1;
                        break;
                    case 5:
                        if (!setsInUse.Contains(5))
                        {
                            setsInUse.Add(5);
                            Console.Write(">");
                        }
                        else
                        {
                            setsInUse.Remove(5);
                            Console.Write(" ");
                        }
                        Console.CursorLeft = 1;
                        break;
                    default:
                        if (setsInUse.Count != 0) exit = false;
                        break;
                }
            }

            for (int i = 0; i < quizQuestions.Length; i++)
            {
                questionSets[i] = setsInUse[rnd.Next(setsInUse.Count)];
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
                            case 0:
                                numberwrong[0]++;
                                break;
                            case 1:
                                numberwrong[1]++;
                                break;
                            case 2:
                                numberwrong[2]++;
                                break;
                            case 3:
                                numberwrong[3]++;
                                break;
                            case 4:
                                numberwrong[4]++;
                                break;
                            case 5:
                                numberwrong[5]++;
                                break;
                        }
                    }
                }
                Console.Write("due to your results, We suggest Revising: ");
                switch (Array.IndexOf(numberwrong, numberwrong.Max()))
                {
                    case 0:
                        Console.WriteLine("Complex number basics");
                        break;
                    case 1:
                        Console.WriteLine("Complex number multiplication");
                        break;
                    case 2:
                        Console.WriteLine("Complex number division");
                        break;
                    case 3:
                        Console.WriteLine("Modulus-Argument form");
                        break;
                    case 4:
                        Console.WriteLine("Roots of polynomials");
                        break;
                    case 5:
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