using System;

namespace AzureIntro.AzureHelpers
{
    public static class StringExtensions
    {
        public static string ToNullIfEmptyOrWhitespace(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;

            return value;
        }
    }
}