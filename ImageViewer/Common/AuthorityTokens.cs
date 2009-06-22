﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using ClearCanvas.Common;
using ClearCanvas.Common.Authorization;

namespace ClearCanvas.ImageViewer.Common
{
	/// <summary>
	/// Default authority group definition class.
	/// </summary>
	[ExtensionOf(typeof(DefineAuthorityGroupsExtensionPoint))]
	public class DefaultAuthorityGroups : IDefineAuthorityGroups
	{
		/// <summary>
		/// Administrators authority group.
		/// </summary>
		public const string Administrators = "Administrators";

		/// <summary>
		/// Healthcare Administrators authority group.
		/// </summary>
		public const string HealthcareAdministrators = "Healthcare Administrators";

		/// <summary>
		/// Clerical authority group.
		/// </summary>
		public const string Clerical = "Clerical";

		/// <summary>
		/// Technologists authority group.
		/// </summary>
		public const string Technologists = "Technologists";

		/// <summary>
		/// Radiologists authority group.
		/// </summary>
		public const string Radiologists = "Radiologists";

		/// <summary>
		/// Radiology Residents authority group.
		/// </summary>
		public const string RadiologyResidents = "Radiology Residents";

		/// <summary>
		/// Emergency Physicians authority group.
		/// </summary>
		public const string EmergencyPhysicians = "Emergency Physicians";

		#region IDefineAuthorityGroups Members

		/// <summary>
		/// Get the authority group definitions.
		/// </summary>
		public AuthorityGroupDefinition[] GetAuthorityGroups()
		{
			return new AuthorityGroupDefinition[]
            {
				new AuthorityGroupDefinition(Administrators,
                    new string[] 
                    {
						AuthorityTokens.Workflow.Study.Search,
						AuthorityTokens.Workflow.Study.View,
						AuthorityTokens.Workflow.Study.Export,
						AuthorityTokens.Workflow.Study.Create,
						AuthorityTokens.Workflow.Study.Modify,
						AuthorityTokens.Workflow.Study.Delete
                    }),

				new AuthorityGroupDefinition(Clerical,
				    new string[] 
				    {
						AuthorityTokens.Workflow.Study.Search
				   }),

                new AuthorityGroupDefinition(Technologists,
                    new string[] 
                    {
						AuthorityTokens.Workflow.Study.Search,
						AuthorityTokens.Workflow.Study.View,
						AuthorityTokens.Workflow.Study.Export,
						AuthorityTokens.Workflow.Study.Create,
						AuthorityTokens.Workflow.Study.Modify,
						AuthorityTokens.Workflow.Study.Delete
                    }),

                new AuthorityGroupDefinition(Radiologists,
                    new string[] 
                    {
						AuthorityTokens.Workflow.Study.Search,
						AuthorityTokens.Workflow.Study.View,
						AuthorityTokens.Workflow.Study.Export,
						AuthorityTokens.Workflow.Study.Create,
						AuthorityTokens.Workflow.Study.Modify,
						AuthorityTokens.Workflow.Study.Delete
                   }),

                new AuthorityGroupDefinition(RadiologyResidents,
                    new string[] 
                    {
						AuthorityTokens.Workflow.Study.Search,
						AuthorityTokens.Workflow.Study.View,
						AuthorityTokens.Workflow.Study.Export,
						AuthorityTokens.Workflow.Study.Create,
						AuthorityTokens.Workflow.Study.Modify,
						AuthorityTokens.Workflow.Study.Delete
                   }),

                new AuthorityGroupDefinition(EmergencyPhysicians,
                    new string[] 
                    {
						AuthorityTokens.Workflow.Study.Search,
						AuthorityTokens.Workflow.Study.View,
						AuthorityTokens.Workflow.Study.Export
                    }),
            };
		}

		#endregion
	}

	/// <summary>
	/// Static class defining all Common Image Viewer authority tokens.
	/// </summary>
	public static class AuthorityTokens
	{
		/// <summary>
		/// General user permission required to use any viewer components.
		/// </summary>
		[AuthorityToken(Description = "General user permission required to use any viewer components.")]
		public const string General = "Viewer/General";

		/// <summary>
		/// Viewer workflow authority tokens.
		/// </summary>
		public class Workflow
		{
			/// <summary>
			/// Generic study related authority tokens.
			/// </summary>
			public class Study
			{
				/// <summary>
				/// Generic user permission to search the study data in the viewer.
				/// </summary>
				[AuthorityToken(Description = "Generic user permission to search the study data in the viewer.")]
				public const string Search = "Viewer/Workflow/Study/Search";

				/// <summary>
				/// Generic user permission to export study data from the viewer.
				/// </summary>
				[AuthorityToken(Description = "Generic user permission to export study data from the viewer.")]
				public const string Export = "Viewer/Workflow/Study/Export";

				/// <summary>
				/// Generic user permission to view study data in the viewer.
				/// </summary>
				[AuthorityToken(Description = "Generic user permission to view study data in the viewer.")]
				public const string View = "Viewer/Workflow/Study/View";

				/// <summary>
				/// Generic user permission to create a new study in the viewer.
				/// </summary>
				[AuthorityToken(Description = "Generic user permission to create a new study in the viewer.")]
				public const string Create = "Viewer/Workflow/Study/Create";

				/// <summary>
				/// Generic user permission to modify study data in the viewer.
				/// </summary>
				[AuthorityToken(Description = "Generic user permission to modify study data in the viewer.")]
				public const string Modify = "Viewer/Workflow/Study/Modify";

				/// <summary>
				/// Generic user permission to delete a study from the viewer.
				/// </summary>
				[AuthorityToken(Description = "Generic user permission to delete a study from the viewer.")]
				public const string Delete = "Viewer/Workflow/Study/Delete";
			}
		}
	}
}
