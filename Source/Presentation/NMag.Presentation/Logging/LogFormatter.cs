using Serilog.Events;
using Serilog.Formatting;
using System.IO;

namespace NMag.Presentation
{
	public class LogFormatter : ITextFormatter
	{
		public void Format(LogEvent logEvent, TextWriter output)
		{
			output.WriteLine(logEvent.MessageTemplate.Render(logEvent.Properties));

			if (logEvent.Exception != null)
				output.WriteLine(logEvent.Exception);
		}
	}
}
