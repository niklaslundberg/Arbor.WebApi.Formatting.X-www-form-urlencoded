using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Owin.Hosting;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Integration
{
    public class when_posting_url_encoded_parameters_to_a_controller_with_complex_immutable_type_parameter
    {
        static Uri baseUri;
        static IDisposable server;
        static HttpClient httpClient;
        static HttpResponseMessage httpResponse;
        static HttpRequestMessage httpRequestMessage;

        Cleanup after = () =>
        {
            using (httpClient)
            {
            }
            using (server)
            {
            }
        };

        Establish context = () =>
        {
            baseUri = new Uri("http://localhost:54321");
            server = WebApp.Start<Startup>(url: baseUri.ToString());

            httpClient = new HttpClient();

            IEnumerable<KeyValuePair<string, string>> pairs = new Dictionary<string, string>
                                                              {
                                                                  {"bookingId", "42"},
                                                                  {"reason", "sunshine"}
                                                              };
            httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, baseUri)
                                 {
                                     Content = new FormUrlEncodedContent(pairs)
                                 };
            Console.WriteLine(httpRequestMessage);
        };

        Because of =
            () => { httpResponse = httpClient.SendAsync(httpRequestMessage).Result; };

        It should_return_http_status_ok = () =>
        {
            Console.WriteLine(httpResponse.Content.ReadAsStringAsync().Result);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        };
    }
}