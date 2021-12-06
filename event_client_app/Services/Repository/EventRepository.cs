using System;
using System.Collections.Generic;
using System.Linq;
using event_client_app.Models;
using event_client_app.Services.IRepository;
using Microsoft.EntityFrameworkCore;

namespace event_client_app.Services.Repository
{
    public class EventRepository : IEventRepository
    {
        private DBAPPContext _dbContext;

        public EventRepository(DBAPPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Event> FilterEventByTitleOrDescriptionAndOrganizerId(string q, int? organizerId)
        {
            var events = _dbContext.Event
                .Include(eve => eve.Organizer)
                .Where(eve =>
                    (eve.Title.Contains(q) || eve.Description.Contains(q)) &&
                    eve.Organizer.UserId == organizerId)
                .ToList();

            return events;
        }

        public List<Event> FilterEventByTitleOrDescription(string q)
        {
            var events = _dbContext.Event
                .Include(eve => eve.Organizer)
                .Where(eve =>
                    eve.Title.Contains(q) || eve.Description.Contains(q))
                .ToList();
            return events;
        }

        public List<Event> FilterEventByOrganizerId(int? organizerId)
        {
            var events = _dbContext.Event
                .Include(eve => eve.Organizer)
                .Where(eve =>
                    eve.OrganizerId == organizerId)
                .ToList();
            return events;
        }

        public List<Event> GetAllEvents()
        {
            var events = _dbContext.Event
                .Include(eve => eve.Organizer)
                .ToList();
            return events;
        }

        public void InsertEvent(Event eve)
        {
            _dbContext.Event.Add(eve);
        }

        public List<Event> FindEventByTitleAndDataAndOrganizer(string title, DateTime dateTime, int organizerId)
        {
            var events = _dbContext.Event
                .Where(eve => eve.Title == title && eve.Date == dateTime && eve.OrganizerId == organizerId)
                .ToList();
            return events;
        }

        public Event FindEventById(int eventId)
        {
            return _dbContext.Event.Find(eventId);
        }

        public List<Event> ValidationEventTitleAndDateAndOrganizer(string title, DateTime dateTime, int organizerId,
            int eventId)
        {
            var events = _dbContext.Event
                .Where(eve =>
                    eve.Title == title && eve.Date == dateTime &&
                    eve.OrganizerId == organizerId &&
                    eve.EventId != eventId)
                .ToList();
            return events;
        }

        public void DeleteEvent(Event eve)
        {
            _dbContext.Event.Remove(eve);
        }

        public void Dispose()
        {
            // _dbContext?.Dispose();
        }
    }
}