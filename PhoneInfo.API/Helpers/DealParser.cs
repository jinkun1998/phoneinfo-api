using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Web;

namespace PhoneInfo.API.Helpers
{
	public static class DealParser
	{
		public enum DealType
		{
			Deals, Top10DailyInterest, Top10ByFans
		}
		public static object Parse(string html, DealType type)
		{
			return type switch
			{
				DealType.Deals => Deals(html),
				DealType.Top10DailyInterest => GetRank(html, "green"),
				DealType.Top10ByFans => GetRank(html, "blue"),
				_ => string.Empty
			};
		}

		private static object GetRank(string html, string className)
		{
			HtmlDocument doc = new();
			doc.LoadHtml(html);

			//load category nodes (.sidebar.col.left > .module.module-rankings.s3 > .module-fit.{className})
			HtmlNode category = doc
				.DocumentNode
				.Descendants()?
				.Where(d => d.HasClass("sidebar") && d.HasClass("col") && d.HasClass("left")) // .sidebar.col.left
				.SelectMany(d => d.ChildNodes)?
				.Where(d => d.HasClass("module") && d.HasClass("module-rankings") && d.HasClass("s3"))? // .module.module-rankings.s3
				.SelectMany(d => d.ChildNodes)?
				.FirstOrDefault(n => n.HasClass("module-fit") && n.HasClass(className)); // module-fit.{className}

			if (category != null)
			{
				// .module.module-rankings.s3 > h4
				string categoryName = category?.ParentNode?.SelectSingleNode("h4")?.InnerText;
				// module-fit.{className} > tbody > tr
				IEnumerable<dynamic> ranks = category?
					.SelectNodes("tbody/tr")? // tbody > tr
					.Where(n => !Regex.IsMatch(n.InnerHtml, @"[\n\r]+", RegexOptions.NonBacktracking)) // just get non-empty nodes
					.Select((tr, index) =>
					{
						// tr > td[headers='th3a']
						string position = tr?.ChildNodes?.FirstOrDefault(c => c?.Attributes["headers"]?.Value == "th3a")?.InnerText?.Replace(".", "");
						if (int.TryParse(position, out int order))
						{
							dynamic element = new ExpandoObject();
							element.position = order;
							element.name = tr?.SelectSingleNode("*/a/nobr")?.InnerText; // tr > * > a > nobr
							element.id = tr?.SelectSingleNode("*/a")?.Attributes["href"]?.Value?.Replace(".php", string.Empty); //tr > * > a[href]
							element.score = tr?
							.ChildNodes?
							.FirstOrDefault(c => c?.Attributes["headers"]?.Value == "th3c")? // tr > td[headers='th3c']
							.InnerText?
							.Replace(",", "");
							return element;
						}
						return default;
					});

				return new
				{
					category = categoryName,
					ranks
				};
			}
			return default;
		}

		static object Deals(string html)
		{
			// create doc
			HtmlDocument doc = new();
			// load doc
			doc.LoadHtml(html);

			// get all node has class = 'pricecut'
			IEnumerable<HtmlNode> devicesClass = doc
				.DocumentNode
				.Descendants()?
				.Where(d => d.HasClass("pricecut"));

			return devicesClass
				.Select(device =>
				{
					dynamic response = new ExpandoObject();

					// get node with class 'row', which is child of device node
					HtmlNode rowClass = device
						.ChildNodes?
						.Where(n => n.HasClass("row"))?
						.FirstOrDefault();

					// get src attr from img node (.row > a > img)
					response.image = rowClass
						.SelectSingleNode("a/img")?.Attributes["src"]?.Value;
					// get href attr from a node
					response.url = rowClass
						.SelectSingleNode("a")?.Attributes["href"]?.Value;
					// get text of h3 node (.row > div > div > h3div > div > h3)
					response.title = rowClass
						.SelectSingleNode("div/div/h3")?.InnerText;
					// get href attr from a node (row >div > div > a)
					response.id = rowClass
						.SelectSingleNode("div/div/a")?.Attributes["href"]?.Value;
					// get text of a node (.row > div > p > a)
					response.description = rowClass
						.SelectSingleNode("div/p/a")?.InnerText;

					// get all child node of node has 'deal' class (.row > .deal)
					IEnumerable<HtmlNode> dealClass = rowClass
						.Descendants()?
						.Where(n => n.HasClass("deal"))?
						.FirstOrDefault()
						.ChildNodes;

					// get price text from node has 'price' class (.row > .deal > .price)
					dynamic priceData = CommonHelper.GetPrice(HttpUtility.HtmlDecode(dealClass?.Where(n => n.HasClass("price"))?.FirstOrDefault()?.InnerText));
					response.deal = new
					{
						// .deal > .memory
						memory = dealClass?.Where(n => n.HasClass("memory"))?.FirstOrDefault()?.InnerText,
						// .deal > .store > img
						storeImage = dealClass?.Where(n => n.HasClass("store"))?.FirstOrDefault()?.SelectSingleNode("img")?.Attributes["src"]?.Value,
						priceData.price,
						priceData.currency,
						// .deal > .discount
						discount = float.Parse(dealClass?.Where(n => n.HasClass("discount"))?.FirstOrDefault()?.InnerText)
					};

					// .pricecut > .stats
					IEnumerable<HtmlNode> historyStatsClass = device
						.ChildNodes?
						.Where(n => n.HasClass("history"))?
						.SelectMany(n => n.Descendants()?
							.Where(nn => nn.HasClass("stats"))
							.SelectMany(nn => nn.ChildNodes))
						.Where(n => n.Name != "#text");

					response.history = GetHistoryStats(historyStatsClass);

					return response;
				});
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
					dynamic priceData = CommonHelper.GetPrice(HttpUtility.HtmlDecode(historyStatsClass?.ElementAtOrDefault(i)?.InnerText));
					histories[i / 2].price = priceData.price;
					histories[i / 2].currency = priceData.currency;
				}
			}
			return histories;
		}
	}
}
