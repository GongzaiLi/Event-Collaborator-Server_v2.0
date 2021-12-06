using System;
using System.Collections.Generic;

namespace event_client_app.View
{
    public class GetOneEventDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int organizerId { get; set; }
        public string organizerFirstName { get; set; }
        public string organizerLastName { get; set; }
        public int attendeeCount { get; set; }
        public int? capacity { get; set; }
        public int isOnline { get; set; }
        public string? url { get; set; }
        public string? venue { get; set; }
        public int requiresAttendanceControl { get; set; }
        public decimal fee { get; set; }

        public List<int> categories { get; set; } = new List<int>();
        public DateTime date { get; set; }
    }
}