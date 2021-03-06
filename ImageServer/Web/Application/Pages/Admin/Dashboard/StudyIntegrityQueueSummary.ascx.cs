#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Web.Common.Data;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Admin.Dashboard
{
    public partial class StudyIntegrityQueueSummary : System.Web.UI.UserControl
    {
        private int _duplicateCount;
        private int _inconsistentDataCount;
        
        public int Duplicates
        {
            get { return _duplicateCount; }
            set { _duplicateCount = value; }
        }

        public int InconsistentData
        {
            get { return _inconsistentDataCount; }
            set { _inconsistentDataCount = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StudyIntegrityQueueController controller = new StudyIntegrityQueueController();
            StudyIntegrityQueueSelectCriteria criteria = new StudyIntegrityQueueSelectCriteria();

            criteria.StudyIntegrityReasonEnum.EqualTo(StudyIntegrityReasonEnum.Duplicate);
            Duplicates = controller.GetReconcileQueueItemsCount(criteria);
            DuplicateLink.PostBackUrl = ImageServerConstants.PageURLs.StudyIntegrityQueuePage + "?Databind=true&Reason=" + StudyIntegrityReasonEnum.Duplicate.Lookup;

            criteria.StudyIntegrityReasonEnum.EqualTo(StudyIntegrityReasonEnum.InconsistentData);
            InconsistentData = controller.GetReconcileQueueItemsCount(criteria);
            InconsistentDataLink.PostBackUrl = ImageServerConstants.PageURLs.StudyIntegrityQueuePage + "?Databind=true&Reason=" + StudyIntegrityReasonEnum.InconsistentData.Lookup ;

            TotalLinkButton.PostBackUrl = ImageServerConstants.PageURLs.StudyIntegrityQueuePage + "?Databind=true";


        }
    }
}