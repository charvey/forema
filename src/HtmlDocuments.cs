using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace forema
{
	public static class HtmlDocuments
	{
		private static HttpClient httpClient = new HttpClient();
		private static Dictionary<string, HtmlDocument> documents = new Dictionary<string, HtmlDocument>();

		public static async Task<HtmlDocument> GetAsync(string url)
		{
			if (!documents.ContainsKey(url))
			{
				var document = new HtmlDocument();
				var html = httpClient.GetStringAsync(url);
				document.LoadHtml(await html);
				documents[url] = document;
			}
			return documents[url];
		}
	}
}
