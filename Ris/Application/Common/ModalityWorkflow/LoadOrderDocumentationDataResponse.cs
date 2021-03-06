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
using ClearCanvas.Common.Serialization;
using ClearCanvas.Enterprise.Common;
using System.Runtime.Serialization;

namespace ClearCanvas.Ris.Application.Common.ModalityWorkflow
{
    [DataContract]
    public class LoadOrderDocumentationDataResponse : DataContractBase
    {
        [DataMember]
        public EntityRef OrderRef;

        [DataMember]
        public Dictionary<string, string> OrderExtendedProperties;

        [DataMember]
        public List<OrderNoteDetail> OrderNotes;

        /// <summary>
        /// Radiologist that has been assigned to read these procedures, or null if none has been assigned.
        /// </summary>
        [DataMember]
        public StaffSummary AssignedInterpreter;
    }
}
