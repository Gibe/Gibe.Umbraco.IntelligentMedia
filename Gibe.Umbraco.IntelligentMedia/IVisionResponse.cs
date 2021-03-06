﻿using System.Collections.Generic;

namespace Gibe.Umbraco.IntelligentMedia
{
	public interface IVisionResponse
	{
		List<ProbableTag> Tags { get; }
		List<ProbableTag> Categories { get;}
		string Json { get; }
		List<ProbableTag> Description { get; }
		int? NumberOfFaces { get; }
		string PrimaryColour { get; }
		string BackgroundColour { get; }
	}


}