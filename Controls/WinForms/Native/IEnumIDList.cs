#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;

namespace ClearCanvas.Controls.WinForms.Native
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid(IID.IID_IEnumIDList)]
	internal interface IEnumIDList
	{
		// Retrieves the specified number of item identifiers in the enumeration sequence and advances the current position by the number of items retrieved. 
		[PreserveSig()]
		uint Next(
			uint celt, // Number of elements in the array pointed to by the rgelt parameter.
			out IntPtr rgelt, // Address of an array of ITEMIDLIST pointers that receives the item identifiers. The implementation must allocate these item identifiers using the Shell's allocator (retrieved by the SHGetMalloc function). The calling application is responsible for freeing the item identifiers using the Shell's allocator.
			out Int32 pceltFetched // Address of a value that receives a count of the item identifiers actually returned in rgelt. The count can be smaller than the value specified in the celt parameter. This parameter can be NULL only if celt is one. 
			);

		// Skips over the specified number of elements in the enumeration sequence. 
		[PreserveSig()]
		uint Skip(
			uint celt // Number of item identifiers to skip.
			);

		// Returns to the beginning of the enumeration sequence. 
		[PreserveSig()]
		uint Reset();

		// Creates a new item enumeration object with the same contents and state as the current one. 
		[PreserveSig()]
		uint Clone(
			out IEnumIDList ppenum // Address of a pointer to the new enumeration object. The calling application must eventually free the new object by calling its Release member function. 
			);
	}
}

// ReSharper restore InconsistentNaming