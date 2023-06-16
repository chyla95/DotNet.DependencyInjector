using DependencyInjector.ConsoleApplication;
using DependencyInjector.Core;
using DependencyInjector.Core.Builder;
using Microsoft.Extensions.DependencyInjection;

IDiBuilder builder = DiApplication.CreateBuilder();
builder.Services.AddTransient<ClockService>();

IDiApplication app = builder.Build(null);

// Option I.
//await app.StartAsync<ConsoleApplication>((consoleApplication, services, configuration) =>
//{
//	consoleApplication.Start();
//});

// Option II.
//await app.StartAsync((services, configuration) =>
//{
//	ConsoleApplication worker = ActivatorUtilities.CreateInstance<ConsoleApplication>(services);
//	worker.Start();
//});

await app.StopAsync();