using Autofac;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;

namespace CandleScraper.Core.DependencyInjection
{
	public static class Collector
	{
		public static void RegisterLocalServices(this ContainerBuilder builder)
		{
			builder.RegisterServices(Assembly.GetCallingAssembly());
		}
		public static void RegisterServices(this ContainerBuilder builder, Assembly assembly)
		{
			builder
				.RegisterAssemblyTypes(assembly)
				.Where(p => p.IsClass && p.Name.EndsWith("Service"))
				.AsSelf()
				.AsImplementedInterfaces();
		}
		public static void RegisterLocalConfiguration(this ContainerBuilder builder, IConfiguration configuration)
		{
			builder.RegisterConfiguration(configuration, Assembly.GetCallingAssembly());
		}
		public static void RegisterConfiguration(this ContainerBuilder builder, IConfiguration configuration, Assembly assembly)
		{
			var configTypes = assembly.DefinedTypes.Where(p => p.IsClass && p.Name.EndsWith("Config")).ToArray();
			foreach(var x in configTypes)
			{
				var configInstance = configuration.GetSection(x.Name).Get(x);
				if(configInstance != null)
				{
					builder.RegisterInstance(configInstance).AsSelf();
				}
			}
		}
	}
}