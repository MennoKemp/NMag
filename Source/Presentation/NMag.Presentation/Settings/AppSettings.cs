using Auxilia.Utilities;
using NMag.Persistence;
using System;

namespace NMag.Presentation
{
	/// <summary>
	/// Handles application settings.
	/// </summary>
	public class AppSettings : ApplicationSettings, IPersistenceSettings
	{
		/// <summary>
		/// Gets the settings directory type.
		/// </summary>
		protected override SettingsDirectoryType SettingsDirectoryType => SettingsDirectoryType.LocalApplicationData;

		/// <summary>
		/// Gets the path for the settings file.
		/// </summary>
		protected override string Path { get; } = nameof(NMag);

		/// <summary>
		/// Gets or sets the project folder directory.
		/// </summary>
		public string ProjectFolderDirectory
		{
			get => GetSetting(this, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
			set => SetSetting(this, value);
		}
	}
}
