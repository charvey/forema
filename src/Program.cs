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

			var chosen = new[] { "Capri", "Ice Blue", "Malibu", "Meadow", "Mint", "Oasis", "Pool", "Spa" }
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

		private static string WriteDress(Dress dress)
		{
			var htmlWriter = new HtmlWriter();
			htmlWriter.StartTag("html");
			htmlWriter.StartTag("head");
			htmlWriter.WriteElement("title", null, $"F&oacute;rema - {dress.Style}");
			htmlWriter.StartTag("style");
			htmlWriter.WriteLine("div.column{width:25%;float:left;}");
			htmlWriter.WriteLine("img{width:100%;}");
			htmlWriter.EndTag("style");
			htmlWriter.EndTag("head");
			htmlWriter.StartTag("body", "style='font-family:arial;'");
			htmlWriter.WriteElement("h1", null, $"{dress.Style} {dress.Name}");

			var pictures = Social.GetPicturesAsync(dress.ProductId).Result
				.Select((p, i) => Tuple.Create(p, i % 4)).GroupBy(x => x.Item2)
				.Select(g => g.Select(x => x.Item1));

			foreach (var column in pictures)
			{
				htmlWriter.StartTag("div", "class='column'");
				foreach (var photo in column)
				{
					htmlWriter.StartTag("a", $"href='{photo.permalink}'");
					htmlWriter.WriteElement("img", $"src='{photo.url}' onerror=\"this.style.display='none'\"");
					htmlWriter.EndTag("a");
				}
				htmlWriter.EndTag("div");
			}
			htmlWriter.EndTag("body");
			htmlWriter.EndTag("html");
			return htmlWriter.ToString();
		}

		private static string WriteIndex(Tuple<string, Dress[]>[] dressSets, Tuple<string, Color[]>[] colorSets)
		{
			var htmlWriter = new HtmlWriter();
			htmlWriter.StartTag("html");
			htmlWriter.StartTag("head");
			htmlWriter.WriteElement("title", null, "F&oacute;rema");
			htmlWriter.StartTag("style");
			htmlWriter.WriteLine("td,th {border: 1px solid lightgray; padding: 10px}");
			htmlWriter.EndTag("style");
			htmlWriter.EndTag("head");
			htmlWriter.StartTag("body", "style='font-family:arial;'");
			htmlWriter.WriteElement("h1", null, "Fórema");
			htmlWriter.StartTag("table", "style='border-collapse: collapse'");
			htmlWriter.StartTag("tr");
			htmlWriter.WriteElement("th", null, "");
			foreach (var dressSet in dressSets)
				htmlWriter.WriteElement("th", null, dressSet.Item1);
			htmlWriter.EndTag("tr");
			foreach (var colorSet in colorSets)
			{
				htmlWriter.StartTag("tr");
				htmlWriter.WriteElement("th", null, colorSet.Item1);
				foreach (var dressSet in dressSets)
				{
					htmlWriter.StartTag("td");
					var filename = (dressSet.Item1 + "_" + colorSet.Item1).Replace(' ', '-') + ".html";
					File.WriteAllText(filename, WriteDocument(dressSet.Item1 + " - " + colorSet.Item1, dressSet.Item2, colorSet.Item2));
					htmlWriter.WriteElement("a", $"href='{filename}'", $"{dressSet.Item2.Length * colorSet.Item2.Length} Dresses");
					htmlWriter.EndTag("td");
				}
				htmlWriter.EndTag("tr");
			}
			htmlWriter.EndTag("table");
			htmlWriter.EndTag("body");
			htmlWriter.EndTag("html");
			return htmlWriter.ToString();
		}

		private static string WriteDocument(string name, IEnumerable<Dress> dresses, IEnumerable<Color> colors)
		{
			var htmlWriter = new HtmlWriter();
			htmlWriter.StartTag("html");
			htmlWriter.StartTag("head");
			htmlWriter.WriteElement("title", null, name);
			htmlWriter.StartTag("style");
			htmlWriter.WriteLine("table,img {width:100%;}");
			htmlWriter.WriteLine("td {text-align:center;}");
			htmlWriter.EndTag("style");
			htmlWriter.EndTag("head");
			htmlWriter.StartTag("body", "style='font-family:arial;'");
			htmlWriter.StartTag("table");
			htmlWriter.StartTag("tr");
			htmlWriter.WriteElement("th", null, "Color");
			foreach (var dress in dresses)
			{
				htmlWriter.StartTag("th");
				htmlWriter.WriteElement("a", $"href='{dress.Link}'", dress.Style);
				htmlWriter.WriteElement("br");

				var filename = $"{dress.Style}.html";
				if (!File.Exists(filename))
					File.WriteAllText(filename, WriteDress(dress));

				htmlWriter.WriteElement("a", $"href='{filename}' style='font-size:small;'", "See it in action");
				htmlWriter.EndTag("th");
			}
			htmlWriter.EndTag("tr");
			foreach (var color in colors)
			{
				htmlWriter.StartTag("tr");
				htmlWriter.WriteElement("td", $"style='background-color:#{color.HexCode};color:#{color.BestTextColor().HexCode};'", color.Name);
				foreach (var dress in dresses)
				{
					htmlWriter.StartTag("td", $"style='background-color: rgba({color.R},{color.G},{color.B},0.25)'");
					htmlWriter.WriteElement("img", $"src='{dress.Image(color)}' onerror=\"this.style.display='none'\"");
					htmlWriter.EndTag("td");
				}
				htmlWriter.EndTag("tr");
			}
			htmlWriter.EndTag("table");
			htmlWriter.EndTag("body");
			htmlWriter.EndTag("html");
			return htmlWriter.ToString();
		}
	}
}
