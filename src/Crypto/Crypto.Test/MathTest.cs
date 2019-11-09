using System.IO;
using System.Threading.Tasks;
using Crypto.Core;
using Crypto.Math;
using Xunit;
using Xunit.Abstractions;

namespace Crypto.Test
{
    public class MathTests
    {
        private readonly ITestOutputHelper _output;

        public MathTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test_LargeDataSet()
        {
            var lines = File.ReadLines("sampleSet.txt");
            RunningStatsCalc calc = new RunningStatsCalc();

            Parallel.ForEach(lines, (line) =>
                {
                    MathDto m = calc.Calculate(long.Parse(line));
                    _output.WriteLine("Mean: {0} SD: {1}", m.Average, m.StandardDeviation);
                }        
            );
        }

        [Fact]
        public void Test_VeryLargeDataSet()
        {
            var lines = File.ReadLines("sampleSetLarge.txt");
            RunningStatsCalc calc = new RunningStatsCalc();

            Parallel.ForEach(lines, (line) =>
                {
                    MathDto m = calc.Calculate(long.Parse(line));
                    _output.WriteLine("Mean: {0} SD: {1}", m.Average, m.StandardDeviation);
                }
            );
        }
    }
}
