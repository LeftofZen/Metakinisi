namespace Endeavour.Logging
{
	public interface ILogger
	{
		void Log(LogLevel level, string formatString, params object[] args);
	}
}
