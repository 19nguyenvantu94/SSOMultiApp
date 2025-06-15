using Authen.Users;
using Finbuckle.MultiTenant;
using Models;

namespace Authen.Models
{

    [MultiTenant]
    [Permissions(Actions.CRUD)]
    public class ClientClaimPolicy
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string? ClientId { get; set; }

        [Required]

        public string? RequiredClaim { get; set; }

        [Required]

        public bool ClaimValue { get; set; }

        [Required]

        public bool IsEnabled { get; set; } = false;


    }
}
