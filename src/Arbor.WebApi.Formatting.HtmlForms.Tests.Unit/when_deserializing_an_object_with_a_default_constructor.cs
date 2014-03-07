using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.SampleTypes;
using FluentAssertions;
using Machine.Specifications;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    [Subject(typeof (XWwwFormUrlEncodedFormatter))]
    public class when_deserializing_an_object_with_a_default_constructor
    {
        static XWwwFormUrlEncodedFormatter formatter;
        static object result;
        static Stream contentStream;
        static HttpContent httpContent;
        static IFormatterLogger logger;
        static Type targetType = typeof (DefaultConstructorObjectWithSimpleGetSetProperties);
        static DefaultConstructorObjectWithSimpleGetSetProperties resultAsRequestedType;

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
                             new KeyValuePair<string, string>("firstName", "Alice"),
                             new KeyValuePair<string, string>("lastName", "Bob")
                         };

            contentStream = values
                .AsXwwwFormUrlEncoded()
                .ToInMemoryStream();

            httpContent = new StreamContent(contentStream);
            formatter = new XWwwFormUrlEncodedFormatter();

            logger = new ConsoleLogger();
        };

        Because of =
            () =>
            {
                result = formatter.ReadFromStreamAsync(targetType, contentStream, httpContent, logger).Result;
                resultAsRequestedType = result as DefaultConstructorObjectWithSimpleGetSetProperties;
            };

        It should_have_set_first_name_to_alice = () => resultAsRequestedType.FirstName.Should().Be("Alice");

        It should_have_set_last_name_to_bob = () => resultAsRequestedType.LastName.Should().Be("Bob");

        It should_return_an_object_not_null = () => result.Should().NotBeNull();

        It should_return_an_object_of_type_the_requested_type =
            () => result.Should().BeOfType<DefaultConstructorObjectWithSimpleGetSetProperties>();
    }
}