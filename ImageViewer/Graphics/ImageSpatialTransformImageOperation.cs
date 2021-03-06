#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer.Graphics
{
	/// <summary>
	/// A specialization of the <see cref="SpatialTransformImageOperation"/> where the
	/// originator is an <see cref="IImageSpatialTransform"/>.
	/// </summary>
	public class ImageSpatialTransformImageOperation : SpatialTransformImageOperation
	{
		/// <summary>
		/// Mandatory constructor.
		/// </summary>
		public ImageSpatialTransformImageOperation(ApplyDelegate applyDelegate)
			: base(applyDelegate)
		{
		}

		/// <summary>
		/// Returns the <see cref="IImageSpatialTransform"/> associated with the 
		/// <see cref="IPresentationImage"/>, or null.
		/// </summary>
		/// <remarks>
		/// When used in conjunction with an <see cref="ImageOperationApplicator"/>,
		/// it is always safe to cast the return value directly to <see cref="ImageSpatialTransform"/>
		/// without checking for null from within the <see cref="BasicImageOperation.ApplyDelegate"/> 
		/// specified in the constructor.
		/// </remarks>
		public override IMemorable GetOriginator(IPresentationImage image)
		{
			return base.GetOriginator(image) as ImageSpatialTransform;
		}
	}
}
