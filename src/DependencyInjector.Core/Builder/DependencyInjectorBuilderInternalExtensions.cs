using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector.Core.Builder
{
	internal static class DependencyInjectorBuilderInternalExtensions
	{
		public static IConfigurationBuilder AddDefaultConfiguration(this IConfigurationBuilder configuration, HostBuilderContext hostBuilderContext)
		{
			return configuration;
		}

		public static IServiceCollection AddDefaultServices(this IServiceCollection services, HostBuilderContext hostBuilderContext)
		{
			return services;
		}
	}
}
