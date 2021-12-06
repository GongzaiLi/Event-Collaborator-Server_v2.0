using System;

namespace event_client_app.View
{
    public class GetEventAttendeDTO
    {
        public int attendeeId { get; set; } // ea
        public string firstName { get; set; } // u
        public string lastName { get; set; } // a
        public DateTime dateOfInterest { get; set; } //ea need sort asd
        public string status { get; set; } // if organizer (all ) else show accepted
    }
}