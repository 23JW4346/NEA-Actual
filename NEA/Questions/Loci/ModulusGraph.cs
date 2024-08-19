using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NEA.Number_Classes;

namespace NEA.Questions.Loci
{
    public partial class ModulusGraph : Form
    {
        public ModulusGraph(Complex operand, int modulus)
        {
            InitializeComponent();
            for (int i = -10; i <= 10; i++)
            {
                Diagram.Series["Real"].Points.AddXY(i, 0);
                Diagram.Series["Imaginary"].Points.AddXY(0, i);
            }
            Diagram.Series["Centre"].Points.Clear();
            Diagram.Series["Centre"].Points.AddXY(operand.GetRealValue(), operand.GetImaginaryValue());
            Diagram.Series["Circle"].Points.Clear();
            double realstart = modulus;
            double imagstart = 0;
            Diagram.Series["Circle"].Points.AddXY(operand.GetRealValue() + realstart, operand.GetImaginaryValue() + imagstart);
            for (int i = 0; i < 360; i++) 
            {
                realstart = modulus * Math.Cos(i);
                imagstart = modulus * Math.Sin(i);
                Diagram.Series["Circle"].Points.AddXY(operand.GetRealValue() + realstart, operand.GetImaginaryValue() + imagstart);
            }

        }
    }
}
