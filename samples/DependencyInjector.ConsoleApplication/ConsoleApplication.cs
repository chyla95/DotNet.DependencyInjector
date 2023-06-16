namespace DependencyInjector.ConsoleApplication
{
	internal class ConsoleApplication
	{
		private readonly ClockService _clockService;

		public ConsoleApplication(ClockService clockService)
		{
			_clockService = clockService;
		}

		public void Start()
		{
			DateTime time = _clockService.GetTime();
			Console.WriteLine($"Time: {time}");
		}
	}
}
