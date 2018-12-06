using Autofac;
using AutoMapper;
using System.Reflection;
using Module = Autofac.Module;

namespace CandleScraper.Core.AutoMapper
{
	public class AutoMapperModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(GetConfiguredMapper());
		}
		private static IMapper GetConfiguredMapper()
		{
			var mapperConfiguration = new MapperConfiguration(RegisterMappings);
			mapperConfiguration.CompileMappings();
			return mapperConfiguration.CreateMapper();
		}
		private static void RegisterMappings(IMapperConfigurationExpression configure)
		{
			configure.AddProfiles(Assembly.GetExecutingAssembly());     // Dynamically load all configurations

			// ...or do it manually below. Example: https://github.com/AutoMapper/AutoMapper/wiki/Configuration
			// ...or see examples in Profiles directory.
		}
	}
}