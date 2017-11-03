namespace Gibe.Umbraco.IntelligentMedia
{
	public class IntelligentMediaSettings
	{
		public bool UseAzure { get; set; }
		public string AzureSubscriptionKey { get; set; }
		public string AzureRegion { get; set; }

		public bool UseGoogle { get; set; }
		public string GoogleApiKey { get; set; }


	}
}