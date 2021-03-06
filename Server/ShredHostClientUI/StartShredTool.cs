#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.Server.ShredHostClientUI
{
    [MenuAction("apply", "shredhostclient-contextmenu/Start")]
    [ButtonAction("activate", "shredhostclient-toolbar/Start")]
    [Tooltip("apply", "Start the shred if it is not already running")]
    [IconSet("apply", IconScheme.Colour, "Icons.StartShredToolSmall.png", "Icons.StartShredToolSmall.png", "Icons.StartShredToolSmall.png")]
    [ClickHandler("activate", "StartShred")]
    [EnabledStateObserver("activate", "Enabled", "EnabledChanged")]

    [ExtensionOf(typeof(ShredHostClientToolExtensionPoint))]
    public class StartShredTool : Tool<IShredHostClientToolContext>
    {
        private bool _enabled;
        private event EventHandler _enabledChanged;

        /// <summary>
        /// Default constructor.  A no-args constructor is required by the
        /// framework.  Do not remove.
        /// </summary>
        public StartShredTool()
        {
            _enabled = true;
        }

        /// <summary>
        /// Called by the framework to initialize this tool.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // TODO: add any significant initialization code here rather than in the constructor
        }

        /// <summary>
        /// Called to determine whether this tool is enabled/disabled in the UI.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            protected set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    EventsHelper.Fire(_enabledChanged, this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Notifies that the Enabled state of this tool has changed.
        /// </summary>
        public event EventHandler EnabledChanged
        {
            add { _enabledChanged += value; }
            remove { _enabledChanged -= value; }
        }

        /// <summary>
        /// Called by the framework when the user clicks the "apply" menu item or toolbar button.
        /// </summary>
        public void StartShred()
        {
            // TODO
            // Add code here to implement the functionality of the tool
            // If this tool is associated with a workspace, you can access the workspace
            // using the Workspace property

            if (null != this.Context.SelectedShred)
            {
                this.Context.StartShred(this.Context.SelectedShred);
            }
        }
    }
}
