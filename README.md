# Gibe.Umbraco.IntelligentMedia

## Installation Instructions

### Dashboard

Add the following to the end of <sections> in config/Dashboard.config

```xml
<section alias="IntelligentMediaSettings">
	<areas>
		<area>developer</area>
	</areas>
	<tab caption="Intelligent Media Settings">
		<control>/App_Plugins/IntelligentMedia/settings.html</control>
	</tab>
</section>
```

### FileSystemProvider

Add the end of the <FileSystemProviders> section of config/FileSystemProviders.config

```xml
<Provider alias="config" type="Umbraco.Core.IO.PhysicalFileSystem, Umbraco.Core">
	<Parameters>
		<add key="virtualRoot" value="~/config/" />
	</Parameters>
</Provider>
```
### Media Setup

Add the following to the media you want to generate machine learning data

| Property Alias | Type |
| -------------- | ---- |
| tags           | Tags |
| categories     | Tags |
| description    | Textstring |
| json           | Textarea |
| primaryColour  | Textstring |
| backgroundColour | Textstring |
| numberOfFaces | Numeric |

