namespace AMKDownloadManager.Core.Api.FileSystem
{
	public interface IFileProvider : IFeature
	{
		/// <summary>
		/// Creates a file to save download resource.
		/// </summary>
		/// <param name="applicationContext">ApplicationContext</param>
		/// <param name="name">Optional file name</param>
		/// <param name="mimeType">Optional file mime type</param>
		/// <param name="size">Download resource size, this may change</param>
		/// <param name="parts">Number of probable parts, (value may change after creation)</param>
		/// <returns></returns>
		IFileManager CreateFile(
			IApplicationContext applicationContext,
			string name,
			string mimeType,
			long? size,
			int? parts
		);
		
		/// <summary>
		/// Resumes a file to save download resource.
		/// </summary>
		/// <param name="applicationContext">ApplicationContext</param>
		/// <param name="filePath">Target file path</param>
		/// <param name="mimeType">Optional file mime type</param>
		/// <param name="size">Download resource size, this may change</param>
		/// <param name="parts">Number of probable chunks, (value may change after creation)</param>
		/// <returns></returns>
		IFileManager ResumeFile(
			IApplicationContext applicationContext,
			string filePath,
			string mimeType,
			long? size,
			int? parts
		);

		/// <summary>
		/// Try to reserve a file for writing, if file name is not available this may offer a new file name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		string TryReserveFileName(string name);
	}
}