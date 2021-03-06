#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common.Caching
{
	/// <summary>
	/// Defines an extension point for implementations of <see cref="ICacheProvider"/>.
	/// </summary>
	[ExtensionPoint]
	public class CacheProviderExtensionPoint : ExtensionPoint<ICacheProvider>
	{
	}

	/// <summary>
	/// Static class providing access to the global singleton appliction cache.
	/// </summary>
	public static class Cache
	{
		/// <summary>
		/// Maintains the singleton instance of each class of provider.
		/// </summary>
		private static readonly Dictionary<Type, ICacheProvider> _providers = new Dictionary<Type, ICacheProvider>();

		/// <summary>
		/// Gets a value indicating if the cache is supported in this environment.
		/// </summary>
		/// <returns></returns>
		public static bool IsSupported()
		{
			var point = new CacheProviderExtensionPoint();
			return point.ListExtensions().Length > 0;
		}

		/// <summary>
		/// Creates a cache client for the specified logical cacheID.
		/// </summary>
		/// <remarks>
		/// This method is safe for concurrent use by multiple threads.
		/// </remarks>
		public static ICacheClient CreateClient(string cacheId)
		{
			// a cacheID is required!
			Platform.CheckForEmptyString(cacheId, "CacheID");

			// TODO a more sophisticated delegate may be required here
			// if more than one cache provider extension exists, there will need to be mechanisms for choosing
			// the appropriate provider, which may be influenced by a) the creation args,
			// and b) potentially some external configuration settings
			var provider = GetProvider(delegate { return true; });

			// create specified cache
			// this call assumes the provider.CreateClient method is thread-safe, which
			// is the responsibility of the provider!
			var client = provider.CreateClient(cacheId);

			// if debug logging enabled, wrap client in a logging decorator set at Debug level
			if (Platform.IsLogLevelEnabled(LogLevel.Debug))
			{
				client = new CacheClientLoggingDecorator(client, LogLevel.Debug);
			}
			return client;
		}

		/// <summary>
		/// Thread-safe method to obtain singleton instance of <see cref="ICacheProvider"/>
		/// matching specified filter.
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		private static ICacheProvider GetProvider(Predicate<ExtensionInfo> filter)
		{
			// determine the provider class
			var point = new CacheProviderExtensionPoint();
			var extension = CollectionUtils.FirstElement(point.ListExtensions(filter));
			if (extension == null)
				throw new CacheException("No cache provider extension found, or those that exist do not support all required features.");

			var providerClass = extension.ExtensionClass;

			// check if we already have an initialized instance of this provider class.
			ICacheProvider provider;
			if (!_providers.TryGetValue(providerClass, out provider))
			{
				// if not, create one
				lock (_providers)
				{
					// ensure that another thread hasn't beat us to it
					if (!_providers.TryGetValue(providerClass, out provider))
					{
						provider = (ICacheProvider)point.CreateExtension(
							new ClassNameExtensionFilter(providerClass.FullName));

						// initialize this provider and store it
						provider.Initialize(new CacheProviderInitializationArgs());
						_providers.Add(providerClass, provider);
					}
				}
			}
			return provider;
		}
	}
}
