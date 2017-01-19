namespace forema
{
	public class Dress
	{
		public string Style { get; set; }
		public string ProductId { get; set; }
		public string Link { get; set; }

		public Dress(string style, string productId, string link)
		{
			this.Style = style;
			this.ProductId = productId;
			this.Link = link;
		}
	}
}
