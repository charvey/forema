using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace forema
{
	public class RootObject
	{
		public List<Update> updates { get; set; }
	}

	public class Update
	{
		public Data data { get; set; }
	}

	public class Data
	{
		public List<Photo> photos { get; set; }
		public int timestamp { get; set; }
	}

	public class Photo
	{
		public string origin { get; set; }
		public string permalink { get; set; }
		public string token { get; set; }
		public string role { get; set; }
		public string url { get; set; }
		public string image_service_url { get; set; }
		public string local_url { get; set; }
		public string display_url { get; set; }
	}

	public static class Social
	{
		private static HttpClient httpClient = new HttpClient();

		public static async Task<IEnumerable<Photo>> GetPicturesAsync(string productId)
		{
			var pictures = new List<Photo>();
			var before = int.MaxValue;
			while (true)
			{
				var url = "http://curations-api.nexus.bazaarvoice.com/content/get/"
					+ "?client=davidsbridal&passkey=kuuqd395w5u7gv43987gxshh"
					+ "&groups=dbi-brand&groups=dbi-instagram&groups=dbi-stories&groups=dbi-twitter"
					+ $"&before={before}&tags={productId}";
				var json = httpClient.GetStringAsync(url);
				var root = JsonConvert.DeserializeObject<RootObject>(await json);
				var photos = root.updates.Select(u => u.data.photos.Single());
				if (!photos.Any()) break;

				pictures.AddRange(photos);
				before = root.updates.Min(u => u.data.timestamp);
			}
			return pictures;
		}
	}
}
