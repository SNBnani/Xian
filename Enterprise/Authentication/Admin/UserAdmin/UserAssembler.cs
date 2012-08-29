#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;

using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Common.Admin.AuthorityGroupAdmin;
using ClearCanvas.Enterprise.Common.Admin.UserAdmin;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Common;

namespace ClearCanvas.Enterprise.Authentication.Admin.UserAdmin
{
    internal class UserAssembler
    {
        internal UserSummary GetUserSummary(User user)
        {
            return new UserSummary(user.UserName, user.DisplayName, user.EmailAddress, user.CreationTime, user.ValidFrom, user.ValidUntil,
                                   user.LastLoginTime, user.Password.ExpiryTime, user.Enabled, user.Sessions.Count);
        }

        internal UserDetail GetUserDetail(User user)
        {
            AuthorityGroupAssembler assembler = new AuthorityGroupAssembler();

        	List<AuthorityGroupSummary> groups = CollectionUtils.Map(
        		user.AuthorityGroups,
        		(AuthorityGroup group) => assembler.CreateAuthorityGroupSummary(group));
				
            return new UserDetail(user.UserName, user.DisplayName, user.EmailAddress, user.CreationTime, user.ValidFrom, user.ValidUntil,
                user.LastLoginTime, user.Enabled, user.Password.ExpiryTime, groups);
        }

        internal void UpdateUser(User user, UserDetail detail, IPersistenceContext context)
        {
            // do not update user.UserName
            // do not update user.Password
            user.DisplayName = detail.DisplayName;
            user.ValidFrom = detail.ValidFrom;
            user.ValidUntil = detail.ValidUntil;
            user.Enabled = detail.Enabled;
            user.Password.ExpiryTime = detail.PasswordExpiryTime;
            user.EmailAddress = detail.EmailAddress;

            // process authority groups
			List<AuthorityGroup> authGroups = CollectionUtils.Map(
				detail.AuthorityGroups,
				(AuthorityGroupSummary group) => context.Load<AuthorityGroup>(group.AuthorityGroupRef, EntityLoadFlags.Proxy));

            user.AuthorityGroups.Clear();
			user.AuthorityGroups.AddAll(authGroups);
        }

        internal List<UserSessionSummary> GetUserSessionSummaries(User user)
        {
            Platform.CheckForNullReference(user, "user");

            if (user.Sessions == null || user.Sessions.Count == 0)
                return null; // data contract

            var list = new List<UserSessionSummary>();
            foreach (var session in user.Sessions)
            {
                list.Add(new UserSessionSummary()
                {
                    Application = session.Application,
                    CreationTime = session.CreationTime,
                    ExpiryTime = session.ExpiryTime,
                    HostName = session.HostName,
                    SessionID = session.SessionId             
                });
            }

            return list;
        }
    }
}
