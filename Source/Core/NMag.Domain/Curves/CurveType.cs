using Auxilia;

namespace NMag
{
	public enum CurveType
	{
		// General
		Routing,

		// Reservoir
		Volume,
		SpillFlow,
		Discharge,
		Evaporation,

		// Power plant
		LoadEfficiency,
		Peaking,

		// Transfer
		HeadFlowCapacity
	}

	public static class CurveTypeExtensions
	{
		public static int GetCode(this CurveType curveType)
		{
			return CurveFactory.CurveTypes.TryGetValue(curveType, out int curveCode)
				? curveCode
				: throw new EnumValueNotDefinedException<CurveType>(curveType);
		}
	}
}
