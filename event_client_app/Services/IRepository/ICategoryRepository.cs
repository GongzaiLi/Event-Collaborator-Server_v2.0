using System.Collections.Generic;
using event_client_app.Models;

namespace event_client_app.Services.IRepository
{
    public interface ICategoryRepository
    {
        Category findCategoryById(int categoryId);
        List<Category> getAllCategories();
    }
}