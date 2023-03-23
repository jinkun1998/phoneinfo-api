using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

namespace PhoneInfo.API.Helpers
{
	public static class CatalogParser
	{
		public enum CatalogType
		{
			BRANCHS, BRANCH, DEVICE
		}

		public static object Parse(string html, CatalogType type)
		{
			return type switch
			{
				CatalogType.BRANCHS => Branchs(html),
				CatalogType.BRANCH => Branch(html),
				CatalogType.DEVICE => Device(html),
				_ => string.Empty
			};
		}

		static object Branch(string html)
		{
			dynamic data = new ExpandoObject();

			HtmlDocument doc = new();
			// load html into doc
			doc.LoadHtml(html);

			// get phones from doc's elements
			data.data = doc
				.DocumentNode
				.Descendants() // get all nodes
				.Where(descendant => descendant.HasClass("makers")) // finding node with class = makers
				.SelectMany(node => node?.SelectNodes("ul/li/a")) // finding node with path ul > li > a
				.Select(node => new
				{
					name = node?.SelectSingleNode("strong/span")?.InnerText, // get text in node strong > span
					img = node?.SelectSingleNode("img")?.Attributes["src"]?.Value, // get text in attr "src" of img element
					url = node?.Attributes["href"]?.Value?.Replace(".php", string.Empty), // get text in attr "href" and replace the extension ".php" to "" of a element
					description = node?.SelectSingleNode("img")?.Attributes["title"]?.Value // get text in attr "title" of img element
				});

			List<object> pageDatas = new();

			// finding pages node with class "review-nav"
			IEnumerable<HtmlNode> pages = doc
				.DocumentNode
				.Descendants()
				.Where(descendant => descendant.HasClass("review-nav"))
				.SelectMany(node => node.Descendants());

			// get all page nodes to receive page count
			IEnumerable<HtmlNode> mergePagesNode = pages?
				.Where(descendant => descendant.HasClass("nav-pages"))
				.Select(node => new { a = node?.SelectNodes("a"), strong = node?.SelectNodes("strong") })
				.SelectMany(page => page.a.Concat(page.strong))
				.Distinct();

			foreach (HtmlNode page in mergePagesNode)
			{
				dynamic pageData = new ExpandoObject();
				pageData.pageNum = int.Parse(page.InnerText);
				if (page.Name != "strong")
				{
					// this is not current page
					pageData.url = page?.Attributes["href"].Value?.Replace(".php", string.Empty);
				}
				else
				{
					// this is current page
					pageData.active = true;
				}
				pageDatas.Add(pageData);
			}

			if (pageDatas.Any())
			{
				data.pages = pageDatas;
			}

			// get next page url
			string nextPage = pages?
				.Where(page => page.HasClass("pages-next"))
				.Select(page => page.Attributes["href"]?.Value)
				.FirstOrDefault();
			if (nextPage is not null)
			{
				if (!(nextPage.Contains('#')))
				{
					data.next = nextPage.Replace(".php", string.Empty);
				}
			}

			// get previous page url
			string prevPage = pages?
				.Where(page => page.HasClass("pages-next-prev"))
				.Select(page => page?.Attributes["href"]?.Value)
				.FirstOrDefault();
			if (prevPage is not null)
			{
				if (!(prevPage.Contains('#')))
				{
					data.prev = prevPage.Replace(".php", string.Empty);
				}
			}

			return data;
		}

		static object Branchs(string html)
		{
			HtmlDocument doc = new();
			// load html into doc
			doc.LoadHtml(html);
			return doc
				.DocumentNode
				.Descendants() // get all nodes
				.Where(descendant => descendant.HasClass("st-text")) // finding node with class name "st-text"
				.SelectMany(node => node?.SelectNodes("table/tr/td/a")) // finding nodes with path table > tr > td > a
				.Select(el => new
				{
					name = Regex.Replace(el?.InnerText?.Replace(" devices", string.Empty), @"[^\D]", string.Empty, RegexOptions.NonBacktracking),
					devices = el?.SelectSingleNode("span")?.InnerText?.Replace(" devices", string.Empty),
					url = el?.Attributes["href"]?.Value?.Replace(".php", string.Empty)
				});
		}

		static object Device(string html)
		{
			HtmlDocument doc = new();
			doc.LoadHtml(html);
			return doc
				.DocumentNode
				.Descendants()
				.Select(descendant => descendant.Attributes["data-spec"])
				.Where(descendant => descendant != null)
				.ToDictionary(descendant => descendant?.Value, descendant => Regex.Replace(HttpUtility.HtmlDecode(descendant?.OwnerNode?.InnerText), @"[\n\r]+", string.Empty, RegexOptions.NonBacktracking));
		}
	}
}
