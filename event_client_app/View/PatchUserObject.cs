using System.ComponentModel.DataAnnotations;

namespace event_client_app.View
{
    public class PatchUserObject
    {
        [MinLength(1)] public string? FirstName { get; set; }
        [MinLength(1)] public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Email Address Validation error")]
        public string? email { get; set; }

        public string? password { get; set; }
        public string? currentPassword { get; set; }
    }
}