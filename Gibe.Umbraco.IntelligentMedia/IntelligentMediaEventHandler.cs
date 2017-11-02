using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Gibe.Umbraco.IntelligentMedia
{
	public class IntelligentMediaEventHandler : ApplicationEventHandler
	{
		protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			MediaService.Saved += MediaServiceSaved;
		}

		public async void MediaServiceSaved(IMediaService sender, SaveEventArgs<IMedia> e)
		{
			var service = new IntelligentMediaService();
			foreach (var mediaItem in e.SavedEntities)
			{
				if (!string.IsNullOrEmpty(mediaItem.GetValue<string>("json")))
				{
					continue;
				}
				
				var visionMedia = new VisionMedia();
				foreach (var api in service.VisionApis()) 
				{
					var visionResponse = await api.MakeRequest(mediaItem).ConfigureAwait(false);
					visionMedia = Merge(visionMedia, visionResponse);
				}
				UpdateMediaItem(mediaItem, visionMedia);
			}
		}

		public VisionMedia Merge(VisionMedia visionMedia, IVisionResponse response)
		{
			var tags = visionMedia.Tags;
			tags.AddRange(response.Tags);
			
			var categories = visionMedia.Categories;
			categories.AddRange(response.Categories);

			var descriptions = visionMedia.Descriptions;
			descriptions.AddRange(response.Description);

			var json = visionMedia.Json + "\r\n" + response.Json;
			
			return new VisionMedia
			{
				Tags = tags,
				Categories = categories,
				Descriptions = descriptions,
				NumberOfFaces = Math.Max(visionMedia.NumberOfFaces??0, response.NumberOfFaces??0),
				PrimaryColour = visionMedia.PrimaryColour ?? response.PrimaryColour,
				BackgroundColour = visionMedia.BackgroundColour ?? response.BackgroundColour,
				Json = json
			};
		}

		public void UpdateMediaItem(IMedia mediaItem, VisionMedia visionMedia)
		{

			mediaItem.Name = visionMedia.Name;
			mediaItem.SetValue("tags",
				String.Join(",", visionMedia.Tags
					.Select(t => t.Tag)
					.Distinct()));
			mediaItem.SetValue("description", 
				visionMedia.Descriptions
				.OrderByDescending(d => d.Confidence)
				.First().Tag);
			mediaItem.SetValue("categories",
					String.Join(",", visionMedia.Categories
						.Select(t => t.Tag.Replace("_", " ").TrimEnd())));
			mediaItem.SetValue("numberOfFaces", visionMedia.NumberOfFaces);
			mediaItem.SetValue("primaryColour", visionMedia.PrimaryColour);
			mediaItem.SetValue("backgroundColour", visionMedia.BackgroundColour);
			mediaItem.SetValue("json", visionMedia.Json);
			ApplicationContext.Current.Services.MediaService.Save(mediaItem);
		}
	}

	public class VisionMedia
	{
		public VisionMedia()
		{
			Tags = new List<ProbableTag>();
			Categories = new List<ProbableTag>();
			Descriptions = new List<ProbableTag>();
			Json = "";
		}
		public string Name => Descriptions.OrderByDescending(d => d.Confidence).First().Tag;
		public List<ProbableTag> Tags { get; set; }
		public List<ProbableTag> Categories { get; set; }
		public List<ProbableTag> Descriptions { get; set; }
		public int? NumberOfFaces { get; set; }
		public string PrimaryColour { get; set; }
		public string BackgroundColour { get; set; }
		public string Json { get; set; }
	}
}