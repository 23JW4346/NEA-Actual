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
        Color[] colours = { Color.Red, Color.Blue, Color.Yellow, Color.Green };
        //draws the real and imaginary axis on the windows form
        public ArgandDiagram()
        {
            Console.OutputEncoding = Encoding.Unicode;
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
        public void CreateLine(double step, Complex operand, bool isleft, string linename)
        {
            Diagram.Series.Add(linename);
            Diagram.Series[linename].BorderWidth = 2;
            Diagram.Series[linename].Color = colours[Diagram.Series.Count()-3];
            Diagram.Series[linename].ChartType = SeriesChartType.Line;
            Diagram.Series[linename].Points.AddXY(operand.GetRealValue(), operand.GetImaginaryValue());
            double real = operand.GetRealValue();
            double imag = operand.GetImaginaryValue();
            for(int i = 0; i < 20;i++)
            {
                imag += step;
                if (isleft) real--;
                else real++;
            }
            Diagram.Series[linename].Points.AddXY(real, imag);
        }

        //draws a circles on windows form for an argand diagram
        public void CreateCircle(Complex operand, int modulus, string linename)
        {
            Diagram.Series.Add(linename);
            Diagram.Series[linename].BorderWidth = 1;
            Diagram.Series[linename].Color = colours[Diagram.Series.Count() - 3];
            Diagram.Series[linename].ChartType = SeriesChartType.Line;
            Diagram.Series[linename].Points.Clear();
            double realstart = modulus;
            double imagstart = 0;
            Diagram.Series[linename].Points.AddXY(operand.GetRealValue() + realstart, operand.GetImaginaryValue() + imagstart);
            for (int i = 0; i < 360; i++)
            {
                realstart = modulus * Math.Cos(i);
                imagstart = modulus * Math.Sin(i);
                Diagram.Series[linename].Points.AddXY(operand.GetRealValue() + realstart, operand.GetImaginaryValue() + imagstart);
            }
        }


        //draws a full line on windows form for an argand diagram
        public void CreateModLine(Complex midpoint, double gradient, string linename)
        {
            Diagram.Series.Add(linename);
            Diagram.Series[linename].BorderWidth = 2;
            Diagram.Series[linename].Color = colours[Diagram.Series.Count() - 3];
            Diagram.Series[linename].ChartType = SeriesChartType.Line;
            double x = midpoint.GetRealValue(), y = midpoint.GetImaginaryValue();
            for(int i = 0; i < 20; i++)
            {
                x--;
                y -= gradient;
            }
            Diagram.Series[linename].Points.AddXY(x, y);
            x = midpoint.GetRealValue();
            y = midpoint.GetImaginaryValue();
            for(int i = 0; i < 40; i++)
            {
                x++;
                y += gradient;
            }
            Diagram.Series[linename].Points.AddXY(x, y);
        }
    }
}
