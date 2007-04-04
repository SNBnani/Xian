using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Tables;

using ClearCanvas.Enterprise;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.Admin;
using ClearCanvas.Ris.Application.Common.Admin.LocationAdmin;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Client.Admin
{
    [MenuAction("launch", "global-menus/Admin/Locations")]
    [ClickHandler("launch", "Launch")]
    [ActionPermission("launch", ClearCanvas.Ris.Application.Common.AuthorityTokens.LocationAdmin)]

    [ExtensionOf(typeof(DesktopToolExtensionPoint))]
    public class LocationSummaryTool : Tool<IDesktopToolContext>
    {
        private IWorkspace _workspace;

        public void Launch()
        {
            if (_workspace == null)
            {
                LocationSummaryComponent component = new LocationSummaryComponent();

                _workspace = ApplicationComponent.LaunchAsWorkspace(
                    this.Context.DesktopWindow,
                    component,
                    SR.TitleLocations,
                    delegate(IApplicationComponent c) { _workspace = null; });
            }
            else
            {
                _workspace.Activate();
            }
        }
    }
    
    /// <summary>
    /// Extension point for views onto <see cref="LocationSummaryComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class LocationSummaryComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// LocationSummaryComponent class
    /// </summary>
    [AssociateView(typeof(LocationSummaryComponentViewExtensionPoint))]
    public class LocationSummaryComponent : ApplicationComponent
    {
        private LocationSummary _selectedLocation;
        private LocationTable _locationTable;
        private CrudActionModel _locationActionHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationSummaryComponent()
        {
        }

        public override void Start()
        {
            //_locationAdminService.LocationChanged += LocationChangedEventHandler;

            _locationTable = new LocationTable();
            _locationActionHandler = new CrudActionModel();
            _locationActionHandler.Add.SetClickHandler(AddLocation);
            _locationActionHandler.Edit.SetClickHandler(UpdateSelectedLocation);
            _locationActionHandler.Add.Enabled = true;
            _locationActionHandler.Delete.Enabled = false;

            base.Start();
        }

        public override void Stop()
        {
            //_locationAdminService.LocationChanged -= LocationChangedEventHandler;
            
            base.Stop();
        }

        //TODO: LocationChangedEventHandler
        //private void LocationChangedEventHandler(object sender, EntityChangeEventArgs e)
        //{
        //    // check if the location with this oid is in the list
        //    int index = _locationTable.Items.FindIndex(delegate(Location loc) { return e.EntityRef.RefersTo(loc); });
        //    if (index > -1)
        //    {
        //        if (e.ChangeType == EntityChangeType.Update)
        //        {
        //            Location loc = _locationAdminService.LoadLocation((EntityRef<Location>)e.EntityRef);
        //            _locationTable.Items[index] = loc;
        //        }
        //        else if (e.ChangeType == EntityChangeType.Delete)
        //        {
        //            _locationTable.Items.RemoveAt(index);
        //        }
        //    }
        //    else
        //    {
        //        if (e.ChangeType == EntityChangeType.Create)
        //        {
        //            Location loc = _locationAdminService.LoadLocation((EntityRef<Location>)e.EntityRef);
        //            if (loc != null)
        //                _locationTable.Items.Add(loc);
        //        }
        //    }
        //}

        #region Presentation Model

        public ITable Locations
        {
            get { return _locationTable; }
        }

        public ActionModelNode LocationListActionModel
        {
            get { return _locationActionHandler; }
        }

        public ISelection SelectedLocation
        {
            get { return _selectedLocation == null ? Selection.Empty : new Selection(_selectedLocation); }
            set
            {
                _selectedLocation = (LocationSummary)value.Item;
                LocationSelectionChanged();
            }
        }

        public void AddLocation()
        {
            LocationEditorComponent editor = new LocationEditorComponent();
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(
                this.Host.DesktopWindow, editor, SR.TitleAddLocation);
        }

        public void UpdateSelectedLocation()
        {
            // can occur if user double clicks while holding control
            if (_selectedLocation == null) return;

            LocationEditorComponent editor = new LocationEditorComponent(_selectedLocation.LocationRef);
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(
                this.Host.DesktopWindow, editor, SR.TitleUpdateLocation);
        }

        public void LoadLocationTable()
        {
            try
            {
                Platform.GetService<ILocationAdminService>(
                    delegate(ILocationAdminService service)
                    {
                        ListAllLocationsResponse response = service.ListAllLocations(new ListAllLocationsRequest(false));
                        if (response.Locations != null)
                        {
                            _locationTable.Items.Clear();
                            _locationTable.Items.AddRange(response.Locations);
                        }
                    });
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        #endregion

        private void LocationSelectionChanged()
        {
            if (_selectedLocation != null)
                _locationActionHandler.Edit.Enabled = true;
            else
                _locationActionHandler.Edit.Enabled = false;
        }

    }
}
