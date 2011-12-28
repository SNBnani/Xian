#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0//

#endregion

// This file is auto-generated by the ClearCanvas.Model.SqlServer.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Xml;
    using ClearCanvas.Dicom;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class Study: ServerEntity
    {
        #region Constructors
        public Study():base("Study")
        {}
        public Study(
             String _studyInstanceUid_
            ,ServerEntityKey _serverPartitionKey_
            ,ServerEntityKey _patientKey_
            ,Int32 _numberOfStudyRelatedSeries_
            ,Int32 _numberOfStudyRelatedInstances_
            ,Decimal _studySizeInKB_
            ,String _responsiblePerson_
            ,String _responsibleOrganization_
            ,XmlDocument _keyXml_
            ,String _specificCharacterSet_
            ,ServerEntityKey _studyStorageKey_
            ,String _patientsName_
            ,String _patientId_
            ,String _issuerOfPatientId_
            ,String _patientsBirthDate_
            ,String _patientsAge_
            ,String _patientsSex_
            ,String _studyDate_
            ,String _studyTime_
            ,String _accessionNumber_
            ,String _studyId_
            ,String _studyDescription_
            ,String _referringPhysiciansName_
            ):base("Study")
        {
            StudyInstanceUid = _studyInstanceUid_;
            ServerPartitionKey = _serverPartitionKey_;
            PatientKey = _patientKey_;
            NumberOfStudyRelatedSeries = _numberOfStudyRelatedSeries_;
            NumberOfStudyRelatedInstances = _numberOfStudyRelatedInstances_;
            StudySizeInKB = _studySizeInKB_;
            ResponsiblePerson = _responsiblePerson_;
            ResponsibleOrganization = _responsibleOrganization_;
            KeyXml = _keyXml_;
            SpecificCharacterSet = _specificCharacterSet_;
            StudyStorageKey = _studyStorageKey_;
            PatientsName = _patientsName_;
            PatientId = _patientId_;
            IssuerOfPatientId = _issuerOfPatientId_;
            PatientsBirthDate = _patientsBirthDate_;
            PatientsAge = _patientsAge_;
            PatientsSex = _patientsSex_;
            StudyDate = _studyDate_;
            StudyTime = _studyTime_;
            AccessionNumber = _accessionNumber_;
            StudyId = _studyId_;
            StudyDescription = _studyDescription_;
            ReferringPhysiciansName = _referringPhysiciansName_;
        }
        #endregion

        #region Public Properties
        [DicomField(DicomTags.StudyInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyInstanceUid")]
        public String StudyInstanceUid
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="ServerPartitionGUID")]
        public ServerEntityKey ServerPartitionKey
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientGUID")]
        public ServerEntityKey PatientKey
        { get; set; }
        [DicomField(DicomTags.NumberOfStudyRelatedSeries, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="NumberOfStudyRelatedSeries")]
        public Int32 NumberOfStudyRelatedSeries
        { get; set; }
        [DicomField(DicomTags.NumberOfStudyRelatedInstances, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="NumberOfStudyRelatedInstances")]
        public Int32 NumberOfStudyRelatedInstances
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudySizeInKB")]
        public Decimal StudySizeInKB
        { get; set; }
        [DicomField(DicomTags.ResponsiblePerson, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="ResponsiblePerson")]
        public String ResponsiblePerson
        { get; set; }
        [DicomField(DicomTags.ResponsibleOrganization, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="ResponsibleOrganization")]
        public String ResponsibleOrganization
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="GUIDXml")]
        public XmlDocument KeyXml
        { get; set; }
        [DicomField(DicomTags.SpecificCharacterSet, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="SpecificCharacterSet")]
        public String SpecificCharacterSet
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyStorageGUID")]
        public ServerEntityKey StudyStorageKey
        { get; set; }
        [DicomField(DicomTags.PatientsName, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsName")]
        public String PatientsName
        { get; set; }
        [DicomField(DicomTags.PatientId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientId")]
        public String PatientId
        { get; set; }
        [DicomField(DicomTags.IssuerOfPatientId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="IssuerOfPatientId")]
        public String IssuerOfPatientId
        { get; set; }
        [DicomField(DicomTags.PatientsBirthDate, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsBirthDate")]
        public String PatientsBirthDate
        { get; set; }
        [DicomField(DicomTags.PatientsAge, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsAge")]
        public String PatientsAge
        { get; set; }
        [DicomField(DicomTags.PatientsSex, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsSex")]
        public String PatientsSex
        { get; set; }
        [DicomField(DicomTags.StudyDate, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyDate")]
        public String StudyDate
        { get; set; }
        [DicomField(DicomTags.StudyTime, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyTime")]
        public String StudyTime
        { get; set; }
        [DicomField(DicomTags.AccessionNumber, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="AccessionNumber")]
        public String AccessionNumber
        { get; set; }
        [DicomField(DicomTags.StudyId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyId")]
        public String StudyId
        { get; set; }
        [DicomField(DicomTags.StudyDescription, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyDescription")]
        public String StudyDescription
        { get; set; }
        [DicomField(DicomTags.ReferringPhysiciansName, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="ReferringPhysiciansName")]
        public String ReferringPhysiciansName
        { get; set; }
        #endregion

        #region Static Methods
        static public Study Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public Study Load(IPersistenceContext read, ServerEntityKey key)
        {
            IStudyEntityBroker broker = read.GetBroker<IStudyEntityBroker>();
            Study theObject = broker.Load(key);
            return theObject;
        }
        static public Study Insert(Study entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                Study newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public Study Insert(IUpdateContext update, Study entity)
        {
            IStudyEntityBroker broker = update.GetBroker<IStudyEntityBroker>();
            StudyUpdateColumns updateColumns = new StudyUpdateColumns();
            updateColumns.StudyInstanceUid = entity.StudyInstanceUid;
            updateColumns.ServerPartitionKey = entity.ServerPartitionKey;
            updateColumns.PatientKey = entity.PatientKey;
            updateColumns.NumberOfStudyRelatedSeries = entity.NumberOfStudyRelatedSeries;
            updateColumns.NumberOfStudyRelatedInstances = entity.NumberOfStudyRelatedInstances;
            updateColumns.StudySizeInKB = entity.StudySizeInKB;
            updateColumns.ResponsiblePerson = entity.ResponsiblePerson;
            updateColumns.ResponsibleOrganization = entity.ResponsibleOrganization;
            updateColumns.KeyXml = entity.KeyXml;
            updateColumns.SpecificCharacterSet = entity.SpecificCharacterSet;
            updateColumns.StudyStorageKey = entity.StudyStorageKey;
            updateColumns.PatientsName = entity.PatientsName;
            updateColumns.PatientId = entity.PatientId;
            updateColumns.IssuerOfPatientId = entity.IssuerOfPatientId;
            updateColumns.PatientsBirthDate = entity.PatientsBirthDate;
            updateColumns.PatientsAge = entity.PatientsAge;
            updateColumns.PatientsSex = entity.PatientsSex;
            updateColumns.StudyDate = entity.StudyDate;
            updateColumns.StudyTime = entity.StudyTime;
            updateColumns.AccessionNumber = entity.AccessionNumber;
            updateColumns.StudyId = entity.StudyId;
            updateColumns.StudyDescription = entity.StudyDescription;
            updateColumns.ReferringPhysiciansName = entity.ReferringPhysiciansName;
            Study newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
