using Auxilia.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NMag.Injections
{
	internal class ServiceInjectionModule : InjectionModule
	{
		protected override void ProtectedLoad(IServiceCollection services)
		{
			GetServiceTypes(Assembly.Load("NMag.Services"), "Service").Execute(s => services.AddTransient(s.Key, s.Value));
		}
	}
}
