using Auxilia;
using Auxilia.Extensions;
using System;
using System.Linq;

namespace NMag
{
	public class Module : ICloneable<Module>, IEquatable<Module>
	{
		public Module(ModuleType type, int id)
		{
			Type = type.ThrowIfNotDefined(nameof(type));
			Id = id.ThrowIfOutOfRange(nameof(id), 1);

			switch (type)
			{
				case ModuleType.Reservoir:
				{
					Reservoir = new ReservoirUnit();
					break;
				}
				case ModuleType.PowerPlant:
				{
					PowerPlant = new PowerPlantUnit();
					break;
				}
				case ModuleType.Transfer:
				{
					Transfer = new TransferUnit();
					break;
				}
				case ModuleType.ControlPoint:
				{
					ControlPoint = new ControlPointUnit();
					break;
				}
				case ModuleType.ImpoundmentFacility:
				{
					Reservoir = new ReservoirUnit();
					PowerPlant = new PowerPlantUnit();
					break;
				}
				default:
				{
					throw new EnumValueNotDefinedException<ModuleType>(type, nameof(type));
				}
			}
		}

		protected Module(Module module)
		{
			module.ThrowIfNull(nameof(module));

			Type = module.Type;
			Id = module.Id;

			Reservoir = module.Reservoir.Clone();
			PowerPlant = module.PowerPlant.Clone();
			Transfer = module.Transfer.Clone();
			ControlPoint = module.ControlPoint.Clone();

			ReleaseModuleId = module.ReleaseModuleId;
			BypassModuleId = module.BypassModuleId;
			SpillModuleId = module.SpillModuleId;
		}

		public ModuleType Type { get; }
		public int Id { get; }
		public string Name
		{
			get => Type switch
			{
				ModuleType.Reservoir => Reservoir.Name,
				ModuleType.PowerPlant => PowerPlant.Name,
				ModuleType.Transfer => Transfer.Name,
				ModuleType.ControlPoint => ControlPoint.Name,
				ModuleType.ImpoundmentFacility => $"{Reservoir.Name}/{PowerPlant.Name}",
				_ => string.Empty
			};
		}
		
		public ReservoirUnit Reservoir { get; private init; }
		public PowerPlantUnit PowerPlant { get; private init; }
		public TransferUnit Transfer { get; }
		public ControlPointUnit ControlPoint { get; }

		public int ReleaseModuleId { get; set; }
		public int BypassModuleId { get; set; }
		public int SpillModuleId { get; set; }

		internal bool CanCombine(Module other)
		{
			return Type == ModuleType.Reservoir && other.Type == ModuleType.PowerPlant ||
			       Type == ModuleType.PowerPlant && other.Type == ModuleType.Reservoir;
		}

		internal Module Combine(Module other)
		{
			other.ThrowIfNull(nameof(other));

			if (Id != other.Id)
				throw new ArgumentException("Id mismatch.", nameof(other));

			Module[] modules = { this, other };

			Module reservoirModule = modules.FirstOrDefault(m => m.Type == ModuleType.Reservoir);
			Module powerPlantModule = modules.FirstOrDefault(m => m.Type == ModuleType.PowerPlant);

			if (reservoirModule != null && powerPlantModule != null)
			{
				return new Module(ModuleType.ImpoundmentFacility, Id)
				{
					Reservoir = reservoirModule.Reservoir,
					PowerPlant = powerPlantModule.PowerPlant,
					ReleaseModuleId = reservoirModule.ReleaseModuleId,
					BypassModuleId = reservoirModule.BypassModuleId,
					SpillModuleId = reservoirModule.SpillModuleId,
				};
			}

			throw new NotSupportedException($"Combining {this} and {other} is not supported.");
		}
		
		public Module Clone()
		{
			return new Module(this);
		}

		public override string ToString()
		{
			return Type.IsAnyOf(ModuleType.ControlPoint, ModuleType.ImpoundmentFacility)
				? $"{Id}: {Name}"
				: $"{Id}: {Type.GetDescription()} {Name}";
		}

		public bool Equals(Module other)
		{
			return Id.Equals(other?.Id);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Module);
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}
