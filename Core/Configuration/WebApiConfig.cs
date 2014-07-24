using Owin;
using System;
using System.Web.Http;

namespace Core.Configuration
{
    public static class WebApiConfig
    {
        public static void Configure(IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException("app");

            var apiConfig = new HttpConfiguration();
            
            apiConfig.MapHttpAttributeRoutes();
            apiConfig.Formatters.Remove(apiConfig.Formatters.XmlFormatter);
            apiConfig.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            app.UseWebApi(apiConfig);
        }
    }
}