using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace event_client_app.View
{

    public class SearchEventQueries
    {
        [FromQuery]public int startIndex { get; set; }
        [FromQuery]public int? count { get; set; }
        [FromQuery]public string? q { get; set; }
        [FromQuery] public List<int> categoryIds { get; set; } = new List<int>();
        [FromQuery]public int? organizerId { get; set; }
        [FromQuery]public string sortBy { get; set; } = "DATE_DESC";
    }
}