﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using ImageProcessor;
using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Gibe.Umbraco.IntelligentMedia.Azure
{
	public class AzureVisionApi : IVisionApi
	{
		private readonly string _subscriptionKey;
		private readonly string _region;

		public AzureVisionApi(IntelligentMediaSettings settings)
		{
			_subscriptionKey = settings.AzureSubscriptionKey;
			_region = settings.AzureRegion;
		}

		public async Task<IVisionResponse> MakeRequest(IMedia media)
		{
			var client = new HttpClient();
			var queryString = HttpUtility.ParseQueryString(string.Empty);

			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

			queryString["visualFeatures"] = "Tags,Description,Categories,Faces,Color";
			queryString["language"] = "en";

			var uri = $"https://{_region}.api.cognitive.microsoft.com/vision/v1.0/analyze?{queryString}";
			
			var umbracoFileString = media.GetValue<string>("umbracoFile");
			var umbracoFile = JsonConvert.DeserializeObject<UmbracoFileData>(umbracoFileString);
			var byteData = GetImageAsByteArray(HttpContext.Current.Server.MapPath(umbracoFile.Src));
			
			using (var content = new ByteArrayContent(byteData))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				return await client.PostAsync(uri, content).ContinueWith(ConvertResponse).Result;
			}
		}

		public async Task<IVisionResponse> ConvertResponse(Task<HttpResponseMessage> httpResponse)
		{
			var json = await httpResponse.Result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<VisionResponse>(json);

			return new AzureVisionResponse(response, json);
		}

		private static byte[] GetImageAsByteArray(string imageFilePath)
		{
			var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
			
			using (var outStream = new MemoryStream())
			{
				using (var imageFactory = new ImageFactory())
				{
					imageFactory.Load(fileStream)
						.Resize(new Size(1000, 1000))
						.Save(outStream);

				}
				outStream.Position = 0;
				return new BinaryReader(outStream).ReadBytes((int)outStream.Length);
			}
		}

		public class UmbracoFileData
		{
			public string Src { get; set; }
		}

		public class VisionResponse
		{
			public Tag[] Tags { get; set; }
			public Category[] Categories { get; set; }
			public Description Description { get; set; }
			public Colors Color { get; set; }
			public IEnumerable<Face> Faces { get; set; }
		}

		public class Face
		{
			public int Age { get; set; }
			public string Gender { get; set; }
		}

		public class Colors
		{
			public string DominantColorForeground { get; set; }
			public string DominantColorBackground { get; set; }
		}

		public class Tag
		{
			public string Name { get; set; }
			public decimal Confidence { get; set; }
		}

		public class Category
		{
			public string Name { get; set; }
			public decimal Score { get; set; }
		}

		public class Description
		{
			public string[] Tags { get; set; }
			public Caption[] Captions { get; set; }
		}

		public class Caption
		{
			public string Text { get; set; }
			public decimal Confidence { get; set; }
		}
	}
	
}