using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NEA.Number_Classes;

namespace NEA.Questions.Loci
{
    public partial class ArgandDiagram : Form
    {
        private Color[] colours = { Color.Red, Color.Blue, Color.Yellow, Color.Green };
        //draws the real and imaginary axis on the windows form
        public ArgandDiagram()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((1920-Size.Width)/2, (1080 - Size.Height)/2);
        }

        //draws a half line on windows forms
        public void CreateLine(double step, Complex operand, bool isleft, string linename)
        {
            Diagram.Series.Add(linename);
            Diagram.Series[linename].BorderWidth = 2;
            Diagram.Series[linename].Color = colours[Diagram.Series.Count()-3];
            Diagram.Series[linename].ChartType = SeriesChartType.Line;
            Diagram.Series[linename].ChartArea = "ChartArea1";
            Diagram.Series[linename].Points.Clear();
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
            Diagram.Series[linename].IsVisibleInLegend = true;
        }

        //draws a circles on windows form for an argand diagram
        public void CreateCircle(Complex operand, double modulus, string linename)
        {
            Diagram.Series.Add(linename);
            Diagram.Series[linename].BorderWidth = 1;
            Diagram.Series[linename].Color = colours[Diagram.Series.Count() - 3];
            Diagram.Series[linename].ChartType = SeriesChartType.Line;
            Diagram.Series[linename].ChartArea = "ChartArea1";
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
            Diagram.Series[linename].IsVisibleInLegend = true;
        }


        //draws a full line on windows form for an argand diagram
        public void CreateModLine(Complex midpoint, double gradient, string linename)
        {
            Diagram.Series.Add(linename);
            Diagram.Series[linename].BorderWidth = 2;
            Diagram.Series[linename].Color = colours[Diagram.Series.Count() - 3];
            Diagram.Series[linename].ChartType = SeriesChartType.Line;
            Diagram.Series[linename].ChartArea = "ChartArea1";
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
            Diagram.Series[linename].IsVisibleInLegend = true;
        }

    }
}
