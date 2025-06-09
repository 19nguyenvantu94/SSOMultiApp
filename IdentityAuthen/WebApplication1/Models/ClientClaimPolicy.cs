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

        public string? ClientId { get; set; }

        public string? RequiredClaim { get; set; }

        public bool ClaimValue { get; set; }

        public bool IsEnabled { get; set; } = false;


    }
}
