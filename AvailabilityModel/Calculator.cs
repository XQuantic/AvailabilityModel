
using System;
using System.Collections.Generic;

namespace AvailabilityModel
{
    public class Calculator
    {
        private List<Probabilities> _pi;
        private List<Probabilities> _pij;
        private List<Probabilities> _pji;

        public ResultContainer Calculate(DataContainer data)
        {
            double h = data.time / data.timeSteps;
            Constructor constructor = new Constructor();
            constructor.CreateModel(data.modelComponents, data.λ, data.μ, data.e, data.v, data.μi, h);
            _pi = constructor.Pi;
            _pij = constructor.Pij;
            _pji = constructor.Pji;

            ResultContainer result = new ResultContainer
            {
                times = new List<double>(), values = new List<double>(), errors = new List<double>()
            };
            double error = 0;
            for (int i = 0; i <= data.timeSteps; i++)
            {
                result.times.Add(h * i);
                result.values.Add(_pi[_pi.Count - 1].CurrentValue);

                double sum = 0;
                for (int j = 0; j < _pij.Count; j++)
                {
                    sum += _pi[j].CurrentValue + _pij[j].CurrentValue + _pji[j].CurrentValue;
                }
                error += Math.Abs(1 - sum - _pi[_pi.Count - 1].CurrentValue);
                result.errors.Add(error);

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
            for (int k = 0; k < _pij.Count; k++)
            {
                _pi[k].CalculateK1();
                _pij[k].CalculateK1();
                _pji[k].CalculateK1();
            }
            _pi[_pi.Count - 1].CalculateK1();
        }

        protected void CalculateK2()
        {
            for (int k = 0; k < _pij.Count; k++)
            {
                _pi[k].CalculateK2();
                _pij[k].CalculateK2();
                _pji[k].CalculateK2();
            }
            _pi[_pi.Count - 1].CalculateK2();
        }

        protected void CalculateK3()
        {
            for (int k = 0; k < _pij.Count; k++)
            {
                _pi[k].CalculateK3();
                _pij[k].CalculateK3();
                _pji[k].CalculateK3();
            }
            _pi[_pi.Count - 1].CalculateK3();
        }

        protected void CalculateK4()
        {
            for (int k = 0; k < _pij.Count; k++)
            {
                _pi[k].CalculateK4();
                _pij[k].CalculateK4();
                _pji[k].CalculateK4();
            }
            _pi[_pi.Count - 1].CalculateK4();
        }

        protected void CalculateNextValue()
        {
            for (int k = 0; k < _pij.Count; k++)
            {
                _pi[k].CalculateNextValue();
                _pij[k].CalculateNextValue();
                _pji[k].CalculateNextValue();
            }
            _pi[_pi.Count - 1].CalculateNextValue();
        }
    }
}
