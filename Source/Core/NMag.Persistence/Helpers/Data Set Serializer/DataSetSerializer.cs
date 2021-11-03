using System.Text;

namespace NMag.Persistence
{
	internal partial class DataSetSerializer
	{
		private const string BlockDelimiter = "***";
		private const string CommentIndicator = "*";
		
		private static readonly Encoding Encoding = Encoding.UTF8;

		private string _currentBlock;
		private int _currentLineIndex;
		private string _currentLine;
	}
}
