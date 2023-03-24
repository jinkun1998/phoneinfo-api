using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoneInfo.API.Helpers
{
	public static class SearchParser
	{
		public enum SearchType
		{
			ALL
		}

		public static object Parse(string html, SearchType type)
		{
			return type switch
			{
				SearchType.ALL => SearchAll(html),
				_ => string.Empty
			};
		}

		private static object SearchAll(string html)
		{
			HtmlDocument doc = new();
			doc.LoadHtml(html);

			IEnumerable<HtmlNode> devices = doc
				.DocumentNode
				.Descendants()
				.Where(d => d.HasClass("makers"))
				.SelectMany(d => d
					.Descendants()
					.Where(dd => dd.Name == "li"));

			return devices
				.Select(d =>
				{
					HtmlNode img = d?.Descendants()?.FirstOrDefault(dd => dd.Name == "img");
					return new
					{
						id = d?.Descendants()?.FirstOrDefault(dd => dd.Name == "a")?.Attributes["href"]?.Value?.Replace(".php", string.Empty),
						name = string.Join(" ", d?.Descendants()?.FirstOrDefault(dd => dd.Name == "span")?.InnerHtml?.Split("<br>")),
						img = img?.Attributes["src"]?.Value,
						description = img?.Attributes["title"]?.Value
					};
				});
		}
	}
}
