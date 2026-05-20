using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTOs
{
    public class WalletTransactionDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Transaction amount is required.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        [StringLength(30, ErrorMessage = "Transaction type cannot exceed 30 characters.")]
        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
