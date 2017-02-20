using HtmlAgilityPack;
using System;
using System.Net.Http;

namespace forema
{
	public class Dress
	{
		public string Style { get; set; }
		public string ProductId { get; set; }
		public string Link { get; set; }
		public string Name
		{
			get
			{
				return document.Value.DocumentNode
					.SelectSingleNode("//h1").NextSibling
					.InnerText.Trim();
			}
		}

		public Dress(string style, string productId, string link)
		{
			this.Style = style;
			this.ProductId = productId;
			this.Link = link;

			this.document = new Lazy<HtmlDocument>(() =>
			{
				var html = httpClient.GetStringAsync(link);
				var document = new HtmlDocument();
				document.LoadHtml(html.Result);
				return document;
			});
		}

		private static HttpClient httpClient = new HttpClient();
		private Lazy<HtmlDocument> document;
	}
}
