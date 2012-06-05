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
using System.Linq;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.Configuration;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Explorer.Dicom
{
	[ButtonAction("activate", "dicomstudybrowser-toolbar/ToolbarOpenStudy", "OpenStudy")]
	[MenuAction("activate", "dicomstudybrowser-contextmenu/MenuOpenStudy", "OpenStudy")]
	[EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
	[VisibleStateObserver("activate", "Visible", "VisibleChanged")]
	[Tooltip("activate", "TooltipOpenStudy")]
	[IconSet("activate", "Icons.OpenToolSmall.png", "Icons.OpenToolSmall.png", "Icons.OpenToolSmall.png")]

	[ViewerActionPermission("activate", ImageViewer.AuthorityTokens.Study.Open)]

	[ExtensionOf(typeof(StudyBrowserToolExtensionPoint))]
	public class OpenStudyTool : StudyBrowserTool
	{
		public override void Initialize()
		{
			base.Initialize();

			SetDoubleClickHandler();
		}

		public void OpenStudy()
		{
			try
			{
			    int numberOfSelectedStudies = Context.SelectedStudies.Count;
                if (Context.SelectedStudies.Count == 0)
					return;

				if (!PermissionsHelper.IsInRole(ImageViewer.AuthorityTokens.Study.Open))
				{
					Context.DesktopWindow.ShowMessageBox(SR.MessageOpenStudyPermissionDenied, MessageBoxActions.Ok);
					return;
				}

				int numberOfLoadableStudies = GetNumberOfLoadableStudies();
				if (numberOfLoadableStudies != numberOfSelectedStudies)
				{
					int numberOfNonLoadableStudies = numberOfSelectedStudies - numberOfLoadableStudies;
					string message;
					if (numberOfSelectedStudies == 1)
					{
						message = SR.MessageCannotOpenNonStreamingStudy;
					}
					else
					{
						if (numberOfNonLoadableStudies == 1)
							message = SR.MessageOneNonStreamingStudyCannotBeOpened;
						else 
							message = String.Format(SR.MessageFormatXNonStreamingStudiesCannotBeOpened, numberOfNonLoadableStudies);
					}

					Context.DesktopWindow.ShowMessageBox(message, MessageBoxActions.Ok);
					return;
				}

				var helper = new OpenStudyHelper
				                 {
				                     WindowBehaviour = ViewerLaunchSettings.WindowBehaviour,
				                     AllowEmptyViewer = ViewerLaunchSettings.AllowEmptyViewer
				                 };

				foreach (var study in Context.SelectedStudies)
					helper.AddStudy(study.StudyInstanceUid, study.Server);

				helper.Title = ImageViewerComponent.CreateTitle(GetSelectedPatients());
				helper.OpenStudies();
			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, Context.DesktopWindow);
			}
		}

		private void SetDoubleClickHandler()
		{
			if (GetAtLeastOneServerSupportsLoading() || base.Context.SelectedServers.Count == 0)
				Context.DefaultActionHandler = OpenStudy;
		}
		protected override void OnSelectedStudyChanged(object sender, EventArgs e)
		{
			UpdateEnabled();
		}

		protected override void OnSelectedServerChanged(object sender, EventArgs e)
		{
			UpdateEnabled();
		}

		private void UpdateEnabled()
		{
            // TODO (Marmot): if user selects a study on a non-streaming server and then selects the parent, 
            // the button will become visible and enabled (only to show an error message if user tries to open the study), 
            // should the button be visible but disabled?
            Visible = GetAtLeastOneServerSupportsLoading(); 
		    Enabled = Context.SelectedStudies.Count > 0 && GetAtLeastOneServerSupportsLoading();
		    SetDoubleClickHandler();
		}

	    private bool GetAtLeastOneServerSupportsLoading()
		{
		    return Context.SelectedServers.AnySupport<IStudyLoader>();
		}

		private int GetNumberOfLoadableStudies()
		{
		    return base.Context.SelectedStudies.Count(s => s.Server.IsSupported<IStudyLoader>());
		}

		private IEnumerable<IPatientData> GetSelectedPatients()
		{
		    return Context.SelectedStudies.Cast<IPatientData>();
		}
	}
}
