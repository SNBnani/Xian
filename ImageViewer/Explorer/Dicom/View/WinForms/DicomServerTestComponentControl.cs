using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.ImageViewer.Explorer.Dicom.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms user-interface for <see cref="DicomServerTestComponent"/>
    /// </summary>
    public partial class DicomServerTestComponentControl : ApplicationComponentUserControl
    {
        private DicomServerTestComponent _component;

        /// <summary>
        /// Constructor
        /// </summary>
        public DicomServerTestComponentControl(DicomServerTestComponent component)
            : base(component)
        {
            InitializeComponent();
            _component = component;

            _aeTitle.DataBindings.Add("Value", component, "AETitle", true, DataSourceUpdateMode.OnPropertyChanged);
            _port.DataBindings.Add("Value", component, "Port", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void _toggleServerButton_Click(object sender, EventArgs e)
        {
            if (_component.IsServerStarted)
            {
                _component.StopServer();
                _toggleServerButton.Text = "Start";
                _aeTitle.Enabled = true;
                _port.Enabled = true;
            }
            else
            {
                _component.StartServer();
                _toggleServerButton.Text = "Stop";
                _aeTitle.Enabled = false;
                _port.Enabled = false;
            }
        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            if (_component.IsStarted)
                _component.StopServer();

            _component.Cancel();
        }
    }
}
