using Auxilia.Extensions;
using Auxilia.Graphs;
using System.Collections.Generic;

namespace NMag.Services
{
	public class GraphCreationService : IGraphCreationService
	{
		public Graph CreateGraph(DataSet dataSet)
		{
			dataSet.ThrowIfNull(nameof(dataSet));

			Graph graph = new Graph();

			foreach (Module module in dataSet.Modules)
				graph.AddNode(new Node(module.Id));

			foreach (Module module in dataSet.Modules)
			{
				List<int> connectedModules = new List<int>();

				if (module.ReleaseModuleId > 0)
				{
					graph.AddLink(new Link(graph[module.Id], graph[module.ReleaseModuleId]));
					connectedModules.Add(module.ReleaseModuleId);
				}

				if (module.BypassModuleId > 0 && !connectedModules.Contains(module.BypassModuleId))
				{
					graph.AddLink(new Link(graph[module.Id], graph[module.BypassModuleId]));
					connectedModules.Add(module.BypassModuleId);
				}

				if (module.SpillModuleId > 0 && !connectedModules.Contains(module.SpillModuleId))
				{
					graph.AddLink(new Link(graph[module.Id], graph[module.SpillModuleId]));
					connectedModules.Add(module.SpillModuleId);
				}
			}

			return graph;
		}
    }
}
