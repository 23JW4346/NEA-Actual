using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NEA.Number_Classes;

namespace NEA.Questions.Loci
{
    public partial class ArgumentGraph : Form
    {
        public ArgumentGraph(double step, Complex operand, bool isleft)
        {
            InitializeComponent();
            for (int i = -10; i <= 10; i++)
            {
                Diagram.Series["Real"].Points.AddXY(i, 0);
                Diagram.Series["Imaginary"].Points.AddXY(0, i);
            }
            Diagram.Series["line"].Points.Clear();
            Diagram.Series["line"].Points.AddXY(operand.GetRealValue(), operand.GetImaginaryValue());
            double real = operand.GetRealValue();
            double imag = operand.GetImaginaryValue();
            while(real >= -10 && real <= 10 && imag <= 10 && imag >= -10) 
            {
                if (isleft)
                {
                    imag += step;
                    real++;
                }
                else
                {
                    real--
                        ;
                    imag -= step;
                }
                Diagram.Series["line"].Points.AddXY(real, imag);
            }
            
        }
    }
}
