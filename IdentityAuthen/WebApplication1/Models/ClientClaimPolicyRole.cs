using Authen.Users.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authen.Models
{
    public class ClientClaimPolicyRole
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int ClientClaimPolicyId { get; set; }
        public Guid RoleId { get; set; }

        public ClientClaimPolicy ClientClaimPolicy { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
