using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace event_client_app.Models
{
    [Table("attendance_status")]
    public class AttendanceStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore] public ICollection<EventAttendees> EventAttendee { get; set; }
    }
}