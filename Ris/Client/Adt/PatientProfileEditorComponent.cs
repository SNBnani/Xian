using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Common;
using ClearCanvas.Desktop.Validation;
using ClearCanvas.Ris.Client;

namespace ClearCanvas.Ris.Client.Adt
{
    public class PatientProfileEditorComponent : NavigatorComponentContainer
    {
        private EntityRef<PatientProfile> _profileRef;
        private PatientProfile _profile;
        private bool _isNew;
        private IAdtService _adtService;

        private PatientProfileDetailsEditorComponent _patientEditor;
        private AddressesSummaryComponent _addressesSummary;
        private PhoneNumbersSummaryComponent _phoneNumbersSummary;
        private EmailAddressesSummaryComponent _emailAddressesSummary;
        private ContactPersonsSummaryComponent _contactPersonsSummary;
        private PatientProfileAdditionalInfoEditorComponent _additionalPatientInfoSummary;


        /// <summary>
        /// Constructs an editor to edit the specified profile
        /// </summary>
        /// <param name="profileRef"></param>
        public PatientProfileEditorComponent(EntityRef<PatientProfile> profileRef)
        {
            _profileRef = profileRef;
            _isNew = false;
        }

        /// <summary>
        /// Constructs an editor to edit a new profile
        /// </summary>
        public PatientProfileEditorComponent()
        {
            _isNew = true;
        }

        public EntityRef<PatientProfile> PatientProfile
        {
            get { return _profileRef; }
        }

        public override void Start()
        {
            _adtService = ApplicationContext.GetService<IAdtService>();

            if (_isNew)
            {
                _profile = new PatientProfile();
                _profile.Mrn.AssigningAuthority = "UHN";    // TODO remove this hack
                _profile.Healthcard.AssigningAuthority = "Ontario";    // TODO remove this hack
            }
            else
            {
                _profile = _adtService.LoadPatientProfile(_profileRef, true);
                this.Host.SetTitle(
                    string.Format(SR.TitlePatientComponent,
                    Format.Custom(_profile.Name),
                    Format.Custom(_profile.Mrn)));
            }


            this.Pages.Add(new NavigatorPage("Patient", _patientEditor = new PatientProfileDetailsEditorComponent()));
            this.Pages.Add(new NavigatorPage("Patient/Addresses", _addressesSummary = new AddressesSummaryComponent()));
            this.Pages.Add(new NavigatorPage("Patient/Phone Numbers", _phoneNumbersSummary = new PhoneNumbersSummaryComponent()));
            this.Pages.Add(new NavigatorPage("Patient/Email Addresses", _emailAddressesSummary = new EmailAddressesSummaryComponent()));
            this.Pages.Add(new NavigatorPage("Patient/Contact Persons", _contactPersonsSummary = new ContactPersonsSummaryComponent()));
            this.Pages.Add(new NavigatorPage("Patient/Additional Info", _additionalPatientInfoSummary = new PatientProfileAdditionalInfoEditorComponent()));

            this.ValidationStrategy = new AllNodesContainerValidationStrategy();

            _patientEditor.Subject = _profile;
            _addressesSummary.Subject = _profile.Addresses;
            _phoneNumbersSummary.Subject = _profile.TelephoneNumbers;
            _emailAddressesSummary.Subject = _profile.EmailAddresses;
            _contactPersonsSummary.Subject = _profile.ContactPersons;
            _additionalPatientInfoSummary.Subject = _profile;

            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Accept()
        {
            if (this.HasValidationErrors)
            {
                this.ShowValidation(true);
            }
            else
            {
                try
                {
                    SaveChanges();
                    this.ExitCode = ApplicationComponentExitCode.Normal;
                }
                catch (ConcurrencyException e)
                {
                    ExceptionHandler.Report(e, SR.ExceptionConcurrencyPatientNotSaved, this.Host.DesktopWindow);
                    this.ExitCode = ApplicationComponentExitCode.Error;
                }
                catch (Exception e)
                {
                    ExceptionHandler.Report(e, SR.ExceptionFailedToSave, this.Host.DesktopWindow);
                    this.ExitCode = ApplicationComponentExitCode.Error;
                }
                this.Host.Exit();
            }
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        private void SaveChanges()
        {
            if (_isNew)
            {
                _adtService.CreatePatientForProfile(_profile);
                _profileRef = new EntityRef<PatientProfile>(_profile);
            }
            else
            {
                _adtService.UpdatePatientProfile(_profile);
            }
        }

    }
}
