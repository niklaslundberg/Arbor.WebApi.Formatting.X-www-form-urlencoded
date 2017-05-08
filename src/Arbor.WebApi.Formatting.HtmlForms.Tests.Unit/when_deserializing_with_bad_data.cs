using System;
using System.Collections.Generic;
using Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes;
using FluentAssertions;
using Machine.Specifications;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    [Subject(typeof(FormsExtensions))]
    public class when_deserializing_with_bad_data
    {
        private static Exception exception;

        private Establish context = () => { };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                FormsExtensions.ParseFromPairs(
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("bad", "data") },
                    typeof(DateWrapper));
            });
        };

        private It should_throw_exception = () =>
        {
            Console.WriteLine(exception);

            exception.Should().NotBeNull();
        };

        private It should_throw_exception_of_type_form_parse_exception =
            () => { exception.Should().BeOfType<FormParseException>(); };
    }
}