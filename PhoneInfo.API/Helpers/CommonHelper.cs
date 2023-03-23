namespace PhoneInfo.API.Helpers
{
	internal static class CommonHelper
	{
		public static dynamic GetPrice(string price)
		{
			string[] priceData = price.Split(' ');
			return new { price = priceData[1], currency = priceData[0] };
		}
	}
}
