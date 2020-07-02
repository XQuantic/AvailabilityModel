using System.Collections.Generic;

namespace AvailabilityModel
{
    class Constructor
    {
        public List<Probabilities> Pi { get; }

        public List<Probabilities> Pij { get; }

        public List<Probabilities> Pji { get; }

        public Constructor()
        {
            Pi = new List<Probabilities>();
            Pij = new List<Probabilities>();
            Pji = new List<Probabilities>();
        }

        public void CreateModel(int components, double λ, double μ, double e, double v, double μi, double h)
        {
            for (int i = 0; i < components; i++)
            {
                Pi.Add(i == 0 ? new Probabilities(1, h) : new Probabilities(0, h));
                Pij.Add(new Probabilities(0, h));
                Pji.Add(new Probabilities(0, h));
            }
            Pi.Add(new Probabilities(0, h));

            for (int j = 0; j < Pij.Count; j++)
            {
                Pi[j].AddInput(Pij[j], μ);
                Pi[j].AddOutput(λ);
                Pi[j].AddInput(Pji[j], e);
                Pi[j].AddOutput(v);
                Pi[j].AddOutput(μi);

                Pij[j].AddInput(Pi[j], λ);
                Pij[j].AddOutput(μ);

                Pji[j].AddInput(Pi[j], v);
                Pji[j].AddOutput(e);

                Pi[j + 1].AddInput(Pi[j], μi);
            }
        }
    }
}
