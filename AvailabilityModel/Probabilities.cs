using System.Collections.Generic;

namespace AvailabilityModel
{
    class Probabilities
    {
        private readonly List<Probabilities> _inputs;
        private readonly List<double> _inputsλ;
        private readonly List<double> _outputsλ;
        private double _nextValue;
        private readonly double _h;

        public double CurrentValue { get; private set; }

        public double K1 { get; private set; }

        public double K2 { get; private set; }

        public double K3 { get; private set; }

        public Probabilities(double initialValue, double h)
        {
            _inputs = new List<Probabilities>();
            _inputsλ = new List<double>();
            _outputsλ = new List<double>();
            CurrentValue = initialValue;
            _h = h;
        }

        public void AddInput(Probabilities input, double λ)
        {
            _inputs.Add(input);
            _inputsλ.Add(λ);
        }

        public void AddOutput(double λ)
        {
            _outputsλ.Add(λ);
        }

        public double CalculateK1()
        {
            double temp = 0;
            for (int i = 0; i < _inputs.Count; i++)
            {
                temp += _inputs[i].CurrentValue * _inputsλ[i];
            }
            foreach (var outputsλ in _outputsλ)
            {
                temp -= CurrentValue * outputsλ;
            }
            K1 = temp * _h;
            return K1;
        }

        public double CalculateK2()
        {
            double temp = 0;
            for (int i = 0; i < _inputs.Count; i++)
            {
                temp += (_inputs[i].CurrentValue + _inputs[i].K1 / 2) * _inputsλ[i];
            }
            foreach (var outputsλ in _outputsλ)
            {
                temp -= (CurrentValue + K1 / 2) * outputsλ;
            }
            K2 = temp * _h;
            return K2;
        }

        public double CalculateK3()
        {
            double temp = 0;
            for (int i = 0; i < _inputs.Count; i++)
            {
                temp += (_inputs[i].CurrentValue + _inputs[i].K2 / 2) * _inputsλ[i];
            }
            foreach (var outputsλ in _outputsλ)
            {
                temp -= (CurrentValue + K2 / 2) * outputsλ;
            }
            K3 = temp * _h;
            return K3;
        }

        public double CalculateK4()
        {
            double temp = 0;
            for (int i = 0; i < _inputs.Count; i++)
            {
                temp += (_inputs[i].CurrentValue + _inputs[i].K3) * _inputsλ[i];
            }
            foreach (var outputsλ in _outputsλ)
            {
                temp -= (CurrentValue + K3) * outputsλ;
            }
            K3 = temp * _h;
            return K3;
        }

        public double CalculateNextValue()
        {
            _nextValue = CurrentValue + (2 * K1 + 2 * K2 + K3) / 6;
            CurrentValue = _nextValue;
            return CurrentValue;
        }
    }

}
