using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjector.Core.Builder
{
	public interface IDiBuilder
	{
		IConfigurationBuilder Configuration { get; }
		IServiceCollection Services { get; }

		IDiApplication Build(DiBuilderConfiguration? configuration);
	}
}