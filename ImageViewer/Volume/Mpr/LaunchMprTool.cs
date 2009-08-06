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

using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.Configuration;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Volume.Mpr
{
	[ButtonAction("open", "global-toolbars/ToolbarOpenSelectionWithMpr", "LaunchMpr")]
	[MenuAction("open", "imageviewer-contextmenu/MenuOpenWithMpr", "LaunchMpr")]
	[MenuAction("open", "global-menus/MenuTools/MenuVolume/MenuOpenSelectionWithMpr", "LaunchMpr")]
	[IconSet("open", IconScheme.Colour, "Icons.OpenMprToolLarge.png", "Icons.OpenMprToolMedium.png", "Icons.OpenMprToolSmall.png")]
	[EnabledStateObserver("open", "Enabled", "EnabledChanged")]
	[ExtensionOf(typeof (ImageViewerToolExtensionPoint))]
	public class LaunchMprTool : ImageViewerTool
	{
		private static readonly IActionSet _emptyActionSet = new ActionSet();

		public LaunchMprTool() {}

		public override IActionSet Actions
		{
			get
			{
				if (base.ImageViewer is MprViewerComponent)
					return _emptyActionSet;
				return base.Actions;
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			base.Context.Viewer.EventBroker.ImageBoxSelected += OnImageBoxSelected;
			base.Context.Viewer.EventBroker.DisplaySetSelected += OnDisplaySetSelected;
		}

		protected override void Dispose(bool disposing)
		{
			base.Context.Viewer.EventBroker.ImageBoxSelected -= OnImageBoxSelected;
			base.Context.Viewer.EventBroker.DisplaySetSelected -= OnDisplaySetSelected;

			base.Dispose(disposing);
		}

		public void LaunchMpr()
		{
			Exception exception = null;
			Volume volume = null;

			IPresentationImage currentImage = this.Context.Viewer.SelectedPresentationImage;
			if (currentImage == null)
				return;

			BackgroundTask task = new BackgroundTask(LoadVolume, true, FilterSourceFrames(currentImage.ParentDisplaySet, currentImage));
			task.Terminated += delegate(object sender, BackgroundTaskTerminatedEventArgs e)
			                   	{
			                   		exception = e.Exception;
			                   		if (e.Reason == BackgroundTaskTerminatedReason.Completed)
			                   			volume = (Volume) e.Result;
			                   	};

			try
			{
				ProgressDialog.Show(task, base.Context.DesktopWindow, true, ProgressBarStyle.Blocks);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				task.Dispose();
			}

			if (exception != null)
			{
				ExceptionHandler.Report(exception, "A failure occured while generating the MPR volume.", base.Context.DesktopWindow);
				return;
			}

			if (volume != null)
			{
				try
				{
					MprViewerComponent component = new MprViewerComponent(volume);
					LaunchImageViewerArgs args = new LaunchImageViewerArgs(ViewerLaunchSettings.WindowBehaviour);
					args.Title = component.Title;
					MprViewerComponent.Launch(component, args);
				}
				catch (Exception e)
				{
					ExceptionHandler.Report(e, this.Context.DesktopWindow);
				}
			}
		}

		private static IEnumerable<Frame> FilterSourceFrames(IDisplaySet displaySet, IPresentationImage currentImage)
		{
			if (currentImage is IImageSopProvider)
			{
				Frame currentFrame = ((IImageSopProvider) currentImage).Frame;
				string studyInstanceUid = currentFrame.StudyInstanceUID;
				string seriesInstanceUid = currentFrame.SeriesInstanceUID;
				string frameOfReferenceUid = currentFrame.FrameOfReferenceUid;
				ImageOrientationPatient imageOrientationPatient = currentFrame.ImageOrientationPatient;

				// perform a very basic filtering of the selected display set based on the currently selected image
				foreach (IPresentationImage image in displaySet.PresentationImages)
				{
					if (image == currentImage)
					{
						yield return currentFrame;
					}
					else if (image is IImageSopProvider)
					{
						Frame frame = ((IImageSopProvider) image).Frame;
						if (frame.StudyInstanceUID == studyInstanceUid
						    && frame.SeriesInstanceUID == seriesInstanceUid
						    && frame.FrameOfReferenceUid == frameOfReferenceUid
						    && !frame.ImageOrientationPatient.IsNull
						    && frame.ImageOrientationPatient.EqualsWithinTolerance(imageOrientationPatient, .01f))
							yield return frame;
					}
				}
			}
		}

		private static void LoadVolume(IBackgroundTaskContext context)
		{
			try
			{
				IEnumerable<Frame> frames = (IEnumerable<Frame>) context.UserState;
				Volume volume = Volume.CreateVolume(frames,
				                                    delegate(int i, int count)
				                                    	{
				                                    		if (context.CancelRequested)
				                                    			throw new BackgroundTaskCancelledException();
				                                    		context.ReportProgress(new BackgroundTaskProgress(i, count, SR.MessageBuildingMprVolume));
				                                    	});
				context.Complete(volume);
			}
			catch (BackgroundTaskCancelledException)
			{
				context.Cancel();
			}
			catch (Exception ex)
			{
				context.Error(ex);
			}
		}

		private sealed class BackgroundTaskCancelledException : Exception {}

		private void OnImageBoxSelected(object sender, ImageBoxSelectedEventArgs e)
		{
			if (e.SelectedImageBox.DisplaySet == null)
				this.UpdateEnabled(null);
		}

		private void OnDisplaySetSelected(object sender, DisplaySetSelectedEventArgs e)
		{
			this.UpdateEnabled(e.SelectedDisplaySet);
		}

		private void UpdateEnabled(IDisplaySet selectedDisplaySet)
		{
			base.Enabled = selectedDisplaySet != null && selectedDisplaySet.PresentationImages.Count > 1;
		}
	}
}