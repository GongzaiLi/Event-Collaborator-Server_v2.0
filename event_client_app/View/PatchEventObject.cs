using System;
using System.Collections.Generic;

namespace event_client_app.View
{
    public class PatchEventObject
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public List<int> categoryIds { get; set; } = new List<int>(); // need delete first then update
        public DateTime? date { get; set; }
        public bool isOnline { get; set; }
        public string? url { get; set; }
        public string? venue { get; set; }
        public int? capacity { get; set; }
        public bool? requiresAttendanceControl { get; set; }
        public int? fee { get; set; }
    }
}