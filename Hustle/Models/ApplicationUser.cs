using Microsoft.AspNetCore.Identity;

namespace Hustle.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
    }
}
