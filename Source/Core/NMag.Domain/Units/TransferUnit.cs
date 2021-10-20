using Auxilia.Extensions;

namespace NMag
{
	public class TransferUnit : Unit<TransferUnit>
	{
		internal TransferUnit()
			: base(CurveFactory.TransferCurveTypes)
		{
		}

		private TransferUnit(TransferUnit unit) 
			: base(unit)
		{
			unit.ThrowIfNull(nameof(unit));

			Mode = unit.Mode;

			Capacity = unit.Capacity;

			InletElevation = unit.InletElevation;
			OutletElevation = unit.OutletElevation;
			HeadLossCoefficient = unit.HeadLossCoefficient;

			IntakeElevation = unit.IntakeElevation;
			TailWaterElevation = unit.TailWaterElevation;
			HeadFlowCapacityCurve = unit.HeadFlowCapacityCurve.Clone();
		}

		public override UnitType Type { get; } = UnitType.Transfer;

		// Optional data
		public TransferMode Mode
		{
			get => _mode;
			set => _mode = value.ThrowIfNotDefined(nameof(value));
		}
		private TransferMode _mode;

		// Transfer mode 100
		public double Capacity { get; set; }

		// Transfer mode 200
		public double InletElevation { get; set; }
		public double OutletElevation { get; set; }
		public double HeadLossCoefficient { get; set; }

		// Transfer mode 300
		public double IntakeElevation { get; set; }
		public double TailWaterElevation { get; set; }
		public Curve HeadFlowCapacityCurve { get; set; } = CurveFactory.CreateCurve(CurveType.HeadFlowCapacity);

		public override TransferUnit Clone()
		{
			return new TransferUnit(this);
		}
	}
}
