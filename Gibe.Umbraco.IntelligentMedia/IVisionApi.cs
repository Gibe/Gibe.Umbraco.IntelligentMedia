using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Gibe.Umbraco.IntelligentMedia
{
	public interface IVisionApi
	{
		Task<IVisionResponse> MakeRequest(IMedia media);
	}
}
