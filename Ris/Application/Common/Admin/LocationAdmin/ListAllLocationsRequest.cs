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
using ClearCanvas.Enterprise.Common;
using System.Runtime.Serialization;

namespace ClearCanvas.Ris.Application.Common.Admin.LocationAdmin
{
    [DataContract]
	public class ListAllLocationsRequest : ListRequestBase
    {
        public ListAllLocationsRequest()
        {
        }

		public ListAllLocationsRequest(SearchResultPage page)
            :base(page)
        {
        }

		[DataMember]
		public FacilitySummary Facility;

		[DataMember]
		public string Name;
    }
}
