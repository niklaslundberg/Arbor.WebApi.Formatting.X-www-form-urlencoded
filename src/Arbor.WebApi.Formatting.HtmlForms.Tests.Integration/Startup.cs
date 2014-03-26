using System.Web.Http;
using Owin;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Integration
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new {controller = "Home", id = RouteParameter.Optional}
                );

            var formatters = config.Formatters;

            config.Formatters.Remove(formatters.XmlFormatter);

            config.Formatters.Insert(0,new XWwwFormUrlEncodedFormatter());

            appBuilder.UseWebApi(config);
        }
    }
}