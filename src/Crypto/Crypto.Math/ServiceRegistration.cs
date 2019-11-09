using System;
using Crypto.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.Math
{
    public static partial class ServiceRegistration
    {
        public static void AddMathServices(this IServiceCollection services)
        {
            services.AddSingleton<IMath, RunningStatsCalc>();
            //services.AddSingleton<IMath, SimpleCalc>();
            //services.AddSingleton<IMath, SimpleDictCalc>();
        }
    }
}
