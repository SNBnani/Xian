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

using Gtk;
//using ClearCanvas.ImageViewer.Actions;
using ClearCanvas.Desktop.Actions;

//namespace ClearCanvas.ImageViewer.View.GTK
namespace ClearCanvas.Desktop.View.GTK
{
    public class GtkToolbarBuilder
    {
        public static void BuildToolbar(Toolbar toolbar, Tooltips tooltips, ActionModelNode node)
        {
           BuildToolbar(toolbar, tooltips, node, 0);
        }

        public static void BuildToolbar(Toolbar toolbar, Tooltips tooltips, ActionModelNode node, int depth)
        {
            if (node.Action != null)
            {

                // create the tool button
				ToolButton button = new ActiveToolbarButton((IClickAction)node.Action, tooltips);
                toolbar.Insert(button, toolbar.NItems);
            }
            else
            {
                foreach (ActionModelNode child in node.ChildNodes)
                {
                    BuildToolbar(toolbar, tooltips, child, depth + 1);
                }
            }
        }
    }
}
