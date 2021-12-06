using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using event_client_app.Models;
using event_client_app.Services;
using event_client_app.Services.IRepository;
using event_client_app.Services.Repository;
using event_client_app.View;
using EventClient.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace event_client_app.Controllers
{
    [ApiController]
    [EnableCors("AllowCors")]
    public class EventController : Controller
    {
        private IEventRepository _eventRepository;
        private IEventCategoryRepository _eventCategoryRepository;
        private IEventAttendeesRepository _eventAttendeesRepository;
        private FilterSearchEvent _filterSearchEvent;
        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;
        private IAttendanceStatusRepository _attendance;

        public EventController(DBAPPContext dbContext)
        {
            _eventRepository = new EventRepository(dbContext);
            _eventCategoryRepository = new EventCategoryRepository(dbContext);
            _eventAttendeesRepository = new EventAttendeesRepository(dbContext);
            _filterSearchEvent = new FilterSearchEvent();
            _userRepository = new UserRepository(dbContext);
            _categoryRepository = new CategoryRepository(dbContext);
            _attendance = new AttendanceStatusRepository(dbContext);
        }


        [HttpGet("events")]
        public IActionResult getEvents([FromForm] SearchEventQueries searchEventQueries)
        {
            #region Comment

            // {
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

            // (select event.id as eventId, count(user_id) as numAcceptedAttendees from event, 
            // event_attendees where event.id = event_attendees.event_id and 
            //     attendance_status_id = (select id from attendance_status 
            // where name = 'accepted') 
            // group by event_id) as table1`

            // var fullEntries = dbContext.tbl_EntryPoint
            //     .Join(
            //         dbContext.tbl_Entry,
            //         entryPoint => entryPoint.EID,
            //         entry => entry.EID,
            //         (entryPoint, entry) => new { entryPoint, entry }
            //     )
            //     .Join(
            //         dbContext.tbl_Title,
            //         combinedEntry => combinedEntry.entry.TID,
            //         title => title.TID,
            //         (combinedEntry, title) => new 
            //         {
            //             UID = combinedEntry.entry.OwnerUID,
            //             TID = combinedEntry.entry.TID,
            //             EID = combinedEntry.entryPoint.EID,
            //             Title = title.Title
            //         }
            //     )
            //     .Where(fullEntry => fullEntry.UID == user.UID)
            //     .Take(10);
            // AttendanceStatus accepted = _context.AttendanceStatus.SingleOrDefault(status => status.Name == "accepted");
            // var data = _context.Event.Include(eve=>eve.Organizer)
            //     .Join(_context.EventAttendees,
            //         eve => eve.EventId,
            //         ea => ea.EventId,
            //         (eve, ea) => new
            //         {
            //             eve, ea
            //         }).Where(eve_ea => eve_ea.ea.AttendanceStatusId == accepted.Id).ToList();


            // var data = (from eve in _context.Event
            //     from ea in _context.EventAttendees
            //     where eve.EventId == ea.EventId 
            //         select new
            //         {
            //             eve, ea
            //         }).ToList();

            #endregion

            List<Event> events = new List<Event>();
            if (searchEventQueries.q is null && searchEventQueries.organizerId is null)
            {
                events = _eventRepository.GetAllEvents();
            }
            else
            {
                if (searchEventQueries.q == null || searchEventQueries.organizerId == null)
                {
                    if (searchEventQueries.q == null)
                    {
                        events = _eventRepository.FilterEventByOrganizerId(searchEventQueries.organizerId);
                    }
                    else
                    {
                        events = _eventRepository.FilterEventByTitleOrDescription(searchEventQueries.q);
                    }
                }
                else
                {
                    events = _eventRepository.FilterEventByTitleOrDescriptionAndOrganizerId(searchEventQueries.q,
                        searchEventQueries.organizerId);
                }
            }

            List<GetEventDTO> getEventDtos = new List<GetEventDTO>();

            foreach (var eve in events)
            {
                List<int> categories = _eventCategoryRepository.GetCategoryIdByEventId(eve.EventId);
                int numAcceptedAttendees = _eventAttendeesRepository.CountAttendeesByEventId(eve.EventId);
                getEventDtos.Add(new GetEventDTO
                {
                    eventId = eve.EventId,
                    title = eve.Title,
                    capacity = eve.Capacity ?? 0,
                    organizerFirstName = eve.Organizer.FirstName,
                    organizerLastName = eve.Organizer.LastName,
                    date = eve.Date,
                    categories = categories,
                    numAcceptedAttendees = numAcceptedAttendees
                });
            }


            if (searchEventQueries.categoryIds.Count > 0)
            {
                _filterSearchEvent.FilterEventByCategoryIds(ref getEventDtos, searchEventQueries.categoryIds);
            }

            if (getEventDtos.Count > 0)
            {
                _filterSearchEvent.SortEvents(ref getEventDtos, searchEventQueries.sortBy);
            }

            if (getEventDtos.Count > 0)
            {
                _filterSearchEvent.FilterEventsLimit(ref getEventDtos, searchEventQueries.count);
            }

            if (getEventDtos.Count > 0)
            {
                _filterSearchEvent.FilterEventsStartIndex(ref getEventDtos, searchEventQueries.startIndex);
            }

            return Ok(getEventDtos);
        }

        [HttpPost("events")]
        public async Task<IActionResult> createEvent(PostEventObject postEventObject)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User user = await _userRepository.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }


            if (DateTime.Compare(postEventObject.date, DateTime.Now) <= 0)
            {
                return BadRequest("Event time should happened in the future");
            }

            if (_eventRepository
                    .FindEventByTitleAndDataAndOrganizer(postEventObject.title, postEventObject.date, user.UserId)
                    .Count >
                0)
            {
                return BadRequest(" There event has already exist");
            }

            Event newEvent = new Event
            {
                Title = postEventObject.title,
                Description = postEventObject.description,
                Date = postEventObject.date,
                IsOnline = postEventObject.isOnline ? 1 : 0,
                Url = postEventObject.url,
                Venue = postEventObject.venue,
                Capacity = postEventObject.capacity,
                RequiresAttendanceControl = postEventObject.requiresAttendanceControl ? 1 : 0,
                Fee = postEventObject.fee,
                OrganizerId = user.UserId,
                Organizer = user
            };
            _eventRepository.InsertEvent(newEvent);
            _userRepository.Save();

            List<int> categories = postEventObject.categoryIds.Distinct().ToList();

            foreach (int category in categories)
            {
                if (_categoryRepository.findCategoryById(category) != null)
                {
                    EventCategory eventCategory = new EventCategory
                    {
                        EventId = newEvent.EventId,
                        CategoryId = category
                    };
                    _eventCategoryRepository.InsertEventCategories(eventCategory);
                }
            }

            _userRepository.Save();

            return Created(new Uri("events", UriKind.Relative), new {eventId = newEvent.EventId});
        }


        [HttpGet("events/{id}")]
        public IActionResult getEvent(int id)
        {
            var findEvent = _eventRepository.FindEventById(id);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            var organizer = _userRepository.FindUserById(findEvent.OrganizerId);

            var categories = _eventCategoryRepository.GetCategoryIdByEventId(id);
            int numAcceptedAttendees = _eventAttendeesRepository.CountAttendeesByEventId(id);

            GetOneEventDTO eventDto = new GetOneEventDTO
            {
                id = id,
                title = findEvent.Title,
                description = findEvent.Description,
                organizerId = organizer.UserId,
                organizerFirstName = organizer.FirstName,
                organizerLastName = organizer.LastName,
                attendeeCount = numAcceptedAttendees,
                capacity = findEvent.Capacity,
                isOnline = findEvent.IsOnline,
                url = findEvent.Url,
                venue = findEvent.Venue,
                requiresAttendanceControl = findEvent.RequiresAttendanceControl,
                fee = findEvent.Fee,
                categories = categories,
                date = findEvent.Date
            };

            return Ok(eventDto);
        }

        [HttpPatch("events/{id}")]
        public async Task<IActionResult> updateEvent(int id, PatchEventObject patchEventObject)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User user = await _userRepository.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var findEvent = _eventRepository.FindEventById(id);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            if (findEvent.OrganizerId != user.UserId)
            {
                return StatusCode(403, "You are not organizer in this event!");
                // return Forbid("You are not organizer in this event!");
            }

            if (DateTime.Compare(findEvent.Date, DateTime.Now) <= 0)
            {
                return StatusCode(403, "Event happened");
                // return Forbid("Event happened");
            }


            _eventCategoryRepository.DeleteAllEventCategoriesByEventId(id);

            foreach (var category in patchEventObject.categoryIds)
            {
                if (_categoryRepository.findCategoryById(category) == null)
                {
                    return BadRequest("Category Id must each reference an existing category.");
                }

                _eventCategoryRepository.InsertEventCategories(new EventCategory
                {
                    CategoryId = category,
                    EventId = id
                });
            }


            findEvent.Title = patchEventObject.title ?? findEvent.Title;

            findEvent.Date = patchEventObject.date ?? findEvent.Date;


            if (_eventRepository
                    .ValidationEventTitleAndDateAndOrganizer(findEvent.Title, findEvent.Date, user.UserId, id)
                    .Count >
                0)
            {
                return BadRequest(" Event's title, Data and organizer is not unique!");
            }

            if (DateTime.Compare(findEvent.Date, DateTime.Now) <= 0)
            {
                return BadRequest("Can not set event in the past time.");
            }

            findEvent.Description = patchEventObject.description ?? findEvent.Description;

            if (patchEventObject.isOnline != null)
            {
                findEvent.IsOnline = patchEventObject.isOnline ? 1 : 0;
            }

            findEvent.Url = patchEventObject.url ?? findEvent.Url;
            findEvent.Venue = patchEventObject.venue ?? findEvent.Venue;
            findEvent.Capacity = patchEventObject.capacity ?? findEvent.Capacity;
            if (patchEventObject.requiresAttendanceControl != null)
            {
                findEvent.RequiresAttendanceControl = (bool) patchEventObject.requiresAttendanceControl ? 1 : 0;
            }

            findEvent.Fee = patchEventObject.fee ?? findEvent.Fee;


            _userRepository.Save();
            return Ok();
        }

        [HttpDelete("events/{id}")]
        public async Task<IActionResult> deleteEvent(int id)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User user = await _userRepository.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var findEvent = _eventRepository.FindEventById(id);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            if (findEvent.OrganizerId != user.UserId)
            {
                return StatusCode(403, "You are not organizer in this event!");
                // return Forbid("You are not organizer in this event!");
            }

            _eventCategoryRepository.DeleteAllEventCategoriesByEventId(id);
            _userRepository.Save(); //

            _eventAttendeesRepository.DeleteAllEventAttendeesByEventId(id);
            _userRepository.Save(); //

            _eventRepository.DeleteEvent(findEvent);
            _userRepository.Save(); //
            return Ok();
        }

        [HttpGet("events/categories")]
        public IActionResult getAllCategories()
        {
            var categories = _categoryRepository.getAllCategories();
            return Ok(categories);
        }

        [HttpGet("events/{id}/attendees")]
        public async Task<IActionResult> getAllEventAttendees(int id)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User loginUser = await _userRepository.FindUserByToken(token);

            var findEvent = _eventRepository.FindEventById(id);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            User organizer = _userRepository.FindUserById(findEvent.OrganizerId);


            List<EventAttendees> attendeesList = organizer.UserId == loginUser.UserId
                ? _eventAttendeesRepository.FindAttendeesByEventId(id)
                : _eventAttendeesRepository.FindAcceptedAttendeesByEventId(id);


            List<GetEventAttendeDTO> getEventAttendeDtos = new List<GetEventAttendeDTO>();

            foreach (var attendee in attendeesList)
            {
                getEventAttendeDtos.Add(new GetEventAttendeDTO
                {
                    attendeeId = attendee.Id,
                    firstName = organizer.FirstName,
                    lastName = organizer.LastName,
                    dateOfInterest = attendee.DataOfInterest,
                    status = attendee.Status.Name
                });
            }

            getEventAttendeDtos.Sort(
                (x, y) => x.dateOfInterest.CompareTo(y.dateOfInterest));

            return Ok(getEventAttendeDtos);
        }

        [HttpPost("events/{id}/attendees")]
        public async Task<IActionResult> addEventAttendees(int id)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User user = await _userRepository.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var findEvent = _eventRepository.FindEventById(id);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            if (DateTime.Compare(findEvent.Date, DateTime.Now) <= 0)
            {
                return StatusCode(403, "Event happened");
                // return Forbid("Event happened");
            }

            var checkAttendees = _eventAttendeesRepository.FindEventAttendeesByUserIdAndEventId(id, user.UserId);

            if (checkAttendees.Count > 0)
            {
                return StatusCode(403, "You have already join the event!");
                // return Forbid("You have already join the event!");
            }

            int attendanceStatusId = findEvent.RequiresAttendanceControl == Decimal.One ? 2 : 1;
            attendanceStatusId = findEvent.OrganizerId == user.UserId ? 1 : attendanceStatusId;

            EventAttendees eventAttendees = new EventAttendees
            {
                EventId = id,
                UserId = user.UserId,
                AttendanceStatusId = attendanceStatusId,
                DataOfInterest = DateTime.Now
            };
            _eventAttendeesRepository.AddNewEventAttendees(eventAttendees);
            _userRepository.Save();
            return Created($"events/{id}/attendees", new {id = eventAttendees.Id});
        }

        [HttpDelete("events/{id}/attendees")]
        public async Task<IActionResult> deleteEventAttendees(int id)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User user = await _userRepository.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var findEvent = _eventRepository.FindEventById(id);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            if (DateTime.Compare(findEvent.Date, DateTime.Now) <= 0)
            {
                return StatusCode(403, "Event happened");
                // return Forbid("Event happened");
            }

            var checkAttendees = _eventAttendeesRepository.FindEventAttendeesByUserIdAndEventId(id, user.UserId);


            if (checkAttendees.Count == 0)
            {
                return StatusCode(403, "You did not join the event");
            }

            EventAttendees eventAttend = checkAttendees[0];

            if (eventAttend.Status.Name == "rejected")
            {
                return StatusCode(403, "You have been rejected!");
            }

            _eventAttendeesRepository.DeleteEventAttendees(eventAttend);
            _userRepository.Save();


            return Ok();
        }

        [HttpPatch("events/{eventId}/attendees/{userId}")]
        public async Task<IActionResult> updateUserEventAttendStatus(int eventId, int userId,
            PatchUserEventAttendStatusObject peso)
        {
            Request.Headers.TryGetValue("X-Authorization", out var token);
            User loginUser = await _userRepository.FindUserByToken(token);
            if (loginUser == null)
            {
                return Unauthorized();
            }

            var findEvent = _eventRepository.FindEventById(eventId);
            if (findEvent == null)
            {
                return NotFound("Can not find the event");
            }

            if (findEvent.OrganizerId != loginUser.UserId)
            {
                return StatusCode(403, "You are not organizer in this event!");
            }

            if (DateTime.Compare(findEvent.Date, DateTime.Now) <= 0)
            {
                return StatusCode(403, "Event happened");
            }

            if (peso.status == null)
            {
                return BadRequest("The status is empty");
            }

            List<AttendanceStatus> statusList = _attendance.findAttendanceStatusByName(peso.status);

            if (statusList.Count == 0)
            {
                return BadRequest("Can not find the status");
            }

            AttendanceStatus newStatus = statusList[0];

            List<EventAttendees> eventAttendeesList =
                _eventAttendeesRepository.FindEventAttendeesByUserIdAndEventId(eventId, userId);

            if (eventAttendeesList.Count == 0)
            {
                return NotFound("Cannot find event attender");
            }

            EventAttendees attendees = eventAttendeesList[0];

            attendees.Status = newStatus;
            attendees.AttendanceStatusId = newStatus.Id;
            _userRepository.Save();

            return Ok();
        }

        // private void TestReference()
        // {
        //     //MemberShip ms = new MemberShip();
        //     MemberShip.GetMember();
        // }
    }
}