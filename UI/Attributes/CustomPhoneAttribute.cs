using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UI.Models
{
    internal class CustomPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            return Regex.IsMatch(value.ToString(), @"^\(\d\d\d+\) \d\d\d+-\d\d\d\d+$");
        }
    }
}