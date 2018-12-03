using Autofac;
using CandleScraper.Core.DependencyInjection;
using CandleScraper.Database.DependencyInjection;
using CandleScraper.ExternalApi.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CandleScraper
{
	class Program
	{
		static void Main(String[] args)
		{
			MainAsync(args).GetAwaiter().GetResult();
		}
		private static async Task MainAsync(String[] args)
		{
			var configuration = Core.Config.ConfigurationProvider.CollectEnvironmentRelatedConfiguration();
			using(var container = CreateContainer(configuration))
			{
				var context = container.Resolve<ApplicationContext>();
				await context.RunAsync();
			}
		}


		// SUPPORT FUNCTIONS ////////////////////////////////////////////////////////////////////////////
		private static IContainer CreateContainer(IConfigurationRoot configuration)
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<ApplicationContext>();

			builder.RegisterLocalServices();
			builder.RegisterLocalConfiguration(configuration);

			builder.RegisterModule<ExternalApiClientModule>();
			builder.RegisterModule(new DatabaseModule(configuration));

			return builder.Build();
		}
	}
}
