Arbor Web API Formatting x-www-form-urlencoded

The XWwwFormUrlEncodedFormatter makes it possible to extract values from a x-www-form-urlencoded http request into .NET poco objects.

It matches x-www-form-urlencoded parameters object properties. 

Make sure to register it as the first formatter or remove the default registered JQueryMvcFormUrlEncodedFormatter instance.

The JQueryMvcFormUrlEncodedFormatter cannot handle immutable classes without a default constructor.

    HttpConfiguration config = GlobalConfiguration.Configuration;
    var formatters = config.Formatters;
    formatters.Insert(0, new XWwwFormUrlEncodedFormatter());

Example: 

    public class BookingController : ApiController
    { 
      public object Post(BookingCancellationRequest request) {
        return request; //Just a sample echo response
      }
    }

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

will be mapped to an object of class

    public class NewUsersRequest
    {
        readonly IEnumerable<string> _newUsers;

        public NewUsersRequest(IEnumerable<string> newUsers) {
            _newUsers = newUsers;
        }
        
        public IEnumerable<string> NewUsers { get { return _newUsers; } }
    }