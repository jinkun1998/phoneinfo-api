namespace PhoneInfo.API.Application.Services
{
	internal static class CommonHelper
	{
		/// <summary>
		/// Split raw price data into dynamic model
		/// </summary>
		/// <param name="price">Raw price data as string</param>
		/// <returns>{price, currency}</returns>
		public static dynamic GetPrice(string price)
		{
			string[] priceData = price.Split(' ');
			return priceData.Length < 2 ?
				new { price = 0, currency = string.Empty } :
			new { price = float.Parse(priceData[1]), currency = priceData[0] };
		}
	}
}
