using Auxilia.Delegation.Commands;
using Auxilia.Graphs;
using Auxilia.Graphs.Graphviz;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NMag.Services;
using System;

namespace NMag.Presentation.Commands
{
	[Command(typeof(CreateGraphCommand))]
	public class CreateGraphCommand : CommandBase
	{
		private readonly ICommandContext _context;
		private readonly IGraphCreationService _graphCreationService;
		private readonly ILogger<CreateGraphCommand> _logger;
		private readonly LayoutAlgorithm _layoutAlgorithm = new LayoutAlgorithm(@"c:\Users\Munno\Dropbox\Repositories\NMag\Source\Graphviz\bin\");

		public CreateGraphCommand(ICommandContext context, IGraphCreationService graphCreationService, ILogger<CreateGraphCommand> logger)
		{
			_context = context;
			_graphCreationService = graphCreationService;
			_logger = logger ?? NullLogger<CreateGraphCommand>.Instance;
		}
		
		[Parameter(nameof(OutputImage), true)]
		public string OutputImage { get; set; }

		[Parameter(nameof(HorizontalSpacing), true)]
		public double HorizontalSpacing { get; set; }

		[Parameter(nameof(VerticalSpacing), true)]
		public double VerticalSpacing { get; set; }

		[Parameter(nameof(EqualSpacing), true)]
		public bool EqualSpacing { get; set; }

		public override bool CanExecute(object parameter = null)
		{
			if (_context.DataSet == null)
			{
				_logger.LogError("No data set loaded.");
				return false;
			}

			return true;
		}
		
		protected override void ExecuteCommand(object parameter = null)
		{
			try
			{
				Graph graph = _graphCreationService.CreateGraph(_context.DataSet);
				
				LayoutSettings layoutSettings = new LayoutSettings
				{
					HorizontalSpacing = HorizontalSpacing,
					VerticalSpacing = VerticalSpacing,
					EqualSpacing = EqualSpacing,

					OutputImagePath = OutputImage
				};

				_layoutAlgorithm.GenerateLayout(graph, layoutSettings);

				_logger.LogInformation("Graph created.");
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "An error occurred while creating graph.");
			}
		}
	}
}
