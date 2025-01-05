using System.ComponentModel.DataAnnotations;
using WebAPI.Abstracts;

namespace WebAPI.ValidationAttributes
{
    public class StartEndAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var timeRange = (NewsDtoAbstract)value;
            if (timeRange == null) {
                return new ValidationResult("傳入的資料未繼承自NewsDtoAbstract，所以無法使用ValidationResult驗證。");
            }

            if(timeRange.StartDateTime >= timeRange.EndDateTime)
            { 
                return new ValidationResult("開始時間不可大於結束時間");
            }

            return ValidationResult.Success;
        }
    }
}
