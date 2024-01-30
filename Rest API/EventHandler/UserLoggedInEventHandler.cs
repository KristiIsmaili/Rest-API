using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Rest_API.EventHandler.LoginHandler;

namespace Rest_API.EventHandler
{
    public class UserLoggedInEventHandler : IEventHandler<UserLoggedInEvent>
    {
        public Task HandleEventAsync(UserLoggedInEvent eventData)
        {
            // Log successful login
            Console.WriteLine($"User '{eventData.Email}' logged in at {eventData.LoginTime}");
            return Task.CompletedTask;
        }

        void IEventHandler<UserLoggedInEvent>.HandleEvent(UserLoggedInEvent eventData)
        {
            throw new NotImplementedException();
        }

        public class UserLoginFailedEventHandler : IEventHandler<UserLoginFailedEvent>
        {
            public void HandleEvent(UserLoginFailedEvent eventData)
            {
                throw new NotImplementedException();
            }

            public Task HandleEventAsync(UserLoginFailedEvent eventData)
            {
                // Log failed login attempt
                Console.WriteLine($"Failed login attempt for user '{eventData.Email}' at {eventData.AttemptTime}: {eventData.ErrorMessage}");
                return Task.CompletedTask;
            }
        }
    }
}
