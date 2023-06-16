using DependencyInjector.Core.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependencyInjector.Core
{
	public sealed class DependencyInjector : IHost
	{
		private static readonly object _lock = new();
		private static DependencyInjectorBuilder? _applicationBuilder;
		private readonly IHost _host;

		public DependencyInjector(IHost host)
		{
			_host = host;
		}

		public IServiceProvider Services => _host.Services;
		public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();
		public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

		public Task StartAsync(CancellationToken cancellationToken = default) => _host.StartAsync(cancellationToken);
		public Task StartAsync(Action<IServiceProvider, IConfiguration> initialize, CancellationToken cancellationToken = default)
		{
			initialize(Services, Configuration);
			return _host.StartAsync(cancellationToken);
		}
		public Task StartAsync<TApp>(Action<TApp> initialize, CancellationToken cancellationToken = default) where TApp : class
		{
			TApp? app = Services.GetService<TApp>();
			if (app is null) throw new NullReferenceException(nameof(app));

			initialize(app);
			return _host.StartAsync(cancellationToken);
		}
		public Task StartAsync<TApp>(Action<TApp, IServiceProvider, IConfiguration> initialize, CancellationToken cancellationToken = default) where TApp : class
		{
			TApp? app = Services.GetService<TApp>();
			if (app is null) throw new NullReferenceException(nameof(app));

			initialize(app, Services, Configuration);
			return _host.StartAsync(cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken = default)
		{
			Lifetime.StopApplication();
			return _host.StopAsync(cancellationToken);
		}
		public void Dispose() => _host.Dispose();

		public static DependencyInjectorBuilder CreateBuilder()
		{
			if (_applicationBuilder is not null) return _applicationBuilder;

			lock (_lock) _applicationBuilder ??= new DependencyInjectorBuilder();
			return _applicationBuilder;
		}
	}
}
