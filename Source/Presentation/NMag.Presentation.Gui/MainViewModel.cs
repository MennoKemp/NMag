using Auxilia;
using Auxilia.Graphs;
using Auxilia.Graphs.Graphviz;
using Auxilia.Presentation.ViewModels;
using NMag.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;

namespace NMag.Presentation
{
	public class MainViewModel : MainViewModelBase
	{
		public MainViewModel()
		{
   //         new DataSetSerializer().Deserialize(@"c:\Users\Munno\Dropbox\NTNU\Master Thesis\Final\Upload\Data\Orkla\Orkla.set");
   //         //Result result = new DataSetSerializer().Deserialize(@"c:\Users\Munno\Dropbox\NTNU\Master Thesis\Final\Upload\Data\Nea_Nidelva.set", out DataSet dataSet);

   //         if (!result.Success)
			//	return;

			//Graph graph = new Graph();
   //         dataSet.Modules.Execute(m => graph.AddNode(new Node(m.Id)));

			//foreach (Module module in dataSet.Modules)
			//{
			//	List<int> connectedModules = new List<int>();

			//	if (module.ReleaseModuleId > 0)
   //             {
   //                 graph.AddLink(new Link(graph[module.Id], graph[module.ReleaseModuleId]));
   //                 connectedModules.Add(module.ReleaseModuleId);
   //             }

			//	if (module.BypassModuleId > 0 && !connectedModules.Contains(module.BypassModuleId))
   //             {
   //                 graph.AddLink(new Link(graph[module.Id], graph[module.BypassModuleId]));
   //                 connectedModules.Add(module.BypassModuleId);
   //             }

			//	if (module.SpillModuleId > 0 && !connectedModules.Contains(module.SpillModuleId))
   //             {
   //                 graph.AddLink(new Link(graph[module.Id], graph[module.SpillModuleId]));
   //                 connectedModules.Add(module.SpillModuleId);
   //             }
			//}

   //         LayoutAlgorithm layoutAlgorithm = new LayoutAlgorithm(@"c:\Users\Munno\Dropbox\Repositories\NMag\Source\Graphviz\bin\");

   //         LayoutSettings settings = new LayoutSettings
   //         {
   //             HorizontalSpacing = 0.75 ,
   //             VerticalSpacing = 0.75
   //         };

   //         //result = layoutAlgorithm.GenerateLayout(graph, settings);

   //         return;

			////IEnumerable<IGrouping<int, Node>> groups = graph.Nodes.GroupBy(n => n.Level);

   //         List<object> moduleData = new List<object>();

   //         //foreach (Edge edge in edges)
   //         //{
   //         //	if (edge.Curve is Microsoft.Msagl.Core.Geometry.Curves.Curve curve)
   //         //	{
   //         //		foreach (LineSegment segment in curve.Segments.OfType<LineSegment>())
   //         //		{
   //         //			Line line = new Line
   //         //			{
   //         //				Stroke = Brushes.Black,
   //         //				StrokeThickness = 3,
   //         //				X1 = segment.Start.X + 100,
   //         //				Y1 = segment.Start.Y - 500,
   //         //				X2 = segment.End.X + 100,
   //         //				Y2 = segment.End.Y - 500,
   //         //			};

   //         //			moduleData.Add(line);
   //         //		}
   //         //	}
   //         //}



   //         //foreach (IGrouping<int, Node> level in graph.Nodes.GroupBy(n => n.Level))
   //         //{
   //         //    int position = 0;

   //         //    foreach (Node node in level)
   //         //    {
   //         //        AddNode(new Point(position * 80, level.Key * 40), new Size(60, 30), node);
   //         //        position++;
   //         //    }
   //         //}

   //         //RaisePropertyChanged(nameof(ModuleData));
		}

		public IEnumerable<object> ModuleData
		{
			get => _moduleData;
			set => SetProperty(ref _moduleData, value.ToList());
		}
		private IList<object> _moduleData = new List<object>();

        //private void AddNode(Point position, Size size, Node node)
        //{
        //    Rectangle rectangle = new Rectangle
        //    {
        //        Width = size.Width,
        //        Height = size.Height,
        //        Fill = Brushes.Red
        //    };

        //    Label label = new Label
        //    {
        //        Width = size.Width,
        //        Height = size.Height,
        //        Content = node.Id,
        //        HorizontalContentAlignment = HorizontalAlignment.Center
        //    };

        //    Canvas.SetLeft(rectangle, position.X);
        //    Canvas.SetTop(rectangle, position.Y);
        //    _moduleData.Add(rectangle);

        //    Canvas.SetLeft(label, position.X);
        //    Canvas.SetTop(label, position.Y);
        //    _moduleData.Add(label);
        //}
	}
}
