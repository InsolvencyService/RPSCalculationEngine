using FluentValidation.Results;
using System.Text;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.Common.Extensions
{
    public static class FluentValidationExtensions
    {
        public static string GetErrorsAsString(this IList<ValidationFailure> errors)
        {
            var bdr = new StringBuilder();
            foreach (var error in errors)
            {
                if (bdr.Length > 0)
                    bdr.Append(", ");
                bdr.Append(error.ErrorMessage);
            }

            return bdr.ToString();
        }
    }
}
