using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Payment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
           
        }

        public void Configure(WebApplication app, IWebHostEnvironment enviroment, IApiVersionDescriptionProvider provider)
        {
           
        }
    }


    public interface IStartupProject
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment enviroment, IApiVersionDescriptionProvider provider);
        void ConfigureServices(IServiceCollection services);
    }
}
