using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibe.Umbraco.IntelligentMedia
{
	public interface IIntelligentMediaService
	{
		IEnumerable<IVisionApi> VisionApis();
		SettingsService<IntelligentMediaSettings> Settings();
	}
}
