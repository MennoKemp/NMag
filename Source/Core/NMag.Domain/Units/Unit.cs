using Auxilia;
using Auxilia.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMag
{
	public abstract class Unit<T> : IUnit, ICloneable<T>
	{
		protected Unit(CurveType[] curveTypes)
		{
			CurveTypes = curveTypes.ThrowIfNull(nameof(curveTypes));
			curveTypes.Execute(t => Curves.Add(t, CurveFactory.CreateCurve(t)));
		}

		protected Unit(IUnit unit)
		{
			unit.ThrowIfNull(nameof(unit));
			
			Name = unit.Name;
			Curves = unit.Curves.Values.Clone().ToDictionary(c => c.Type);
		}

		public abstract UnitType Type { get; }
		public string Name { get; set; }
		
		public CurveType[] CurveTypes { get; }
		public Dictionary<CurveType, Curve> Curves { get; set; } = new Dictionary<CurveType, Curve>();

		public void SetCurve(Curve curve)
		{
			curve.ThrowIfNull(nameof(curve));

			if (!Curves.ContainsKey(curve.Type))
				throw new ArgumentException($"Invalid type {curve.Type} for {this}.", nameof(curve));

			Curves[curve.Type] = curve;
		}

		public abstract T Clone();

		public override string ToString()
		{
			return $"{Type}: {Name}";
		}
	}
}
