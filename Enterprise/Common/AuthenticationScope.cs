using System;
using System.Security.Principal;
using System.Threading;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Common.Authentication;

namespace ClearCanvas.Enterprise.Common
{
	/// <summary>
	/// Establishes an authentication scope around a block of code.
	/// </summary>
	/// <remarks>
	/// The purpose of this class is to establish an authentication scope.  Within the scope,
	/// the <see cref="Thread.CurrentPrincipal"/> is set to a value representative of the user
	/// session associated with the scope.  When the scope is disposed, the thread principal
	/// is returned to it's previous value.
	/// </remarks>
	public class AuthenticationScope : IDisposable
	{
		#region CustomPrincipal class

		class CustomPrincipal : GenericPrincipal, IUserCredentialsProvider
		{
			private readonly SessionToken _token;

			public CustomPrincipal(IIdentity identity, SessionToken token, string[] roles)
				: base(identity, roles)
			{
				_token = token;
			}

			public SessionToken SessionToken
			{
				get { return _token; }
			}

			string IUserCredentialsProvider.UserName
			{
				get { return Identity.Name; }
			}

			string IUserCredentialsProvider.SessionTokenId
			{
				get { return _token.Id; }
			}
		}

		#endregion

		[ThreadStatic]
		private static AuthenticationScope _current;

		private readonly AuthenticationScope _parent;
		private readonly string _userName;
		private readonly string _application;
		private readonly string _hostName;
		private readonly CustomPrincipal _principal;
		private readonly IPrincipal _previousPrincipal;
		private bool _disposed;


		#region Constructors

		/// <summary>
		/// Creates a new user session based on specified credentials, and constructs an instance of this
		/// class for that user session.
		/// </summary>
		/// <remarks>
		/// The user session is terminated when this instance is disposed.
		/// </remarks>
		/// <param name="userName"></param>
		/// <param name="application"></param>
		/// <param name="hostName"></param>
		/// <param name="password"></param>
		public AuthenticationScope(string userName, string application, string hostName, string password)
		{
			_userName = userName;
			_application = application;
			_hostName = hostName;

			_principal = InitiateSession(password);

			// if the session was successfully initiated (no exception thrown), then 
			// set the thread principal and establish this as the current scope
			_previousPrincipal = Thread.CurrentPrincipal;
			Thread.CurrentPrincipal = _principal;
			_parent = _current;
			_current = this;
		}

		#endregion

		#region Public API

		public static AuthenticationScope Current
		{
			get { return _current; }
		}

		public SessionToken SessionToken
		{
			get { return _principal.SessionToken; }
		}

		public IPrincipal Principal
		{
			get { return _principal; }
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;

				if (this != _current)
					throw new InvalidOperationException("Disposed out of order.");

				try
				{
					// attempt to terminate the session
					TerminateSession();
				}
				finally
				{
					// even if it fails, we are still disposing of this scope, so we set the head
					// to point to the parent and revert the Thread.CurrentPrincipal
					_current = _parent;
					Thread.CurrentPrincipal = _previousPrincipal;
				}
			}
		}

		#endregion

		#region Helpers

		private CustomPrincipal InitiateSession(string password)
		{
			CustomPrincipal principal = null;
			Platform.GetService<IAuthenticationService>(
				delegate(IAuthenticationService service)
				{
					// obtain session
					InitiateSessionResponse response = service.InitiateSession(
						new InitiateSessionRequest(_userName, _application, _hostName, password, true));

					// create principal
					principal = new CustomPrincipal(
						new GenericIdentity(_userName),
						response.SessionToken,
						response.AuthorityTokens);

				});
			return principal;
		}

		private void TerminateSession()
		{
			Platform.GetService<IAuthenticationService>(
				delegate(IAuthenticationService service)
				{
					// terminate session
					service.TerminateSession(
						new TerminateSessionRequest(_userName, _principal.SessionToken));
				});
		}

		#endregion
	}
}