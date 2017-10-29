using System.IO;
using Newtonsoft.Json;
using Umbraco.Core.IO;

namespace Gibe.Umbraco.IntelligentMedia
{
	public class SettingsService<T> where T : new()
	{
		private const string Filename = "IntelligentMedia.config";

		private readonly IFileSystem _fileSystem;

		public SettingsService(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
			
		}

		public T Fetch()
		{
			if (!_fileSystem.FileExists(Filename))
			{
				return new T();
			}

			using (var stream = _fileSystem.OpenFile(Filename))
			{
				var reader = new StreamReader(stream);

				return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
			}
		}

		public void Update(T settings)
		{
			using (var settingsMemoryStream = new MemoryStream())
			{
				using (var settingsStream = new StreamWriter(settingsMemoryStream))
				{
					settingsStream.Write(JsonConvert.SerializeObject(settings));
					settingsStream.Flush();

					settingsMemoryStream.Position = 0;
					_fileSystem.AddFile(Filename, settingsMemoryStream, true);
				}
			}
		}
	}
}