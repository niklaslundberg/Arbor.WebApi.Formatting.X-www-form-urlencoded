using System;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    public class FormParseException : Exception
    {
        public FormParseException(string message) : base(message)
        {

        }

        public FormParseException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}