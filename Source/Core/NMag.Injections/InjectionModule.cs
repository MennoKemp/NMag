using Auxilia.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NMag.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NMag.Injections
{
	public abstract class InjectionModule
	{
		private static readonly InjectionModule[] _modules;

		static InjectionModule()
		{
			_modules = typeof(InjectionModule).Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(InjectionModule)))
				.Select(Activator.CreateInstance)
				.OfType<InjectionModule>()
				.ToArray();
		}

		public static void Load(IServiceCollection services, IPersistenceSettings settings)
		{
			services.AddSingleton(settings);
			_modules.Execute(m => m.ProtectedLoad(services));
		}

		internal static Dictionary<Type, Type> GetServiceTypes(Assembly assembly, string serviceSuffix)
		{
			Dictionary<Type, Type> services = new Dictionary<Type, Type>();

			IEnumerable<Type> types = assembly.GetTypes().Where(
				t => t.IsClass && 
				!t.IsAbstract && 
				t.Name.EndsWith(serviceSuffix));

			foreach (Type type in types)
			{
				Type[] interfaces = type.GetInterfaces();

				if (interfaces.Length != 1)
					continue;

				Type service = interfaces.Single();

				if (!service.Name.EndsWith(serviceSuffix))
					continue;

				services.Add(service, type);
			}

			return services;
		}

		protected abstract void ProtectedLoad(IServiceCollection services);
	}
}
