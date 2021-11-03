using Auxilia.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NMag.Persistence;
using NMag.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NMag.Injections.Tests
{
	[TestFixture]
	public class InjectionTests
	{
		protected IServiceProvider _serviceProvider;

		[SetUp]
		public void SetUp()
		{
			IServiceCollection serviceCollection = new ServiceCollection();
			InjectionModule.Load(serviceCollection, new DummyApplicationSettings());
			_serviceProvider = serviceCollection.BuildServiceProvider();
		}

		[Test]
		[TestCase(typeof(IDataSetDao))]
		[TestCase(typeof(IProjectDao))]
		public void PersistenceInjections_GetInstances_CorrectlyInjected(Type serviceType)
		{
			Assert.NotNull(_serviceProvider.GetService(serviceType));
		}

		[Test]
		[TestCase(typeof(IProjectService))]
		[TestCase(typeof(IGraphCreationService))]
		public void ServiceInjections_GetInstances_CorrectlyInjected(Type serviceType)
		{
			Assert.NotNull(_serviceProvider.GetService(serviceType));
		}

		[Test]
		[TestCase(nameof(PersistenceInjections_GetInstances_CorrectlyInjected), "NMag.Persistence", "Dao")]
		[TestCase(nameof(ServiceInjections_GetInstances_CorrectlyInjected), "NMag.Services", "Service")]
		public void Services_CheckCovered_CoveredByTest(string testMethodName, string assemblyName, string serviceSuffix)
		{
			List<Type> coveredByTests = GetType().GetMethod(testMethodName)
				.GetCustomAttributes<TestCaseAttribute>()
				.Select(a => a.Arguments.Single())
				.OfType<Type>()
				.ToList();

			List<Type> uncoveredByTests = InjectionModule.GetServiceTypes(Assembly.Load(assemblyName), serviceSuffix)
				.Select(s => s.Key)
				.Where(s => !coveredByTests.Contains(s))
				.ToList();

			Assert.AreEqual(0, uncoveredByTests.Count, $"Not covered by test:{Environment.NewLine}{uncoveredByTests.Select(t => t.Name).Combine(Environment.NewLine)}");
		}
	}
}
