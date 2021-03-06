#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Windows.Forms;

using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Admin.View.WinForms
{
	/// <summary>
	/// Provides a Windows Forms user-interface for <see cref="FacilityEditorComponent"/>
	/// </summary>
	public partial class FacilityEditorComponentControl : ApplicationComponentUserControl
	{
		private readonly FacilityEditorComponent _component;

		/// <summary>
		/// Constructor
		/// </summary>
		public FacilityEditorComponentControl(FacilityEditorComponent component)
			: base(component)
		{
			InitializeComponent();
			_component = component;

			_name.DataBindings.Add("Value", _component, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
			_code.DataBindings.Add("Value", _component, "Code", true, DataSourceUpdateMode.OnPropertyChanged);
			_description.DataBindings.Add("Value", _component, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
			_informationAuthority.DataSource = _component.InformationAuthorityChoices;
			_informationAuthority.DataBindings.Add("Value", _component, "InformationAuthority", true, DataSourceUpdateMode.OnPropertyChanged);
			_acceptButton.DataBindings.Add("Enabled", _component, "AcceptEnabled", true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void _acceptButton_Click(object sender, EventArgs e)
		{
			_component.Accept();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			_component.Cancel();
		}
	}
}
