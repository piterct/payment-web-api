using Microsoft.Extensions.Options;
using static Payment.Api.Configuration.SwaggerConfig.SwaggerDefaultValues;
using Swashbuckle.AspNetCore.SwaggerGen;
using Payment.Business.Interfaces.Services;
using Payment.Business.Services;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Notifications;

namespace Payment.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            #region Configurations
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            #endregion

            #region Services
            services.AddScoped<ISellerService, SellerService>();
            #endregion

            #region Notification
            services.AddScoped<INotifier, Notifier>();
            #endregion

            return services;
        }
    }
}
