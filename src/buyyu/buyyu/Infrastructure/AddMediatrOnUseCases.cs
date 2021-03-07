using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace buyyu.web.Infrastructure
{
	public static class MediatorExtensions
	{
		public static IServiceCollection AddMediatrOnUseCases(this IServiceCollection services)
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

			var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.BL.dll").ToList();

			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

			toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

			var useCaseAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic && x.GetName().Name.EndsWith(".BL")).ToArray();

			services.AddMediatR(useCaseAssemblies);

			return services;
		}
	}
}