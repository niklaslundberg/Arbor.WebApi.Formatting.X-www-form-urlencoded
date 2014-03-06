using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using FluentAssertions;
using Machine.Specifications;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    [Subject(typeof (XWwwFormUrlEncodedFormatter))]
    public class when_deserializing_an_immutable_object_with_enumerable_constructor_parameter
    {
        static XWwwFormUrlEncodedFormatter formatter;
        static object result;
        static Stream contentStream;
        static HttpContent httpContent;
        static IFormatterLogger logger;
        static Type targetType = typeof (NewUsersRequest);
        static NewUsersRequest resultAsRequestedType;

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
                             new KeyValuePair<string, string>("newUsers", "Alice"),
                             new KeyValuePair<string, string>("newUsers", "Bob")
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
                resultAsRequestedType = result as NewUsersRequest;
            };

        It should_have_set_booking_id_to_1234 = () => resultAsRequestedType.NewUsers.Should().NotBeEmpty();

        It should_have_set_reason_to_sunshine = () => resultAsRequestedType.NewUsers.Should().HaveCount(2);

        It should_return_an_object_not_null = () => result.Should().NotBeNull();

        It should_return_an_object_of_type_the_requested_type =
            () => result.Should().BeOfType<NewUsersRequest>();
    }
}