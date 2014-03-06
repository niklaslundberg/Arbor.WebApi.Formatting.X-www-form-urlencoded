using System;
using System.Net.Http.Formatting;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    internal class ConsoleLogger : IFormatterLogger
    {
        public void LogError(string errorPath, string errorMessage)
        {
            Console.WriteLine(errorMessage + ": " + errorPath);
        }

        public void LogError(string errorPath, Exception exception)
        {
            Console.WriteLine(exception + ": " + errorPath);
        }
    }
}