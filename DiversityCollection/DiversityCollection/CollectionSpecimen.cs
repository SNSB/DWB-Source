using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DiversityCollection
{
    public class CollectionSpecimen : DiversityWorkbench.CollectionSpecimen
    {
        #region Parameter

        public enum AvailabilityState { Unknown, NotAvailable, ReadOnly, Available };

        #region Data

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimenImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAgent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProject;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterReference;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelation;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterPart;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProcessing;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterTransaction;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnit;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterIdentification;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAnalysis;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnitInPart;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterGeography;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterGeographyList;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisationSystem;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterHabitat;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterExpedition;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterExpeditionEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterExpeditionSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterExpeditionUnit;

        public static string SqlSpecimen = "SELECT CollectionSpecimenID, Version, CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, " +
                      "AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, " +
                      "LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, " +
                      "Problems, DataWithholdingReason, InternalNotes, ExternalDatasourceID, ExternalIdentifier " +
                      "FROM CollectionSpecimen ";

        public static string SqlSpecimenImage = "SELECT CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, LogCreatedWhen, Description, Title, " +
                      "IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LicenseNotes, LicenseURI, DisplayOrder " +
                      "FROM CollectionSpecimenImage";

        public static string SqlSpecimenImageProperty = "SELECT CollectionSpecimenID, URI, Property, Description " +
                      "FROM CollectionSpecimenImageProperty";

        public static string SqlSpecimenImageOldVersion = "SELECT CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, LogCreatedWhen, Description " +
                      "FROM CollectionSpecimenImage";


        public static string SqlAgent = "SELECT CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason " +
                      "FROM CollectionAgent";
        public static string SqlProject = "SELECT CollectionSpecimenID, ProjectID " +
                      "FROM CollectionProject";
        public static string SqlReference = "SELECT CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, IdentificationSequence, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI " +
                      "FROM CollectionSpecimenReference";
        public static string SqlRelation = "SELECT CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, " +
                      "RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID " +
                      "FROM CollectionSpecimenRelation";

        public static string SqlRelationInvers = "SELECT CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, " +
                      "RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID " +
                      "FROM CollectionSpecimenRelation";

        public static string SqlPart = "SELECT CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, PartSublabel, " +
                      "CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, ResponsibleName, ResponsibleAgentURI, DataWithholdingReason " +
                      "FROM CollectionSpecimenPart";
        public static string SqlProcessing = "SELECT CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, /*ToolUsage,*/ SpecimenProcessingID " +
                      "FROM CollectionSpecimenProcessing";
        public static string SqlProcessingMethod = "SELECT CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, MethodMarker " +
                      "FROM CollectionSpecimenProcessingMethod";
        public static string SqlProcessingMethodList = "SELECT T.CollectionSpecimenID, T.SpecimenProcessingID, T.MethodID, T.ProcessingID, M.DisplayText + ' ' + T.MethodMarker AS DisplayText, MethodMarker " +
                    "FROM CollectionSpecimenProcessingMethod AS T INNER JOIN " +
                    "Method AS M ON T.MethodID = M.MethodID INNER JOIN " +
                    "CollectionSpecimenProcessing AS S ON T.CollectionSpecimenID = S.CollectionSpecimenID AND T.SpecimenProcessingID = S.SpecimenProcessingID";
        public static string SqlProcessingMethodParameter = "SELECT CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, MethodMarker " +
                      "FROM CollectionSpecimenProcessingMethodParameter";
        public static string SqlProcessingMethodParameterList = "SELECT T.CollectionSpecimenID, T.SpecimenProcessingID, T.ProcessingID, T.MethodID, T.ParameterID, T.Value, P.DisplayText, T.MethodMarker " +
                    "FROM CollectionSpecimenProcessingMethodParameter AS T INNER JOIN " +
                    "CollectionSpecimenProcessing AS S ON T.CollectionSpecimenID = S.CollectionSpecimenID AND T.SpecimenProcessingID = S.SpecimenProcessingID INNER JOIN " +
                    "Parameter AS P ON T.ParameterID = P.ParameterID AND T.MethodID = P.MethodID";
        
        public static string SqlTransaction = "SELECT CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, TransactionReturnID, TransactionTitle " +
                      "FROM CollectionSpecimenTransaction";

        public static string SqlPartDescription = "SELECT CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, DescriptionHierarchyCache, Notes, IdentificationUnitID " +
                      "FROM CollectionSpecimenPartDescription";

        public static string SqlPartRegulation = "SELECT CollectionSpecimenID, SpecimenPartID, Regulation " +
                      "FROM CollectionSpecimenPartRegulation";

        public static string SqlUnit = "SELECT CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, HierarchyCache, TaxonomicGroup, OnlyObserved, " +
                      "RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, " +
                      "DisplayOrder, Notes, ParentUnitID, DataWithholdingReason, RetrievalType, NumberOfUnitsModifier " +
                      "FROM IdentificationUnit";
        public static string SqlUnitInPart = "SELECT CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder, Description " +
                      "FROM IdentificationUnitInPart";
        public static string SqlIdentification = "SELECT CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear,  " +
                      "IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier,  " +
                      "TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, TermURI, DependsOnIdentificationSequence  " +
                      "FROM Identification";
        public static string SqlAnalysis = "SELECT CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, " +
                      "ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes /*, ToolUsage*/ " +
                      "FROM IdentificationUnitAnalysis";
        public static string SqlGeoAnalysis = "SELECT CollectionSpecimenID, IdentificationUnitID, AnalysisDate, " +
                      "ResponsibleName, ResponsibleAgentURI, Notes " +
                      "FROM IdentificationUnitGeoAnalysis";
        public static string SqlAnalysisMethod = "SELECT CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, MethodMarker " +
             "FROM IdentificationUnitAnalysisMethod";
        public static string SqlAnalysisMethodList = "SELECT IdentificationUnitAnalysisMethod.CollectionSpecimenID, IdentificationUnitAnalysisMethod.IdentificationUnitID, M.MethodID, " +
            "IdentificationUnitAnalysisMethod.AnalysisID, IdentificationUnitAnalysisMethod.AnalysisNumber, IdentificationUnitAnalysisMethod.MethodMarker, M.DisplayText + ' ' + IdentificationUnitAnalysisMethod.MethodMarker AS DisplayText " +
            "FROM IdentificationUnitAnalysisMethod INNER JOIN " +
            "Method AS M ON IdentificationUnitAnalysisMethod.MethodID = M.MethodID ";
        public static string SqlAnalysisMethodParameter = "SELECT CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, MethodMarker, Value " +
            "FROM IdentificationUnitAnalysisMethodParameter";
        public static string SqlAnalysisMethodParameterList = "SELECT IdentificationUnitAnalysisMethodParameter.CollectionSpecimenID, IdentificationUnitAnalysisMethodParameter.IdentificationUnitID, " +
            "IdentificationUnitAnalysisMethodParameter.AnalysisID, IdentificationUnitAnalysisMethodParameter.AnalysisNumber, " +
            "IdentificationUnitAnalysisMethodParameter.MethodID, IdentificationUnitAnalysisMethodParameter.ParameterID, IdentificationUnitAnalysisMethodParameter.MethodMarker, IdentificationUnitAnalysisMethodParameter.Value, " +
            "P.DisplayText " +
            "FROM IdentificationUnitAnalysisMethodParameter INNER JOIN " +
            "Parameter AS P ON IdentificationUnitAnalysisMethodParameter.ParameterID = P.ParameterID AND IdentificationUnitAnalysisMethodParameter.MethodID = P.MethodID";

        public static string SqlEvent = "SELECT CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, CollectionTime, CollectionDay, CollectionMonth, CollectionYear,  " +
                      "CollectionEndDay, CollectionEndMonth, CollectionEndYear,  " +
                      "CollectionTimeSpan, CollectionDateSupplement, CollectionDateCategory, LocalityDescription, LocalityVerbatim, HabitatDescription, ReferenceTitle, ReferenceURI, ReferenceDetails,  " +
                      "CollectingMethod, Notes, CountryCache, DataWithholdingReason, DataWithholdingReasonDate " +
                      "FROM  CollectionEvent";
        public static string SqlEventImage = "SELECT CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, Title, " +
                      "IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear " +
                      "FROM CollectionEventImage";
        public static string SqlEventImageOldVersion = "SELECT CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason " +
                      "FROM CollectionEventImage";

        public static string SqlEventRegulation = "SELECT CollectionEventID, Regulation, TransactionID " +
                      "FROM CollectionEventRegulation";

        public static string SqlEventLocalisation = "SELECT CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation,  " +
                      "DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RecordingMethod " +
                      "FROM  CollectionEventLocalisation";
        public static string SqlLocalisationSystem = "SELECT LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, MeasurementUnit, DefaultAccuracyOfLocalisation, DiversityModule,  " +
                      "ParsingMethod, DisplayText, DisplayEnable, DisplayOrder, Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2,  " +
                      "DescriptionLocation2 " +
                      "FROM LocalisationSystem " +
                      "WHERE (DisplayEnable = 1) " +
                      "ORDER BY DisplayOrder";
        public static string SqlEventProperty = "SELECT CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, " +
                      "AverageValueCache " +
                      "FROM CollectionEventProperty";
        public static string SqlProperty = "SELECT PropertyID, PropertyParentID, PropertySystemName, DefaultAccuracyOfProperty, DefaultMeasurementUnit, ParsingMethodName, DisplayText, " +
                      "DisplayEnabled, DisplayOrder, Description " +
                      "WHERE (DisplayEnabled = 1) " +
                      "FROM Property";
        public static string SqlCollectionEventMethod = "SELECT CollectionEventID, MethodID, MethodMarker FROM  CollectionEventMethod";
        public static string SqlCollectionEventMethodList = "SELECT E.CollectionEventID, E.MethodID, M.DisplayText + ' ' + E.MethodMarker AS DisplayText, E.MethodMarker " +
                    "FROM CollectionEventMethod AS E INNER JOIN " +
                    "Method AS M ON E.MethodID = M.MethodID ";
        public static string SqlCollectionEventMethodParameter = "SELECT CollectionEventID, MethodID, ParameterID, Value, MethodMarker FROM CollectionEventParameterValue";
        public static string SqlCollectionEventMethodParameterList = "SELECT E.CollectionEventID, E.MethodID, E.ParameterID, E.Value, P.DisplayText, E.MethodMarker FROM CollectionEventParameterValue E, "
                    + " Parameter P WHERE E.ParameterID = P.ParameterID AND E.MethodID = P.MethodID";

        public static string SqlEventSeries = "SELECT SeriesID, SeriesParentID, Description, SeriesCode, Notes, DateStart, DateEnd, DateSupplement " +
                      "FROM CollectionEventSeries";
        public static string SqlEventSeriesGeography = "SELECT SeriesID, Geography.ToString() AS GeographyAsString FROM CollectionEventSeries";
        public static string SqlEventSeriesEvent = "SELECT dbo.CollectionEvent.CollectionEventID, SeriesID, RTRIM(CONVERT(nvarchar(500), " +
            "(CASE WHEN CollectionYear IS NULL THEN '-' ELSE cast(CollectionYear AS varchar) END) + " +
            "(CASE WHEN CollectionMonth IS NULL THEN '/--' ELSE '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + cast(CollectionMonth AS varchar) END) + " +
            "(CASE WHEN CollectionDay IS NULL  THEN '/--' ELSE  '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + cast(CollectionDay AS varchar) END) + " +
            "CASE WHEN CollectionDateSupplement IS NULL THEN '' ELSE ' ' + CollectionDateSupplement END + " +
            "CASE WHEN LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0 THEN '' ELSE ' ' + LocalityDescription + ' ' END + " +
            "CASE WHEN HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0 THEN '' ELSE ' ' + HabitatDescription + ' ' END) + " +
            "CASE WHEN CollectionDay IS NULL AND CollectionMonth IS NULL  " +
            "AND CollectionYear IS NULL AND (LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0) " +
            "AND (HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0) " +
            "THEN '(ID: ' + CONVERT(nvarchar, dbo.CollectionEvent.CollectionEventID) + ')  ' ELSE '' END) " +
            "AS DisplayText, " +
            " RTRIM(CONVERT(nvarchar(50), (CASE WHEN CollectionYear IS NULL THEN '' ELSE cast(CollectionYear AS varchar) END)  + '-' " +
            " + (CASE WHEN CollectionMonth IS NULL THEN '' ELSE cast(CollectionMonth AS varchar) END) + '-' " +
            " + (CASE WHEN CollectionDay IS NULL THEN '' ELSE cast(CollectionDay AS varchar) END)  )) " +
            "AS CollectionDate " +
            "FROM  dbo.CollectionEvent ";

        public static string SqlEventSeriesSpecimen = "SELECT CollectionSpecimenID, CollectionEventID, CASE WHEN AccessionNumber IS NULL THEN '[' + CAST(CollectionSpecimenID AS VARCHAR) + ']' ELSE AccessionNumber END AS AccessionNumber " +
                      "FROM CollectionSpecimen";
        public static string SqlEventSeriesUnit = "SELECT CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, RelatedUnitID, TaxonomicGroup, Gender, UnitIdentifier, UnitDescription " +
                      "FROM IdentificationUnit";
        public static string SqlEventSeriesDescriptor = "SELECT SeriesID, DescriptorID, Descriptor, URL, DescriptorType " +
                      "FROM CollectionEventSeriesDescriptor";
        public static string SqlEventSeriesImage = "SELECT SeriesID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, Title, " +
                      "IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear " +
                      "FROM CollectionEventSeriesImage";
        public static string SqlEventSeriesImageOldVersion = "SELECT SeriesID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason " +
                      "FROM CollectionEventSeriesImage";

        public static string SqlPlotEvent = "SELECT E.CollectionEventID, L.Location2 AS PlotID, RTRIM(CONVERT(nvarchar(500), (CASE WHEN CollectionYear IS NULL THEN '-' ELSE CAST(CollectionYear AS varchar) " +
            "END) + (CASE WHEN CollectionMonth IS NULL THEN '/--' ELSE '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CAST(CollectionMonth AS varchar) END) + (CASE WHEN CollectionDay IS NULL " +
            "THEN '/--' ELSE '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(CollectionDay AS varchar) END) + CASE WHEN CollectionDateSupplement IS NULL " +
            "THEN '' ELSE ' ' + CollectionDateSupplement END + CASE WHEN LocalityDescription IS NULL OR " +
            "len(Rtrim(LocalityDescription)) = 0 THEN '' ELSE ' ' + LocalityDescription + ' ' END + CASE WHEN HabitatDescription IS NULL OR " +
            "len(Rtrim(HabitatDescription)) = 0 THEN '' ELSE ' ' + HabitatDescription + ' ' END) + CASE WHEN CollectionDay IS NULL AND CollectionMonth IS NULL AND " +
            "CollectionYear IS NULL AND (LocalityDescription IS NULL OR " +
            "len(Rtrim(LocalityDescription)) = 0) AND (HabitatDescription IS NULL OR " +
            "len(Rtrim(HabitatDescription)) = 0) THEN '(ID: ' + CONVERT(nvarchar, E.CollectionEventID) + ')  ' ELSE '' END) AS DisplayText, RTRIM(CONVERT(nvarchar(50), " +
            "(CASE WHEN CollectionYear IS NULL THEN '' ELSE CAST(CollectionYear AS varchar) END) + '-' + (CASE WHEN CollectionMonth IS NULL " +
            "THEN '' ELSE CAST(CollectionMonth AS varchar) END) + '-' + (CASE WHEN CollectionDay IS NULL THEN '' ELSE CAST(CollectionDay AS varchar) END))) " +
            "AS CollectionDate " +
            "FROM  CollectionEvent AS E INNER JOIN " +
            "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
            "WHERE (L.LocalisationSystemID = 13) AND (L.Location2 LIKE N'http:%') ";
 
#if !DEBUG
        public static string SqlCollection = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                      "Location, CollectionOwner, DisplayOrder " +
                      "FROM Collection";
#endif
#if DEBUG
        public static string SqlCollection = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                      "Location, CollectionOwner, DisplayOrder, Type " +
                      "FROM Collection";
#endif
        public static string SqlExternalIdentifier = "SELECT ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes " +
                      "FROM ExternalIdentifier";

        #endregion

        #endregion

        #region Construction
        public CollectionSpecimen(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Specimen

        public void fillSpecimen(int SpecimenID, ref DiversityCollection.Datasets.DataSetCollectionSpecimen dsSpecimen, ref DiversityCollection.Datasets.DataSetCollectionEventSeries dsEventSeries)
        {
            try
            {
                string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString();

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, dsSpecimen.CollectionSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAgent, dsSpecimen.CollectionAgent, DiversityCollection.CollectionSpecimen.SqlAgent + WhereClause + " ORDER BY CollectorsSequence", DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProject, dsSpecimen.CollectionProject, DiversityCollection.CollectionSpecimen.SqlProject + WhereClause + " ORDER BY ProjectID", DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterReference, dsSpecimen.CollectionSpecimenReference, DiversityCollection.CollectionSpecimen.SqlReference + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterRelation, dsSpecimen.CollectionSpecimenRelation, DiversityCollection.CollectionSpecimen.SqlRelation + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, dsSpecimen.CollectionSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause, DiversityWorkbench.Settings.ConnectionString);

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, dsSpecimen.CollectionSpecimenPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProcessing, dsSpecimen.CollectionSpecimenProcessing, DiversityCollection.CollectionSpecimen.SqlProcessing + WhereClause + " ORDER BY ProcessingDate, ProcessingID", DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterTransaction, dsSpecimen.CollectionSpecimenTransaction, DiversityCollection.CollectionSpecimen.SqlTransaction + WhereClause + " ORDER BY TransactionID", DiversityWorkbench.Settings.ConnectionString);

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, dsSpecimen.IdentificationUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                // Temporary change until version of database is
                // TODO - remove after update to new version of database
                string Version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.Version()");
                if (Version == "02.06.21")
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, dsSpecimen.Identification, DiversityCollection.CollectionSpecimen.SqlIdentification.Replace(", DependsOnIdentificationSequence", "") + WhereClause + " ORDER BY IdentificationSequence", DiversityWorkbench.Settings.ConnectionString);
                else
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, dsSpecimen.Identification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + " ORDER BY IdentificationSequence", DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, dsSpecimen.IdentificationUnitAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnitInPart, dsSpecimen.IdentificationUnitInPart, DiversityCollection.CollectionSpecimen.SqlUnitInPart + WhereClause, DiversityWorkbench.Settings.ConnectionString);

                // Event
                if (dsSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    System.Data.DataRow[] RR = dsSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                    if (RR.Length > 0)
                    {
                        if (!RR[0]["CollectionEventID"].Equals(System.DBNull.Value))
                        {
                            int EventID = int.Parse(RR[0]["CollectionEventID"].ToString());
                            WhereClause = " WHERE CollectionEventID = " + EventID.ToString();

                            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEvent, dsSpecimen.CollectionEvent, DiversityCollection.CollectionSpecimen.SqlEvent + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEventImage, dsSpecimen.CollectionEventImage, DiversityCollection.CollectionSpecimen.SqlEventImage + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterGeography, dsSpecimen.CollectionEventLocalisation, DiversityCollection.CollectionSpecimen.SqlEventLocalisation + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterHabitat, dsSpecimen.CollectionEventProperty, DiversityCollection.CollectionSpecimen.SqlEventProperty + WhereClause, DiversityWorkbench.Settings.ConnectionString);

                            System.Data.DataRow[] RE = dsSpecimen.CollectionEvent.Select("CollectionEventID = " + EventID.ToString());
                            if (RE.Length > 0)
                            {
                                if (!RE[0]["SeriesID"].Equals(System.DBNull.Value))
                                {
                                    //this.fillExpedition(ref dsSpecimen, ref dsExpedition);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        public string FindNextAccessionNumber(string AccessionNumber)
        {
            string AccNr = "";
            string SQL = "SELECT COUNT(*) FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int i = System.Int32.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            if (i > 0)
                System.Windows.Forms.MessageBox.Show("You can not change a valid AccessionNumber");
            else
            {
                SQL = "SELECT dbo.NextAccessionNumber('" + AccessionNumber + "')";
                cmd.CommandText = SQL;
                con.Open();
                AccNr = cmd.ExecuteScalar().ToString();
                con.Close();
                if (AccNr.Length == 0)
                    System.Windows.Forms.MessageBox.Show("The system could not generate a new unique AccessionNumber");
            }
            return AccNr;
        }

        public static AvailabilityState Availability(int CollectionSpecimenID)
        {
            AvailabilityState A = AvailabilityState.Unknown;
            try
            {
                string SQL = "SELECT COUNT(*) " +
                    "FROM CollectionSpecimenID_UserAvailable AS A " +
                    "WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                string Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Test == "1")
                    A = AvailabilityState.Available;
                else
                {
                    SQL = "SELECT COUNT(*) " +
                        "FROM CollectionSpecimenID_ReadOnly AS A " +
                        "WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                    Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Test == "1")
                        A = AvailabilityState.ReadOnly;
                    else
                        A = AvailabilityState.NotAvailable;
                }

                //string SQL = "SELECT COUNT(*) " +
                //    "FROM CollectionSpecimenID_AvailableReadOnly AS A " +
                //    "WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                //string Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //if (Test == "1")
                //    A = AvailabilityState.ReadOnly;
                ////else
                //{
                //    SQL = "SELECT COUNT(*) " +
                //        "FROM CollectionSpecimenID_Available AS A " +
                //        "WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                //    Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //    if (Test == "1")
                //        A = AvailabilityState.Available;
                //    else if (A == AvailabilityState.Unknown) 
                //        A = AvailabilityState.NotAvailable;
                //}


                ////string SQL = "SELECT COUNT(*) " +
                ////    "FROM CollectionSpecimenID_UserAvailable AS A " +
                ////    "WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                ////string Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                ////if (Test == "1")
                ////    return AvailabilityState.Available;
                ////else
                ////{
                ////    SQL = "SELECT COUNT(*) " +
                ////        "FROM CollectionSpecimenID_ReadOnly AS A " +
                ////        "WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                ////    Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                ////    if (Test == "1")
                ////        return AvailabilityState.ReadOnly;
                ////    else return AvailabilityState.NotAvailable;
                ////}
            }
            catch (System.Exception ex) { }
            return A;
        }

        public static void AvailabilityOfProjectReset() { _AvailabilityOfProject = null; }

        private static System.Collections.Generic.Dictionary<int, AvailabilityState> _AvailabilityOfProject;

        public static AvailabilityState AvailabilityOfProject(int ProjectID)
        {
            if (_AvailabilityOfProject != null && _AvailabilityOfProject.ContainsKey(ProjectID))
                return _AvailabilityOfProject[ProjectID];

            AvailabilityState A = AvailabilityState.Unknown;
            try
            {
                string Message = "";
                string SQL = "SELECT TOP 1 [ReadOnly] " +
                    "FROM [dbo].[ProjectUser] " +
                    "U WHERE U.ProjectID = " + ProjectID.ToString() + " AND U.LoginName = "; ;
                string ReadOnly = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL + "SUSER_SNAME()", ref Message);
                if (ReadOnly == "")
                    ReadOnly = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL + "USER_NAME()", ref Message);
                switch (ReadOnly)
                {
                    case "":
                        A = AvailabilityState.NotAvailable;
                        break;
                    case "False":
                        A = AvailabilityState.Available;
                        break;
                    case "True":
                        A = AvailabilityState.ReadOnly;
                        break;
                }
            }
            catch (System.Exception ex) { }
            if (_AvailabilityOfProject == null)
                _AvailabilityOfProject = new Dictionary<int, AvailabilityState>();
            if (!_AvailabilityOfProject.ContainsKey(ProjectID))
                _AvailabilityOfProject.Add(ProjectID, A);
            return A;
        }

        #endregion

        #region Resources

        // For DEBUG alle relevanten Ordner in bin/Debug Directory kopieren
        public static void CopyWorkbenchDirectory()
        {
            try
            {
                System.Collections.Generic.List<string> FoldersForTransfer = new List<string>();
                FoldersForTransfer.Add("Archive");
                FoldersForTransfer.Add("Export");
                FoldersForTransfer.Add("Import");
                FoldersForTransfer.Add("LabelPrinting");
                FoldersForTransfer.Add("Maps");
                FoldersForTransfer.Add("Report");
                FoldersForTransfer.Add("Transaction");
                DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.CopyWorkbenchDirectory(FoldersForTransfer);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }


}
