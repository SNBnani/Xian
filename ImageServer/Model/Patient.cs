#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using ClearCanvas.Dicom;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model.SelectBrokers;

namespace ClearCanvas.ImageServer.Model
{
    public class Patient : ServerEntity
    {
        #region Constructors
        public Patient()
            : base("Patient")
        {
        }
        #endregion

        #region Private Members
        private ServerEntityKey _serverPartitionKey;
        private String _patientName;
        private String _patientId;
        private String _issuerOfPatientId;
        private int _numberOfPatientRelatedStudies;
        private int _numberOfPatientRelatedSeries;
        private int _numberOfPatientRelatedInstances;
        #endregion

        #region Public Properties
        public ServerEntityKey ServerPartitionKey
        {
            get { return _serverPartitionKey; }
            set { _serverPartitionKey = value; }
        }

        [DicomField(DicomTags.PatientsName, DefaultValue = DicomFieldDefault.Null)]
        public String PatientName
        {
            get { return _patientName; }
            set { _patientName = value; }
        }

        [DicomField(DicomTags.PatientId, DefaultValue = DicomFieldDefault.Null)]
        public String PatientId
        {
            get { return _patientId; }
            set { _patientId = value; }
                 
        }

        [DicomField(DicomTags.IssuerOfPatientId, DefaultValue = DicomFieldDefault.Null)]
        public String IssuerOfPatientId
        {
            get { return _issuerOfPatientId; }
            set { _issuerOfPatientId = value; }
        }

        [DicomField(DicomTags.NumberOfPatientRelatedStudies, DefaultValue = DicomFieldDefault.Null)]
        public int NumberOfPatientRelatedStudies
        {
            get { return _numberOfPatientRelatedStudies; }
            set { _numberOfPatientRelatedStudies = value; }
        }

        [DicomField(DicomTags.NumberOfPatientRelatedSeries, DefaultValue = DicomFieldDefault.Null)]
        public int NumberOfPatientRelatedSeries
        {
            get { return _numberOfPatientRelatedSeries; }
            set { _numberOfPatientRelatedSeries = value; }
        }

        [DicomField(DicomTags.NumberOfPatientRelatedInstances, DefaultValue = DicomFieldDefault.Null)]
        public int NumberOfPatientRelatedInstances
        {
            get { return _numberOfPatientRelatedInstances; }
            set { _numberOfPatientRelatedInstances = value; }
        }
        #endregion

        #region Static Methods
        static public Patient Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public Patient Load(IReadContext read, ServerEntityKey key)
        {
            ISelectPatient broker = read.GetBroker<ISelectPatient>();
            Patient thePatient = broker.Load(key);
            return thePatient;
        }
        #endregion

    }
}
