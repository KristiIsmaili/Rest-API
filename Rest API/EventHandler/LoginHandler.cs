using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.EventHandler
{
    public class LoginHandler
    {

        public class UserLoggedInEvent
        {
            public string Email { get; set; }
            public DateTime LoginTime { get; set; }
        }

        // Define custom event for failed login
        public class UserLoginFailedEvent
        {
            public string Email { get; set; }
            public string ErrorMessage { get; set; }
            public DateTime AttemptTime { get; set; }

        }

    }
}
