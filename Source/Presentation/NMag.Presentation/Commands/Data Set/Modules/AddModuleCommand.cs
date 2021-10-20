using Auxilia.Delegation.Commands;
using Microsoft.Extensions.Logging;
using System;

namespace NMag.Presentation.Commands
{
	[Command(typeof(AddModuleCommand))]
	public abstract class AddModuleCommand : CommandBase
	{
		protected readonly ICommandContext _context;
		protected readonly ILogger _logger;

		protected AddModuleCommand(ICommandContext context, ILogger<AddReservoirCommand> logger)
		{
			_context = context;
			_logger = logger;
		}

		[Parameter(nameof(Id), true)]
		public int? Id { get; set; }

		protected abstract ModuleType ModuleType { get; }

		public override bool CanExecute(object parameter = null)
		{
			if (_context.DataSet == null)
			{
				_logger.LogError("No data set loaded.");
				return false;
			}

			if (Id is int id)
			{
				if (_context.DataSet.Modules.Contains(id))
				{
					_logger.LogError("Data set already contains a module with id {Id}.", Id);
					return false;
				}

				if (id < 1)
				{
					_logger.LogError("Id must be larger than 0.");
					return false;
				}
			}
			
			return true;
		}

		protected override void ExecuteCommand(object parameter = null)
		{
			try
			{
				Module module = Id is int id
					? _context.DataSet.Modules.Add(ModuleType, id)
					: _context.DataSet.Modules.Add(ModuleType);

				SetName(module);

				_logger.LogInformation("{Module} added.", module);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "An error occurred while adding {ModuleType} module.", ModuleType);
			}
		}

		protected abstract void SetName(Module module);
	}
}
