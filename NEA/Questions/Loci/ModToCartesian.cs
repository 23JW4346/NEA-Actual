using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;
using System.Windows.Forms;
using System.IO;
using System.CodeDom.Compiler;

namespace NEA.Questions.Loci
{
    public class ModToCartesian : IQuestion
    {
        private Complex operand;
        private string loci;
        private string answer;
        private int modulus;
        private ArgandDiagram diagram;

        public ModToCartesian(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while(operand.GetComplex() == "") operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            modulus = rnd.Next(1, 6);
            loci = Program.CreateModCircle(temp, modulus);
            Calculate();
        }

        public ModToCartesian(Random rnd, string filename)
        {
            if (GetQuestion(filename))
            {
                Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                loci = Program.CreateModCircle(temp, modulus);
            }
            else
            {
                operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                while (operand.GetComplex() == "") operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                modulus = rnd.Next(1, 6);
                loci = Program.CreateModCircle(temp, modulus);
            }
            Calculate();
        }

        public void Calculate()
        {
            Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            answer = Program.CreateCartesianCircle(temp, modulus * modulus);
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == this.answer)
            {
                return true;
            }

            return false;
        }

        public bool GetQuestion(string filename)
        {
            bool found = false;
            string tempfile = Path.GetTempFileName();
            using (StreamReader sr = new StreamReader(filename))
            using (StreamWriter sw = new StreamWriter(tempfile))
            {
                while (!sr.EndOfStream)
                {
                    string line;
                    line = sr.ReadLine();
                    if (line == "ModToCart" && !found)
                    {
                        operand = new Complex(sr.ReadLine());
                        modulus = int.Parse(sr.ReadLine());
                        found = true;
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
                sr.Close();
                sw.Close();
            }
            File.Delete(filename);
            File.Move(tempfile, filename);
            return found;
        }

        public string Hint()
        {
            throw new NotImplementedException();
        }

        public void LoadDiagram()
        {
            diagram = new ArgandDiagram();
            diagram.CreateCircle(operand, modulus);
            Task.Run(() => Application.Run(diagram));
        }

        public string PrintAnswer(bool correct)
        {
            if (correct)
            {
                return $"Correct!\nThe Answer is {answer}";
            }
            return $"Incorrect!\nThe Answer was {answer}";
        }

        public string PrintQuestion()
        {
            return $"Write This Loci {loci} in Cartesian Form (as a circle)";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModToCart",
                operand.GetComplex(),
                modulus.ToString()
            };
        }
    }
}
