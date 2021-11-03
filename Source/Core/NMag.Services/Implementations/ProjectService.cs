using NMag.Persistence;
using System.Collections.Generic;

namespace NMag.Services
{
	public class ProjectService : IProjectService
	{
		private readonly IProjectDao _projectDao;

		public ProjectService(IProjectDao projectDao)
		{
			_projectDao = projectDao;
		}

		public Project GetProject(string projectName)
		{
			_projectDao.GetProject(projectName);
			throw new System.NotImplementedException();
		}

		public IEnumerable<string> GetProjectNames()
		{
			throw new System.NotImplementedException();
		}
	}
}
