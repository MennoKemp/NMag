using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NMag.Persistence
{
	public class ProjectDao : IProjectDao
	{
		private readonly IPersistenceSettings _persistenceSettings;
		private readonly IDataSetDao _dataSetDao;

		public ProjectDao(IPersistenceSettings persistenceSettings, IDataSetDao dataSetDao)
		{
			_dataSetDao = dataSetDao;
			_persistenceSettings = persistenceSettings;
		}

		public IEnumerable<string> GetProjectNames()
		{
			return Directory.GetDirectories(_persistenceSettings.ProjectFolderDirectory)
				.Select(Path.GetDirectoryName);
		}

		public Project GetProject(string projectName)
		{
			return new Project
			{
				Name = projectName,
				DataSet = _dataSetDao.GetDataSet(Path.Combine(_persistenceSettings.ProjectFolderDirectory, projectName))
			};
		}
	}
}
