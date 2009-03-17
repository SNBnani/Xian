using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.Services.ServerTree;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.Dicom.ServiceModel.Query;
using ClearCanvas.Dicom.Utilities;
using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer.Layout.Basic
{
	[ExceptionPolicyFor(typeof(PriorStudyLoaderException))]
	[ExtensionOf(typeof(ExceptionPolicyExtensionPoint))]
	public class PriorStudyLoaderExceptionPolicy : IExceptionPolicy
	{
		public PriorStudyLoaderExceptionPolicy()
		{
		}

		#region IExceptionPolicy Members

		public void Handle(System.Exception e, IExceptionHandlingContext exceptionHandlingContext)
		{
			if (e is PriorStudyLoaderException)
				Handle(e as PriorStudyLoaderException, exceptionHandlingContext);
		}

		#endregion

		private static void Handle(PriorStudyLoaderException exception, IExceptionHandlingContext context)
		{
			if (exception.FindFailed || (exception.TotalFailures == exception.TotalQueryResults))
			{
				context.ShowMessageBox(SR.MessageFailedToLoadAnyPriors);
			}
			else if (exception.TotalFailures > 0 && exception.PartialFailures> 0)
			{
				string message = String.Format(SR.FormatXCompleteYPartialPriorLoadFailures, 
					exception.TotalFailures, exception.PartialFailures);

				context.ShowMessageBox(message);
			}
			else if (exception.TotalFailures > 0)
			{
				string message;
				if (exception.TotalFailures == 1)
					message = SR.Message1CompletePriorLoadFailures;
				else
					message = String.Format(SR.FormatXCompletePriorLoadFailures, exception.TotalFailures);

				context.ShowMessageBox(message);
			}
			else if (exception.PartialFailures > 0)
			{
				string message;
				if (exception.PartialFailures == 1)
					message = SR.Message1PartialPriorLoadFailures;
				else
					message = String.Format(SR.FormatXPartialPriorLoadFailures, exception.PartialFailures);

				context.ShowMessageBox(message);
			}
			else
			{
				context.ShowMessageBox(SR.MessageUnexpectedPriorLoadFailure);
			}
		}
	}

	[ExtensionOf(typeof(PriorStudyFinderExtensionPoint))]
	public class PriorStudyFinder : ClearCanvas.ImageViewer.PriorStudyFinder
	{
		private volatile bool _cancel;

		public PriorStudyFinder()
		{
		}

		public override StudyItemList FindPriorStudies()
		{
			_cancel = false;
			StudyItemList results = new StudyItemList();

			foreach (Patient patient in Viewer.StudyTree.Patients)
			{
				//TODO: option to find priors only when a single study is loaded?
				if (_cancel)
					break;

				//TODO: add option for trimming last word and/or using wildcards?
				using (StudyRootQueryBridge bridge = new StudyRootQueryBridge(Platform.GetService<IStudyRootQuery>()))
				{
					IList<StudyRootStudyIdentifier> studies = bridge.QueryByPatientId(patient.PatientId);
					foreach (StudyRootStudyIdentifier study in studies)
					{
						results.Add(ConvertToStudyItem(study));
					}
				}
			}

			return results;
		}

		public override void Cancel()
		{
			_cancel = true;
		}

		private StudyItem ConvertToStudyItem(StudyRootStudyIdentifier study)
		{
			StudyItem item = new StudyItem();
			item.Server = FindServer(study.RetrieveAeTitle);
			if (item.Server == null)
				item.StudyLoaderName = "DICOM_LOCAL";
			else
				item.StudyLoaderName = "CC_STREAMING";

			item.AccessionNumber = study.AccessionNumber;
			item.ModalitiesInStudy = DicomStringHelper.GetDicomStringArray(study.ModalitiesInStudy ?? new string[0]);
			item.NumberOfStudyRelatedInstances = (uint)(study.NumberOfStudyRelatedInstances ?? 0);
			item.PatientId = study.PatientId;
			item.PatientsBirthDate = study.PatientsBirthDate;
			item.PatientsName = new PersonName(study.PatientsName);
			item.SpecificCharacterSet = study.SpecificCharacterSet;
			item.StudyDate = study.StudyDate;
			item.StudyDescription = study.StudyDescription;
			item.StudyInstanceUID = study.StudyInstanceUid;

			return item;
		}

		private static ApplicationEntity FindServer(string retrieveAETitle)
		{
			List<Server> remoteServers = Services.Configuration.DefaultServers.GetAll();
			foreach (Server server in remoteServers)
			{
				if (server.IsStreaming && server.AETitle == retrieveAETitle)
					return new ApplicationEntity(server.Host, server.AETitle, server.Port, server.HeaderServicePort, server.WadoServicePort);
			}

			return null;
		}
	}
}
