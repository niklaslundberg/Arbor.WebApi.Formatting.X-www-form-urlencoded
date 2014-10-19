using System;
using System.Net.Http.Formatting;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    internal class ConsoleLogger : IFormatterLogger
    {
        public void LogError(string errorPath, string errorMessage)
        {
            Console.Error.WriteLine("{0}: {1}", errorMessage, errorPath);
        }

        public void LogError(string errorPath, Exception exception)
        {
            Console.Error.WriteLine("{0}: {1}", exception, errorPath);
        }
    }
}