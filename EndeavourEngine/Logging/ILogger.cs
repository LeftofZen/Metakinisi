namespace Endeavour.Logging
{
	public interface ILogger
	{
		void Log(LogLevel level, string formatString, params object[] args);
		void AddSink(ILogSink logSink);
		void RemoveSink(ILogSink logSink);
	}
}
