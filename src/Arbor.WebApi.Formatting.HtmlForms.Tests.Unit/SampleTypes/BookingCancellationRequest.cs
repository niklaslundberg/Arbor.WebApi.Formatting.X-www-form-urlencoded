namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.SampleTypes
{
    public class BookingCancellationRequest
    {
        public BookingCancellationRequest(int bookingId, string reason)
        {
            BookingId = bookingId;
            Reason = reason;
        }

        public string Reason { get; }

        public int BookingId { get; }
    }
}