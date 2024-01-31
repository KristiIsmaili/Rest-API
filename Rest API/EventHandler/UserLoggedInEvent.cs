using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.EventHandler
{
    public class UserLoggedInEvent
    {
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
