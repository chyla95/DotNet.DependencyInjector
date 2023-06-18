using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector.Core
{
	public interface IDiApplication : IHost
	{
		IConfiguration Configuration { get; }
		IHostApplicationLifetime Lifetime { get; }

		Task StartAsync(Action<IServiceProvider, IConfiguration> initialize, CancellationToken cancellationToken = default);
		Task StartAsync<TApp>(Action<TApp, IServiceProvider, IConfiguration> initialize, CancellationToken cancellationToken = default) where TApp : class;
		Task StartAsync<TApp>(Action<TApp> initialize, CancellationToken cancellationToken = default) where TApp : class;
	}
}