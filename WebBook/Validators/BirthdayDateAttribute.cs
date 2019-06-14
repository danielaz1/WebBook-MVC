using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBook.Validators
{
    public class BirthdayDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime) value;
                DateTime now = DateTime.Now;
                TimeSpan diff = now - date;

                int years = 365 * 18;

                if (diff.TotalDays >= years)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("For adults only");

            }
            return new ValidationResult("Empty Date");
        }
    }

}
