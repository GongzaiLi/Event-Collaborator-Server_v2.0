using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace event_client_app.Models
{
    [Table("event_attendees")]
    public class EventAttendees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // public int EventId
        [Column("event_id")] public int EventId { get; set; }

        // [JsonIgnore] public Event Event { get; set; }

        [Column("user_id")] public int UserId { get; set; }
        // public User User { get; set; }


        [Column("attendance_status_id")] public int AttendanceStatusId { get; set; }
        
        public AttendanceStatus Status { get; set; }

        [Column("date_of_interest")] public DateTime DataOfInterest { get; set; }
    }
}