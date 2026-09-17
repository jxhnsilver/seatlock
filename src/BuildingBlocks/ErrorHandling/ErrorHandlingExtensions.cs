using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.ErrorHandling
{
    public static class ErrorHandlingExtensions
    {
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
