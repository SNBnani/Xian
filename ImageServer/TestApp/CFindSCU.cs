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
using System.Net;
using System.Text;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;

namespace ClearCanvas.ImageServer.TestApp
{
    class CFindSCU:IDicomClientHandler
    {
        private string _aeTitle;
        private DicomClient _dicomClient;
        private OnResultReceived _callback;
        
        public string AETitle
        {
            get { return _aeTitle; }
            set { _aeTitle = value; }
        }

        public delegate void OnResultReceived(DicomAttributeCollection ds);

        public void Query(string remoteAE, string remoteHost, int remotePort, OnResultReceived callback)
        {
            _callback = callback;

            IPAddress addr = Dns.GetHostAddresses(remoteHost)[0];
            ClientAssociationParameters _assocParams = new ClientAssociationParameters(AETitle, remoteAE, new IPEndPoint(addr, remotePort));

            byte pcid = _assocParams.AddPresentationContext(SopClass.StudyRootQueryRetrieveInformationModelFind);
            _assocParams.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
            _assocParams.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

            _dicomClient = DicomClient.Connect(_assocParams, this);

        }

        private void SendCFind()
        {
            DicomMessage msg = new DicomMessage();
            DicomAttributeCollection cFindDataset = msg.DataSet;

            // set the Query Retrieve Level
            cFindDataset[DicomTags.QueryRetrieveLevel].SetStringValue("STUDY");

            // set the other tags we want to retrieve
            cFindDataset[DicomTags.StudyInstanceUid].SetStringValue("");
            cFindDataset[DicomTags.PatientsName].SetStringValue("");
            cFindDataset[DicomTags.PatientId].SetStringValue("");
            cFindDataset[DicomTags.ModalitiesInStudy].SetStringValue("");
            cFindDataset[DicomTags.StudyDescription].SetStringValue("");


            byte pcid = _dicomClient.AssociationParams.FindAbstractSyntax(SopClass.StudyRootQueryRetrieveInformationModelFind);
            _dicomClient.SendCFindRequest(pcid, _dicomClient.NextMessageID(), msg);

        }

        #region IDicomClientHandler Members

        public void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
        {
            SendCFind();
        }

        public void OnReceiveAssociateReject(DicomClient client, ClientAssociationParameters association, DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            
        }

        public void OnReceiveRequestMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            
        }

        public void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            if (message.Status.Status == DicomState.Pending)
            {
                string studyinstanceuid = message.DataSet[DicomTags.StudyInstanceUid].GetString(0, "");
                Console.WriteLine(studyinstanceuid);
                if (_callback != null)
                    _callback(message.DataSet);
            }
            else
            {
                _dicomClient.SendReleaseRequest();
            }
        }

        public void OnReceiveReleaseResponse(DicomClient client, ClientAssociationParameters association)
        {
            
        }

        public void OnReceiveAbort(DicomClient client, ClientAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnNetworkError(DicomClient client, ClientAssociationParameters association, Exception e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnDimseTimeout(DicomClient server, ClientAssociationParameters association)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
