using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace event_client_app.Models
{
    public class Event
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }


        public string Title { get; set; }


        public string Description { get; set; }


        public DateTime Date { get; set; } // not sure

        [Column("image_filename")] public string? ImageName { get; set; }

        [Column("is_online")] public int IsOnline { get; set; }


        public string? Url { get; set; }


        public string? Venue { get; set; }

        public int? Capacity { get; set; }

        [Column("requires_attendance_control")]
        [DefaultValue(0)]
        public int RequiresAttendanceControl { get; set; }
        
        // [DefaultValue(0.00)]
        public decimal Fee { get; set; }


        [Column("organizer_id")] public int OrganizerId;
        public User Organizer { get; set; }

        
        // [JsonIgnore] 
        // public ICollection<EventAttendees> EventAttendees { get; set; }
        //
        // [JsonIgnore] 
        // public ICollection<EventCategory> EventCategories { get; set; }
    }
}