using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(50, ErrorMessage = "Item name cannot exceed 50 characters.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(30, ErrorMessage = "Category cannot exceed 30 characters.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Available quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Available quantity cannot be negative.")]
        public int AvailableQuantity { get; set; }
    }
}
