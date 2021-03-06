﻿using AutoMapper;
using CandleScraper.Core.AutoMapper;
using Xunit;

namespace UnitTests.AutoMapper
{
	public class AutoMapperTest
	{
		[Fact]
		public void AutomapperConfigurationTest()
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