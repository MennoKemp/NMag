using System.Collections.Generic;

namespace NMag
{
	public interface IUnit
	{
		UnitType Type { get; }
		string Name { get; set; }

		CurveType[] CurveTypes { get; }
		Dictionary<CurveType, Curve> Curves { get; set; }
	}
}
