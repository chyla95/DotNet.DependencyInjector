using DependencyInjector.Core.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector.Core
{
	public sealed class DiApplication : IHost
	{
		private static readonly object _lock = new();
		private static DiBuilder? _applicationBuilder;
		private readonly IHost _host;

		public DiApplication(IHost host)
		{
			_host = host;
		}

		public IServiceProvider Services => _host.Services;
		public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();
		public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

		public Task StartAsync(CancellationToken cancellationToken = default) => _host.StartAsync(cancellationToken);
		public async Task StartAsync(Action<IServiceProvider, IConfiguration> initialize, CancellationToken cancellationToken = default)
		{
			await _host.StartAsync(cancellationToken);
			initialize(Services, Configuration);
		}
		public async Task StartAsync<TApp>(Action<TApp> initialize, CancellationToken cancellationToken = default) where TApp : class
		{
			TApp? app = Services.GetService<TApp>();
			if (app is null) throw new NullReferenceException(nameof(app));

			await _host.StartAsync(cancellationToken);
			initialize(app);
		}
		public async Task StartAsync<TApp>(Action<TApp, IServiceProvider, IConfiguration> initialize, CancellationToken cancellationToken = default) where TApp : class
		{
			TApp? app = Services.GetService<TApp>();
			if (app is null) throw new NullReferenceException(nameof(app));

			await _host.StartAsync(cancellationToken);
			initialize(app, Services, Configuration);
		}

		public Task StopAsync(CancellationToken cancellationToken = default)
		{
			Lifetime.StopApplication();
			return _host.StopAsync(cancellationToken);
		}
		public void Dispose() => _host.Dispose();

		public static DiBuilder CreateBuilder()
		{
			if (_applicationBuilder is not null) return _applicationBuilder;

			lock (_lock) _applicationBuilder ??= new DiBuilder();
			return _applicationBuilder;
		}
	}
}
