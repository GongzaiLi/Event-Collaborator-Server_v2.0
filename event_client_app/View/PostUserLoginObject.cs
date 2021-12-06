using System.ComponentModel.DataAnnotations;

namespace event_client_app.View
{
    public class PostUserLoginObject
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email Address Validation error")]
        public string email { get; set; }

        [Required] public string password { get; set; }
    }
}