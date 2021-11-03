using System.Collections.Generic;

namespace NMag.Services
{
	public interface IProjectService
	{
		IEnumerable<string> GetProjectNames();

		Project GetProject(string projectName);
	}
}
