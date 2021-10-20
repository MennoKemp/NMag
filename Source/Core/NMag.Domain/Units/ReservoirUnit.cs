namespace NMag
{
	public class ReservoirUnit : Unit<ReservoirUnit>
	{
		internal ReservoirUnit()
			: base(CurveFactory.ReservoirCurveTypes)
		{
		}

		private ReservoirUnit(ReservoirUnit unit)
			: base(unit)
		{
			LowestRegulatedWaterLevel = unit.LowestRegulatedWaterLevel;
			HighestRegulatedWaterLevel = unit.HighestRegulatedWaterLevel;
			Volume = unit.Volume;

			OutletLevel = unit.OutletLevel;
		}

		public override UnitType Type { get; } = UnitType.Reservoir;
		
		public double LowestRegulatedWaterLevel { get; set; }
		public double HighestRegulatedWaterLevel { get; set; }
		public double Volume { get; set; }

		public double OutletLevel { get; set; }
		
		public override ReservoirUnit Clone()
		{
			return new ReservoirUnit(this);
		}
	}
}
