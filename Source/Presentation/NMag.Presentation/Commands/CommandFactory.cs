using Auxilia.Delegation.Commands;
using System;

namespace NMag.Presentation.Commands
{
	public class CommandFactory : ICommandFactory<CommandBase>
	{
		public CommandBase CreateCommand(Type commandType)
		{
			return (CommandBase)Program.Host.Services.GetService(commandType);
		}
	}
}
