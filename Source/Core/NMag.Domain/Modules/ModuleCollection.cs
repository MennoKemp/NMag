using Auxilia.Extensions;
using Auxilia.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMag
{
	public class ModuleCollection : ICollection<Module>
	{
		private readonly Dictionary<int, Module> _modules;

		public ModuleCollection()
		{
			_modules = new Dictionary<int, Module>();
		}
		public ModuleCollection(IEnumerable<Module> modules)
		{
			_modules = modules.ToDictionary(m => m.Id);
		}

		public int Count
		{
			get => _modules.Count;
		}

		public bool IsReadOnly { get; } = false;

		public Module this[int id]
		{
			get
			{
				_modules.TryGetValue(id, out Module module);
				return module;
			}
		}

		public IEnumerator<Module> GetEnumerator()
		{
			return _modules.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _modules.Values.GetEnumerator();
		}

		public int GetNewId()
		{
			return MathUtils.FindLowestPositive(_modules.Keys);
		}

		public Module Add(ModuleType moduleType)
		{
			return Add(moduleType, MathUtils.FindLowestPositive(_modules.Keys, false));
		}
		public Module Add(ModuleType moduleType, int id)
		{
			Module module = new Module(moduleType, id);
			Add(module);
			return module;
		}
		public void Add(Module module)
		{
			module.ThrowIfNull(nameof(module));

			if (this[module.Id] is Module existingModule)
			{
				module = existingModule.CanCombine(module) 
					? existingModule.Combine(module) 
					: throw new ArgumentException($"Already contains a module with id {module.Id}.", nameof(module));

				_modules.Remove(module.Id);
			}
			
			_modules.Add(module.Id, module);
		}

		public bool Remove(int id)
		{
			return _modules.Remove(id);
		}
		public bool Remove(Module item)
		{
			return item != null && _modules.Remove(item.Id);
		}
		
		public void Clear()
		{
			_modules.Clear();
		}

		public bool Contains(int id)
		{
			return _modules.ContainsKey(id);
		}
		public bool Contains(Module item)
		{
			return item != null && _modules.ContainsKey(item.Id);
		}

		public void CopyTo(Module[] array, int arrayIndex)
		{
			_modules.Values.CopyTo(array, arrayIndex);
		}
	}
}
