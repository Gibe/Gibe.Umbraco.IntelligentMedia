using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Gibe.Umbraco.IntelligentMedia.Controllers
{
	public class IntelligentMediaSettingsController : UmbracoApiController
	{
		private readonly IntelligentMediaService _intelligentMediaService;

		public IntelligentMediaSettingsController(IntelligentMediaService intelligentMediaService)
		{
			_intelligentMediaService = intelligentMediaService;
		}

		public IntelligentMediaSettingsController()
		{
			_intelligentMediaService = new IntelligentMediaService();
		}

		[HttpGet]
		public IntelligentMediaSettings Settings()
		{
			return _intelligentMediaService.Settings().Fetch();
		}

		[HttpPost]
		public void Settings([FromBody]IntelligentMediaSettings settings)
		{
			_intelligentMediaService.Settings().Update(settings);
		}
	}
}