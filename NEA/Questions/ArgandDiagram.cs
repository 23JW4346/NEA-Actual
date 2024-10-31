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

        private int instance = 1;

        //draws the real and imaginary axis on the windows form
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

        //draws a half line on windows forms
        public void CreateLine(double step, Complex operand, bool isleft)
        {
            Diagram.Series.Add("line " + instance);
            Diagram.Series["line " + instance].BorderWidth = 2;
            Diagram.Series["line " + instance].Color = Color.Red;
            Diagram.Series["line " + instance].ChartType = SeriesChartType.Line;
            Diagram.Series["line " + instance].Points.AddXY(operand.GetRealValue(), operand.GetImaginaryValue());
            double real = operand.GetRealValue();
            double imag = operand.GetImaginaryValue();
            for(int i = 0; i < 20;i++)
            {
                imag += step;
                if (isleft) real--;
                else real++;
                Diagram.Series["line " + instance].Points.AddXY(real, imag);
            }
            instance++;
        }

        //draws a circles on windows form for an argand diagram
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


        //draws a full line on windows form for an argand diagram
        public void CreateModLine(Complex midpoint, double gradient)
        {
            Diagram.Series.Add("line " + instance);
            Diagram.Series["line " + instance   ].BorderWidth = 2;
            Diagram.Series["line " + instance].Color = Color.Red;
            Diagram.Series["line " + instance].ChartType = SeriesChartType.Line;
            double x = midpoint.GetRealValue(), y = midpoint.GetImaginaryValue();
            bool isleft = false;
            if (gradient < 0) isleft = true;
            for(int i = 0; i < 20; i++)
            {
                if (isleft) x++;
                else x--;
                y -= gradient;
            }
            Diagram.Series["line " + instance].Points.AddXY(x, y);
            x = midpoint.GetRealValue();
            y = midpoint.GetImaginaryValue();
            for(int i = 0; i < 40; i++)
            {
                if (isleft) x--;
                else x++;
                y += gradient;
            }
            Diagram.Series["line " + instance].Points.AddXY(x, y);
            instance++;
        }
    }
}
