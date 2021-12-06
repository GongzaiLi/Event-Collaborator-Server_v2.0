using System.ComponentModel.DataAnnotations;

namespace event_client_app.View
{
    public class PostUserRegisterObject
    {
        [Required] 
        public string firstName { get; set; }
        [Required] public string lastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email Address Validation error")]
        public string email { get; set; }

        [Required] public string password { get; set; }
    }
}