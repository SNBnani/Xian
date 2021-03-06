#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections;
using System.Text;


namespace ClearCanvas.Healthcare {


    /// <summary>
    /// OrderCancelInfo component
    /// </summary>
	public partial class OrderCancelInfo
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="reason"></param>
		/// <param name="comment"></param>
		public OrderCancelInfo(OrderCancelReasonEnum reason, string comment)
		{
			CustomInitialize();

			_reason = reason;
			_comment = comment;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="reason"></param>
		/// <param name="cancelledBy"></param>
		public OrderCancelInfo(OrderCancelReasonEnum reason, Staff cancelledBy)
		{
			CustomInitialize();

			_reason = reason;
			_cancelledBy = cancelledBy;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="reason"></param>
		/// <param name="cancelledBy"></param>
		/// <param name="comment"></param>
		public OrderCancelInfo(OrderCancelReasonEnum reason, Staff cancelledBy, string comment)
		{
			CustomInitialize();

			_reason = reason;
			_cancelledBy = cancelledBy;
			_comment = comment;
		}
	
		/// <summary>
		/// This method is called from the constructor.  Use this method to implement any custom
		/// object initialization.
		/// </summary>
		private void CustomInitialize()
		{
		}
	}
}