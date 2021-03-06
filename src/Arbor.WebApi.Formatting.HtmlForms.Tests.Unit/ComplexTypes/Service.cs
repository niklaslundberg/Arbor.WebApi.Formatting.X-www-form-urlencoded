﻿namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes
{
    public class Service
    {
        public Service(string title, int otherProperty)
        {
            Title = title;
            OtherProperty = otherProperty;
        }

        public string Title { get; }

        public int OtherProperty { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(OtherProperty)}: {OtherProperty}";
        }
    }
}