using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forema
{
	public static class Colors
	{
		private static readonly string[] SupplementalProductPages = new[]
		{
			"http://www.davidsbridal.com/Product_short-mesh-cowl-back-bridesmaid-dress-f16007"
		};
		private static ISet<Color> all;

		public static ISet<Color> GetAll()
		{
			if (all == null)
			{
				all = new HashSet<Color>();

				var colorTasks = Dresses.GetAll().Select(d => GetColorsAsync(d.Link));
				var supplementalTasks = SupplementalProductPages.Select(d => GetColorsAsync(d));

				foreach (var colorTask in colorTasks)
					foreach (var color in colorTask.Result)
						all.Add(color);

				foreach (var supplementalTask in supplementalTasks)
					foreach (var color in supplementalTask.Result)
						if (!all.Contains(color))
						{
							Console.WriteLine($"Does not contain {color.Name}");
							all.Add(color);
						}
			}
			return all;
		}

		private static async Task<IEnumerable<Color>> GetColorsAsync(string url)
		{
			return (await HtmlDocuments.GetAsync(url)).DocumentNode
				.SelectNodes("//div[@id='product_color_swatch_heading']//div[@id]")
				.Select(d => new Color(d.Attributes["title"].Value, d.Attributes["style"].Value.Replace("background-color:#", "").Replace(";", "")));
		}
	}
}
