using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NMag
{
	public static class CurveFactory
	{
		internal static Dictionary<CurveType, int> CurveTypes { get; } = new Dictionary<CurveType, int>
		{
			{ CurveType.Routing, 600 },

			{ CurveType.Volume, 200 },
			{ CurveType.SpillFlow, 300 },
			{ CurveType.Discharge, 400 },
			{ CurveType.Evaporation, 500 },

			{ CurveType.LoadEfficiency, 200 },
			{ CurveType.Peaking, 400 },
			
			{ CurveType.HeadFlowCapacity, 300 }
		};

		internal static CurveType[] ReservoirCurveTypes { get; } =
		{
			CurveType.Volume,
			CurveType.SpillFlow,
			CurveType.Discharge,
			CurveType.Evaporation,
			CurveType.Routing
		};

		internal static CurveType[] PowerPlantCurveTypes { get; } =
		{
			CurveType.LoadEfficiency,
			CurveType.Peaking,
			CurveType.Routing
		};

		internal static CurveType[] TransferCurveTypes { get; } =
		{
			CurveType.HeadFlowCapacity,
			CurveType.Routing
		};

		internal static CurveType[] ControlPointCurveTypes { get; } =
		{
			CurveType.Routing
		};

		public static Curve CreateCurve(CurveType curveType)
		{
			return new Curve(curveType);
		}
		public static Curve CreateCurve(CurveType curveType, IList<Point> points)
		{
			return new Curve(curveType, points);
		}
		private static Curve CreateCurve(int curveCode, CurveType[] curveTypes, IList<Point> points)
		{
			return FindCurveType(curveCode, curveTypes) is CurveType curveType
				? CreateCurve(curveType, points)
				: throw new ArgumentException($"Invalid code \"{curveCode}\"", nameof(curveCode));
		}

		private static CurveType? FindCurveType(int curveCode, IEnumerable<CurveType> curveTypes)
		{
			return curveTypes.Aggregate<CurveType, CurveType?>(null, (match, type) => CurveTypes[type].Equals(curveCode) ? type : match);
		}

		public static Curve CreateUnitCurve(IUnit unit, int curveCode, IList<Point> points)
		{
			return CreateCurve(curveCode, unit.CurveTypes, points);
		}
	}
}
