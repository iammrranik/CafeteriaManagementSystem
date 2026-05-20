using DAL.EF;
using System.ComponentModel.DataAnnotations;

namespace App.Validations
{
    public class UniqueIdCardNo : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var db = (CafeteriaDbContext)validationContext.GetService(typeof(CafeteriaDbContext));

            var data = (from u in db.Users
                        where u.IdCardNo.Equals(value.ToString())
                        select u).SingleOrDefault();
            if (data == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Id Card Number Exists");
        }
    }
}
