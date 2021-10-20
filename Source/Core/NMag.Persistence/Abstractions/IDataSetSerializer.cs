using System.IO;

namespace NMag.Persistence
{
	public interface IDataSetSerializer
	{
		void Serialize(DataSet dataSet, string filePath);
		void Serialize(DataSet dataSet, FileStream fileStream);

		DataSet Deserialize(string filePath);
		DataSet Deserialize(FileStream fileStream);
	}
}
