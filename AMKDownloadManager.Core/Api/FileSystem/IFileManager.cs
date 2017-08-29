using System.IO;

namespace AMKDownloadManager.Core.Api.FileSystem
{
	public interface IFileManager
	{
		/// <summary>
		/// Allows atomic (flush-on-demand) writing to file. 
		/// </summary>
		/// <param name="stream">Data stream to be written</param>
		/// <param name="fileStart">Start offset of file to begin write</param>
		/// <param name="streamStart">Start offset of stream to begin read</param>
		/// <param name="length">Length of data to write to file</param>
		void SaveStream(Stream stream, long fileStart, long streamStart, long length);
		
		/// <summary>
		/// Allows atomic (flush-on-demand) writing to file. 
		/// </summary>
		/// <param name="binary">Binary data to be written</param>
		/// <param name="fileStart">Start offset of file to begin write</param>
		/// <param name="binaryStart">Start offset of data to begin read</param>
		/// <param name="length">Length of data to write to file</param>
		void SaveBinary(byte[] binary, long fileStart, long binaryStart, long length);
	}
}