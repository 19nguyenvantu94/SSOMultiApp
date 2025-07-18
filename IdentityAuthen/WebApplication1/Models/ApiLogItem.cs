﻿using BlazorIdentity.Users;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Authen.Users.Models
{
    [Permissions(Actions.CRUD)]
    public partial class ApiLogItem
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "FieldRequired")]
        public DateTime RequestTime { get; set; }

        [Required(ErrorMessage = "FieldRequired")]
        public long ResponseMillis { get; set; }

        [Required(ErrorMessage = "FieldRequired")]
        public int StatusCode { get; set; }

        [Required(ErrorMessage = "FieldRequired")]
        public string Method { get; set; } = string.Empty;

        [Required(ErrorMessage = "FieldRequired")]
        [MaxLength(2048)]
        public string Path { get; set; } = string.Empty;

        [MaxLength(2048)]
        public string QueryString { get; set; } = string.Empty;

        [MaxLength(256)]
        public string RequestBody { get; set; } = string.Empty;

        [MaxLength(256)]
        public string ResponseBody { get; set; } = string.Empty;

        [MaxLength(45)]
        public string IPAddress { get; set; } = string.Empty;

        public Guid? ApplicationUserId { get; set; }
 
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
