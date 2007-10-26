#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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


namespace ClearCanvas.ImageViewer.Imaging
{
	/// <summary>
	/// A provider of an <see cref="IColorMap"/>, accessed and manipulated via the <see cref="ColorMapManager"/> property.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="ColorMapManager"/> property allows access to and manipulation of the installed <see cref="IColorMap"/>.
	/// </para>
	/// <para>
	/// Implementors should not return null for the <see cref="ColorMapManager"/> property.
	/// </para>
	/// </remarks>
	/// <seealso cref="IColorMap"/>
	/// <seealso cref="IColorMapManager"/>
	public interface IColorMapProvider : IDrawable
	{
		/// <summary>
		/// Gets the <see cref="IColorMapManager"/> associated with the provider.
		/// </summary>
		/// <remarks>
		/// This property should never return null.
		/// </remarks>
		IColorMapManager ColorMapManager { get; }
	}
}
