namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit.SampleTypes
{
    public class BookingCancellationRequest
    {
        readonly int _bookingId;
        readonly string _reason;

        public BookingCancellationRequest(int bookingId, string reason)
        {
            _bookingId = bookingId;
            _reason = reason;
        }

        public string Reason
        {
            get { return _reason; }
        }

        public int BookingId
        {
            get { return _bookingId; }
        }
    }
}