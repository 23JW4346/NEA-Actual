using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NEA.Number_Classes;

namespace NEA.Questions.MultiDivide
{
    public class MultiAlg : IQuestion
    {
        private int algA, algB;

        private int[] known;

        private string answer;

        public MultiAlg()
        {
            known = new int[4];
            known[0] = (int)new Number().GetValue();
            known[1] = (int)new Number().GetValue();
            known[2] = (int)new Number().GetValue();
            algA = (int)new Number().GetValue();
            Calculate();
        }

        public MultiAlg(string filename)
        {
            if (GetQuestion(filename)) Calculate();
            else
            {
                known = new int[4];
                known[0] = (int)new Number().GetValue();
                known[1] = (int)new Number().GetValue();
                known[2] = (int)new Number().GetValue();
                algA = (int)new Number().GetValue();
                Calculate();
            }
        }

        public void Calculate()
        {
            Complex operand1 = new Complex(algA, known[0]);
            Complex operand2 = new Complex(known[1], known[2]);
            algB = (int)(operand1.GetRealValue() * operand2.GetRealValue() - operand1.GetImaginaryValue() * operand2.GetImaginaryValue());
            known[3] = (int)(operand1.GetRealValue() * operand2.GetImaginaryValue() + operand1.GetImaginaryValue() * operand2.GetRealValue());
        }

        public bool CheckAnswer(string answer)
        {
            this.answer = algA + "," + algB;
            if (answer == this.answer)
            {
                return true;
            }
            SaveQuestion("Questions.txt");
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
                    if (line == "MultiAlg" && !found)
                    {
                        algA = int.Parse(sr.ReadLine());
                        algB = int.Parse(sr.ReadLine());
                        for(int i = 0; i < known.Length; i++)
                        {
                            known[i] = int.Parse(sr.ReadLine());
                        }

                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                    found = true;
                }
                sr.Close(); 
                sw.Close();
            }
            File.Delete(filename);
            File.Move(tempfile, filename);
            return found;
        }

        public void LoadDiagram()
        {
            throw new NotImplementedException();
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
            string complex1 = "a" + (known[0] < 0 ? "" : "+") + known[0];
            Complex complex2 = new Complex(known[1], known[2]);
            string answer = "b" + (known[3] < 0 ? "" : "+") + known[3];
            return $"Find real numbers a and b such that ({complex1}i)({complex2.GetComplex()}) = {answer}i \nPlease seperate a and b with a comma, no spaces.";
        }

        public void SaveQuestion(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine();
                sw.WriteLine("MultiAlg");
                sw.WriteLine(algA);
                sw.WriteLine(algB);
                foreach(int i in known)
                {
                    sw.WriteLine(i);
                }
            }
        }
    }
}
