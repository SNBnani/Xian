﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Runtime.Remoting.Messaging;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Iod.Modules;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Network.Scu
{
	/// <summary>
	/// Scu class for printing a image.
	/// </summary>
	public partial class PrintScu : ScuBase
	{
		#region Events/Delegates

		/// <summary>
		/// Print delegate for printing ASynch.
		/// </summary>
		private delegate DicomState PrintDelegate(string clientAETitle, string remoteAE, string remoteHost, int remotePort, FilmSession filmSession);

		#endregion

		#region Private Variables

		/// <summary>
		/// Private enum for knowing the state of the print request, so we know what to send to the SCP
		/// </summary>
		private enum EventObjectType
		{
			FilmSession,
			FilmBox,
			ImageBox
		}

		private EventObjectType _eventObjectType;

		private int _numberOfImageBoxesSent;
		private FilmSession _filmSession;

		#endregion

		#region Public Methods/Properties

		/// <summary>
		/// Specifies whether the SCU will be printing color or grayscale.  Default is grayscale.
		/// </summary>
		public bool ColorPrinting { get; set; }

		/// <summary>
		/// Prints with the specified parameters.  Create all image boxes up-front.
		/// </summary>
		/// <param name="clientAETitle">The client AE title.</param>
		/// <param name="remoteAE">The remote AE.</param>
		/// <param name="remoteHost">The remote host.</param>
		/// <param name="remotePort">The remote port.</param>
		/// <param name="filmSession">The film session to print.</param>
		public DicomState Print(string clientAETitle, string remoteAE, string remoteHost, int remotePort, FilmSession filmSession)
		{
			_filmSession = filmSession;
			_filmSession.PrintScu = this;

			Connect(clientAETitle, remoteAE, remoteHost, remotePort);
			if (Status == ScuOperationStatus.Canceled)
				return DicomState.Cancel;
			if (Status == ScuOperationStatus.AssociationRejected || Status == ScuOperationStatus.Failed || Status == ScuOperationStatus.ConnectFailed ||
				Status == ScuOperationStatus.NetworkError || Status == ScuOperationStatus.TimeoutExpired)
				return DicomState.Failure;
			return ResultStatus;
		}

		/// <summary>
		/// Begins the print asynchronously.
		/// </summary>
		/// <param name="clientAETitle">The client AE title.</param>
		/// <param name="remoteAE">The remote AE.</param>
		/// <param name="remoteHost">The remote host.</param>
		/// <param name="remotePort">The remote port.</param>
		/// <param name="filmSession">The film session to print.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		public IAsyncResult BeginPrint(string clientAETitle, string remoteAE, string remoteHost, int remotePort, FilmSession filmSession, AsyncCallback callback, object asyncState)
		{
			var printDelegate = new PrintDelegate(this.Print);
			return printDelegate.BeginInvoke(clientAETitle, remoteAE, remoteHost, remotePort, filmSession, callback, asyncState);
		}

		/// <summary>
		/// Ends the asynchronous print.
		/// </summary>
		/// <param name="ar">The ar.</param>
		/// <returns></returns>
		public DicomState EndPrint(IAsyncResult ar)
		{
			var printDelegate = ((AsyncResult)ar).AsyncDelegate as PrintDelegate;

			if (printDelegate == null)
				throw new InvalidOperationException("cannot get results, asynchresult is null");

			return printDelegate.EndInvoke(ar);
		}

		#endregion

		#region Overridden Methods

		/// <summary>
		/// Adds the appropriate presentation context.
		/// </summary>
		protected override void SetPresentationContexts()
		{
			AddSopClassToPresentationContext(this.ColorPrinting
				? SopClass.BasicColorPrintManagementMetaSopClass
				: SopClass.BasicGrayscalePrintManagementMetaSopClass);
		}

		/// <summary>
		/// Called when received associate accept.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		public override void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
		{
			base.OnReceiveAssociateAccept(client, association);
			if (Canceled)
			{
				client.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
			}
			else
			{
				CreateFilmSession(_filmSession);
			}
		}

		/// <summary>
		/// Called when received response message.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="presentationID">The presentation ID.</param>
		/// <param name="message">The message.</param>
		public override void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
		{
			try
			{
				this.ResultStatus = message.Status.Status;
				if (message.Status.Status != DicomState.Success)
				{
					Platform.Log(LogLevel.Warn, string.Format("{0} status received in Print Scu response message", message.Status.Status));
					return;
				}

				EventsHelper.Fire(this.ProgressUpdated, this, new ProgressUpdateEventArgs(_numberOfImageBoxesSent));

				if (Canceled)
				{
					Platform.Log(LogLevel.Info, "Cancel requested by user.  Closing association.");
					client.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
					return;
				}

				Platform.Log(LogLevel.Info, "Success status received in Print Scu");

				var affectedUid = new DicomUid(message.AffectedSopInstanceUid, "Instance UID", UidType.SOPInstance);

				switch (message.CommandField)
				{
					case DicomCommandField.NCreateResponse:
						switch (_eventObjectType)
						{
							case EventObjectType.FilmSession:
								_filmSession.OnCreated(affectedUid);
								break;
							case EventObjectType.FilmBox:
								{
									var responseFilmBoxModule = new BasicFilmBoxModuleIod(message.DataSet);
									_filmSession.OnFilmBoxCreated(affectedUid,
										CollectionUtils.Map<ReferencedInstanceSequenceIod, DicomUid>(
											responseFilmBoxModule.ReferencedImageBoxSequenceList,
											imageBoxModule => new DicomUid(imageBoxModule.ReferencedSopInstanceUid, "Instance UID", UidType.SOPInstance)
										));
								}
								break;
						}

						break;

					case DicomCommandField.NDeleteResponse:
						switch (_eventObjectType)
						{
							case EventObjectType.FilmSession:
								_filmSession.OnDeleted();
								this.ReleaseConnection(client);
								break;
							case EventObjectType.FilmBox:
								_filmSession.OnFilmBoxDeleted();
								break;
						}

						break;

					case DicomCommandField.NSetResponse:
						_numberOfImageBoxesSent++;
						_filmSession.OnImageBoxSet(affectedUid);
						break;

					case DicomCommandField.NActionResponse:
						_filmSession.OnFilmBoxPrinted(affectedUid);

						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				this.FailureDescription = ex.Message;
				Platform.Log(LogLevel.Error, ex.ToString());
				ReleaseConnection(client);
				throw;
			}
		}

		#endregion

		#region Send message methods

		protected void CreateFilmSession(FilmSession filmSession)
		{
			var message = new DicomMessage(null, (DicomAttributeCollection)filmSession.DicomAttributeProvider);
			this.Client.SendNCreateRequest(null, GetPresentationContextId(this.AssociationParameters), this.Client.NextMessageID(), message, DicomUids.BasicFilmSession);
			_eventObjectType = EventObjectType.FilmSession;

			Platform.Log(LogLevel.Debug, "Creating film session...");
		}

		protected void CreateFilmBox(FilmSession filmSession, FilmBox filmBox)
		{
			var referencedFilmSessionSequence = new ReferencedInstanceSequenceIod
			{
				ReferencedSopClassUid = SopClass.BasicFilmSessionSopClassUid,
				ReferencedSopInstanceUid = filmSession.SopInstanceUid.UID
			};

			filmBox.ReferencedFilmSessionSequenceList.Add(referencedFilmSessionSequence);

			var message = new DicomMessage(null, (DicomAttributeCollection)filmBox.DicomAttributeProvider);
			this.Client.SendNCreateRequest(null, GetPresentationContextId(this.AssociationParameters), this.Client.NextMessageID(), message, DicomUids.BasicFilmBoxSOP);
			_eventObjectType = EventObjectType.FilmBox;
			Platform.Log(LogLevel.Debug, "Creating film box...");
		}

		protected void SetImageBox(ImageBox imageBox)
		{
			var message = new DicomMessage(null, (DicomAttributeCollection)imageBox.DicomAttributeProvider)
			{
				RequestedSopClassUid = this.ColorPrinting ? SopClass.BasicColorImageBoxSopClassUid : SopClass.BasicGrayscaleImageBoxSopClassUid,
				RequestedSopInstanceUid = imageBox.SopInstanceUid.UID
			};

			this.Client.SendNSetRequest(GetPresentationContextId(this.AssociationParameters), this.Client.NextMessageID(), message);
			_eventObjectType = EventObjectType.ImageBox;
			Platform.Log(LogLevel.Debug, "Setting image box {0}...", _numberOfImageBoxesSent);
		}

		protected void PrintFilmBox(FilmBox filmBox)
		{
			var message = new DicomMessage(null, null)
			{
				RequestedSopInstanceUid = filmBox.SopInstanceUid.UID,
				RequestedSopClassUid = SopClass.BasicFilmBoxSopClassUid,
				ActionTypeId = 1
			};

			this.Client.SendNActionRequest(GetPresentationContextId(this.AssociationParameters), this.Client.NextMessageID(), message);
			_eventObjectType = EventObjectType.FilmBox;
			Platform.Log(LogLevel.Debug, "Printing film box...");
		}

		protected void DeleteFilmBox(FilmBox filmBox)
		{
			var message = new DicomMessage(null, null)
			{
				RequestedSopInstanceUid = filmBox.SopInstanceUid.UID,
				RequestedSopClassUid = SopClass.BasicFilmBoxSopClassUid
			};

			this.Client.SendNDeleteRequest(GetPresentationContextId(this.AssociationParameters), this.Client.NextMessageID(), message);
			_eventObjectType = EventObjectType.FilmBox;
			Platform.Log(LogLevel.Debug, "Deleting film box...");
		}

		protected void DeleteFilmSession(FilmSession filmSession)
		{
			var message = new DicomMessage(null, null)
			{
				RequestedSopInstanceUid = filmSession.SopInstanceUid.UID,
				RequestedSopClassUid = SopClass.BasicFilmSessionSopClassUid
			};

			this.Client.SendNDeleteRequest(GetPresentationContextId(this.AssociationParameters), this.Client.NextMessageID(), message);
			_eventObjectType = EventObjectType.FilmSession;
			Platform.Log(LogLevel.Debug, "Deleting film session...");
		}

		private byte GetPresentationContextId(AssociationParameters association)
		{
			return association.FindAbstractSyntaxOrThrowException(this.ColorPrinting
				? SopClass.BasicColorPrintManagementMetaSopClass
				: SopClass.BasicGrayscalePrintManagementMetaSopClass);
		}

		#endregion

		#region Progress Updates

		public event EventHandler<ProgressUpdateEventArgs> ProgressUpdated;

		public class ProgressUpdateEventArgs : EventArgs
		{
			public ProgressUpdateEventArgs(int numberOfImageBoxesSent)
			{
				this.NumberOfImageBoxesSent = numberOfImageBoxesSent;
			}

			public int NumberOfImageBoxesSent { get; private set; }
		}

		#endregion

		#region IDisposable Members

		private bool _disposed;
		/// <summary>
		/// Disposes the specified disposing.
		/// </summary>
		/// <param name="disposing">if set to <c>true</c> [disposing].</param>
		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;
			if (disposing)
			{
				// Dispose of other Managed objects, ie

			}
			// FREE UNMANAGED RESOURCES
			base.Dispose(true);
			_disposed = true;
		}
		#endregion

	}
}
