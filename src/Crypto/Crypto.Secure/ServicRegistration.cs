using System;
using Crypto.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.Secure
{
    public static partial class ServiceRegistration
    {
        public static void AddSecureServices(this IServiceCollection services)
        {
            services.AddSingleton<ISecure, AESSecure>();
        }
    }
}
