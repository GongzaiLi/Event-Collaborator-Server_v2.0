using System;
using System.Collections.Generic;
using event_client_app.Models;

namespace event_client_app.Services.IRepository
{
    public interface IEventCategoryRepository
    {
        List<int> GetCategoryIdByEventId(int eventId);

        void InsertEventCategories(EventCategory categories);

        void DeleteAllEventCategoriesByEventId(int eventId);
        
    }
}