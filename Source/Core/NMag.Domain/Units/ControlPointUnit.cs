namespace NMag
{
	public class ControlPointUnit : Unit<ControlPointUnit>
	{
		internal ControlPointUnit()
			: base(CurveFactory.ControlPointCurveTypes)
		{
		}

		private ControlPointUnit(IUnit unit) 
			: base(unit)
		{
		}

		public override UnitType Type { get; } = UnitType.ControlPoint;

		public override ControlPointUnit Clone()
		{
			return new ControlPointUnit(this);
		}
	}
}
