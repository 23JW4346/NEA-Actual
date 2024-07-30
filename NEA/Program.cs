using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NEA.Number_Classes;

namespace NEA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Complex c = new Complex();
            Console.WriteLine(c.GetModulus().GetString());
            Console.ReadKey();
        }
    }
}
