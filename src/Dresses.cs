using System.Collections.Generic;
using System.Linq;

namespace forema
{
	public static class Dresses
	{
        private static Dictionary<string, Dress> dresses = new[]
        {
            new Dress("F15607", "10204672", "http://www.davidsbridal.com/Product_one-shoulder-short-dress-with-illusion-neck-f15607"),
            new Dress("F15612", "10313316", "http://www.davidsbridal.com/Product_sleeveless-short-mesh-dress-with-side-cascade-f15612-f15612-19632--1"),
            new Dress("F15701", "10318954", "http://www.davidsbridal.com/Product_short-mesh-dress-with-sweetheart-illusion-neckline-f15701-f15701-19645--1"),
            new Dress("F15711", "10235635", "http://www.davidsbridal.com/Product_short-one-shoulder-contrast-corded-dress-f15711"),
            new Dress("F16007", "10275464", "http://www.davidsbridal.com/Product_short-mesh-bridesmaid-dress-with-cowl-back-f16007"),
            new Dress("F17019", "10407964", "http://www.davidsbridal.com/Product_short-lace-and-mesh-dress-with-illusion-neckline-f17019"),
            new Dress("F17048", "10379364", "http://www.davidsbridal.com/Product_short-strapless-bridesmaid-dress-with-pleated-top-f17048"),
            new Dress("F18031", "10569356", "http://www.davidsbridal.com/Product_sleeveless-bridesmaids-dress-with-allover-lace-f18031"),
            new Dress("F18092", "10593222", "http://www.davidsbridal.com/Product_short-convertible-mesh-dress-with-pleats-f18092"),
            new Dress("F19038", "10528518", "http://www.davidsbridal.com/Product_short-illusion-one-shoulder-dress-f19038"),
            new Dress("F19073", "10571136", "http://www.davidsbridal.com/Product_short-mesh-pleated-dress-with-halter-neckline-f19073"),
            new Dress("F19112", "10516631", "http://www.davidsbridal.com/Product_short-mesh-dress-with-high-neck-beading-f19112"),
            new Dress("W10479", "10273753", "http://www.davidsbridal.com/Product_short-mesh-dress-with-split-sleeves-w10479"),
            new Dress("W10942", "10516350", "http://www.davidsbridal.com/Product_mesh-spaghetti-strap-short-dress-w10942"),
            new Dress("W10953", "10527956", "http://www.davidsbridal.com/Product_short-strapless-mesh-dress-with-sweetheart-neck-w10953")
        }.ToDictionary(d => d.Style);

		public static Dress[] GetAll()
		{
            return dresses.Values.OrderBy(d => d.Style).ToArray();
        }

        public static Dress[] FirstPass()
        {
            return new[] {
                dresses["F18092"],
                dresses["F19038"],
                dresses["W10479"],
                dresses["F15701"],
                dresses["F15612"],
                dresses["F19112"],
                dresses["F19073"],
                dresses["W10942"],
                dresses["F16007"],
                dresses["F17048"],
                dresses["W10953"]
            };
        }

        public static Dress[] SecondPass()
        {
            return new[] {
                dresses["F15607"],
                dresses["F15701"],
                dresses["F15711"],
                dresses["F16007"],
                dresses["F18031"],
                dresses["F17019"]
            };
        }
    }
}
