using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector.Core.Builder
{
	public sealed class DiBuilder : IDiBuilder
	{
		private readonly IHostBuilder _hostBuilder;
		private readonly DiBuilderOptions _diBuilderOptions;

		public DiBuilder()
		{
			_hostBuilder = Host.CreateDefaultBuilder();
			_diBuilderOptions = new DiBuilderOptions();

			Services = new ServiceCollection();
			Configuration = new ConfigurationBuilder();
		}

		public IConfigurationBuilder Configuration { get; }
		public IServiceCollection Services { get; }

		public IDiApplication Build() => Build(null);
		public IDiApplication Build(Action<DiBuilderOptions>? configure)
		{
			// Apply defaults
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

			// Apply options
			if (configure is not null) configure(_diBuilderOptions);

			string? environmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentVariableNames.EnvironmentName);
			if (_diBuilderOptions.EnvironmentName is not null) environmentName = _diBuilderOptions.EnvironmentName;
			if(environmentName is not null) _hostBuilder.UseEnvironment(environmentName);

			// Build app
			IHost host = _hostBuilder.Build();
			return new DiApplication(host);
		}
	}
}
