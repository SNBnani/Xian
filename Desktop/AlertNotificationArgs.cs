﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;


namespace ClearCanvas.Desktop
{
	/// <summary>
	/// Holds parameters that control the creation of an alert notification.
	/// </summary>
	public class AlertNotificationArgs
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="level"></param>
		/// <param name="message"></param>
		public AlertNotificationArgs(AlertLevel level, string message)
		{
			this.Level = level;
			this.Message = message;
		}

		/// <summary>
		/// Gets or sets the alert level.
		/// </summary>
		public AlertLevel Level { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the link text, if the alert has a contextual link.
		/// </summary>
		public string LinkText { get; set; }

		/// <summary>
		/// Gets or sets the link action, if the alert has a contextual link.
		/// </summary>
		public Action<DesktopWindow> LinkAction { get; set; }

		/// <summary>
		/// Gets or sets a value that determines whether the alert notification is dismissed upon clicking the link.
		/// </summary>
		public bool DismissOnLinkClicked { get; set; }
	}
}
