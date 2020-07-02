using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace SmallWorld
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        PlotModel pm = new PlotModel();
        LinearAxis XAxis =
            new LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Minimum = 0, Maximum = 1000 };
        public int numberExp = 0;

        public class DataContainer
        {
            public double λ;
            public double μ;
            public double e;
            public double v;
            public double μi;
            public double Time;
            public double TimeSteps;
            public double H;
            public int ModelComponents;
        }

        public class ResultContainer
        {
            public List<double> Times = new List<double>();
            public List<double> Values = new List<double>();
            public List<double> Errors = new List<double>();
        }

        public class Calculator
        {
            private List<Probabilities> Pi;
            private List<Probabilities> Pij;
            private List<Probabilities> Pji;

            public ResultContainer Calculate(DataContainer data)
            {
                double H = data.Time / data.TimeSteps;
                Constructor constuctor = new Constructor();
                constuctor.CreateModel(data.ModelComponents, data.λ, data.μ, data.e, data.v, data.μi, H);
                Pi = constuctor.Pi;
                Pij = constuctor.Pij;
                Pji = constuctor.Pji;

                ResultContainer result = new ResultContainer();
                double error = 0;
                for (int i = 0; i <= data.TimeSteps; i++)
                {
                    result.Times.Add(H * i);
                    result.Values.Add(Pi[Pi.Count - 1].CurrentValue);

                    double Summ = 0;
                    for (int j = 0; j < Pij.Count; j++)
                    {
                        Summ += Pi[j].CurrentValue + Pij[j].CurrentValue + Pji[j].CurrentValue;
                    }
                    error += Math.Abs(1 - Summ - Pi[Pi.Count - 1].CurrentValue);
                    result.Errors.Add(error);

                    CalculateK1();
                    CalculateK2();
                    CalculateK3();
                    CalculateK4();
                    CalculateNextValue();
                }
                return result;
            }

            protected void CalculateK1()
            {
                for (int k = 0; k < Pij.Count; k++)
                {
                    Pi[k].CalculateK1();
                    Pij[k].CalculateK1();
                    Pji[k].CalculateK1();
                }
                Pi[Pi.Count - 1].CalculateK1();
            }

            protected void CalculateK2()
            {
                for (int k = 0; k < Pij.Count; k++)
                {
                    Pi[k].CalculateK2();
                    Pij[k].CalculateK2();
                    Pji[k].CalculateK2();
                }
                Pi[Pi.Count - 1].CalculateK2();
            }

            protected void CalculateK3()
            {
                for (int k = 0; k < Pij.Count; k++)
                {
                    Pi[k].CalculateK3();
                    Pij[k].CalculateK3();
                    Pji[k].CalculateK3();
                }
                Pi[Pi.Count - 1].CalculateK3();
            }

            protected void CalculateK4()
            {
                for (int k = 0; k < Pij.Count; k++)
                {
                    Pi[k].CalculateK4();
                    Pij[k].CalculateK4();
                    Pji[k].CalculateK4();
                }
                Pi[Pi.Count - 1].CalculateK4();
            }

            protected void CalculateNextValue()
            {
                for (int k = 0; k < Pij.Count; k++)
                {
                    Pi[k].CalculateNextValue();
                    Pij[k].CalculateNextValue();
                    Pji[k].CalculateNextValue();
                }
                Pi[Pi.Count - 1].CalculateNextValue();
            }
        }

        private ResultContainer Result;
        private DataContainer Source;

        private void MainMenu_Load(object sender, EventArgs e)
        {
           pm.Axes.Add(XAxis);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Source = ParseData();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            Calculator calc = new Calculator();
            Result = calc.Calculate(Source);
            double T = 0;
            for (int i = 0; i < Result.Values.Count; i++)
            {
                if (i != 0)
                {
                   T += (Result.Values[i - 1] - Result.Values[i]) * Result.Times[i - 1] * -1;
                }
            }
            label12.Text = "" + T;
            plot1.Model = pm;
            {

                if (numberExp != 5)
                {
                    plot1.Model.Series.Add(getFunction());
                }
                else
                {
                    numberExp = 0;
                    plot1.Model.Series.Clear();
                    plot1.Model.Series.Add(getFunction());
                }
                numberExp += 1;
            }
            pm.InvalidatePlot(true);
        }

        private LineSeries getFunction()
            {
                int n = Result.Values.Count-1;
                LineSeries[] fs = new LineSeries[5];
                OxyColor[] colors = new OxyColor[5];
                colors[0] = OxyColor.FromRgb(0, 255, 0);
                colors[1] = OxyColor.FromRgb(0, 250, 154);
                colors[2] = OxyColor.FromRgb(32, 178, 170);
                colors[3] = OxyColor.FromRgb(70, 130, 180);
                colors[4] = OxyColor.FromRgb(0, 0, 0);
                fs[numberExp] = new LineSeries();
                fs[numberExp].Color = colors[numberExp];
                fs[numberExp].Title = "Эксперимент " + (numberExp + 1);
                for (int x = 0; x <= n; x++)
                {
                    DataPoint data = new DataPoint(x, Result.Values[x]);
                    fs[numberExp].Points.Add(data);
                }
                return fs[numberExp];
        }

        private DataContainer ParseData()
        {
            DataContainer Temp = new DataContainer();
            if (textBox2.Text == "")
            {
                throw new Exception("");
            }
            double Intensity = Convert.ToDouble(textBox2.Text);
            Temp.λ = Intensity;
            if (textBox3.Text == "")
            {
                throw new Exception("");
            }
            Intensity = Convert.ToDouble(textBox3.Text);
            Temp.μ = Intensity;
            if (textBox4.Text == "")
            {
                throw new Exception("");
            }
            Intensity = Convert.ToDouble(textBox4.Text);
            Temp.e = Intensity;
            if (textBox5.Text == "")
            {
                throw new Exception("");
            }
            Intensity = Convert.ToDouble(textBox5.Text);
            Temp.v = Intensity;
            if (textBox1.Text == "")
            {
                throw new Exception("");
            }
            Intensity = Convert.ToDouble(textBox1.Text);
            Temp.μi = Intensity;
            if (textBox6.Text == "")
            {
                throw new Exception("");
            }
            double interval = Convert.ToDouble(textBox6.Text);
            if (interval < 0)
            {
                throw new Exception("");
            }
            Temp.Time = interval;
            if (textBox7.Text == "")
            {
                throw new Exception("");
            }
            double points = Convert.ToDouble(textBox7.Text);
            Temp.TimeSteps = points;
            if (textBox9.Text == "")
            {
                throw new Exception("");
            }
            int modelComponents = Convert.ToInt32(textBox9.Text);
            Temp.ModelComponents = modelComponents;
            label13.Text = "" + Temp.Time/Temp.TimeSteps;
            return Temp;
        }
    }
}
