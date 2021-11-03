using System;

namespace NMag.Persistence
{
	internal class DeserializationException : Exception
	{
		public DeserializationException(string message, int lineIndex, string line, string dataBlock)
			: base(message)
		{
			LineIndex = lineIndex;
			Line = line;
			DataBlock = dataBlock;
		}

		public int LineIndex { get; }
		public string Line { get; }

		public string DataBlock { get; set; }

		public override string ToString()
		{
			return $"An error occurred while deserializing {DataBlock} at line {LineIndex}: {Message}";
		}
	}
}
