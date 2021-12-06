using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace event_client_app.Configure.ValidationAttribute
{
    public class Attribute
    {
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        public class DateInTheFutureAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext context)
            {
                var futureDate = value as DateTime?;
                var memberNames = new List<string>() { context.MemberName };

                if (futureDate != null)
                {
                    if (futureDate.Value.Date < DateTime.UtcNow.Date)
                    {
                        return new ValidationResult("This must be a date in the future", memberNames);
                    }
                }

                return ValidationResult.Success;
            }
        }
    }
}