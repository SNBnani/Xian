#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Common;
using Path = System.IO.Path;
using Timer = System.Threading.Timer;

namespace ClearCanvas.Ris.Client
{
	/// <summary>
	/// Manages a cache of temporary files on disk.
	/// </summary>
	/// <remarks>
	/// Allows arbitrary data to be stored on disk in a Windows temporary file, with a specified time-to-live.
	/// Expired temporary files will be periodically cleaned up.  All temporary files, regardless of whether expired,
	/// will be deleted when the application shuts down, except in the case of a crash.
	/// </remarks>
	public class TempFileManager
	{
		#region Entry class

		/// <summary>
		/// Represents an entry in the map.
		/// </summary>
		class Entry
		{
			private readonly TimeSpan _timeToLive;
			private readonly string _file;
			private DateTime _expiryTime;

			public Entry(string file, TimeSpan ttl)
			{
				_file = file;
				_timeToLive = ttl;
				Renew();	// initialize expiry time
			}

			public string File
			{
				get { return _file; }
			}

			public bool IsExpired
			{
				get { return DateTime.Now > _expiryTime; }
			}

			public void Renew()
			{
				_expiryTime = DateTime.Now + _timeToLive;
			}
		}

		#endregion

		private const uint SweepInterval = 15000; // 15 seconds

		private static readonly TempFileManager _instance = new TempFileManager();

		private readonly Dictionary<object, Entry> _entryMap = new Dictionary<object, Entry>();
		private readonly Timer _timer;
		private readonly object _syncObj = new object();


		/// <summary>
		/// Gets the singleton instance of this class.
		/// </summary>
		public static TempFileManager Instance
		{
			get { return _instance; }
		}

		#region Public API

		/// <summary>
		/// Create a temporary file associated with the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fileExtension"></param>
		/// <param name="timeToLive"></param>
		/// <returns>The path of the file.</returns>
		public string CreateFile(object key, string fileExtension, TimeSpan timeToLive)
		{
			return CreateTempFile(key, fileExtension, timeToLive);
		}

		/// <summary>
		/// Creates a temporary file associated with the specified key, and containing the specified data.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fileExtension"></param>
		/// <param name="data"></param>
		/// <param name="timeToLive"></param>
		/// <returns>The path of the file.</returns>
		public string CreateFile(object key, string fileExtension, byte[] data, TimeSpan timeToLive)
		{
			var file = CreateTempFile(key, fileExtension, timeToLive);
	
			// write data to the temp file
			File.WriteAllBytes(file, data);

			return file;
		}

		/// <summary>
		/// Gets the temporary file associated with the specified key, if it exists, otherwise null.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetFile(EntityRef key)
		{
			// lock so that we don't read anything that is being removed concurrently by the sweep
			lock (_syncObj)
			{
				Entry entry;
				if (!_entryMap.TryGetValue(key, out entry))
					return null;

				// if the file does not actually exist for whatever reason, return null
				if (!File.Exists(entry.File))
					return null;

				// renew the item since it has just been accessed
				entry.Renew();

				return entry.File;
			}
		}

		#endregion

		private TempFileManager()
		{
			// set up timer to periodically remove expired entries
			_timer = new Timer(TimerCallback, null, SweepInterval, SweepInterval);

			// also remove all entries when the desktop is shutdown
			// (it isn't nice to have this dependency here, but seems to be no other easy way to do this)
			Desktop.Application.Quitting +=
				(sender, args) =>
					{
						_timer.Dispose();
						Clean(obj => true);
					};
		}

		private string CreateTempFile(object key, string fileExtension, TimeSpan timeToLive)
		{
			// create a temp file on disk to store the data
			// the OS always returns a file with the .tmp extension, so we rename it to the specified extension
			var tmpFile = Path.GetTempFileName();
			var file = tmpFile.Replace(".tmp", "." + fileExtension);
			File.Move(tmpFile, file);

			// lock while we update the map
			lock (_syncObj)
			{
				// add entry for this file
				_entryMap.Add(key, new Entry(file, timeToLive));
			}

			Platform.Log(LogLevel.Debug, "TempFileManager: created file {0}", file);
			return file;
		}

		private void TimerCallback(object state)
		{
			// clean expired entries
			Clean(entry => entry.IsExpired);
		}

		/// <summary>
		/// Delete all entries and associated files, matching the specified condition.
		/// </summary>
		/// <param name="condition"></param>
		private void Clean(Predicate<Entry> condition)
		{
			List<KeyValuePair<object, Entry>> deletionCandidates;

			lock (_syncObj)
			{
				deletionCandidates = _entryMap.Where(kvp => condition(kvp.Value)).ToList();
			}

			var deletions = new List<KeyValuePair<object, Entry>>();
			foreach (var entry in deletionCandidates)
			{
				if(TryDeleteFile(entry.Value.File))
				{
					deletions.Add(entry);
					Platform.Log(LogLevel.Debug, "TempFileManager: deleted file {0}", entry.Value.File);
				}
			}

			lock (_syncObj)
			{
				// remove successful deletions from map
				foreach (var kvp in deletions)
				{
					_entryMap.Remove(kvp.Key);
				}
			}
		}

		private bool TryDeleteFile(string file)
		{
			try
			{
				// this is a nop if the file does not exist
				File.Delete(file);	

				// return true if the file no longer exists
				return !File.Exists(file);
			}
			catch (Exception e)
			{
				// this will likely happen a lot, so it isn't really an exceptional condition
				// therefore, just log at debug level
				Platform.Log(LogLevel.Debug, e, SR.ExceptioinFailedToDeleteTemporaryFiles);
				return false;
			}
		}

	}
}
