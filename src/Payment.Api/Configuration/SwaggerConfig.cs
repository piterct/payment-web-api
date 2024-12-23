﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Payment.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }

                });

            return app;
        }

        public class SwaggerDefaultValues : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var apiVersionMetadata = context
                    .ApiDescription
                    .ActionDescriptor
                    .EndpointMetadata
                    .OfType<ApiVersionAttribute>().FirstOrDefault();

                if (apiVersionMetadata != null)
                    operation.Deprecated = apiVersionMetadata.Deprecated;

                foreach (var parameter in operation.Parameters)
                {
                    var description = context.ApiDescription
                        .ParameterDescriptions
                        .First(p => p.Name == parameter.Name);

                    var routeInfo = description.RouteInfo;

                    operation.Deprecated = OpenApiOperation.DeprecatedDefault;

                    parameter.Description ??= description.ModelMetadata?.Description;

                    if (routeInfo == null)
                    {
                        continue;
                    }

                    if (parameter.In != ParameterLocation.Path && parameter.Schema.Default == null)
                    {
                        parameter.Schema.Default = new OpenApiString(routeInfo?.DefaultValue?.ToString());
                    }

                    if (routeInfo != null) parameter.Required |= !routeInfo.IsOptional;
                }
            }

            public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
            {
                private readonly IApiVersionDescriptionProvider _provider;

                public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;

                public void Configure(SwaggerGenOptions options)
                {
                    foreach (var description in _provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }
                }

                static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
                {
                    var info = new OpenApiInfo()
                    {
                        Title = "API",
                        Version = description.ApiVersion.ToString(),
                        Description = "ASP.NET Core WebAPI.",
                        Contact = new OpenApiContact() { Name = "Michael Peter", Email = "michael_piterct@hotmail.com" },
                        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                    };

                    if (description.IsDeprecated)
                    {
                        info.Description += " This version is obsolete!";
                    }

                    return info;
                }
            }

        }
    }

}
