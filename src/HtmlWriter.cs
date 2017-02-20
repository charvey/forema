using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace forema
{
	public class HtmlWriter
	{
		private StringBuilder builder = new StringBuilder();
		private Stack<string> stack = new Stack<string>();

		public void StartTag(string name, string attributes = null)
		{
			WriteLine($"<{name}{AttributeText(attributes)}>");
			stack.Push(name);
		}

		public void EndTag(string name)
		{
			Debug.Assert(name == stack.Pop());
			WriteLine($"</{name}>");
		}

		public void WriteElement(string name, string attributes = null)
		{
			WriteLine($"<{name}{AttributeText(attributes)}/>");
		}

		public void WriteElement(string name, string attributes, string contents)
		{
			WriteLine($"<{name}{AttributeText(attributes)}>{contents}</{name}>");
		}

		public void WriteLine(string text)
		{
			builder.AppendLine(new string('\t', stack.Count) + text);
		}

		private string AttributeText(string attributes)
		{
			return attributes == null
				? string.Empty
				: " " + attributes;
		}

		public override string ToString()
		{
			return builder.ToString();
		}
	}
}
