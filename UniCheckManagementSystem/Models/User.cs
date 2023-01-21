using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UniCheckManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }

        [DisplayName("Потребителско име")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string UserName { get; set; }

        [DisplayName("Парола")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}