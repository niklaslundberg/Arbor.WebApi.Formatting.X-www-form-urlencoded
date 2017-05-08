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
    public class when_deserializing_nested_complex_type
    {
        static XWwwFormUrlEncodedFormatter formatter;
        static object result;
        static Stream contentStream;
        static HttpContent httpContent;
        static IFormatterLogger logger;
        static Type targetType = typeof (ComplexRootObject);
        static ComplexRootObject target;

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
                             new KeyValuePair<string, string>("rootTitle", "myRootTitle"),
                             new KeyValuePair<string, string>("rootOtherProperty", "911"),
                             new KeyValuePair<string, string>("subTypes[0].subTitle", "myFirstSubTitle"),
                             new KeyValuePair<string, string>("subTypes[0].subOtherProperty", "42"),
                             new KeyValuePair<string, string>("subTypes[0].subListItems[0].note", "The quick brown"),
                             new KeyValuePair<string, string>("subTypes[0].subListItems[1].note", "fox"),
                             new KeyValuePair<string, string>("subTypes[0].subListItems[2].note", "jumps"),
                             new KeyValuePair<string, string>("subTypes[1].subTitle", "mySecondSubTypeTitle"),
                             new KeyValuePair<string, string>("subTypes[1].subOtherProperty", "123")
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
                target = result as ComplexRootObject;

                Console.WriteLine("Instance: " + JsonConvert.SerializeObject(target, Newtonsoft.Json.Formatting.Indented));
            };

        It should_have_simple_property_set = () => target.RootTitle.Should().Be("myRootTitle");

        It should_have_other_simple_property_set = () => target.RootOtherProperty.Should().Be(911);

        private It should_not_be_null = () => target.SubTypes.First().SubTitle.Should().Be("myFirstSubTitle");

        It should_return_nested_objects = () => target.SubTypes.Should().HaveCount(2);

        It should_return_first_nested_object_with_property_set = () => target.SubTypes.First().SubListItems.Should().HaveCount(3);

        It should_return_second_nested_object_with_property_set = () => target.SubTypes.First().SubTitle.Should().Be("myFirstSubTitle");

        private It should_return_an_object_of_type_the_requested_type =
            () =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));

                result.Should().BeOfType<ComplexRootObject>();
            };
    }
}