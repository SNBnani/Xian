using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common.Admin.PatientAdmin;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Ris.Client.Adt
{
    /// <summary>
    /// Extension point for views onto <see cref="PatientComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class PatientOverviewComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    [ExtensionPoint]
    public class PatientOverviewToolExtensionPoint : ExtensionPoint<ITool>
    {
    }

    public interface IPatientOverviewToolContext : IToolContext
    {
        EntityRef PatientProfile { get; }
        IDesktopWindow DesktopWindow { get; }
    }

    public class AlertListItem
    {
        public AlertListItem(string name, string message, string icon)
        {
            this.Name = name;
            this.Message = message;
            this.Icon = icon;
        }

        public string Name;
        public string Message;
        public string Icon;
    }

    /// <summary>
    /// PatientComponent class
    /// </summary>
    [AssociateView(typeof(PatientOverviewComponentViewExtensionPoint))]
    public class PatientOverviewComponent : ApplicationComponent
    {
        class PatientOverviewToolContext : ToolContext, IPatientOverviewToolContext
        {
            private PatientOverviewComponent _component;

            internal PatientOverviewToolContext(PatientOverviewComponent component)
            {
                _component = component;
            }

            public EntityRef PatientProfile
            {
                get { return _component._profileRef; }
            }

            public IDesktopWindow DesktopWindow
            {
                get { return _component.Host.DesktopWindow; }
            }
        }

        private EntityRef _profileRef;
        private PatientProfileDetail _patientProfile;
        private List<AlertNotificationDetail> _alertNotifications;
        private bool _hasReconciliationCandidates;

        private ToolSet _toolSet;
        private ResourceResolver _resourceResolver;

        /// <summary>
        /// Constructor
        /// </summary>
        public PatientOverviewComponent(
            EntityRef profileRef, 
            PatientProfileDetail patientProfile, 
            List<AlertNotificationDetail> alertNotifications,
            bool hasReconciliationCandidates)
        {
            _profileRef = profileRef;
            _patientProfile = patientProfile;
            _alertNotifications = alertNotifications;
            _hasReconciliationCandidates = hasReconciliationCandidates;

            _resourceResolver = new ResourceResolver(this.GetType().Assembly);
        }

        public override void Start()
        {
            _toolSet = new ToolSet(new PatientOverviewToolExtensionPoint(), new PatientOverviewToolContext(this));

            base.Start();
        }

        public override void Stop()
        {
            _toolSet.Dispose();
            base.Stop();
        }

        public override IActionSet ExportedActions
        {
            get { return _toolSet.Actions; }
        }

        #region Presentation Model

        public string Name
        {
            //TODO: PersonNameDetail formatting
            get { return String.Format("{0}, {1}", _patientProfile.Name.FamilyName, _patientProfile.Name.GivenName); }
        }

        public string Mrn
        {
            get { return String.Format("Mrn: {0} {1}", _patientProfile.MrnAssigningAuthority, _patientProfile.Mrn); }
        }

        public string HealthCard
        {
            get { return _patientProfile == null ? "" : 
                String.Format("Healthcard: {0} {1} {2}", _patientProfile.HealthcardAssigningAuthority, _patientProfile.Healthcard, _patientProfile.HealthcardVC); }
        }

        public string AgeSex
        {
            get 
            {
                if (_patientProfile.DeathIndicator)
                {
                    TimeSpan age = _patientProfile.TimeOfDeath.Value.Subtract(_patientProfile.DateOfBirth);
                    return String.Format("Age/Sex: {0} ({1}) Deceased", (int)age.Days / 365, _patientProfile.Sex.Value);
                }
                else
                {
                    TimeSpan age = Platform.Time.Date.Subtract(_patientProfile.DateOfBirth);
                    return String.Format("Age/Sex: {0} ({1})", (int)age.Days / 365, _patientProfile.Sex.Value);
                }
            }
        }

        public string DateOfBirth
        {
            get { return String.Format("DOB: {0}", Format.Date(_patientProfile.DateOfBirth)); }
        }

        public ResourceResolver ResourceResolver
        {
            get { return _resourceResolver; }
        }

        public string PatientImage
        {
            get { return "AlertMessenger.png"; }
        }

        public List<AlertListItem> AlertList
        {
            get 
            {
                List<AlertListItem> alertListItems = new List<AlertListItem>();

                foreach (AlertNotificationDetail detail in _alertNotifications)
                {
                    alertListItems.Add(new AlertListItem(detail.Type, GetAlertTooltip(detail), GetAlertIcon(detail)));
                }

                // Display Reconciliation Alert as one of the Alert List Items
                if (_hasReconciliationCandidates)
                {
                    AlertNotificationDetail detail = new AlertNotificationDetail();
                    detail.Type = "Reconciliation Alert";
                    alertListItems.Add(new AlertListItem(detail.Type, GetAlertTooltip(detail), GetAlertIcon(detail)));
                }

                return alertListItems;
            }
        }

        #endregion

        private string GetAlertIcon(AlertNotificationDetail detail)
        {
            string icon = "";

            switch (detail.Type)
            {
                case "Note Alert":
                    icon = "AlertPen.png";
                    break;
                case "Language Alert":
                    icon = "AlertWorld.png";
                    break;
                case "Reconciliation Alert":
                    icon = "AlertMessenger.png";
                    break;
                case "Schedule Alert":
                    icon = "AlertClock.png";
                    break;
                default:
                    icon = "AlertMessenger.png";
                    break;
            }

            return icon;
        }

        private string GetAlertTooltip(AlertNotificationDetail detail)
        {
            string alertTooltip = "";
            string patientName = String.Format("{0}. {1}"
                , _patientProfile.Name.GivenName.Substring(0, 1)
                , _patientProfile.Name.FamilyName);

            switch (detail.Type)
            {
                case "Note Alert":
                    alertTooltip = String.Format("{0} has high severity notes: {1}"
                        , patientName
                        , StringUtilities.Combine<string>(detail.Reasons, "\r\n"));
                    break;
                case "Language Alert":
                    alertTooltip = String.Format("{0} speaks: {1}"
                        , patientName
                        , StringUtilities.Combine<string>(detail.Reasons, ", "));
                    break;
                case "Reconciliation Alert":
                    alertTooltip = String.Format(SR.MessageUnreconciledRecordsAlert, _patientProfile.Name.GivenName.Substring(0, 1), _patientProfile.Name.FamilyName);
                    break;
                default:
                    break;
            }

            return alertTooltip;
        }

        public void ShowPatientDemographicsDialog()
        {
            //TODO: ShowPatientDemographicsDialog
            // This is to illustrate the concept only, eventually we want to show some dialog/form other than the editor
            ApplicationComponent.LaunchAsDialog(
                this.Host.DesktopWindow,
                new PatientProfileEditorComponent(_profileRef),
                SR.TitleEditPatient);
        }

    }
}
