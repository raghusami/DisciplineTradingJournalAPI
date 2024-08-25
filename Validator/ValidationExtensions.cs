using Microsoft.Extensions.DependencyInjection;

namespace LISServiceAPI.Validator
{
    public static class ValidationExtensions
    {
        public static IServiceCollection RegisterValidation(this IServiceCollection services)
        {
           
            return services;
        }
    }
}
