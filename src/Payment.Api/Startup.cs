﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Payment.Api
{
    public class Startup: IStartupProject
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
           
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment, IApiVersionDescriptionProvider provider)
        {
           
        }
    }


    public interface IStartupProject
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment environment, IApiVersionDescriptionProvider provider);
        void ConfigureServices(IServiceCollection services);
    }
}
