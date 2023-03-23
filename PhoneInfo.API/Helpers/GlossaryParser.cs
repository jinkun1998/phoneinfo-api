using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PhoneInfo.API.Helpers
{
	public static class GlossaryParser
	{
		public enum GlossaryType
		{
			LIST, TERM
		}

		public static object Parse(string html, GlossaryType type)
		{
			return type switch
			{
				GlossaryType.LIST => Glossaries(html),
				GlossaryType.TERM => Term(html),
				_ => string.Empty
			};
		}

		private static object Term(string html)
		{
			HtmlDocument doc = new();
			doc.LoadHtml(html);

			HtmlNode body = doc
				.DocumentNode
				.Descendants()?
				.Where(d => d.Id == "body")?
				.FirstOrDefault();


			string title = body
				.Descendants()?
				.Where(c => c.HasClass("review-header"))?
				.SelectMany(d => d.
					Descendants()?
					.Where(dc => dc.HasClass("article-hgroup")))?
				.FirstOrDefault()?
				.SelectSingleNode("h1")?
				.InnerText;

			string text = body
				.Descendants()?
				.FirstOrDefault(c => c.HasClass("st-text") && c.HasChildNodes)?
				.InnerHtml;

			return new { title, text };
		}

		private static object Glossaries(string html)
		{
			List<dynamic> responses = new();

			HtmlDocument doc = new();
			doc.LoadHtml(html);

			IEnumerable<HtmlNode> termsClass = doc
				.DocumentNode
				.Descendants()?
				.Where(d => d.HasClass("st-text"))?
				.SelectMany(d => d.ChildNodes)
				.Where(d => d.Name != "#text");

			for (int i = 0; i < termsClass?.Count(); i++)
			{
				if (i % 2 == 0)
				{
					dynamic index = new ExpandoObject();
					index.letter = termsClass?.ElementAtOrDefault(i)?.InnerText;
					index.list = new List<dynamic>();
					responses.Add(index);
				}
				else
				{
					IEnumerable<HtmlNode> terms = termsClass?
						.ElementAtOrDefault(i)?
						.ChildNodes?
						.Where(c => c.Name == "a");

					foreach (HtmlNode term in terms)
					{
						dynamic index = new ExpandoObject();
						index.id = term?.Attributes["href"]?.Value.Replace("glossary.php3?term=", string.Empty);
						index.name = term?.InnerText;
						responses[i / 2]?.list.Add(index);
					}
				}
			}

			return responses;
		}
	}
}
