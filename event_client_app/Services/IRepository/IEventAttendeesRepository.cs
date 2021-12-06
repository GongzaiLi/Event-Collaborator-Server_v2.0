using System;
using System.Collections.Generic;
using event_client_app.Models;

namespace event_client_app.Services.IRepository
{
    public interface IEventAttendeesRepository : IDisposable
    {
        int CountAttendeesByEventId(int eventId);
        
        void DeleteAllEventAttendeesByEventId(int eventId);

        List<EventAttendees> FindAttendeesByEventId(int eventId);
        List<EventAttendees> FindAcceptedAttendeesByEventId(int eventId);
        
        List<EventAttendees> FindEventAttendeesByUserIdAndEventId(int eventId, int userId);

        void AddNewEventAttendees(EventAttendees attendees);

        void DeleteEventAttendees(EventAttendees eventAttendees);
    }
}