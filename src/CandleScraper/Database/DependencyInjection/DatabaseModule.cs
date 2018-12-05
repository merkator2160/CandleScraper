using Autofac;
using CandleScraper.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Reflection;
using Module = Autofac.Module;

namespace CandleScraper.Database.DependencyInjection
{
	public class DatabaseModule : Module
	{
		private const String _defaultDbConnectionName = "MongoDbConnection";
		private readonly IConfiguration _configuration;
		private readonly Assembly _currentAssembly;


		public DatabaseModule(IConfiguration configuration)
		{
			_currentAssembly = Assembly.GetExecutingAssembly();
			_configuration = configuration;
		}


		// COMPONENT REGISTRATION /////////////////////////////////////////////////////////////////
		protected override void Load(ContainerBuilder builder)
		{
			RegisterContext(builder);
			RegisterRepositories(builder);
			builder.RegisterLocalConfiguration(_configuration);
		}
		public void RegisterContext(ContainerBuilder builder)
		{
			builder.RegisterInstance(new MongoClient(_configuration.GetConnectionString("MongoDbConnection")))
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.RegisterType<DataContext>();
		}
		public void RegisterRepositories(ContainerBuilder builder)
		{
			builder
				.RegisterAssemblyTypes(_currentAssembly)
				.Where(t => t.Name.EndsWith("Repository"));

			builder
				.RegisterType<RepositoryBundle>()
				.AsSelf()
				.AsImplementedInterfaces();
		}
	}
}