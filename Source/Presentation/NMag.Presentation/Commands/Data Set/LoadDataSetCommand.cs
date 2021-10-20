using Auxilia.Delegation.Commands;
using Auxilia.Extensions;
using Auxilia.Utilities;
using Microsoft.Extensions.Logging;
using NMag.Persistence;
using System;

namespace NMag.Presentation.Commands
{
	[Command(typeof(LoadDataSetCommand))]
	public class LoadDataSetCommand : CommandBase
	{
		private readonly ICommandContext _context;
		private readonly IDataSetSerializer _dataSetSerializer;
		private readonly ILogger _logger;

		public LoadDataSetCommand(ICommandContext commandContext, IDataSetSerializer dataSetSerializer, ILogger<LoadDataSetCommand> logger)
		{
			_context = commandContext.ThrowIfNull(nameof(commandContext));
			_dataSetSerializer = dataSetSerializer.ThrowIfNull(nameof(dataSetSerializer));
			_logger = logger;
		}

		[Parameter(nameof(Path))]
		public string Path { get; set; }

		public override bool CanExecute(object parameter = null)
		{
			if (string.IsNullOrEmpty(Path))
			{
				_logger.LogError("No data set specified.");
				return false;
			}

			PathInfo dataSetPath = new PathInfo(Path);

			if (!dataSetPath.Exists)
			{
				_logger.LogError("Cannot find data set \"{Path}\"", Path);
				return false;
			}

			if (!dataSetPath.Extension.Equals(".set"))
			{
				_logger.LogError("File \"{Path}\" not a data set.", Path);
				return false;
			}

			return true;
		}
		
		protected override void ExecuteCommand(object parameter = null)
		{
			try
			{
				_context.DataSet = _dataSetSerializer.Deserialize(Path);
				_logger.LogInformation("Data set loaded.");
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "An error occurred while loading data set \"{Path}\".", Path);
			}
		}
	}
}
