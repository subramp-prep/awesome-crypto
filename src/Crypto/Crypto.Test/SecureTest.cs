using System;
using Crypto.Secure;
using FluentAssertions;
using Xunit;

namespace Crypto.Test
{
    public class SecureTest
    {
        [Theory]
        [InlineData("Foo")]
        [InlineData("4")]
        [InlineData("1.62")]
        public void Test_EncryptDecrypt(string sample)
        {
            using(AESSecure aes = new AESSecure())
            {
                var encrypt = aes.Encrypt(sample);
                aes.Decrypt(encrypt).Should().BeEquivalentTo(sample);
            }
        }

        [Theory]
        [InlineData("")]
        public void Test_Encrypt_Empty(string sample)
        {
            using (AESSecure aes = new AESSecure())
            {
                Exception ex = Assert.Throws<ArgumentNullException>(() => aes.Encrypt(sample));

                Assert.NotNull(ex);
            }
        }

        [Theory]
        [InlineData("")]
        public void Test_Decrypt_Empty(string sample)
        {
            using (AESSecure aes = new AESSecure())
            {
                Exception ex = Assert.Throws<ArgumentNullException>(() => aes.Decrypt(sample));

                Assert.NotNull(ex);
            }
        }
    }
}
