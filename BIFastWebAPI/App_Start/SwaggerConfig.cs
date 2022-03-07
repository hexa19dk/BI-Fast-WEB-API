using System.Web.Http;
using WebActivatorEx;
using BIFastWebAPI;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace BIFastWebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        
                        c.SingleApiVersion("v1", "BIFastWebAPI");
                    })
                .EnableSwaggerUi(c =>{});
        }
    }
}
