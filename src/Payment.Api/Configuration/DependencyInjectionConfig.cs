﻿using Microsoft.Extensions.Options;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Queries;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Interfaces.Services;
using Payment.Business.Notifications;
using Payment.Business.Queries;
using Payment.Business.Services;
using Payment.Data.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Payment.Api.Configuration.SwaggerConfig.SwaggerDefaultValues;

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
            services.AddScoped<IOrderService, OrderService>();
            #endregion

            #region Notification
            services.AddScoped<INotifier, Notifier>();
            #endregion

            #region Repositories
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            #endregion

            #region Queries
            services.AddScoped<IOrderQuery, OrderQuery>();
            #endregion

            return services;
        }
    }
}
