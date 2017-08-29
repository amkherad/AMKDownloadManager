using System;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// An application feature.
    /// </summary>
	public interface IFeature
	{
		int Order { get; }

		void LoadConfig(
			IAppContext appContext,
			IConfigProvider configProvider
		);
	}
}