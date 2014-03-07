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
    public class when_deserializing_an_immutable_object_with_simple_constructor_parameters
    {
        static XWwwFormUrlEncodedFormatter formatter;
        static object result;
        static Stream contentStream;
        static HttpContent httpContent;
        static IFormatterLogger logger;
        static Type targetType = typeof (BookingCancellationRequest);
        static BookingCancellationRequest resultAsRequestedType;

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
                             new KeyValuePair<string, string>("bookingId", "1234"),
                             new KeyValuePair<string, string>("reason", "Sunshine")
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
                resultAsRequestedType = result as BookingCancellationRequest;
            };

        It should_have_set_booking_id_to_1234 = () => resultAsRequestedType.BookingId.Should().Be(1234);

        It should_have_set_reason_to_sunshine = () => resultAsRequestedType.Reason.Should().Be("Sunshine");

        It should_return_an_object_not_null = () => result.Should().NotBeNull();

        It should_return_an_object_of_type_the_requested_type =
            () => result.Should().BeOfType<BookingCancellationRequest>();
    }
}