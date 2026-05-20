using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTOs
{
    public class MealBookingDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Menu Item ID is required.")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Ordered quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Ordered quantity must be at least 1.")]
        public int OrderedQuantity { get; set; }

        [Required(ErrorMessage = "Total price is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total price cannot be negative.")]
        public double TotalPrice { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        public DateTime BookingDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Booking status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; }



    }
}
