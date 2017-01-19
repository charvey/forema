using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace forema
{
	public static class Colors
	{
		private static HttpClient httpClient = new HttpClient();
		private static readonly string[] SupplementalProductPages = new[]
		{
			"http://www.davidsbridal.com/Product_short-mesh-cowl-back-bridesmaid-dress-f16007"
		};

		public static ISet<Color> GetAll()
		{
			var colors = new HashSet<Color>();

			var colorTasks = Dresses.GetAll().Select(d => GetColorsAsync(new Uri(d.Link)));
			var supplementalTasks = SupplementalProductPages.Select(d => GetColorsAsync(new Uri(d)));

			foreach (var colorTask in colorTasks)
				foreach (var color in colorTask.Result)
					colors.Add(color);

			foreach (var supplementalTask in supplementalTasks)
				foreach (var color in supplementalTask.Result)
					if (!colors.Contains(color))
					{
						Console.WriteLine($"Does not contain {color.Name}");
						colors.Add(color);
					}

			return colors;
		}

		private static async Task<IEnumerable<Color>> GetColorsAsync(Uri uri)
		{
			var html = httpClient.GetStringAsync(uri);
			var document = new HtmlDocument();
			document.LoadHtml(await html);
			return document.DocumentNode
				.SelectNodes("//div[@id='product_color_swatch_heading']//div[@id]")
				.Select(d => new Color(d.Attributes["title"].Value, d.Attributes["style"].Value.Replace("background-color:#", "").Replace(";", "")));
		}
	}
}
