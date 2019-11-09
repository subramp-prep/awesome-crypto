using System;
using AutoFixture;
using Crypto.Secure;
using Crypto.Core;
using Crypto.Math;
using CryptoService.Controllers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Crypto.Test
{
    public class CryptoControllerTestBase
    {
        protected readonly CryptoController controller;
        protected readonly Mock<IMath> mockMath;
        protected readonly Mock<ISecure> mockSecure;
        protected readonly IFixture fixture;

        public CryptoControllerTestBase()
        {
            mockMath = new Mock<IMath>();
            mockSecure = new Mock<ISecure>();
            controller = new CryptoController(mockMath.Object, mockSecure.Object);
        }

        /// <summary>
        /// End-to-end testing for the final Running stats 
        /// </summary>
        [Fact]
        public void Original_TestCase_Using_RunningStats()
        {
            RunningStatsCalc calc = new RunningStatsCalc();

            MathDto m1 = new MathDto()
            {
                Average = 4,
                StandardDeviation = 0.0
            };

            m1.Should().BeEquivalentTo(calc.Calculate(4));

            MathDto m2 = new MathDto()
            {
                Average = 5.5,
                StandardDeviation = 1.5
            };

            m2.Should().BeEquivalentTo(calc.Calculate(7));

            MathDto m3 = new MathDto()
            {
                Average = 5.667,
                StandardDeviation = 1.247
            };

            m3.Should().BeEquivalentTo(calc.Calculate(6));

            AESSecure secure = new AESSecure();
            var c = calc.Calculate(9);
            secure.Decrypt(secure.Encrypt(c.Average.ToString())).Should().BeEquivalentTo("6.5");
            secure.Decrypt(secure.Encrypt(c.StandardDeviation.ToString())).Should().BeEquivalentTo("1.803");

            MathDto m4 = new MathDto()
            {
                Average = 5.4,
                StandardDeviation = 2.728
            };

            m4.Should().BeEquivalentTo(calc.Calculate(1));
        }

        /// <summary>
        /// End-to-end testing for the simple implementation
        /// </summary>
        [Fact]
        public void Original_TestCase_UsingSimple()
        {
            SimpleCalc calc = new SimpleCalc();

            MathDto m1 = new MathDto()
            {
                Average = 4,
                StandardDeviation = 0
            };

            m1.Should().BeEquivalentTo(calc.Calculate(4));

            MathDto m2 = new MathDto()
            {
                Average = 5.5,
                StandardDeviation = 1.5
            };

            m2.Should().BeEquivalentTo(calc.Calculate(7));

            MathDto m3 = new MathDto()
            {
                Average = 5.667,
                StandardDeviation = 1.246
            };

            m3.Should().BeEquivalentTo(calc.Calculate(6));

            AESSecure secure = new AESSecure();
            var c = calc.Calculate(9);
            secure.Decrypt(secure.Encrypt(c.Average.ToString())).Should().BeEquivalentTo("6.5");
            secure.Decrypt(secure.Encrypt(c.StandardDeviation.ToString())).Should().BeEquivalentTo("1.803");

            MathDto m4 = new MathDto()
            {
                Average = 5.4,
                StandardDeviation = 2.728
            };

            m4.Should().BeEquivalentTo(calc.Calculate(1));
        }

        /// <summary>
        /// End-to-end testing for HashTable based implementation
        /// Warning - Very Naive!!!
        /// </summary>
        [Fact]
        public void Original_TestCase_UsingHashTable()
        {
            SimpleDictCalc calc = new SimpleDictCalc();

            MathDto m1 = new MathDto()
            {
                Average = 4,
                StandardDeviation = 0
            };
            var c1 = calc.Calculate(4);
            m1.Should().BeEquivalentTo(c1);

            MathDto m2 = new MathDto()
            {
                Average = 5.5,
                StandardDeviation = 1.5
            };
            var c2 = calc.Calculate(7);
            m2.Should().BeEquivalentTo(c2);

            MathDto m3 = new MathDto()
            {
                Average = 5.667,
                StandardDeviation = 1.247
            };
            var c3 = calc.Calculate(6);
            m3.Should().BeEquivalentTo(c3);

            AESSecure secure = new AESSecure();
            var c = calc.Calculate(9);
            secure.Decrypt(secure.Encrypt(c.Average.ToString())).Should().BeEquivalentTo("6.5");
            secure.Decrypt(secure.Encrypt(c.StandardDeviation.ToString())).Should().BeEquivalentTo("1.803");

            MathDto m4 = new MathDto()
            {
                Average = 5.4,
                StandardDeviation = 2.728
            };

            var c4 = calc.Calculate(1);
            m4.Should().BeEquivalentTo(c4);
        }
    }
}
