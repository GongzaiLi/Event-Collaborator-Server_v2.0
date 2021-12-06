using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace event_client_app.Models
{
    [Table("event_category")]
    public class EventCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column("event_id")]
        public int EventId { get; set; }
        // public Event Events { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }
        // public Category Categories { get; set; }
    }
}