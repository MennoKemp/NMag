using Auxilia.Delegation.Commands;
using Microsoft.Extensions.Logging;

namespace NMag.Commands
{
	[Command(typeof(CreateDataSetCommand))]
	public class CreateDataSetCommand : CommandBase
	{
		private readonly ICommandContext _context;
		private readonly ILogger _logger;

		[Parameter(nameof(Name))]
		public string Name { get; set; }

		public CreateDataSetCommand(ICommandContext context, ILogger<CreateDataSetCommand> logger)
		{
			_context = context;
			_logger = logger;
		}

		public override bool CanExecute(object parameter = null)
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				_logger.LogError("No name specified.");
				return false;
			}

			return true;
		}

		protected override void ExecuteCommand(object parameter = null)
		{
			_context.DataSet = new DataSet(Name);
			_logger.LogInformation("Created data set {Name}", Name);
		}
	}
}
