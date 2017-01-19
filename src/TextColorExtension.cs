namespace forema
{
	public static class TextColorExtension
	{
		public static Color BestTextColor(this Color color)
		{
			return (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) > 186
					? new Color("black", "000000")
					: new Color("white", "ffffff");
		}
	}
}
