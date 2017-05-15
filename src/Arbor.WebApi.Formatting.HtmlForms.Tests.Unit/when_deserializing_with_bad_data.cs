using System;
using System.Collections.Generic;
using Arbor.ModelBinding.Core;
using Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Extensions.Primitives;

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
                    new List<KeyValuePair<string, StringValues>> { new KeyValuePair<string, StringValues>("bad", "data") },
                    typeof(DateWrapper));
            });
        };

        private It should_throw_exception = () =>
        {
            Console.WriteLine(exception);

            exception.Should().NotBeNull();
        };

        private It should_throw_exception_of_type_argument_exception =
            () => { exception.Should().BeOfType<ArgumentException>(); };
    }
}