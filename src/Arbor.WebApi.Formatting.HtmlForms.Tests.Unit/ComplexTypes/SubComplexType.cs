using System.Collections.Generic;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes
{
    public class SubComplexType
    {
        public SubComplexType(string subTitle, int subOtherProperty)
        {
            SubTitle = subTitle;
            SubOtherProperty = subOtherProperty;
        }

        public string SubTitle { get; }

        public int SubOtherProperty { get; }

        public ICollection<SubListItem> SubListItems { get; set; }
    }
}