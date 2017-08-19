using System;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core.Extensions
{
    /// <summary>
    /// Application component.
    /// </summary>
	public interface IComponent
	{
        /// <summary>
        /// Name of the component.
        /// </summary>
        /// <value>The name.</value>
		string Name { get; }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
		string Description { get; }
        /// <summary>
        /// Author information.
        /// </summary>
        /// <value>The author.</value>
		string Author { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
		Version Version { get; }

        /// <summary>
        /// Installs the component for the first time.
        /// </summary>
        /// <param name="app">App.</param>
        void Install(IAppContext app);

        /// <summary>
        /// Uninstalls the component.
        /// </summary>
        /// <param name="app">App.</param>
        void Uninstall(IAppContext app);

        /// <summary>
        /// Initialize the component with specified application service pool.
        /// </summary>
        /// <param name="app">App.</param>
        void Initialize(IAppContext app);

        /// <summary>
        /// Unload the the component with specified application service pool.
        /// </summary>
        /// <param name="app">App.</param>
        void Unload(IAppContext app);
	}
}