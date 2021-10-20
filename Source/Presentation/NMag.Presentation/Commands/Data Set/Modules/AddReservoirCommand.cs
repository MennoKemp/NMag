using Auxilia.Delegation.Commands;
using Microsoft.Extensions.Logging;

namespace NMag.Presentation.Commands
{
	[Command(typeof(AddReservoirCommand))]
	public class AddReservoirCommand : AddModuleCommand
	{
		public AddReservoirCommand(ICommandContext context, ILogger<AddReservoirCommand> logger)
			: base(context, logger)
		{
		}

		[Parameter(nameof(Name), true)]
		public string Name { get; set; }

		protected override ModuleType ModuleType { get; } = ModuleType.Reservoir;
		
		protected override void SetName(Module module)
		{
			module.Reservoir.Name = Name;
		}
	}
}
