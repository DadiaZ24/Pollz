namespace Oscars.Backend.Utils
{
	public class Parser
	{
		public string? ParseString(object value)
		{
			return value?.ToString();
		}

		public int? ParseInt(object value)
		{
			var stringValue = value?.ToString();
			return string.IsNullOrEmpty(stringValue) ? null : (int?)int.Parse(stringValue);
		}

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