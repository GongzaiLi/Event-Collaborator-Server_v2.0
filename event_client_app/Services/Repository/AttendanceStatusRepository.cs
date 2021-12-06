using System.Collections.Generic;
using System.Linq;
using event_client_app.Models;
using event_client_app.Services.IRepository;

namespace event_client_app.Services.Repository
{
    public class AttendanceStatusRepository : IAttendanceStatusRepository
    {
        private DBAPPContext _dbContext;

        public AttendanceStatusRepository(DBAPPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AttendanceStatus> findAttendanceStatusByName(string Name)
        {
            return _dbContext.AttendanceStatus
                .Where(a => a.Name == Name).ToList();
        }
    }
}