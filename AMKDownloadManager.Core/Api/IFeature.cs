using System.Collections.Generic;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// An application feature. All parts of the application implemented as a module so it can be replaced later.
    /// </summary>
	public interface IFeature
	{
		int Order { get; }

		/// <summary>
		/// Signals the features to load dependencies when they are ready.
		/// </summary>
		void ResolveDependencies(
			IApplicationContext appContext,
			ITypeResolver typeResolver
			);
		
		/// <summary>
		/// Loads feature configurations from config provider. also get called when a config is changing.
		/// </summary>
		/// <param name="applicationContext">The application context</param>
		/// <param name="configProvider">Application config provider module</param>
		/// <param name="changes">A list of changed configs, this can be null if no change happened or config reload requested</param>
		void LoadConfig(
			IApplicationContext applicationContext,
			IConfigProvider configProvider,
			HashSet<string> changes
		);
	}
}