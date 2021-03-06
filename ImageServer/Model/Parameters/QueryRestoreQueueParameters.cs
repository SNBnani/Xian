#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.ImageServer.Enterprise;

namespace ClearCanvas.ImageServer.Model.Parameters
{
	public class QueryRestoreQueueParameters : ProcedureParameters
	{
		public QueryRestoreQueueParameters()
            : base("QueryRestoreQueue")
        {
        }

		public ServerEntityKey PartitionArchiveKey
		{
			set { SubCriteria["PartitionArchiveKey"] = new ProcedureParameter<ServerEntityKey>("PartitionArchiveKey", value); }
		}

		public string ProcessorId
		{
			set { SubCriteria["ProcessorId"] = new ProcedureParameter<string>("ProcessorId", value); }
		}

		public RestoreQueueStatusEnum RestoreQueueStatusEnum
		{
			set { SubCriteria["RestoreQueueStatusEnum"] = new ProcedureParameter<ServerEnum>("RestoreQueueStatusEnum", value);}
		}
	}
}
