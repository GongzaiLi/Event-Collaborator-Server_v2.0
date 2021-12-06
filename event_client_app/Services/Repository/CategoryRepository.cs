using System.Collections.Generic;
using System.Linq;
using event_client_app.Models;
using event_client_app.Services.IRepository;

namespace event_client_app.Services.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DBAPPContext _context;

        public CategoryRepository(DBAPPContext context)
        {
            _context = context;
        }

        public Category findCategoryById(int categoryId)
        {
            return _context.Category.Find(categoryId);
        }

        public List<Category> getAllCategories()
        {
            return _context.Category.ToList();
        }
    }
}