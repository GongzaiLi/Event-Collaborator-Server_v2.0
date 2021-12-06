using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;

namespace event_client_app.View
{
    public class PostEventObject
    {
        [Required] public string title { get; set; }
        [Required] public string description { get; set; }
        [Required] public List<int> categoryIds { get; set; }
        [Required] public DateTime date { get; set; }
        public bool isOnline { get; set; } = false;
        public string? url { get; set; }
        public string? venue { get; set; }
        public int? capacity { get; set; }
        public bool requiresAttendanceControl { get; set; } = false;
        public decimal fee { get; set; }
    }
}