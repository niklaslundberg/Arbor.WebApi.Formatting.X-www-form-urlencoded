namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.ComplexTypes
{
    public class SubListItem
    {
        public string Note { get; set; }

        public override string ToString()
        {
            return $"{nameof(Note)}: '{Note}'";
        }
    }
}