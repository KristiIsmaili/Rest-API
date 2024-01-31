using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Explicit implementation of HandleEvent method
        void IEventHandler<UserLoggedInEvent>.HandleEvent(UserLoggedInEvent eventData)
        {
            // Delegate to the async implementation
            HandleEventAsync(eventData).GetAwaiter().GetResult();
        }

    }
}
