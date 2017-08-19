using System;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.Modifiers
{
    /// <summary>
    /// Service to modify a download request to fulfill it's service.
    /// </summary>
	public interface IRequestModifier : IFeature
	{
        /// <summary>
        /// Apply modification to the specified request.
        /// </summary>
        /// <param name="request">Request.</param>
		void Apply(IRequest request);
	}
}