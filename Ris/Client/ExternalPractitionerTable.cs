﻿using System.Reflection;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Client.Formatting;
using System;

namespace ClearCanvas.Ris.Client
{
	/// <summary>
	/// External Practitioner table used by the Admin summary page.
	/// </summary>
	public class ExternalPractitionerAdminTable : Table<ExternalPractitionerSummary>
	{
		public ExternalPractitionerAdminTable()
		{
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnFamilyName
				, item => item.Name.FamilyName, 1.0f));
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnGivenName,
				item => item.Name.GivenName, 1.0f));
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnLicenseNumber,
				item => item.LicenseNumber, 0.5f));
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnBillingNumber,
				item => item.BillingNumber, 0.5f));
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, bool>(SR.ColumnVerified,
				item => item.IsVerified, 0.25f));
		}
	}

	/// <summary>
	/// External Practitioner table used by the folder system.
	/// </summary>
	public class ExternalPractitionerWorkflowTable : Table<ExternalPractitionerSummary>
	{
		private PropertyInfo _timeInfo;

		public ExternalPractitionerWorkflowTable()
		{
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnName
				, item => PersonNameFormat.Format(item.Name), 1.0f));
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnLicenseNumber,
				item => item.LicenseNumber, 0.5f));
			this.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnBillingNumber,
				item => item.BillingNumber, 0.5f));
			this.Columns.Add(new DateTimeTableColumn<ExternalPractitionerSummary>(SR.ColumnTime,
				item => _timeInfo == null ? item.LastVerifiedTime : (DateTime?)_timeInfo.GetValue(item, null), 0.75f));
		}

		public string PropertyNameForTimeColumn
		{
			get { return _timeInfo == null ? null : _timeInfo.Name; }
			set
			{
				// Get the field info that matches the sortByTimeFieldName.  Make sure the field name is actually a date time type
				var info = string.IsNullOrEmpty(value) ? null : typeof(ExternalPractitionerSummary).GetProperty(value);
				if (info != null && info.PropertyType.Equals(typeof (DateTime?)))
					_timeInfo = info;
			}
		}
	}
}
