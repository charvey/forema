using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace forema
{
	class Program
	{
		static void Main(string[] args)
		{
			var allColors = Colors.GetAll();

			var amanadasBlue = new[] { "Capri", "Cobalt", "Cornflower", "Horizon", "Ice Blue", "Malibu", "Marine", "Morning Glory", "Pacific", "Pool", "Regency", "Sea Glass", "Spa", "Steel Blue" }
				.Select(b => allColors.Single(c => c.Name == b));

			Func<Color, bool> isBlue = color => color.B > (color.G + color.R) / 2 && color.B >= color.G && color.B >= color.R;
			Func<Color, bool> isRed = color => color.R > (color.G + color.B) / 2 && color.R >= color.G && color.R >= color.B;
			Func<Color, bool> isGreen = color => color.G > (color.R + color.B) / 2 && color.G >= color.R && color.G >= color.B;
			Func<Color, bool> shouldBeBlue = color => isBlue(color) && !amanadasBlue.Contains(color);

			foreach (var color in allColors.Where(shouldBeBlue))
				Console.WriteLine($"Consider adding {color.Name} to blues");
			foreach (var color in amanadasBlue.Where(b => !isBlue(b)))
				Console.WriteLine($"Consider removing {color} from blues");

			WriteDocument("all.html", Dresses.GetAll(), allColors.OrderBy(c => c.Name));
			WriteDocument("amanda-blues.html", Dresses.GetAll(), amanadasBlue.OrderByDescending(c => c.R + c.B + c.G));
			WriteDocument("sorted.html", Dresses.GetAll(), allColors.OrderBy(c => c.R + c.G + c.B));
			WriteDocument("should-be-blue.html", Dresses.GetAll(), allColors.Where(shouldBeBlue).OrderBy(c => c.R + c.G + c.B));
			WriteDocument("red.html", Dresses.GetAll(), allColors.Where(isRed).OrderByDescending(c => c.R + c.B + c.G));
			WriteDocument("green.html", Dresses.GetAll(), allColors.Where(isGreen).OrderByDescending(c => c.R + c.B + c.G));
			WriteDocument("blue.html", Dresses.GetAll(), allColors.Where(isBlue).OrderByDescending(c => c.R + c.B + c.G));

			Console.ReadLine();
		}

		private static void WriteDocument(string filename, IEnumerable<Dress> dresses, IEnumerable<Color> colors)
		{
			var html = "<html>\n";
			html += "\t<head>\n";
			html += "\t<title>Fórema</title>"
			html += "\t\t<style>table,img {width:100%;} td {text-align:center;}</style>\n";
			html += "\t</head>\n";
			html += "\t<body style='font-family:arial;'>\n";
			html += "\t\t<table>\n";
			html += "\t\t\t<tr>\n";
			html += "\t\t\t\t<th>Color</th>\n";
			foreach (var dress in dresses)
				html += $"\t\t\t\t<th><a href='{dress.Link}'>{dress.Style}</a></th>\n";
			html += "\t\t\t</tr>\n";
			foreach (var color in colors)
			{
				html += "\t\t\t<tr>\n";
				html += $"\t\t\t\t<td style='background-color:#{color.HexCode};color:#{color.BestTextColor().HexCode};'>{color.Name}</td>\n";
				foreach (var dress in dresses)
				{
					html += $"\t\t\t\t<td style='background-color: rgba({color.R},{color.G},{color.B},0.25)'>\n";
					var src = $"http://img.davidsbridal.com/is/image/DavidsBridalInc/Image-{dress.Style}-{dress.ProductId}-{color.Name.Replace(" ", "")}?wid=508&hei=763&fmt=jpg";
					html += $"\t\t\t\t\t<img src='{src}' onerror=\"this.style.display='none'\" />\n";
					html += "\t\t\t\t</td>\n";
				}
				html += "\t\t\t</tr>\n";
			}
			html += "\t\t</table>\n";
			html += "\t</body>\n";
			html += "</html>";
			File.WriteAllText(filename, html);
		}
	}
}
