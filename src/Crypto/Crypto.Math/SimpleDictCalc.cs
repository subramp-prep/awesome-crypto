using System;
using System.Collections.Concurrent;
using System.Linq;
using m = System.Math;
using Crypto.Core;

namespace Crypto.Math
{
    public class SimpleDictCalc : IMath
    {
        private ConcurrentDictionary<long, int> _lookups;

        public SimpleDictCalc()
        {
            _lookups = new ConcurrentDictionary<long, int>();
        }

        public MathDto Calculate(long input)
        {
            var mean = Mean(input);
            var sd = StandardVariance(mean);

            return new MathDto
            {
                Average = m.Round(mean, 3),
                StandardDeviation = m.Round(sd, 3)
            };
        }

        private double Mean(long input)
        {
            if(!_lookups.ContainsKey(input))
            {
                _lookups.TryAdd(input, 1);
            }
            else
            {
                _lookups[input] += 1;
            }

            var sum = _lookups.Sum(k => k.Key * k.Value);

            double mean = (double)sum / (double)_lookups.Values.Sum();

            return mean;
        }

        private double StandardVariance(double mean)
        {
            var sumForVariance = _lookups.Sum(k => m.Pow(mean - k.Key, 2)*k.Value);
            var variance = (double)sumForVariance / (double)_lookups.Values.Sum();

            return m.Sqrt(variance);
        }
    }
}
