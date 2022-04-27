namespace Endeavour.Logging
{
	public class ConsoleLogSink : ILogSink
	{
		public void Log(string logline)
			=> Console.WriteLine(logline);
	}

	public class FileLogSink : ILogSink
	{
		string filename;

		public FileLogSink(string filename)
			=> this.filename = filename;

		public void Log(string logline)
			=> File.WriteAllText(filename, logline);
	}

	public class InGameLogSink : ILogSink
	{
		public void Log(string logline) => throw new NotImplementedException();
	}
}
