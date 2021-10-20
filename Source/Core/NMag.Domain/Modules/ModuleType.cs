using System.ComponentModel;

namespace NMag
{
	public enum ModuleType
	{
		Reservoir = 0,
		[Description("Power Plant")]
		PowerPlant = 1,
		Transfer = 2,
		ControlPoint = 3,
		ImpoundmentFacility = 4
	}
}
