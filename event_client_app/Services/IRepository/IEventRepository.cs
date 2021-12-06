using System;
using System.Collections.Generic;
using event_client_app.Models;

namespace event_client_app.Services.IRepository
{
    public interface IEventRepository : IDisposable
    {
        List<Event> FilterEventByTitleOrDescriptionAndOrganizerId(string q, int? organizerId);
        List<Event> FilterEventByTitleOrDescription(string q);
        List<Event> FilterEventByOrganizerId(int? organizerId);
        List<Event> GetAllEvents();

        void InsertEvent(Event eve);

        List<Event> FindEventByTitleAndDataAndOrganizer(string title, DateTime dateTime, int organizerId);

        Event FindEventById(int eventId);

        List<Event> ValidationEventTitleAndDateAndOrganizer(string title, DateTime dateTime, int organizerId,
            int eventId);

        void DeleteEvent(Event eve);
    }
}