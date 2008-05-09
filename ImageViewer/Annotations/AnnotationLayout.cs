#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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
using System.Text;

namespace ClearCanvas.ImageViewer.Annotations
{
	internal class SharedAnnotationLayoutProxy : AnnotationLayout
	{
		private readonly IAnnotationLayout _realLayout;

		public SharedAnnotationLayoutProxy(IAnnotationLayout realLayout)
		{
			_realLayout = realLayout;
		}

		public override IEnumerable<AnnotationBox> AnnotationBoxes
		{
			get { return _realLayout.AnnotationBoxes; }
		}
	}

	internal class EmptyAnnotationLayout : AnnotationLayout
	{
		public EmptyAnnotationLayout()
		{
		}

		public override IEnumerable<AnnotationBox> AnnotationBoxes
		{
			get { yield break; }
		}
	}

	/// <summary>
	/// Abstract base class for <see cref="IAnnotationLayout"/>.
	/// </summary>
	public abstract class AnnotationLayout : IAnnotationLayout
	{
		private static readonly IAnnotationLayout _empty = new EmptyAnnotationLayout();
		private bool _visible;

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AnnotationLayout()
		{
			_visible = true;
		}

		/// <summary>
		/// Gets an empty <see cref="IAnnotationLayout"/>.
		/// </summary>
		public static IAnnotationLayout Empty
		{
			get { return _empty; }	
		}

		#region IAnnotationLayout Members

		/// <summary>
		/// Gets the entire set of <see cref="AnnotationBox"/>es.
		/// </summary>
		public abstract IEnumerable<AnnotationBox> AnnotationBoxes { get; }

		/// <summary>
		/// Gets or sets whether the <see cref="AnnotationLayout"/> is visible.
		/// </summary>
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		#endregion
	}
}
