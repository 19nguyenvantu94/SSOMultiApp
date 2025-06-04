using System.ComponentModel.DataAnnotations;

namespace Authen.Users.Models
{
    public partial class Message
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;
        public DateTime When { get; set; }
        public Guid UserID { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}
