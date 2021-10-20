using Auxilia.Extensions;

namespace NMag
{
	public class DataSet
	{
        public DataSet(string name)
        {
			Name = name.ThrowIfNullOrEmpty(nameof(name));
        }

		public string Name { get; set; }

		public ModuleCollection Modules { get; } = new ModuleCollection();
	}
}
