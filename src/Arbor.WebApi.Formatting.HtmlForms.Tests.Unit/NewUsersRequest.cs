using System.Collections.Generic;

namespace Arbor.WebApi.Formatting.HtmlForms.Tests.Unit
{
    public class NewUsersRequest
    {
        readonly IEnumerable<string> _newUsers;

        public NewUsersRequest(IEnumerable<string> newUsers)
        {
            _newUsers = newUsers;
        }

        public IEnumerable<string> NewUsers { get { return _newUsers; } }
    }
}