using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace event_client_app.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }


        public string Email { get; set; }

        [Column("first_name")] public string FirstName { get; set; }

        [Column("last_name")] public string LastName { get; set; }


        [Column("image_filename")]
        public string? ImageName { get; set; }

        public string password { get; set; }

        // [DefaultValue("null")]
        [Column("auth_token")] public string? AuthToken { get; set; }

        [JsonIgnore]
        public ICollection<Event> Events { get; set; }

        // [JsonIgnore]
        // public List<Event> Events { get; set; } = new List<Event>();
        // public ICollection<Event> Events { get; set; }
    }
}