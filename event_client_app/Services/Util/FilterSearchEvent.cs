using System;
using System.Collections.Generic;
using System.Linq;
using event_client_app.View;

namespace event_client_app.Services
{
    public class FilterSearchEvent
    {
        public void FilterEventByCategoryIds(ref List<GetEventDTO> getEventDtos, List<int> categoryIds)
        {
            int index = 0;
            while (index < getEventDtos.Count)
            {
                if (getEventDtos[index].categories.Any(c => categoryIds.Contains(c)))
                {
                    index++;
                    continue;
                }

                getEventDtos.RemoveAt(index);
            }
        }

        public void SortEvents(ref List<GetEventDTO> getEventDtos, string sotyBy)
        {
            switch (sotyBy)
            {
                /*
                ALPHABETICAL_ASC: alphabetically by title, A - Z   === event
                ALPHABETICAL_DESC: alphabetically by title, Z - A  === event 
                DATE_ASC: date, from earliest to latest           === event
                DATE_DESC: date, from latest to earliest          === event ======
                ATTENDEES_ASC: the number of signed up (approved) attendees, from least to most   === attendees
                ATTENDEES_DESC: the number of signed up (approved) attendees, from most to least   === attendees
                CAPACITY_ASC: the capacity from least to most                                     === event
                CAPACITY_DESC: the capacity from most to least                                    === event
             */
                case "ALPHABETICAL_ASC":
                    getEventDtos.Sort((x, y) => String.Compare(x.title, y.title, StringComparison.Ordinal));
                    break;
                case "ALPHABETICAL_DESC":
                    getEventDtos.Sort((x, y) => String.Compare(y.title, x.title, StringComparison.Ordinal));
                    break;
                case "DATE_ASC":
                    getEventDtos.Sort((x, y) => x.date.CompareTo(y.date));
                    break;
                case "ATTENDEES_ASC":
                    getEventDtos.Sort((x, y) => x.numAcceptedAttendees.CompareTo(y.numAcceptedAttendees));
                    break;
                case "ATTENDEES_DESC":
                    getEventDtos.Sort((x, y) => y.numAcceptedAttendees.CompareTo(x.numAcceptedAttendees));
                    break;
                case "CAPACITY_ASC":
                    getEventDtos.Sort((x, y) => x.capacity.CompareTo(y.capacity));
                    break;
                case "CAPACITY_DESC":
                    getEventDtos.Sort((x, y) => y.capacity.CompareTo(x.capacity));
                    break;
                default:
                    getEventDtos.Sort((x, y) => y.date.CompareTo(x.date));
                    break;
            }
        }

        public void FilterEventsLimit(ref List<GetEventDTO> getEventDtos, int? count)
        {
            int last = count ?? getEventDtos.Count;
            last = last > getEventDtos.Count ? getEventDtos.Count : last;
            getEventDtos = getEventDtos.GetRange(0, last);
            
        }
        
        public void FilterEventsStartIndex(ref List<GetEventDTO> getEventDtos, int startIndex)
        {
            /*
             * List sublist = list.GetRange(5, 5); // (gets elements 5,6,7,8,9)
             * List anotherSublist = list.GetRange(0, 4); // gets elements 0,1,2,3)
             */
            int start = startIndex > getEventDtos.Count ? getEventDtos.Count : startIndex;
            int end = getEventDtos.Count - start;
            getEventDtos = getEventDtos.GetRange(start, end);;
            
        }
    }
}