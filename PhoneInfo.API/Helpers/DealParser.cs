using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PhoneInfo.API.Helpers
{
	public static class DealParser
	{
		public static class Type
		{
			public const string Deals = "DEALS";
		}
		public static object Parse(string html, string type)
		{
			return type switch
			{
				Type.Deals => Deals(html),
				_ => string.Empty
			};
		}

		static object Deals(string html)
		{
			List<object> responses = new();

			HtmlDocument doc = new();
			doc.LoadHtml(html);

			var devicesClass = doc
				.DocumentNode
				.Descendants()
				.Where(d => d.HasClass("pricecut"));

			foreach (HtmlNode device in devicesClass)
			{
				dynamic response = new ExpandoObject();

				var rowClass = device
					.ChildNodes
					.Where(n => n.HasClass("row"))
					.FirstOrDefault();

				response.image = rowClass
					.SelectSingleNode("a/img")?.Attributes["src"]?.Value;
				response.url = rowClass
					.SelectSingleNode("a")?.Attributes["href"]?.Value;
				response.title = rowClass
					.SelectSingleNode("div/div/h3")?.InnerText;
				response.link = rowClass
					.SelectSingleNode("div/div/a")?.Attributes["href"]?.Value;
				response.description = rowClass
					.SelectSingleNode("div/p/a")?.InnerText;

				var historyRowClass = device
					.ChildNodes
					.Where(n => n.HasClass("history"))
					.SelectMany(n => n.ChildNodes
						.Where(nn => nn.HasClass("row")));

				response.deal = new
				{
					memory=,
					storeImg =,
					price=,
					discount=
				};
			}

			return responses;
		}
	}
}
