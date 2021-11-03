using NMag.Commands;

namespace NMag.Presentation.Commands
{
	public class CommandContext : ICommandContext
	{
		public DataSet DataSet { get; set; }
	}
}