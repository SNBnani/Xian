﻿using System;
using ClearCanvas.ImageViewer.Services.LocalDataStore;
using ClearCanvas.Desktop.View.WinForms;
using ClearCanvas.Desktop;
using System.Windows.Forms;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.Services.Tools.View.WinForms
{
	[ExtensionOf(typeof(ReindexLocalDataStoreApplicationViewExtensionPoint))]
	public class ReindexLocalDataStoreApplicationView : ApplicationView, IReindexLocalDataStoreApplicationView
	{
		private Form _form;
		private IReindexLocalDataStore _reindexer;

		#region IReindexLocalDataStoreApplicationView Members

		public void Initialize(IReindexLocalDataStore reindexer)
		{
			_reindexer = reindexer;
			Cursor.Current = Cursors.WaitCursor;
		}

		public void RunModal()
		{
			Cursor.Current = Cursors.Default;
			_form = new ReindexLocalDataStoreDialogForm(_reindexer);
			SplashScreenManager.DismissSplashScreen(_form);
			_form.ShowDialog();
		}

		#endregion

		public override IDesktopWindowView CreateDesktopWindowView(DesktopWindow window)
		{
			throw new InvalidOperationException();
		}

		public override DialogBoxAction ShowMessageBox(string message, MessageBoxActions actions)
		{
			Cursor.Current = Cursors.Default;
			SplashScreenManager.DismissSplashScreen(null);
			return base.ShowMessageBox(message, actions);
		}
		#region IDisposable Members

		public void Dispose()
		{
			Cursor.Current = Cursors.Default;

			if (_form == null)
				return;

			_form.Dispose();
			_form = null;
		}

		#endregion
	}
}
