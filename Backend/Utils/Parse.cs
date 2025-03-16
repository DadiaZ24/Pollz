namespace Oscars.Backend.Utils
{
	/// <summary>
	/// Configuration settings for JWT (JSON Web Token).
	/// </summary>
	public class Parser
	{
		/// <summary>
		/// Parses an object to a string.
		/// </summary>
		/// <param name="value">The object to parse.</param>
		/// <returns>The string representation of the object, or null if the object is null.</returns>
		public string? ParseString(object value)
		{
			return value?.ToString();
		}
		/// <summary>
		/// Parses an object to an integer.
		/// </summary>
		/// <param name="value">The object to parse.</param>
		/// <returns>The integer representation of the object, or null if the object is null or cannot be parsed.</returns>
		public int? ParseInt(object value)
		{
			var stringValue = value?.ToString();
			return string.IsNullOrEmpty(stringValue) ? null : (int?)int.Parse(stringValue);
		}
		/// <summary>
		/// Parses a string to a DateTime.
		/// </summary>
		/// <param name="dateTimeString">The string to parse.</param>
		/// <returns>The DateTime representation of the string, or null if the string cannot be parsed.</returns>
		public DateTime? ParseDateTime(string dateTimeString)
		{
			if (DateTime.TryParse(dateTimeString, out DateTime result))
			{
				return result;
			}
			return null;
		}
	}
}
