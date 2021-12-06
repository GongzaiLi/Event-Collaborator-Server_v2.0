using System;
using System.Collections.Generic;

namespace event_client_app.View
{
    public class GetEventDTO
    {
        public int eventId{get; set; }//

        public string title{get; set; }//

        public int capacity{get; set; }//

        public string organizerFirstName{get; set; }//

        public string organizerLastName{get; set; }//

        public DateTime date{get; set; }

        public List<int> categories {get; set; }

        public int numAcceptedAttendees { get; set; }

        //     "eventId": 12,
        //     "title": "event",
        //     "capacity": null,
        //     "organizerFirstName": "a",
        //     "organizerLastName": "a",
        //     "date": "2024-11-14T05:59:38.000Z",
        //     "categories": [
        //     3,
        //     16,
        //     17,
        //     19,
        //     24
        //         ],
        //     "numAcceptedAttendees": 0
        // },
    }
}