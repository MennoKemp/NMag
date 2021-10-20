using Auxilia.Graphs;

namespace NMag.Services
{
	public interface IGraphCreationService
	{
		Graph CreateGraph(DataSet dataSet);
	}
}