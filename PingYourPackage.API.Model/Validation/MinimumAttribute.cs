using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumAttribute : ValidationAttribute
    {
        private readonly int minimumValue;

        public MinimumAttribute(int minimum) :
            base(errorMessage: "The {0} field value must be minimum {1}.")
        {

            this.minimumValue = minimum;
        }

        public override string FormatErrorMessage(string name)
        {

            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                this.minimumValue);
        }

        public override bool IsValid(object value)
        {

            int intValue;
            if (value != null && int.TryParse(value.ToString(), out intValue))
            {

                return (intValue >= this.minimumValue);
            }

            return false;
        }
    }
}
