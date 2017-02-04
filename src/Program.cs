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

			var chosen = new[] { "Capri", "Malibu", "Mint", "Pool", "Sea Glass", "Spa" }
				.Select(b => allColors.Single(c => c.Name == b));

			Func<Color, bool> isBlue = color => color.B > (color.G + color.R) / 2 && color.B >= color.G && color.B >= color.R;
			Func<Color, bool> isRed = color => color.R > (color.G + color.B) / 2 && color.R >= color.G && color.R >= color.B;
			Func<Color, bool> isGreen = color => color.G > (color.R + color.B) / 2 && color.G >= color.R && color.G >= color.B;
			Func<Color, bool> shouldBeBlue = color => isBlue(color) && !chosen.Contains(color);

			foreach (var color in allColors.Where(shouldBeBlue))
				Console.WriteLine($"Consider adding {color.Name} to Chosen");
			foreach (var color in chosen.Where(b => !isBlue(b)))
				Console.WriteLine($"Consider removing {color.Name} from Chosen");

            var dressSets = new Tuple<string, Dress[]>[]
            {
                Tuple.Create("All Dresses",Dresses.GetAll()),
                Tuple.Create("Early Ideas",Dresses.FirstPass()),
                Tuple.Create("Final Choices",Dresses.SecondPass()),
            };

            var colorSets = new Tuple<string, Color[]>[]
            {
                Tuple.Create("All Colors",allColors.OrderBy(c=>c.Name).ToArray()),
                Tuple.Create("Chosen Colors", chosen.OrderByDescending(c => c.R + c.G + c.B).ToArray()),
                Tuple.Create("Dark to Light",allColors.OrderBy(c => c.R + c.G + c.B).ToArray()),
                Tuple.Create("Reds", allColors.Where(isRed).OrderByDescending(c => c.R + c.B + c.G).ToArray()),
                Tuple.Create("Greens", allColors.Where(isGreen).OrderByDescending(c => c.R + c.B + c.G).ToArray()),
                Tuple.Create("Blues", allColors.Where(isBlue).OrderByDescending(c => c.R + c.B + c.G).ToArray())
            };

            File.WriteAllText("index.html", WriteIndex(dressSets, colorSets));
		}

		private static string WriteIndex(Tuple<string, Dress[]>[] dressSets, Tuple<string, Color[]>[] colorSets)
		{
			var html = "<html>\n";
			html += "\t<head>\n";
			html += "\t\t<title>F&oacute;rema</title>\n";
            html += "\t\t<style>\n";
            html += "\t\t\ttd,th {border: 1px solid lightgray; padding: 10px}\n";
            html += "\t\t</style>\n";
			html += "\t</head>\n";
			html += "\t<body style='font-family:arial;'>\n";
			html += "\t\t<h1>Fórema</h1>\n";
			html += "\t\t<table style='border-collapse: collapse'>\n";
            html += "\t\t\t<tr>\n";
            html += "\t\t\t\t<th></th>\n";
            foreach (var dressSet in dressSets)
                html += $"\t\t\t\t<th>{dressSet.Item1}</th>\n";
            html += "\t\t\t</tr>\n";
            foreach (var colorSet in colorSets)
            {
                html += "\t\t\t<tr>\n";
                html += $"\t\t\t\t<th>{colorSet.Item1}</th>\n";
                foreach (var dressSet in dressSets)
                {
                    html += $"\t\t\t\t<td>\n";
                    var filename = (dressSet.Item1 + "_" + colorSet.Item1).Replace(' ', '-') + ".html";
                    File.WriteAllText(filename, WriteDocument(dressSet.Item1 + " - " + colorSet.Item1, dressSet.Item2, colorSet.Item2));
                    html += $"\t\t\t\t\t<a href='{filename}'>{dressSet.Item2.Length * colorSet.Item2.Length} Dresses</a>\n";
                    html += $"\t\t\t\t</td>\n";
                }
                html += "\t\t\t</tr>\n";
            }
            html += "\t\t</table>\n";
			html += "\t</body>\n";
			html += "</html>";
			return html;
		}

		private static string WriteDocument(string name, IEnumerable<Dress> dresses, IEnumerable<Color> colors)
		{
			var html = "<html>\n";
			html += "\t<head>\n";
			html += $"\t\t<title>{name}</title>\n";
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
			return html;
		}
	}
}
