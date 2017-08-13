using System;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.Modifiers
{
	public interface IRequestModifier : IFeature
	{
		void Apply(IRequest request);
	}
}