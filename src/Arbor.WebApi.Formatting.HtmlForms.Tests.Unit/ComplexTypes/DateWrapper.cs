using System;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes
{
    public class DateWrapper
    {
        public DateWrapper(DateTime aDateTime, string aRequiredValue)
        {
            if (string.IsNullOrWhiteSpace(aRequiredValue))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(aRequiredValue));
            }

            ADateTime = aDateTime;
            ARequiredValue = aRequiredValue;
        }

        public DateTime ADateTime { get; }

        public string ARequiredValue { get; }
    }
}