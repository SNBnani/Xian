#region License

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
    /// Defines a base interface for views that serve desktop objects.
    /// </summary>
    /// <remarks>
	/// The view provides the on-screen representation of the object.
	/// </remarks>
    public interface IDesktopObjectView : IView, IDisposable
    {
        /// <summary>
        /// Occurs when the <see cref="Visible"/> property changes.
        /// </summary>
        event EventHandler VisibleChanged;

        /// <summary>
        /// Occurs when the <see cref="Active"/> property changes.
        /// </summary>
        event EventHandler ActiveChanged;

        /// <summary>
        /// Occurs when the user has requested that the object be closed.
        /// </summary>
        event EventHandler CloseRequested;

        /// <summary>
        /// Sets the title that is displayed to the user.
        /// </summary>
        void SetTitle(string title);

        /// <summary>
        /// Opens the view (makes it first visible on the screen).
        /// </summary>
        void Open();

        /// <summary>
        /// Shows the view.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the view.
        /// </summary>
        void Hide();

        /// <summary>
        /// Activates the view.
        /// </summary>
        void Activate();

        /// <summary>
        /// Gets a value indicating whether the view is visible on the screen.
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Gets a value indicating whether the view is active.
        /// </summary>
        bool Active { get; }
    }
}
