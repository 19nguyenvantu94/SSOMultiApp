using Authen.Users.Models;

namespace Authen.Models
{
    public class ClientClaimPolicyRole
    {
        public int ClientClaimPolicyId { get; set; }
        public Guid RoleId { get; set; }

        public ClientClaimPolicy ClientClaimPolicy { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
