#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using System.Drawing;
using ClearCanvas.Dicom.Iod.Sequences;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Mathematics;

namespace ClearCanvas.ImageViewer.PresentationStates.Dicom.GraphicAnnotationSerializers
{
	internal class PolylineGraphicAnnotationSerializer : GraphicAnnotationSerializer<IPointsGraphic>
	{
		protected override void Serialize(IPointsGraphic graphic, GraphicAnnotationSequenceItem serializationState)
		{
			if (!graphic.Visible)
				return; // if the graphic is not visible, don't serialize it!

			GraphicAnnotationSequenceItem.GraphicObjectSequenceItem annotationElement = new GraphicAnnotationSequenceItem.GraphicObjectSequenceItem();

			graphic.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				IList<PointF> polyline = graphic.Points;

				annotationElement.GraphicAnnotationUnits = GraphicAnnotationSequenceItem.GraphicAnnotationUnits.Pixel;
				annotationElement.GraphicDimensions = 2;
				annotationElement.GraphicType = GraphicAnnotationSequenceItem.GraphicType.Polyline;
				annotationElement.NumberOfGraphicPoints = polyline.Count;

				// add shape vertices
				List<PointF> list = new List<PointF>(polyline.Count);
				for (int n = 0; n < polyline.Count; n++)
				{
					list.Add(polyline[n]);
				}
				annotationElement.GraphicData = list.ToArray();

				if (FloatComparer.AreEqual(list[0], list[list.Count - 1]))
				{
					// shape is closed - we are required to indicate fill state
					annotationElement.GraphicFilled = GraphicAnnotationSequenceItem.GraphicFilled.N;
				}
			}
			finally
			{
				graphic.ResetCoordinateSystem();
			}

			serializationState.AppendGraphicObjectSequence(annotationElement);
		}
	}
}