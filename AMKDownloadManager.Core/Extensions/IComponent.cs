using System;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core.Extensions
{
	public interface IComponent
	{
		string Name { get; }
		string Description { get; }
		string Author { get; }

		Version Version { get; }

        void Initialize(IAppContext app);
        void Unload(IAppContext app);
	}
}