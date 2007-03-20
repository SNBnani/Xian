using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.RegistrationWorkflow
{
    [DataContract]
    public class GetWorklistRequest : DataContractBase
    {
        public GetWorklistRequest(string worklistClassName, RegistrationWorklistSearchCriteria searchCriteria)
        {
            this.WorklistClassName = worklistClassName;
            this.SearchCriteria = searchCriteria;
        }

        [DataMember(IsRequired = true)]
        public string WorklistClassName;

        [DataMember]
        public RegistrationWorklistSearchCriteria SearchCriteria;
    }
}
