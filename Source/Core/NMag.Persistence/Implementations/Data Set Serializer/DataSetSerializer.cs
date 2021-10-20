using System.Text;

namespace NMag.Persistence
{
	public partial class DataSetSerializer : IDataSetSerializer
	{
		private const string BlockDelimiter = "***";
		private const string CommentIndicator = "*";
		
		private static readonly Encoding Encoding = Encoding.UTF7;

		private string _currentBlock;
		private int _currentLineIndex;
		private string _currentLine;
	}
}
