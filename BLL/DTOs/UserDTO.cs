using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ID Card Number is required.")]
        [StringLength(30, ErrorMessage = "ID Card Number cannot exceed 30 characters.")]
        public string IdCardNo { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 32 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User Type ID is required.")]
        public int UserTypeId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Wallet balance cannot be negative.")]
        public double WalletBalance { get; set; }

        [StringLength(200, ErrorMessage = "Profile status cannot exceed 200 characters.")]
        public string? ProfileStatus { get; set; }
    }
}
