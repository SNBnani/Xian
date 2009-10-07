using System.Net;
using System.IO;

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Provide access to remote files with Http scheme.
	/// </summary>
	/// <remarks>
	/// This provider class does not create remote directory before uploading files.
	/// </remarks>
	public class HttpFileAccessProvider : RemoteFileAccessProvider
	{
		private readonly string _userId;
		private readonly string _password;

		/// <summary>
		/// Default constructor for no authentication.
		/// </summary>
		public HttpFileAccessProvider()
		{
		}

		/// <summary>
		/// Constructor with authentication provided.
		/// </summary>
		public HttpFileAccessProvider(string userId, string password)
		{
			_userId = userId;
			_password = password;
		}

		/// <summary>
		/// Upload one file from local to remote.
		/// </summary>
		/// <param name="request"></param>
		/// <remarks>
		/// The remote directories are not created before uploading files.
		/// </remarks>
		protected override void Upload(FileTransferRequest request)
		{
			var webClient = new WebClient();

			if (!string.IsNullOrEmpty(_userId) && !string.IsNullOrEmpty(_password))
				webClient.Credentials = new NetworkCredential(_userId, _password);

			webClient.UploadFile(request.RemoteFile, request.LocalFile);
		}

		/// <summary>
		/// Download one file from remote to local
		/// </summary>
		/// <param name="request"></param>
		protected override void Download(FileTransferRequest request)
		{
			var webClient = new WebClient();

			if (!string.IsNullOrEmpty(_userId) && !string.IsNullOrEmpty(_password))
				webClient.Credentials = new NetworkCredential(_userId, _password);

			var downloadDirectory = Path.GetDirectoryName(request.LocalFile);
			if (!Directory.Exists(downloadDirectory))
				Directory.CreateDirectory(downloadDirectory);

			webClient.DownloadFile(request.RemoteFile, request.LocalFile);
		}
	}
}
