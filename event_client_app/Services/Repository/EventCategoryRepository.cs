using System;
using System.Collections.Generic;
using System.Linq;
using event_client_app.Models;
using event_client_app.Services.IRepository;

namespace event_client_app.Services.Repository
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private DBAPPContext _dbContext;

        public EventCategoryRepository(DBAPPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<int> GetCategoryIdByEventId(int eventId)
        {
            return _dbContext.EventCategory
                .Where(ec => ec.EventId == eventId).Select(ec => ec.CategoryId).ToList();
        }

        public void InsertEventCategories(EventCategory category)
        {
            _dbContext.EventCategory.Add(category);
        }

        public void DeleteAllEventCategoriesByEventId(int eventId)
        {
            List<EventCategory> eventCategories = _dbContext.EventCategory
                .Where(ec => ec.EventId == eventId).ToList();
            _dbContext.EventCategory.RemoveRange(eventCategories);
        }

        public void Dispose()
        {
            // _dbContext?.Dispose(); // ???????
        }
    }
}