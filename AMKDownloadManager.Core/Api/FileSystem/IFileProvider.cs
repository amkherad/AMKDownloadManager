﻿namespace AMKDownloadManager.Core.Api.FileSystem
{
	public interface IFileProvider : IFeature
	{
		/// <summary>
		/// Creates a file to save download resource.
		/// </summary>
		/// <param name="appContext">ApplicationContext</param>
		/// <param name="name">Optional file name</param>
		/// <param name="size">Download resource size, this may change</param>
		/// <param name="chunks">Number of probable chunks, this may change</param>
		/// <returns></returns>
		IFileManager CreateFile(IAppContext appContext, string name, long? size, int? chunks);

		/// <summary>
		/// Try to reserve a file for writing, if file name is not available this may offer a new file name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		string TryReserveFileName(string name);
	}
}