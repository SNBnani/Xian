#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using ClearCanvas.Dicom;
    using ClearCanvas.ImageServer.Enterprise;

   public class StudyUpdateColumns : EntityUpdateColumns
   {
       public StudyUpdateColumns()
       : base("Study")
       {}
       [DicomField(DicomTags.AccessionNumber, DefaultValue = DicomFieldDefault.Null)]
        public System.String AccessionNumber
        {
            set { SubParameters["AccessionNumber"] = new EntityUpdateColumn<System.String>("AccessionNumber", value); }
        }
       [DicomField(DicomTags.NumberOfStudyRelatedInstances, DefaultValue = DicomFieldDefault.Null)]
        public System.Int32 NumberOfStudyRelatedInstances
        {
            set { SubParameters["NumberOfStudyRelatedInstances"] = new EntityUpdateColumn<System.Int32>("NumberOfStudyRelatedInstances", value); }
        }
       [DicomField(DicomTags.NumberOfStudyRelatedSeries, DefaultValue = DicomFieldDefault.Null)]
        public System.Int32 NumberOfStudyRelatedSeries
        {
            set { SubParameters["NumberOfStudyRelatedSeries"] = new EntityUpdateColumn<System.Int32>("NumberOfStudyRelatedSeries", value); }
        }
        public ClearCanvas.ImageServer.Enterprise.ServerEntityKey PatientKey
        {
            set { SubParameters["PatientKey"] = new EntityUpdateColumn<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("PatientKey", value); }
        }
       [DicomField(DicomTags.PatientId, DefaultValue = DicomFieldDefault.Null)]
        public System.String PatientId
        {
            set { SubParameters["PatientId"] = new EntityUpdateColumn<System.String>("PatientId", value); }
        }
       [DicomField(DicomTags.PatientsBirthDate, DefaultValue = DicomFieldDefault.Null)]
        public System.String PatientsBirthDate
        {
            set { SubParameters["PatientsBirthDate"] = new EntityUpdateColumn<System.String>("PatientsBirthDate", value); }
        }
       [DicomField(DicomTags.PatientsName, DefaultValue = DicomFieldDefault.Null)]
        public System.String PatientsName
        {
            set { SubParameters["PatientsName"] = new EntityUpdateColumn<System.String>("PatientsName", value); }
        }
       [DicomField(DicomTags.PatientsSex, DefaultValue = DicomFieldDefault.Null)]
        public System.String PatientsSex
        {
            set { SubParameters["PatientsSex"] = new EntityUpdateColumn<System.String>("PatientsSex", value); }
        }
        public QueueStudyStateEnum QueueStudyStateEnum
        {
            set { SubParameters["QueueStudyStateEnum"] = new EntityUpdateColumn<QueueStudyStateEnum>("QueueStudyStateEnum", value); }
        }
       [DicomField(DicomTags.ReferringPhysiciansName, DefaultValue = DicomFieldDefault.Null)]
        public System.String ReferringPhysiciansName
        {
            set { SubParameters["ReferringPhysiciansName"] = new EntityUpdateColumn<System.String>("ReferringPhysiciansName", value); }
        }
        public ClearCanvas.ImageServer.Enterprise.ServerEntityKey ServerPartitionKey
        {
            set { SubParameters["ServerPartitionKey"] = new EntityUpdateColumn<ClearCanvas.ImageServer.Enterprise.ServerEntityKey>("ServerPartitionKey", value); }
        }
       [DicomField(DicomTags.SpecificCharacterSet, DefaultValue = DicomFieldDefault.Null)]
        public System.String SpecificCharacterSet
        {
            set { SubParameters["SpecificCharacterSet"] = new EntityUpdateColumn<System.String>("SpecificCharacterSet", value); }
        }
       [DicomField(DicomTags.StudyDate, DefaultValue = DicomFieldDefault.Null)]
        public System.String StudyDate
        {
            set { SubParameters["StudyDate"] = new EntityUpdateColumn<System.String>("StudyDate", value); }
        }
       [DicomField(DicomTags.StudyDescription, DefaultValue = DicomFieldDefault.Null)]
        public System.String StudyDescription
        {
            set { SubParameters["StudyDescription"] = new EntityUpdateColumn<System.String>("StudyDescription", value); }
        }
       [DicomField(DicomTags.StudyId, DefaultValue = DicomFieldDefault.Null)]
        public System.String StudyId
        {
            set { SubParameters["StudyId"] = new EntityUpdateColumn<System.String>("StudyId", value); }
        }
       [DicomField(DicomTags.StudyInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        public System.String StudyInstanceUid
        {
            set { SubParameters["StudyInstanceUid"] = new EntityUpdateColumn<System.String>("StudyInstanceUid", value); }
        }
        public StudyStatusEnum StudyStatusEnum
        {
            set { SubParameters["StudyStatusEnum"] = new EntityUpdateColumn<StudyStatusEnum>("StudyStatusEnum", value); }
        }
       [DicomField(DicomTags.StudyTime, DefaultValue = DicomFieldDefault.Null)]
        public System.String StudyTime
        {
            set { SubParameters["StudyTime"] = new EntityUpdateColumn<System.String>("StudyTime", value); }
        }
    }
}
