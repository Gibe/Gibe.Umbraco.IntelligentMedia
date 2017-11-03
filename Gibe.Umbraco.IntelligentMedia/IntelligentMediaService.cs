using System.Collections.Generic;
using Gibe.Umbraco.IntelligentMedia.Azure;
using Gibe.Umbraco.IntelligentMedia.Google;
using Umbraco.Core.IO;

namespace Gibe.Umbraco.IntelligentMedia
{
	public class IntelligentMediaService : IIntelligentMediaService
	{
		public IEnumerable<IVisionApi> VisionApis()
		{
			var settings = Settings().Fetch();
			var visionApi = new GoogleVisionApi(settings);
			return new List<IVisionApi> {visionApi, new AzureVisionApi(settings) };
		}
		
		public SettingsService<IntelligentMediaSettings> Settings()
		{
			return new SettingsService<IntelligentMediaSettings>(FileSystemProviderManager.Current
				.GetUnderlyingFileSystemProvider("config"));
		}
	}
}