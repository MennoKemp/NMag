using NMag.Persistence;

namespace NMag.Injections.Tests
{
	public class DummyApplicationSettings : IPersistenceSettings
	{
		public string ProjectFolderDirectory { get; }
	}
}
