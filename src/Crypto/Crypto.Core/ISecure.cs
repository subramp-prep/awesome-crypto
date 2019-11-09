using System;
namespace Crypto.Core
{
    public interface ISecure
    {
        string Encrypt(string text);

        string Decrypt(string data);
    }
}
