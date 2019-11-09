using System;
using System.Collections.Generic;
using System.Linq;
using Crypto.Core;
using m = System.Math;

namespace Crypto.Math
{
    /// <summary>
    /// Intuition: To implement a simple calculator 
    /// This implementation is not 
    /// - Take O(n) space
    /// </summary>
    public class SimpleCalc : IMath
    {
        private long _sum;
        private long _sumOfSquares;
        private long _count;
        private double _mean;

        //mean = sum_x / n
        //stdev = sqrt(sum_x2 / n - mean ^ 2)

        public MathDto Calculate(long input)
        {
            Mean(input);
            var sd = StandardVariance();

            return new MathDto
            {
                Average = _mean,
                StandardDeviation = m.Round(sd, 3)
            };
        }

        private void Mean(long input)
        {
            _sum += input;
            _count += 1;
            _sumOfSquares += (long)m.Pow(input, 2);

            _mean = m.Round((double)_sum / (double)_count, 3);
        }

        private double StandardVariance()
        {
            return m.Sqrt(m.Round((double)_sumOfSquares / _count, 3) - m.Pow(_mean, 2));
        }
    }
}
