namespace Endeavour.Logging
{
	public class Logger : ILogger
	{
		HashSet<ILogSink> sinks = new();

		public void Log(LogLevel level, string formatString, params object[] args)
		{
			var logline = $"[{DateTime.Now}] [{level}] {string.Format(formatString, args)}";

			foreach (var sink in sinks)
				sink.Log(logline);
		}

		public void AddSink(ILogSink logSink)
		{
			if (sinks.Add(logSink))
				throw new ArgumentException("sink already exists");
		}

		public void RemoveSink(ILogSink logSink)
		{
			if (!sinks.Remove(logSink))
				throw new ArgumentException("unable to remove sink");
		}
	}
}
