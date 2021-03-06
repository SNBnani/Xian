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
    /// Defines the public interface to a <see cref="DesktopObject"/>.
    /// </summary>
    public interface IDesktopObject
    {
        /// <summary>
        /// Gets the runtime name of the object, or null if the object is not named.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the title that is presented to the user on the screen.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the current state of the object.
        /// </summary>
        DesktopObjectState State { get; }

        /// <summary>
        /// Activates the object.
        /// </summary>
        void Activate();

        /// <summary>
        /// Tries to close the object, interacting with the user if necessary.
        /// </summary>
        /// <returns>True if the object is closed, otherwise false.</returns>
        bool Close();

        /// <summary>
        /// Tries to close the object, interacting with the user only if specified.
        /// </summary>
        /// <param name="interactive">A value specifying whether user interaction is allowed.</param>
        /// <returns>True if the object is closed, otherwise false.</returns>
        bool Close(UserInteraction interactive);

        /// <summary>
        /// Checks if the object is in a closable state (would be able to close without user interaction).
        /// </summary>
        /// <returns>True if the object can be closed without user interaction.</returns>
        bool QueryCloseReady();

        /// <summary>
        /// Gets a value indicating whether this object is currently active.
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// Gets a value indicating whether this object is currently visible.
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Occurs when the <see cref="Active"/> property changes.
        /// </summary>
        event EventHandler ActiveChanged;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property changes.
        /// </summary>
        event EventHandler VisibleChanged;

        /// <summary>
        /// Occurs when the <see cref="Title"/> property changes.
        /// </summary>
        event EventHandler TitleChanged;

        /// <summary>
        /// Occurs when the object is about to close.
        /// </summary>
        event EventHandler<ClosingEventArgs> Closing;

        /// <summary>
        /// Occurs when the object has closed.
        /// </summary>
        event EventHandler<ClosedEventArgs> Closed;
    }
}
