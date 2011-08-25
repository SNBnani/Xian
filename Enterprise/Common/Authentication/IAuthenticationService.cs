#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.ServiceModel;

namespace ClearCanvas.Enterprise.Common.Authentication
{
	/// <summary>
	/// Provides methods for validating user credentials and obtaining the set of authority tokens granted to a user.
	/// </summary>
	[EnterpriseCoreService]
	[ServiceContract]
	[Authentication(false)]
	public interface IAuthenticationService
	{
		/// <summary>
		/// Initiates a new session for the specified user, first verifying the password,
		/// and returns a new session token if successful.
		/// </summary>
		/// <exception cref="UserAccessDeniedException">Invalid username or password.</exception>
		/// <exception cref="PasswordExpiredException">Password is valid but has expired.</exception>
		[OperationContract]
        [FaultContract(typeof(UserAccessDeniedException))]
        [FaultContract(typeof(PasswordExpiredException))]
		InitiateSessionResponse InitiateSession(InitiateSessionRequest request);

		/// <summary>
		/// Validates an existing user session, returning an updated session token.
		/// </summary>
		/// <exception cref="InvalidUserSessionException">Session token expired or otherwise invalid.</exception>
		[OperationContract]
        [FaultContract(typeof(InvalidUserSessionException))]
        ValidateSessionResponse ValidateSession(ValidateSessionRequest request);

		/// <summary>
		/// Terminates an existing user session.
		/// </summary>
		/// <exception cref="InvalidUserSessionException">Session token expired or otherwise invalid.</exception>
		[OperationContract]
        [FaultContract(typeof(InvalidUserSessionException))]
        TerminateSessionResponse TerminateSession(TerminateSessionRequest request);

		/// <summary>
		/// Changes the password for the specified user account.
		/// </summary>
        /// <exception cref="UserAccessDeniedException">Invalid user credentials.</exception>
		/// <exception cref="RequestValidationException">The new password does not meet password policy restrictions.</exception>
		[OperationContract]
		[FaultContract(typeof(RequestValidationException))]
        [FaultContract(typeof(UserAccessDeniedException))]
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);

		/// <summary>
		/// Obtains the set of authority tokens that have been granted to the 
		/// specified user.  Note: we may want to remove this method and use ValidateSession instead,
		/// since that method also returns the current list of authorizations
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		GetAuthorizationsResponse GetAuthorizations(GetAuthorizationsRequest request);

        /// <summary>
        /// Resets the password for the specified user account and sends an email to the user
        /// </summary>
        /// <exception cref="UserAccessDeniedException">Invalid user credentials or the user doesn't have an email address configured.</exception>
        [OperationContract]
        [FaultContract(typeof(UserAccessDeniedException))]
        ResetPasswordResponse ResetPassword(ResetPasswordRequest request);
	}
}