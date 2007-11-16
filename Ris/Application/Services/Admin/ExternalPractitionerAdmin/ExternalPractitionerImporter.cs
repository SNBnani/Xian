#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core.Modelling;

namespace ClearCanvas.Ris.Application.Services.Admin.ExternalPractitionerAdmin
{
    [ExtensionOf(typeof(DataImporterExtensionPoint), Name = "External Practitioner Importer")]
    [ExtensionOf(typeof(ApplicationRootExtensionPoint))]
    public class ExternalPractitionerImporter : DataImporterBase
    {
        private const int _numFields = 25;

        IPersistenceContext _context;

        #region DataImporterBase overrides

        public override bool SupportsCsv
        {
            get { return true; }
        }

        /// <summary>
        /// Import external practitioner from CSV format.
        /// </summary>
        /// <param name="lines">
        /// Each string in the list must contain 25 CSV fields, as follows:
        ///     0 - FamilyName
        ///     1 - GivenName
        ///     2 - MiddleName
        ///     3 - Prefix
        ///     4 - Suffix
        ///     5 - Degree
        ///     6 - LicenseNumberId
        ///     7 - LicenseNumberAssigningAuthority
        ///     8 - Street
        ///     9 - Unit
        ///     10 - City
        ///     11 - Province
        ///     12 - PostalCode
        ///     13 - Country
        ///     14 - ValidFrom
        ///     15 - ValidUntil
        ///     16 - Phone CountryCode
        ///     17 - Phone AreaCode
        ///     18 - Phone Number
        ///     19 - Phone Extension
        ///     20 - ValidFrom
        ///     21 - ValidUntil
        ///     22 - Fax CountryCode
        ///     23 - Fax AreaCode
        ///     24 - Fax Number
        ///     25 - Fax Extension
        ///     26 - ValidFrom
        ///     27 - ValidUntil
        /// </param>
        /// <param name="context"></param>
        public override void ImportCsv(List<string> rows, IUpdateContext context)
        {
            _context = context;

            List<ExternalPractitioner> importedEPs = new List<ExternalPractitioner>();

            foreach (string row in rows)
            {
                string[] fields = ParseCsv(row, _numFields);

                string epFamilyName = fields[0];
                string epGivenName = fields[1];
                string epMiddlename = fields[2];
                string epPrefix = fields[3];
                string epSuffix = fields[4];
                string epDegree = fields[5];

                string epLicenseId = fields[6];
                string epLicenseAssigningAuthority = fields[7];

                string addressStreet = fields[8];
                string addressUnit = fields[9];
                string addressCity = fields[10];
                string addressProvince = fields[11];
                string addressPostalCode = fields[12];
                string addressCountry = fields[13];

                DateTime? addressValidFrom = ParseDateTime(fields[14]);
                DateTime? addressValidUntil = ParseDateTime(fields[15]);

                string phoneCountryCode = fields[16];
                string phoneAreaCode = fields[17];
                string phoneNumber = fields[18];
                string phoneExtension = fields[19];
                DateTime? phoneValidFrom = ParseDateTime(fields[20]);
                DateTime? phoneValidUntil = ParseDateTime(fields[21]);

                string faxCountryCode = fields[22];
                string faxAreaCode = fields[23];
                string faxNumber = fields[24];
                string faxExtension = fields[25];
                DateTime? faxValidFrom = ParseDateTime(fields[26]);
                DateTime? faxValidUntil = ParseDateTime(fields[27]);

                PractitionerLicenseAuthorityEnum licenseAuth = context.GetBroker<IEnumBroker>().Find<PractitionerLicenseAuthorityEnum>(epLicenseAssigningAuthority);
                PractitionerLicenseNumber licenseNumber = new PractitionerLicenseNumber(epLicenseId, licenseAuth);

                ExternalPractitioner ep = GetExternalPracitioner(licenseNumber, importedEPs);

                if (ep == null)
                {
                    ep = new ExternalPractitioner();
                    ep.LicenseNumber = licenseNumber;

                    ep.Name = new PersonName(epFamilyName, epGivenName, epMiddlename, epPrefix, epSuffix, epDegree);


                    try
                    {
                        Address epAddress = new Address(
                            addressStreet,
                            addressUnit,
                            addressCity,
                            addressProvince,
                            addressPostalCode,
                            addressCountry,
                            AddressType.B,
                            new DateTimeRange(addressValidFrom, addressValidUntil));
                        Validation.Validate(epAddress);
                        ep.Addresses.Add(epAddress);
                    }
                    catch(EntityValidationException) { /* invalid address - ignore */ }


                    try
                    {
                        TelephoneNumber epTelephone = new TelephoneNumber(
                            phoneCountryCode,
                            phoneAreaCode,
                            phoneNumber,
                            phoneExtension,
                            TelephoneUse.WPN,
                            TelephoneEquipment.PH,
                            new DateTimeRange(phoneValidFrom, phoneValidUntil));

                        Validation.Validate(epTelephone);
                        ep.TelephoneNumbers.Add(epTelephone);
                    }
                    catch (EntityValidationException) { /* invalid phone - ignore */ }

                    try
                    {
                        TelephoneNumber epFax = new TelephoneNumber(
                            faxCountryCode,
                            faxAreaCode,
                            faxNumber,
                            faxExtension,
                            TelephoneUse.WPN,
                            TelephoneEquipment.FX,
                            new DateTimeRange(faxValidFrom, faxValidUntil));

                        Validation.Validate(epFax);
                        ep.TelephoneNumbers.Add(epFax);

                    }
                    catch (EntityValidationException) { /* invalid phone - ignore */ }

                    _context.Lock(ep, DirtyState.New);

                    importedEPs.Add(ep);
                }
            }
        }

        #endregion

        #region Private Methods

        private ExternalPractitioner GetExternalPracitioner(PractitionerLicenseNumber license, List<ExternalPractitioner> importedEPs)
        {
            // if licenseId is not supplied, then assume the record does not exist
            if (string.IsNullOrEmpty(license.Id))
                return null;

            ExternalPractitioner externalPractitioner = null;

            externalPractitioner = CollectionUtils.SelectFirst<ExternalPractitioner>(importedEPs,
                delegate(ExternalPractitioner ep) { return Equals(ep.LicenseNumber, license); });

            if (externalPractitioner == null)
            {
                ExternalPractitionerSearchCriteria criteria = new ExternalPractitionerSearchCriteria();
                criteria.LicenseNumber.Id.EqualTo(license.Id);
                criteria.LicenseNumber.AssigningAuthority.EqualTo(license.AssigningAuthority);

                IExternalPractitionerBroker broker = _context.GetBroker<IExternalPractitionerBroker>();
                externalPractitioner = CollectionUtils.FirstElement<ExternalPractitioner>(broker.Find(criteria));
            }

            return externalPractitioner;
        }

        private DateTime? ParseDateTime(string p)
        {
            DateTime? dt;

            try
            {
                dt = DateTime.Parse(p);
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        #endregion
    }
}
