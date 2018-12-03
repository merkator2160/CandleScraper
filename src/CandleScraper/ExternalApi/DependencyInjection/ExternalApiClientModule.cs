using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace CandleScraper.ExternalApi.DependencyInjection
{
	public class ExternalApiClientModule : Module
	{
		private readonly Assembly _currentAssembly;


		public ExternalApiClientModule()
		{
			_currentAssembly = Assembly.GetAssembly(typeof(ExternalApiClientModule));
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		protected override void Load(ContainerBuilder builder)
		{
			RegisterClients(builder);
		}
		public void RegisterClients(ContainerBuilder builder)
		{
			builder
				.RegisterAssemblyTypes(_currentAssembly)
				.Where(t => t.Name.EndsWith("Client"))
				.AsSelf()
				.AsImplementedInterfaces();
		}
	}
}