using System.Collections.Generic;

namespace NMag.Persistence
{
	public interface IProjectDao
	{
		IEnumerable<string> GetProjectNames();

		Project GetProject(string projectName);
	}
}
