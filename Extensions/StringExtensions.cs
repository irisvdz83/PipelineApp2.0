using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace PipelineApp2._0.Extensions;
public static class StringExtensions
{
    /// <summary>
    /// Checks if the source input string case-insensitive contains a given value.
    /// </summary>
    /// <param name="input">The source input string.</param>
    /// <param name="value">The given value.</param>
    /// <returns>True if the input string case-insensitive contains the given value. False otherwise.</returns>
    public static bool CaseInsensitiveContains(this string? input, string? value)
    {
        return input != null && value != null && input.Contains(value, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Checks if the source input string case-insensitive equals a given value.
    /// </summary>
    /// <param name="input">The source input string.</param>
    /// <param name="value">The given value.</param>
    /// <returns>True if the input string case-insensitive equals the given value. False otherwise.</returns>
    public static bool CaseInsensitiveEquals(this string? input, string? value)
    {
        return input != null && value != null && string.Equals(input, value, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Checks if the source input string case-insensitive starts with a given value.
    /// </summary>
    /// <param name="input">The source input string.</param>
    /// <param name="value">The given value.</param>
    /// <returns>True if the input string case-insensitive starts with the given value. False otherwise.</returns>
    public static bool CaseInsensitiveStartsWith(this string? input, string? value)
    {
        return input != null && value != null && input.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Checks if the source input string case-insensitive ends with a given value.
    /// </summary>
    /// <param name="input">The source input string.</param>
    /// <param name="value">The given value.</param>
    /// <returns>True if the input string case-insensitive ends with the given value. False otherwise.</returns>
    public static bool CaseInsensitiveEndsWith(this string? input, string? value)
    {
        return input != null && value != null && input.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Checks if the source input string is found and it is alone-standing (not part of another word).
    /// </summary>
    /// <param name="input">The source input string.</param>
    /// <param name="value">String to search for.</param>
    /// <param name="regexOptions">Regex options to be considered in the search.</param>
    /// <returns>True if the input string contains the value string and it is alone-standing.</returns>
    public static bool ContainsWholeWord(this string? input, string? value, RegexOptions regexOptions = RegexOptions.None)
    {
        if (input.IsNullOrEmpty()) return false;
        return Regex.IsMatch(input, $"\\b{value}\\b", regexOptions, TimeSpan.FromSeconds(3));
    }
}
