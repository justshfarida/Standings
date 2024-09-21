using Microsoft.Extensions.DependencyInjection;
using Standings.Application.Interfaces.ITokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Infrastructure.Registerations
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler, Implementations.TokenServices.TokenHandler>();
        }
    }
}
