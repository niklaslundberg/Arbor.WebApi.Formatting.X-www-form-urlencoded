using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes;
using Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.SampleTypes;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    [Subject(typeof (XWwwFormUrlEncodedFormatter))]
    public class when_deserializing_complex_type
    {
        static XWwwFormUrlEncodedFormatter formatter;
        static object result;
        static Stream contentStream;
        static HttpContent httpContent;
        static IFormatterLogger logger;
        static Type targetType = typeof (ItemWithServices);
        static ItemWithServices target;

        Cleanup after = () =>
        {
            using (contentStream)
            {
            }
            using (httpContent)
            {
            }
        };

        Establish context = () =>
        {
            var values = new List<KeyValuePair<string, string>>
                         {
                             new KeyValuePair<string, string>("description", "myDescription"),
                             new KeyValuePair<string, string>("numberOfItems", "33"),
                             new KeyValuePair<string, string>("services[0].title", "myFirstServiceTitle"),
                             new KeyValuePair<string, string>("services[0].otherProperty", "42"),
                             new KeyValuePair<string, string>("services[1].title", "mySecondServiceTitle"),
                             new KeyValuePair<string, string>("services[1].otherProperty", "123")
                         };

            contentStream = values
                .AsXwwwFormUrlEncoded()
                .ToInMemoryStream();

            httpContent = new StreamContent(contentStream);
            formatter = new XWwwFormUrlEncodedFormatter();
            logger = new ConsoleLogger();
        };

        private Because of =
            () =>
            {
                result = formatter.ReadFromStreamAsync(targetType, contentStream, httpContent, logger).Result;
                target = result as ItemWithServices;

                Console.WriteLine("Instance: " + JsonConvert.SerializeObject(target, Newtonsoft.Json.Formatting.Indented));
            };

        It should_have_simple_property_set = () => target.Description.Should().Be("myDescription");

        It should_have_other_simple_property_set = () => target.NumberOfItems.Should().Be(33);

        private It should_not_be_null = () => target.Services.Should().NotBeNull();

        It should_return_nested_objects = () => target.Services.Should().HaveCount(2);

        It should_return_first_nested_object_with_property_set = () => target.Services[0].Title.Should().Be("myFirstServiceTitle");

        It should_return_second_nested_object_with_property_set = () => target.Services[1].Title.Should().Be("mySecondServiceTitle");

        It should_return_an_object_of_type_the_requested_type =
            () => result.Should().BeOfType<ItemWithServices>();
    }
}