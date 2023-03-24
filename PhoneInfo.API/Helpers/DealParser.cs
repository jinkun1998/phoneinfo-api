using HtmlAgilityPack;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace PhoneInfo.API.Helpers
{
	public static class DealParser
	{
		public enum DealType
		{
			DEALS
		}
		public static object Parse(string html, DealType type)
		{
			return type switch
			{
				DealType.DEALS => Deals(html),
				_ => string.Empty
			};
		}

		static object Deals(string html)
		{
			List<object> responses = new();

			HtmlDocument doc = new();
			doc.LoadHtml(html);

			IEnumerable<HtmlNode> devicesClass = doc
				.DocumentNode
				.Descendants()?
				.Where(d => d.HasClass("pricecut"));

			foreach (HtmlNode device in devicesClass)
			{
				dynamic response = new ExpandoObject();

				HtmlNode rowClass = device
					.ChildNodes?
					.Where(n => n.HasClass("row"))?
					.FirstOrDefault();

				response.image = rowClass
					.SelectSingleNode("a/img")?.Attributes["src"]?.Value;
				response.url = rowClass
					.SelectSingleNode("a")?.Attributes["href"]?.Value;
				response.title = rowClass
					.SelectSingleNode("div/div/h3")?.InnerText;
				response.id = rowClass
					.SelectSingleNode("div/div/a")?.Attributes["href"]?.Value;
				response.description = rowClass
					.SelectSingleNode("div/p/a")?.InnerText;

				IEnumerable<HtmlNode> dealClass = rowClass
					.Descendants()?
					.Where(n => n.HasClass("deal"))?
					.FirstOrDefault()
					.ChildNodes;

				dynamic priceData = CommonHelper.GetPrice(HttpUtility.HtmlDecode(dealClass?.Where(n => n.HasClass("price"))?.FirstOrDefault()?.InnerText));
				response.deal = new
				{
					memory = dealClass?.Where(n => n.HasClass("memory"))?.FirstOrDefault()?.InnerText,
					storeImage = dealClass?.Where(n => n.HasClass("store"))?.FirstOrDefault().SelectSingleNode("img")?.Attributes["src"]?.Value,
					priceData.price,
					priceData.currency,
					discount = float.Parse(dealClass?.Where(n => n.HasClass("discount"))?.FirstOrDefault()?.InnerText)
				};

				IEnumerable<HtmlNode> historyStatsClass = device
					.ChildNodes?
					.Where(n => n.HasClass("history"))?
					.SelectMany(n => n.Descendants()?
						.Where(nn => nn.HasClass("stats"))
						.SelectMany(nn => nn.ChildNodes))
					.Where(n => n.Name != "#text");

				response.history = GetHistoryStats(historyStatsClass);

				responses.Add(response);
			}

			return responses;
		}

		private static IEnumerable<object> GetHistoryStats(IEnumerable<HtmlNode> historyStatsClass)
		{
			List<dynamic> histories = new();
			for (int i = 0; i < historyStatsClass?.Count(); i++)
			{
				if (i % 2 == 0)
				{
					dynamic index = new ExpandoObject();
					index.time = historyStatsClass?.ElementAtOrDefault(i)?.InnerText;
					histories.Add(index);
				}
				else
				{
					dynamic priceData = HttpUtility.HtmlDecode(historyStatsClass?.ElementAtOrDefault(i)?.InnerText);
					histories[i / 2].price = priceData.price;
					histories[i / 2].currency = priceData.currency;
				}
			}
			return histories;
		}
	}
}
