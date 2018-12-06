using AutoMapper;
using CandleScraper.Core.AutoMapper;
using Xunit;

namespace IntegrationTests.AutoMapper
{
	public class AutoMapperTest
	{
		[Fact]
		public void PandaConfigurationTest()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfiles(typeof(AutoMapperModule).Assembly);
			});

			mapperConfiguration.CompileMappings();
			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}