using NEA.Number_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA.Questions.Loci
{
    public class ModGraph : IQuestion
    {

        private Complex operand;
        private string loci;
        private int modulus;
        private ArgandDiagram diagram;

        public ModGraph(Random rnd)
        {
            operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            while (operand.GetComplex() == "") operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
            Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
            modulus = rnd.Next(1, 6);
            if (temp.GetComplex()[0] == '-')
            {
                loci = $"|z{temp.GetComplex()}|={modulus}";
            }
            else
            {
                loci = $"|z+{temp.GetComplex()}|={modulus}";
            }
        }

        public ModGraph(Random rnd, string filename)
        {
            if (GetQuestion(filename))
            {
                Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                if (temp.GetComplex()[0] == '-')
                {
                    loci = $"|z{temp.GetComplex()}|={modulus}";
                }
                else
                {
                    loci = $"|z+{temp.GetComplex()}|={modulus}";
                }
            }
            else
            {
                operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                while (operand.GetComplex() == "") operand = new Complex(rnd.Next(-3, 4), rnd.Next(-3, 4));
                Complex temp = new Complex(-operand.GetRealValue(), -operand.GetImaginaryValue());
                modulus = rnd.Next(1, 6);
                if (temp.GetComplex()[0] == '-')
                {
                    loci = $"|z{temp.GetComplex()}|={modulus}";
                }
                else
                {
                    loci = $"|z+{temp.GetComplex()}|={modulus}";
                }
            }
        }
        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public bool CheckAnswer(string answer)
        {
            if (answer == loci)
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
                return $"Correct!\nThe Answer is {loci}";
            }
            return $"Incorrect!\nThe Answer was {loci}";
        }

        public string PrintQuestion()
        {
            return $"What is the equation for this Complex loci?";
        }

        public List<string> SaveQuestion()
        {
            return new List<string>
            {
                "ModGraph",
                operand.GetComplex(),
                modulus.ToString()
            };
        }
    }
}
