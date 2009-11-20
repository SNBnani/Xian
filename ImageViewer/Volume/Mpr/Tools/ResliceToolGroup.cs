#region License

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
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.Annotations;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Volume.Mpr.Utilities;

namespace ClearCanvas.ImageViewer.Volume.Mpr.Tools
{
	[DropDownButtonAction("dropdown", "global-toolbars/ToolbarMpr/ToolbarReslice", "ActivateLastSelected", "DropDownModel")]
	[IconSet("dropdown", IconScheme.Colour, "Icons.ResliceToolSmall.png", "Icons.ResliceToolMedium.png", "Icons.ResliceToolLarge.png")]
	[CheckedStateObserver("dropdown", "Active", "ActivationChanged")]
	[LabelValueObserver("dropdown", "Label", "SelectedToolChanged")]
	[TooltipValueObserver("dropdown", "Tooltip", "TooltipChanged")]
	[GroupHint("dropdown", "Tools.Volume.MPR.Reslicing")]
	[MouseToolButton(XMouseButtons.Left, false)]
	[ExtensionOf(typeof (MprViewerToolExtensionPoint))]
	public partial class ResliceToolGroup : MouseImageViewerToolGroup<MprViewerTool>
	{
		private static readonly Color[,] _colors = {
		                                           	{Color.Red, Color.Salmon},
		                                           	{Color.DodgerBlue, Color.DeepSkyBlue},
		                                           	{Color.GreenYellow, Color.Lime},
		                                           	{Color.Yellow, Color.Gold},
		                                           	{Color.Magenta, Color.Violet},
		                                           	{Color.Aqua, Color.Turquoise},
		                                           	{Color.White, Color.LightGray}
		                                           };

		private ResliceTool _lastSelectedTool;

		protected override IEnumerable<MprViewerTool> CreateTools()
		{
			int index = 0;

			if (this.ImageViewer == null)
				yield break;

			// create one instance of the slave tool for each mutable slice set
			foreach (IMprVolume volume in this.ImageViewer.Volumes)
			{
				foreach (IMprSliceSet sliceSet in volume.SliceSets)
				{
					IMprStandardSliceSet standardSliceSet = sliceSet as IMprStandardSliceSet;
					if (standardSliceSet != null && !standardSliceSet.IsReadOnly)
					{
						ResliceTool tool = new ResliceTool(this);
						tool.SliceSet = standardSliceSet;
						tool.HotColor = _colors[index, 0];
						tool.NormalColor = _colors[index, 1];
						index = (index + 1)%_colors.Length; // advance to next color
						yield return tool;
					}
				}
			}
		}

		public new MprViewerComponent ImageViewer
		{
			get { return base.ImageViewer as MprViewerComponent; }
		}

		public override void Initialize()
		{
			base.Initialize();
			base.TooltipPrefix = SR.MenuReslice;

			_mprWorkspaceState = new MprWorkspaceState(this.ImageViewer);
			_resliceToolsState = new ResliceToolGraphicsState(this);

			this.InitializeResetAll();
			if (this.ImageViewer != null)
			{
				this.ImageViewer.PhysicalWorkspace.LayoutCompleted += PhysicalWorkspace_LayoutCompleted;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.ImageViewer != null)
				{
					this.ImageViewer.PhysicalWorkspace.LayoutCompleted -= PhysicalWorkspace_LayoutCompleted;
				}

				if (_mprWorkspaceState != null)
				{
					_mprWorkspaceState.Dispose();
					_mprWorkspaceState = null;
				}

				if (_resliceToolsState != null)
				{
					_resliceToolsState.Dispose();
					_resliceToolsState = null;
				}
			}
			this.DisposeResetAll();
			base.Dispose(disposing);
		}

		private void PhysicalWorkspace_LayoutCompleted(object sender, EventArgs e)
		{
			this.ReinitializeTools();
		}

		protected override void OnToolSelected(MprViewerTool tool)
		{
			base.OnToolSelected(tool);
			_lastSelectedTool = (ResliceTool) tool;

			if (_lastSelectedTool == null)
				base.TooltipPrefix = SR.ToolbarReslice;
			else
				base.TooltipPrefix = _lastSelectedTool.Label;
		}

		public void ActivateLastSelected()
		{
			IList<MprViewerTool> tools = base.SlaveTools;
			if (tools == null || tools.Count == 0)
				return;
			if (_lastSelectedTool == null)
				tools[0].Select();
			else
				_lastSelectedTool.Select();
		}

		public string Label
		{
			get
			{
				if (_lastSelectedTool == null)
					return SR.ToolbarReslice;
				return _lastSelectedTool.Label;
			}
		}

		public ActionModelNode DropDownModel
		{
			get { return ActionModelRoot.CreateModel(this.GetType().FullName, "mprviewer-reslicemenu", this.Actions); }
		}

		#region MPR Workspace State

		private MprWorkspaceState _mprWorkspaceState;
		private ResliceToolGraphicsState _resliceToolsState;

		private class ResliceToolGraphicsState : IMemorable, IDisposable
		{
			private ResliceToolGroup _owner;
			private object _initialState;

			public ResliceToolGraphicsState(ResliceToolGroup owner)
			{
				_owner = owner;
				_initialState = this.CreateMemento();
			}

			public void Dispose()
			{
				_owner = null;
				_initialState = null;
			}

			public object InitialState
			{
				get { return _initialState; }
			}

			public object CreateMemento()
			{
				ResliceToolGraphicsStateMemento state = new ResliceToolGraphicsStateMemento();
				foreach (ResliceTool tool in _owner.SlaveTools)
				{
					if (tool.SliceSet != null)
						state.Add(tool, tool.CreateMemento());
				}
				return state;
			}

			public void SetMemento(object memento)
			{
				ResliceToolGraphicsStateMemento state = memento as ResliceToolGraphicsStateMemento;
				if (state == null)
					return;

				foreach (ResliceTool tool in _owner.SlaveTools)
				{
					if (tool.SliceSet != null && state.ContainsKey(tool))
						tool.SetMemento(state[tool]);
				}
			}

			private class ResliceToolGraphicsStateMemento : Dictionary<ResliceTool, object> {}
		}

		private class MprWorkspaceState : IMemorable, IDisposable
		{
			private MprViewerComponent _mprViewer;
			private object _initialState;

			public MprWorkspaceState(MprViewerComponent mprViewer)
			{
				_mprViewer = mprViewer;
				_initialState = this.CreateMemento();
			}

			public void Dispose()
			{
				_mprViewer = null;
				_initialState = null;
			}

			public object InitialState
			{
				get { return _initialState; }
			}

			public object CreateMemento()
			{
				MprWorkspaceStateMemento state = new MprWorkspaceStateMemento();
				foreach (IImageSet imageSet in _mprViewer.MprWorkspace.ImageSets)
				{
					foreach (MprDisplaySet displaySet in imageSet.DisplaySets)
						state.Add(displaySet, displaySet.CreateMemento());
				}
				return state;
			}

			public void SetMemento(object memento)
			{
				MprWorkspaceStateMemento state = memento as MprWorkspaceStateMemento;
				if (state == null)
					return;

				foreach (KeyValuePair<MprDisplaySet, object> pair in state)
					pair.Key.SetMemento(pair.Value);
			}

			private class MprWorkspaceStateMemento : Dictionary<MprDisplaySet, object> {}
		}

		private class ImageHint
		{
			private readonly int _imageIndex = -1;
			private readonly MprDisplaySet _displaySet = null;

			public ImageHint(IPresentationImage image)
			{
				if (image != null)
				{
					_displaySet = image.ParentDisplaySet as MprDisplaySet;
					if (_displaySet != null)
						_imageIndex = _displaySet.PresentationImages.IndexOf(image);
				}
			}

			public IPresentationImage Image
			{
				get
				{
					if (_displaySet != null && _imageIndex >= 0 && _imageIndex < _displaySet.PresentationImages.Count)
						return _displaySet.PresentationImages[_imageIndex];
					return null;
				}
			}
		}

		#endregion

		#region Static Helpers - Finding ImageBoxes

		/// <summary>
		/// Finds the ImageBox displaying the specified slice set
		/// </summary>
		protected static IImageBox FindImageBox(IMprSliceSet sliceSet, MprViewerComponent viewer)
		{
			if (sliceSet == null || viewer == null)
				return null;

			foreach (IImageBox imageBox in viewer.PhysicalWorkspace.ImageBoxes)
			{
				if (imageBox.DisplaySet != null && imageBox.DisplaySet.Uid == sliceSet.Uid)
					return imageBox;
			}
			return null;
		}

		#endregion

		#region Static Helpers - Colourising Annotation Items

		/// <summary>
		/// Colourises the display set description annotation item in the specified image
		/// </summary>
		private static void ColorizeDisplaySetDescription(IPresentationImage image, Color color)
		{
			if (image is IAnnotationLayoutProvider)
			{
				IAnnotationLayoutProvider provider = (IAnnotationLayoutProvider) image;
				foreach (AnnotationBox annotationBox in provider.AnnotationLayout.AnnotationBoxes)
				{
					if (annotationBox.AnnotationItem != null && annotationBox.AnnotationItem.GetIdentifier() == "Presentation.DisplaySetDescription")
					{
						annotationBox.Color = color.Name;
					}
				}
			}
		}

		#endregion

		#region Static Helpers - Manipulating Graphics

		/// <summary>
		/// Moves the graphic from where ever it is to the target image.
		/// </summary>
		private static void TranslocateGraphic(IGraphic graphic, IPresentationImage targetImage)
		{
			IPresentationImage oldImage = graphic.ParentPresentationImage;
			if (oldImage != targetImage)
			{
				IApplicationGraphicsProvider imageOnUnexecute = oldImage as IApplicationGraphicsProvider;
				if (imageOnUnexecute != null)
					imageOnUnexecute.ApplicationGraphics.Remove(graphic);

				IApplicationGraphicsProvider imageOnExecute = targetImage as IApplicationGraphicsProvider;
				if (imageOnExecute != null)
					imageOnExecute.ApplicationGraphics.Add(graphic);

				if (oldImage != null)
					oldImage.Draw();
			}
		}

		#endregion

		#region Static Helpers - Reslicing Math

		/// <summary>
		/// Sets the slicing plane for the specified slice set based on two points on the specified source image.
		/// </summary>
		private static void SetSlicePlane(IMprStandardSliceSet sliceSet, IPresentationImage sourceImage, Vector3D startPoint, Vector3D endPoint)
		{
			IImageSopProvider imageSopProvider = sourceImage as IImageSopProvider;
			if (imageSopProvider == null)
				return;

			ImageOrientationPatient orientation = imageSopProvider.Frame.ImageOrientationPatient;
			Vector3D orientationRow = new Vector3D((float) orientation.RowX, (float) orientation.RowY, (float) orientation.RowZ);
			Vector3D orientationColumn = new Vector3D((float) orientation.ColumnX, (float) orientation.ColumnY, (float) orientation.ColumnZ);

			if (sliceSet != null && !sliceSet.IsReadOnly)
			{
				IImageBox imageBox = FindImageBox(sliceSet, sourceImage.ImageViewer as MprViewerComponent);
				sliceSet.SlicerParams = VolumeSlicerParams.Create(sliceSet.Volume, orientationColumn, orientationRow, startPoint, endPoint);

				IPresentationImage closestImage = GetClosestSlice(startPoint + (endPoint - startPoint) * 2, imageBox.DisplaySet);
				if (closestImage == null)
					imageBox.TopLeftPresentationImageIndex = imageBox.DisplaySet.PresentationImages.Count/2;
				else
					imageBox.TopLeftPresentationImage = closestImage;
			}
		}

		/// <summary>
		/// Computes the closest image in a display set to the specified position in patient coordinates.
		/// </summary>
		private static IPresentationImage GetClosestSlice(Vector3D positionPatient, IDisplaySet displaySet)
		{
			float closestDistance = float.MaxValue;
			IPresentationImage closestImage = null;

			foreach (IPresentationImage image in displaySet.PresentationImages)
			{
				if (image is IImageSopProvider)
				{
					Frame frame = (image as IImageSopProvider).Frame;
					Vector3D positionCenterOfImage = frame.ImagePlaneHelper.ConvertToPatient(new PointF((frame.Columns - 1)/2F, (frame.Rows - 1)/2F));
					Vector3D distanceVector = positionCenterOfImage - positionPatient;
					float distance = distanceVector.Magnitude;

					if (distance <= closestDistance)
					{
						closestDistance = distance;
						closestImage = image;
					}
				}
			}

			return closestImage;
		}

		#endregion
	}
}