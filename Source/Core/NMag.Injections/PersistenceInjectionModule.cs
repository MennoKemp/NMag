using Auxilia.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NMag.Injections
{
	internal class PersistenceInjectionModule : InjectionModule
	{
		protected override void ProtectedLoad(IServiceCollection services)
		{
			GetServiceTypes(Assembly.Load("NMag.Persistence"), "Dao").Execute(s => services.AddTransient(s.Key, s.Value));
		}
	}
}
