using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTOs
{
    public class UserTypeDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User type name is required.")]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; }
    }
}
