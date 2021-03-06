#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0//

#endregion

// This file is auto-generated by the ClearCanvas.Model.SqlServer.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;

    public partial class PartitionSopClassSelectCriteria : EntitySelectCriteria
    {
        public PartitionSopClassSelectCriteria()
        : base("PartitionSopClass")
        {}
        public PartitionSopClassSelectCriteria(PartitionSopClassSelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new PartitionSopClassSelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionSopClass", ColumnName="ServerPartitionGUID")]
        public ISearchCondition<ServerEntityKey> ServerPartitionKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServerPartitionKey"))
              {
                 SubCriteria["ServerPartitionKey"] = new SearchCondition<ServerEntityKey>("ServerPartitionKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["ServerPartitionKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionSopClass", ColumnName="ServerSopClassGUID")]
        public ISearchCondition<ServerEntityKey> ServerSopClassKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServerSopClassKey"))
              {
                 SubCriteria["ServerSopClassKey"] = new SearchCondition<ServerEntityKey>("ServerSopClassKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["ServerSopClassKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionSopClass", ColumnName="Enabled")]
        public ISearchCondition<Boolean> Enabled
        {
            get
            {
              if (!SubCriteria.ContainsKey("Enabled"))
              {
                 SubCriteria["Enabled"] = new SearchCondition<Boolean>("Enabled");
              }
              return (ISearchCondition<Boolean>)SubCriteria["Enabled"];
            } 
        }
    }
}
