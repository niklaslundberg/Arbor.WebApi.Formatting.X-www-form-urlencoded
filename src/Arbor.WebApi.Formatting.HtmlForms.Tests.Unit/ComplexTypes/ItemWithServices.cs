using System.Collections.Generic;
using System.Linq;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes
{
    public class ItemWithServices
    {
        public override string ToString()
        {
            string services = Services != null
                ? string.Join(", ", Services.Select(service => service.ToString()))
                : "No Services";

            return $"{nameof(Description)}: {Description}, {nameof(Services)}: {services}";
        }

        public string Description { get; set; }

        public int NumberOfItems { get; set; }

        public List<Service> Services { get; set; }// = new List<Service>();
    }
}