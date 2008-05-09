using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;

#pragma warning disable 0419,1574,1587,1591

namespace ClearCanvas.ImageViewer.Clipboard
{
	[MenuAction("deleteAll", "clipboard-contextmenu/MenuDeleteAllClipboardItems", "DeleteAll")]
	[ButtonAction("deleteAll", "clipboard-toolbar/ToolbarDeleteAllClipboardItems", "DeleteAll")]
	[Tooltip("deleteAll", "TooltipDeleteAllClipboardItems")]
	[IconSet("deleteAll", IconScheme.Colour, "Icons.DeleteAllClipboardItemsToolSmall.png", "Icons.DeleteAllClipboardItemsToolSmall.png", "Icons.DeleteClipboardItemToolSmall.png")]
	[EnabledStateObserver("deleteAll", "Enabled", "EnabledChanged")]
	
	[ExtensionOf(typeof(ClipboardToolExtensionPoint))]
	public class DeleteAllClipboardItemsTool : ClipboardTool
	{
		public DeleteAllClipboardItemsTool()
		{
		}

		public override void Initialize()
		{
			base.Initialize();
			this.Enabled = this.Context.ClipboardItems.Count > 0;
		}

		protected override void OnSelectionChanged()
		{
			this.Enabled = this.Context.ClipboardItems.Count > 0;
		}

		protected override void OnClipboardItemsChanged()
		{
			this.Enabled = this.Context.ClipboardItems.Count > 0;
		}

		public void DeleteAll()
		{
			bool anyLocked = false;
			
			List<IClipboardItem> items = new List<IClipboardItem>(this.Context.ClipboardItems);
			foreach (ClipboardItem item in items)
			{
				if (item.Locked)
				{
					anyLocked = true;
				}
				else
				{
					((IDisposable)item).Dispose();
					this.Context.ClipboardItems.Remove(item);
				}
			}

			if (anyLocked)
				this.Context.DesktopWindow.ShowMessageBox(SR.MessageUnableToClearClipboardItems, MessageBoxActions.Ok);
		}
	}
}
