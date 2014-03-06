Arbor Web API Formatting x-www-form-urlencoded

The XWwwFormUrlEncodedFormatter makes it possible to extract values from a x-www-form-urlencoded http request into .NET poco objects.

It matches x-www-form-urlencoded parameters object properties. 

Example: 

    public class BookingCancellationRequest
    {
        readonly int _bookingId;
        readonly string _reason;

        public BookingCancellationRequest(int bookingId, string reason) {
            _bookingId = bookingId;
            _reason = reason;
        }
        
        public string Reason { get { return _reason; } }

        public int BookingId { get { return _bookingId; } }
    }

An object of type BookingCancellationRequest can be created automatically by the body

    bookingId=123&reason=sunshine

If there are multiple key-value pairs with the same key then the values will be mapped to a string array.

    newUsers=Alice&newUsers=Bob

    public class NewUsersRequest
    {
        readonly IEnumerable<string> _newUsers;

        public NewUsersRequest(IEnumerable<string> newUsers) {
            _newUsers = newUsers;
        }
        
        public IEnumerable<string> NewUsers { get { return _newUsers; } }
    }