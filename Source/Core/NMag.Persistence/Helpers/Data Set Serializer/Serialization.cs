using Auxilia.Extensions;
using System.IO;

namespace NMag.Persistence
{
	internal partial class DataSetSerializer
	{
		public void Serialize(DataSet dataSet, string filePath)
		{
			dataSet.ThrowIfNull(nameof(dataSet));
			filePath.ThrowIfNullOrEmpty(nameof(filePath));

			Serialize(dataSet, File.OpenWrite(filePath));
		}

		public void Serialize(DataSet dataSet, FileStream fileStream)
		{
		}
	}
}
