using System;

namespace forema
{
	public class Color
	{
		public string Name { get; set; }
		public string HexCode { get; set; }

		public byte R { get { return Convert.ToByte(HexCode.Substring(0, 2), 16); } }
		public byte G { get { return Convert.ToByte(HexCode.Substring(2, 2), 16); } }
		public byte B { get { return Convert.ToByte(HexCode.Substring(4, 2), 16); } }

		public Color(string name, string hexCode)
		{
			this.Name = name;
			this.HexCode = hexCode;
		}

		public override bool Equals(object obj)
		{
			var otherColor = obj as Color;
			if (otherColor == null) return false;
			return this.Name == otherColor.Name
				&& this.HexCode == otherColor.HexCode;
		}

		public override int GetHashCode()
		{
			return (Name + HexCode).GetHashCode();
		}
	}
}
