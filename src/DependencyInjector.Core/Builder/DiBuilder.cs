using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector.Core.Builder
{
	public sealed class DiBuilder : IDiBuilder
	{
		private readonly IHostBuilder _hostBuilder;

		public DiBuilder()
		{
			_hostBuilder = Host.CreateDefaultBuilder();

			Services = new ServiceCollection();
			Configuration = new ConfigurationBuilder();
		}

		public IConfigurationBuilder Configuration { get; private set; }
		public IServiceCollection Services { get; private set; }

		public IDiApplication Build(DiBuilderConfiguration? configuration)
		{
			if (configuration is { EnvironmentName: not null })
			{
				string? environmentName = Environment.GetEnvironmentVariable(configuration.EnvironmentName);
				if (string.IsNullOrWhiteSpace(environmentName)) throw new NullReferenceException(environmentName);
				_hostBuilder.UseEnvironment(environmentName);
			}

			_hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configuration) =>
			{
				configuration.AddDefaultConfiguration(hostBuilderContext);
				foreach (var configurationSource in Configuration.Sources) configuration.Add(configurationSource);
			});
			_hostBuilder.ConfigureServices((hostBuilderContext, services) =>
			{
				services.AddDefaultServices(hostBuilderContext);
				foreach (var service in Services) services.Add(service);
			});

			IHost host = _hostBuilder.Build();
			return new DiApplication(host);
		}
	}
}
