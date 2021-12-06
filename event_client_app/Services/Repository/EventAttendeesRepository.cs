using System.Collections.Generic;
using System.Linq;
using event_client_app.Models;
using event_client_app.Services.IRepository;
using Microsoft.EntityFrameworkCore;

namespace event_client_app.Services.Repository
{
    public class EventAttendeesRepository : IEventAttendeesRepository
    {
        private DBAPPContext _dbContext;

        public EventAttendeesRepository(DBAPPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CountAttendeesByEventId(int eventId)
        {
            return _dbContext.EventAttendees
                .Where(ea => ea.EventId == eventId && ea.AttendanceStatusId == 1).ToList()
                .Count;
        }

        public void DeleteAllEventAttendeesByEventId(int eventId)
        {
            List<EventAttendees> eventAttendees = _dbContext.EventAttendees
                .Where(ec => ec.EventId == eventId).ToList();
            _dbContext.EventAttendees.RemoveRange(eventAttendees);
        }

        public List<EventAttendees> FindAttendeesByEventId(int eventId)
        {
            return _dbContext.EventAttendees.Include(ea => ea.Status)
                .Where(ea => ea.EventId == eventId)
                .ToList();
        }

        public List<EventAttendees> FindAcceptedAttendeesByEventId(int eventId)
        {
            return _dbContext.EventAttendees.Include(ea => ea.Status)
                .Where(ea => ea.EventId == eventId && ea.Status.Name == "accepted")
                .ToList();
        }

        public List<EventAttendees> FindEventAttendeesByUserIdAndEventId(int eventId, int userId)
        {
            return _dbContext.EventAttendees.Include(ea => ea.Status)
                .Where(e => e.EventId == eventId && e.UserId == userId).ToList();
        }

        public void AddNewEventAttendees(EventAttendees attendees)
        {
            _dbContext.EventAttendees.Add(attendees);
        }

        public void DeleteEventAttendees(EventAttendees eventAttendees)
        {
            _dbContext.EventAttendees.Remove(eventAttendees);
        }

        public void Dispose()
        {
            // _dbContext?.Dispose();// ????
        }
    }
}