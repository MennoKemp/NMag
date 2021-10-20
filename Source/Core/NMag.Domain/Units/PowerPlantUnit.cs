namespace NMag
{
	public class PowerPlantUnit : Unit<PowerPlantUnit>
	{
		internal PowerPlantUnit()
			: base(CurveFactory.PowerPlantCurveTypes)
		{
		}

		private PowerPlantUnit(PowerPlantUnit unit) 
			: base(unit)
		{
			MaximumDischarge = unit.MaximumDischarge;
			EnergyEquivalent = unit.EnergyEquivalent;

			NominalHead = unit.NominalHead;
			IntakeLevel = unit.IntakeLevel;
			TailWaterLevel = unit.TailWaterLevel;
			HeadLossCoefficient = unit.HeadLossCoefficient;
		}

		public override UnitType Type { get; } = UnitType.PowerPlant;

		public double MaximumDischarge { get; set; }
		public double EnergyEquivalent { get; set; }

		public double NominalHead { get; set; }
		public double IntakeLevel { get; set; }
		public double TailWaterLevel { get; set; }
		public double HeadLossCoefficient { get; set; }

		public override PowerPlantUnit Clone()
		{
			return new PowerPlantUnit(this);
		}
	}
}
