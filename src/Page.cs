using System.Collections.Generic;

namespace forema
{
	public class Page
	{
		public string Name { get; set; }
		public string Filename { get; set; }
		public IEnumerable<Dress> Dresses { get; set; }
		public IEnumerable<Color> Colors { get; set; }

		public Page(string name, string filename, IEnumerable<Dress> dresses, IEnumerable<Color> colors)
		{
			this.Name = name;
			this.Filename = filename;
			this.Dresses = dresses;
			this.Colors = colors;
		}
	}
}
