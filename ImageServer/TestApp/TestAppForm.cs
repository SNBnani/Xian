using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Database;

namespace ClearCanvas.ImageServer.TestApp
{
    public partial class TestAppForm : Form
    {
        public TestAppForm()
        {
            
            InitializeComponent();
        }

        private void checkBoxLoadTest_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                IPersistentStore store = PersistentStoreRegistry.GetDefaultStore();

                FileSystemMonitor monitor = new FileSystemMonitor(store);

                monitor.Load();



                IReadContext ctx = store.OpenReadContext();

                IInsertStudyStorage insert = ctx.GetBroker<IInsertStudyStorage>();

                StudyStorageInsertParameters criteria = new StudyStorageInsertParameters();

                criteria.StudyInstanceUid = "1.2.3.4";
                criteria.ExpirationTime = DateTime.Now;
                criteria.ScheduledTime = DateTime.Now;
                criteria.FilesystemRef = monitor.Filesystems[0].GetKey();
                criteria.Folder = "20070101";
                criteria.ServerPartitionRef = monitor.Partitions[0].GetKey();

                IList<StudyStorageLocation> storage = insert.Execute(criteria);

                StudyStorageLocation storageEntry = storage[0];
            }
            catch (Exception x)
            {
                Platform.Log(x);
            }
        }
    }
}