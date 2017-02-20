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
				return HtmlDocuments.GetAsync(Link).Result.DocumentNode
					.SelectSingleNode("//h1").NextSibling
					.InnerText.Trim();
			}
		}
		public string Image(Color color)
		{
			return $"http://img.davidsbridal.com/is/image/DavidsBridalInc/Image-{Style}-{ProductId}-{color.Name.Replace(" ", "")}?wid=508&hei=763&fmt=jpg";
		}

		public Dress(string style, string productId, string link)
		{
			this.Style = style;
			this.ProductId = productId;
			this.Link = link;
		}
	}
}
