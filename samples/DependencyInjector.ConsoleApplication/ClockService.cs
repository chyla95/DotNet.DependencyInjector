namespace DependencyInjector.ConsoleApplication
{
	internal class ClockService
	{
		public DateTime GetTime() => DateTime.UtcNow;
	}
}
