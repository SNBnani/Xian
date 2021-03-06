#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Enterprise.Authentication {


    /// <summary>
    /// UserSession entity
    /// </summary>
	public partial class UserSession
	{
    	private bool _terminated;

		/// <summary>
		/// Gets a session token that references this user session.
		/// </summary>
		/// <returns></returns>
		public virtual SessionToken GetToken()
		{
			CheckNotTerminated();

			return new SessionToken(_sessionId, _expiryTime);
		}

		/// <summary>
		/// Checks that this session is valid for the specified user and that the user account is still active,
		/// optionally checking whether the session has expired.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="checkExpiry"></param>
		public virtual void Validate(string userName, bool checkExpiry)
		{
			CheckNotTerminated();

			DateTime currentTime = Platform.Time;
			if (_user.UserName != userName || !_user.IsActive(currentTime))
			{
				// session does not match user name
				// the error message is deliberately vague
				throw new InvalidUserSessionException();
			}

			// check expiry time if specified
			if (checkExpiry && IsExpiredHelper(currentTime))
			{
				// session has expired
				// the error message is deliberately vague
				throw new InvalidUserSessionException();
			}
		}

		/// <summary>
		/// Renews the session, setting a new expiry based on the specified timeout.
		/// </summary>
		/// <remarks>
		/// The session may be renewed even if it has already expired.  To ensure
		/// the session has not expired prior to renewing, use <see cref="Validate"/>.
		/// </remarks>
		/// <param name="timeout"></param>
		public virtual void Renew(TimeSpan timeout)
		{
			Platform.CheckPositive(timeout.TotalMilliseconds, "timeout");
			CheckNotTerminated();

			// check expiry time if specified
			DateTime currentTime = Platform.Time;

			// renew the session expiration time
			_expiryTime = currentTime + timeout;
		}

		/// <summary>
		/// Gets a value indicating if this session is expired.
		/// </summary>
    	public virtual bool IsExpired
    	{
			get
			{
				CheckNotTerminated();
				return IsExpiredHelper(Platform.Time);
			}
    	}

		/// <summary>
		/// Gets a value indicating whether this session has been terminated.
		/// </summary>
    	public virtual bool IsTerminated
    	{
			get { return _terminated; }
    	}

		/// <summary>
		/// Terminates this session.  After calling this method, this object can no longer be used.
		/// </summary>
		public virtual void Terminate()
		{
			CheckNotTerminated();

			// remove this session from the user's collection
			_user.Sessions.Remove(this);

			// set user to null
			_user = null;

			// set terminated
			_terminated = true;
		}

		#region Helpers

		private void CheckNotTerminated()
		{
			if(_terminated)
				throw new InvalidOperationException("This session has already been terminated.");
		}

		private bool IsExpiredHelper(DateTime currentTime)
		{
			return _expiryTime < currentTime;
		}

		/// <summary>
		/// This method is called from the constructor.  Use this method to implement any custom
		/// object initialization.
		/// </summary>
		private void CustomInitialize()
		{
		}

		#endregion
	}
}