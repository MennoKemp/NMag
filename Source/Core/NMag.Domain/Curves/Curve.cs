using Auxilia;
using Auxilia.Extensions;
using System.Collections.Generic;
using System.Windows;

namespace NMag
{
	public class Curve : ICloneable<Curve>
	{
		internal Curve(CurveType type)
		{
			Type = type.ThrowIfNotDefined(nameof(type));
		}
		internal Curve(CurveType type, IList<Point> points)
			: this(type)
		{
			Points = points;
			IsEnabled = true;
		}

		private Curve(Curve curve)
		{
			curve.ThrowIfNull(nameof(curve));

			Type = curve.Type;
			IsEnabled = curve.IsEnabled;

			Points = new List<Point>(curve.Points);
		}

		public CurveType Type { get; }
		public bool IsEnabled { get; set; }

		public IList<Point> Points
		{
			get => _points;
			set => _points = value.ThrowIfNull(nameof(value));
		}
		private IList<Point> _points;

		public Curve Clone()
		{
			return new Curve(this);
		}
	}
}
