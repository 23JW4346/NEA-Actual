using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Number_Classes
{
    //Made the number class so i could use it as a parent class for other types of Numbers (Fraction, Surd, etc)
    public class Number
    {
        private double value;
        protected bool isnegative = false;
        protected static Random rnd = new Random();

        public Number()
        {
            while (value == 0) value = rnd.Next(-10, 11);
            if (value < 0) isnegative = true;
        }

        public Number(double invalue)
        {
            value = invalue;
            if (value < 0) isnegative = true;
        }

        public virtual double GetTop() => throw new NotImplementedException();

        public virtual double GetBottom() => throw new NotImplementedException();


        public virtual string GetString(bool isimag)
        {
            if (isimag)
            {
                if (value == 1) return "i";
                else if (value == -1) return "-i";
                else if (value != 0) return value.ToString() + "i";
                
            }
            else
            {
                if (value != 0) return value.ToString();
            }
            return "0";
        }

        public virtual double GetValue() => value;
        
        public bool GetNegative() => isnegative;
    }
}
