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
using ClearCanvas.Dicom;
using ClearCanvas.ImageServer.Enterprise;

namespace ClearCanvas.ImageServer.Model.Parameters
{
    public class CreatePatientForStudyParameters : ProcedureParameters
    {
        public CreatePatientForStudyParameters()
            : base("CreatePatientForStudy")
        {
        }

        public ServerEntityKey StudyKey
        {
            set { SubCriteria["StudyKey"] = new ProcedureParameter<ServerEntityKey>("StudyKey", value); }
        }

        [DicomField(DicomTags.PatientsName)]
        public string PatientsName
        {
            set { SubCriteria["PatientsName"] = new ProcedureParameter<string>("PatientsName", value); }
        }

        [DicomField(DicomTags.PatientId)]
        public string PatientId
        {
            set { SubCriteria["PatientId"] = new ProcedureParameter<string>("PatientId", value); }
        }

        [DicomField(DicomTags.IssuerOfPatientId)]
        public string IssuerOfPatientId
        {
            set { SubCriteria["IssuerOfPatientId"] = new ProcedureParameter<string>("IssuerOfPatientId", value); }
        }

        [DicomField(DicomTags.SpecificCharacterSet)]
        public string SpecificCharacterSet
        {
            set { SubCriteria["SpecificCharacterSet"] = new ProcedureParameter<string>("SpecificCharacterSet", value); }
        }

    }
}
