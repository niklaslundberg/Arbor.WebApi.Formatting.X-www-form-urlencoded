using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.SampleTypes;
using Newtonsoft.Json;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Integration
{
    public class HomeController : ApiController
    {
        public HttpResponseMessage Post(BookingCancellationRequest bookingCancellationRequest)
        {
            if (bookingCancellationRequest == null)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            var json = JsonConvert.SerializeObject(bookingCancellationRequest);
            var stringContent = new StringContent(json, Encoding.UTF8,
                "application/json");
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                                      {
                                          Content = stringContent
                                      };
            Console.WriteLine(httpResponseMessage);

            return httpResponseMessage;
        }
    }
}