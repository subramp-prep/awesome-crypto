using System;
using m = System.Math;
using Crypto.Core;

namespace Crypto.Math
{
    /// <summary>
    /// Intuition stolen from
    /// URL: https://www.johndcook.com/blog/skewness_kurtosis/
    /// </summary>
    public class RunningStatsCalc : IMath
    {
        private long n;
        private double M1, M2, M3, M4;

        public RunningStatsCalc()
        {
            n = 0;
            M1 = M2 = M3 = M4 = 0.0;
        }

        public MathDto Calculate(long x)
        {
            double delta, delta_n, delta_n2, term1;

            long n1 = n;
            n++;
            delta = x - M1;
            delta_n = delta / n;
            delta_n2 = delta_n * delta_n;
            term1 = delta * delta_n * n1;
            M1 += delta_n;
            M4 += term1 * delta_n2 * (n * n - 3 * n + 3) + 6 * delta_n2 * M2 - 4 * delta_n * M3;
            M3 += term1 * delta_n * (n - 2) - 3 * delta_n * M2;
            M2 += term1;

            // variance 
            double variance = 0;
            //if (M2 > 1)
            variance = (double)M2 / (n);

            return new MathDto()
            {
                Average = m.Round(M1, 3),
                StandardDeviation = m.Round(m.Sqrt(variance), 3)
            };
        }

        public static RunningStatsCalc operator + (RunningStatsCalc a, RunningStatsCalc b)
        {
            RunningStatsCalc combined = new RunningStatsCalc();

            combined.n = a.n + b.n;

            double delta = b.M1 - a.M1;
            double delta2 = delta * delta;
            double delta3 = delta * delta2;
            double delta4 = delta2 * delta2;

            combined.M1 = (a.n * a.M1 + b.n * b.M1) / combined.n;

            combined.M2 = a.M2 + b.M2 +
                          delta2 * a.n * b.n / combined.n;

            combined.M3 = a.M3 + b.M3 +
                          delta3 * a.n * b.n * (a.n - b.n) / (combined.n * combined.n);
            combined.M3 += 3.0 * delta * (a.n * b.M2 - b.n * a.M2) / combined.n;

            combined.M4 = a.M4 + b.M4 + delta4 * a.n * b.n * (a.n * a.n - a.n * b.n + b.n * b.n) /
                          (combined.n * combined.n * combined.n);
            combined.M4 += 6.0 * delta2 * (a.n * a.n * b.M2 + b.n * b.n * a.M2) / (combined.n * combined.n) +
                          4.0 * delta * (a.n * b.M3 - b.n * a.M3) / combined.n;

            return combined;
        }
    }
}
