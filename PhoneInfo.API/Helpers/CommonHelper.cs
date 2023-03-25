namespace PhoneInfo.API.Helpers
{
	internal static class CommonHelper
	{
		public static dynamic GetPrice(string price)
		{
			string[] priceData = price.Split(new[] { ' ', ' ' }, System.StringSplitOptions.TrimEntries);
			return new { price = priceData[1], currency = priceData[0] };
		}
	}
}
