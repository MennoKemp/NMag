using Auxilia.Utilities;
using System.IO;

namespace NMag.Persistence
{
	public class DataSetDao : IDataSetDao
	{
		private const string DataSetExtension = "set";

		private readonly DataSetSerializer _dataSetSerializer = new DataSetSerializer();

		public DataSet GetDataSet(string projectDirectory)
		{
			string dataSetPath = new PathInfo(projectDirectory, Path.GetDirectoryName(projectDirectory))
				.ChangeExtension(DataSetExtension)
				.FullPath;

			return _dataSetSerializer.Deserialize(dataSetPath);
		}
	}
}
