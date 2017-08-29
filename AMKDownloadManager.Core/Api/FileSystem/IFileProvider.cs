namespace AMKDownloadManager.Core.Api.FileSystem
{
	public interface IFileProvider
	{
		/// <summary>
		/// Creates a file to save download resource.
		/// </summary>
		/// <param name="provider">Download path provider</param>
		/// <param name="name">Full path to file</param>
		/// <param name="size">Download resource size, this may change</param>
		/// <param name="chunks">Number of probable chunks, this may change</param>
		/// <returns></returns>
		IFileManager CreateFile(IDownloadPathProvider provider, string name, long size, int? chunks);

		/// <summary>
		/// Try to reserve a file for writing, if file name is not available this may offer a new file name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		string TryReserveFileName(string name);
	}
}