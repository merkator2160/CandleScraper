using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CandleScraper.Core.Config
{
	public static class ConfigurationProvider
	{
		private const String _defaultEnvironmentVariableName = "ENVIRONMENT";


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public static IConfigurationRoot CollectEnvironmentRelatedConfiguration()
		{
			return CollectEnvironmentRelatedConfiguration(_defaultEnvironmentVariableName);
		}
		public static IConfigurationRoot CollectEnvironmentRelatedConfiguration(String environmentVariableName)
		{
			var environment = Environment.GetEnvironmentVariable(environmentVariableName);
			if(String.IsNullOrWhiteSpace(environment))
				throw new ArgumentNullException($"Environment variable was not found: \"{environmentVariableName}\"!");

			return CreateConfiguration(environment, Directory.GetCurrentDirectory());
		}
		public static IConfigurationRoot CreateConfiguration(String environment, String basePath)
		{
			var builder = new ConfigurationBuilder()
				.AddEnvironmentVariables()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

			return builder.Build();
		}
	}
}