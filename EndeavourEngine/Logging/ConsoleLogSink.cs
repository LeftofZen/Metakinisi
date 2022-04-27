namespace Endeavour.Logging
{
	public struct ConsoleLogSink : ILogSink
	{
		public void Log(string logline)
			=> Console.WriteLine(logline);
	}

	public struct FileLogSink : ILogSink
	{
		readonly string filename;

		public FileLogSink(string filename)
			=> this.filename = filename;

		public void Log(string logline)
			=> File.WriteAllText(filename, logline);
	}

	public struct InGameLogSink : ILogSink
	{
		public void Log(string logline) => throw new NotImplementedException();
	}
}
