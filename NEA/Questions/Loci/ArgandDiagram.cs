using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NEA.Number_Classes;

namespace NEA.Questions.Loci
{
    public partial class ArgandDiagram : Form
    {
        public ArgandDiagram()
        {
            InitializeComponent();
            Diagram.Series.Clear();
            Diagram.Series.Add("Real");
            Diagram.Series["Real"].BorderWidth = 2;
            Diagram.Series["Real"].ChartType = SeriesChartType.Line;
            Diagram.Series["Real"].Color = Color.Black;
            Diagram.Series.Add("Imaginary");
            Diagram.Series["Imaginary"].BorderWidth = 2;
            Diagram.Series["Imaginary"].ChartType = SeriesChartType.Line; 
            Diagram.Series["Imaginary"].Color = Color.Black;
            for (int i = -10; i <= 10; i++)
            {
                Diagram.Series["Real"].Points.AddXY(i, 0);
                Diagram.Series["Imaginary"].Points.AddXY(0, i);
            }
        }

        public void CreateLine(double step, Complex operand, bool isleft)
        {
            Diagram.Series.Add("line");
            Diagram.Series["line"].BorderWidth = 2;
            Diagram.Series["line"].Color = Color.Red;
            Diagram.Series["line"].ChartType = SeriesChartType.Line;
            Diagram.Series["line"].Points.AddXY(operand.GetRealValue(), operand.GetImaginaryValue());
            double real = operand.GetRealValue();
            double imag = operand.GetImaginaryValue();
            while (real >= -10 && real <= 10 && imag <= 10 && imag >= -10)
            {
                imag += step;
                if (isleft) real--;
                else real++;
                Diagram.Series["line"].Points.AddXY(real, imag);
            }
        }

        public void CreateCircle(Complex operand, int modulus)
        {
            Diagram.Series.Add("circle");
            Diagram.Series["circle"].BorderWidth = 1;
            Diagram.Series["circle"].Color = Color.Red;
            Diagram.Series["circle"].ChartType = SeriesChartType.Line;
            Diagram.Series["circle"].Points.Clear();
            double realstart = modulus;
            double imagstart = 0;
            Diagram.Series["circle"].Points.AddXY(operand.GetRealValue() + realstart, operand.GetImaginaryValue() + imagstart);
            for (int i = 0; i < 360; i++)
            {
                realstart = modulus * Math.Cos(i);
                imagstart = modulus * Math.Sin(i);
                Diagram.Series["circle"].Points.AddXY(operand.GetRealValue() + realstart, operand.GetImaginaryValue() + imagstart);
            }
        }

        public void CreateModLine(Complex operand1, Complex operand2)
        {
            Diagram.Series.Add("line");
            Diagram.Series["line"].BorderWidth = 2;
            Diagram.Series["line"].Color = Color.Red;
            Diagram.Series["line"].ChartType = SeriesChartType.Line;
            double gradient;
            (double, double) midpoint = ((operand1.GetRealValue() + operand2.GetRealValue()) / 2, (operand1.GetImaginaryValue() + operand2.GetImaginaryValue()) / 2);
            if (operand1.GetImaginaryValue() - operand2.GetImaginaryValue() == 0)
            {
                Diagram.Series["line"].Points.AddXY(-10, midpoint.Item2);
                Diagram.Series["line"].Points.AddXY(10, midpoint.Item2);
            }
            else if (operand1.GetRealValue() - operand2.GetRealValue() == 0)
            {
                Diagram.Series["line"].Points.AddXY(midpoint.Item1, -10);
                Diagram.Series["line"].Points.AddXY(midpoint.Item1, 10);
            }
            else
            {
                gradient = -1 / (operand1.GetImaginaryValue() - operand2.GetImaginaryValue()) / (operand1.GetRealValue() - operand2.GetRealValue());
                while (midpoint.Item1 >= -10 && midpoint.Item2 <= 10 && midpoint.Item2 >= -10)
                {
                    midpoint.Item1--;
                    midpoint.Item2 -= gradient;
                    Diagram.Series["line"].Points.AddXY(midpoint.Item1, midpoint.Item2);
                }
                while (midpoint.Item1 <= 10 && midpoint.Item2 <= 10 && midpoint.Item2 >= -10)
                {
                    midpoint.Item1++;
                    midpoint.Item2 += gradient;
                    Diagram.Series["line"].Points.AddXY(midpoint.Item1, midpoint.Item2);
                }
            }
        }
    }
}
