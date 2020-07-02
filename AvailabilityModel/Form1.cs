using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Windows.Forms;

namespace AvailabilityModel
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private readonly PlotModel _pm = new PlotModel();
        private readonly LinearAxis _xAxis = new LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Minimum = 0, Maximum = 1000 };
        private int _numberExp;
        private ResultContainer _result;
        private DataContainer _source;

        private void MainMenu_Load(object sender, EventArgs e)
        {
            _pm.Axes.Add(_xAxis);
        }

        private void CalculateProbs_Click(object sender, EventArgs e)
        {
            try
            {
                _source = ParseData();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Calculator calc = new Calculator();
            _result = calc.Calculate(_source);
            double T = 0;
            for (int i = 0; i < _result.values.Count; i++)
            {
                if (i != 0)
                {
                    T += (_result.values[i - 1] - _result.values[i]) * _result.times[i - 1] * -1;
                }
            }
            label12.Text = "" + T;
            plot1.Model = _pm;
            {

                if (_numberExp != 5)
                {
                    plot1.Model.Series.Add(GetFunction());
                }
                else
                {
                    _numberExp = 0;
                    plot1.Model.Series.Clear();
                    plot1.Model.Series.Add(GetFunction());
                }
                _numberExp += 1;
            }
            _pm.InvalidatePlot(true);
        }

        private LineSeries GetFunction()
        {
            int n = _result.values.Count - 1;
            LineSeries[] fs = new LineSeries[5];
            OxyColor[] colors = new OxyColor[5];
            colors[0] = OxyColor.FromRgb(0, 255, 0);
            colors[1] = OxyColor.FromRgb(0, 250, 154);
            colors[2] = OxyColor.FromRgb(32, 178, 170);
            colors[3] = OxyColor.FromRgb(70, 130, 180);
            colors[4] = OxyColor.FromRgb(0, 0, 0);
            fs[_numberExp] = new LineSeries();
            fs[_numberExp].Color = colors[_numberExp];
            fs[_numberExp].Title = "Эксперимент " + (_numberExp + 1);
            for (int x = 0; x <= n; x++)
            {
                DataPoint data = new DataPoint(x, _result.values[x]);
                fs[_numberExp].Points.Add(data);
            }
            return fs[_numberExp];
        }

        private DataContainer ParseData()
        {
            DataContainer temp = new DataContainer();
            if (textBox2.Text == "")
            {
                throw new Exception("Заполните поля интенсивности");
            }
            double Intensity = Convert.ToDouble(textBox2.Text);
            temp.λ = Intensity;
            if (textBox3.Text == "")
            {
                throw new Exception("Заполните поля интенсивности");
            }
            Intensity = Convert.ToDouble(textBox3.Text);
            temp.μ = Intensity;
            if (textBox4.Text == "")
            {
                throw new Exception("Заполните поля интенсивности");
            }
            Intensity = Convert.ToDouble(textBox4.Text);
            temp.e = Intensity;
            if (textBox5.Text == "")
            {
                throw new Exception("Заполните поля интенсивности");
            }
            Intensity = Convert.ToDouble(textBox5.Text);
            temp.v = Intensity;
            if (textBox1.Text == "")
            {
                throw new Exception("Заполните поля интенсивности");
            }
            Intensity = Convert.ToDouble(textBox1.Text);
            temp.μi = Intensity;
            if (textBox6.Text == "")
            {
                throw new Exception("Введите период");
            }
            double interval = Convert.ToDouble(textBox6.Text);
            if (interval < 0)
            {
                throw new Exception("Введите положительный период");
            }
            temp.time = interval;
            if (textBox7.Text == "")
            {
                throw new Exception("Введите точки отсчётов");
            }
            double points = Convert.ToDouble(textBox7.Text);
            temp.timeSteps = points;
            if (textBox9.Text == "")
            {
                throw new Exception("Введите количество узлов");
            }
            int modelComponents = Convert.ToInt32(textBox9.Text);
            temp.modelComponents = modelComponents;
            label13.Text = "" + temp.time / temp.timeSteps;
            return temp;
        }
    }
}
