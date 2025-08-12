using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DiversityCollection
{
    public class History
    {
        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DataSetCollectionSpecimen;
        private int? _SpecimenID;

        public int SpecimenID
        {
            get 
            {
                if (this._SpecimenID == null)
                    this._SpecimenID = int.Parse(this._DataSetCollectionSpecimen.Tables["CollectionSpecimen"].Rows[0]["CollectionSpecimenID"].ToString());
                return (int)_SpecimenID; 
            }
            set { _SpecimenID = value; }
        }

        public History(DiversityCollection.Datasets.DataSetCollectionSpecimen DS)
        {
            this._DataSetCollectionSpecimen = DS;
        }

        private void buttonHeaderHistory_Click(object sender, EventArgs e)
        {
            string Title = "History of " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString() + " (SpecimenID: " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString() + ")";
            try
            {
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                LogTables.Add(this.dtCollectionSpecimenHistory);
                if (this._DataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    LogTables.Add(this.dtCollectionEventHistory);
                    int EventID = int.Parse(this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString());
                    if (this._DataSetCollectionSpecimen.CollectionEventImage.Rows.Count > 0)
                        LogTables.Add(this.dtCollectionEventImageHistory);
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(EventID, "CollectionEventID", this._DataSetCollectionSpecimen.CollectionEventLocalisation.TableName, this._DataSetCollectionSpecimen.CollectionEvent.TableName));
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(EventID, "CollectionEventID", this._DataSetCollectionSpecimen.CollectionEventProperty.TableName, this._DataSetCollectionSpecimen.CollectionEvent.TableName));
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(EventID, "CollectionEventID", this._DataSetCollectionSpecimen.CollectionEventMethod.TableName, this._DataSetCollectionSpecimen.CollectionEvent.TableName));
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(EventID, "CollectionEventID", this._DataSetCollectionSpecimen.CollectionEventParameterValue.TableName, this._DataSetCollectionSpecimen.CollectionEvent.TableName));
                }
                if (this._DataSetCollectionSpecimen.CollectionAgent.Rows.Count > 0)
                    LogTables.Add(this.dtCollectionAgentHistory);
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.CollectionProject.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.CollectionSpecimenImage.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.CollectionSpecimenProcessing.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.CollectionSpecimenRelation.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.CollectionSpecimenPart.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.IdentificationUnit.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.Identification.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.IdentificationUnitAnalysis.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.IdentificationUnitGeoAnalysis.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.IdentificationUnitInPart.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));

                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.IdentificationUnitAnalysisMethod.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory(this.SpecimenID, "CollectionSpecimenID", this._DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameter.TableName, this._DataSetCollectionSpecimen.CollectionSpecimen.TableName));

                //if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
                //    LogTables.Add(this.dtCollectionEventSeriesHistory);
                //if (this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Count > 0)
                //    LogTables.Add(this.dtCollectionEventSeriesImageHistory);

                DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                f.ShowDialog();
                //if (f.DataRestored)
                //    this.setSpecimen(this.ID);
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable dtCollectionSpecimenHistory
        {
            get
            {
                System.Data.DataTable dtCollectionSpecimen_Log = new DataTable("Collection specimen");
                string SqlCurrent = "SELECT CollectionSpecimen.Version, CollectionSpecimen.AccessionNumber, CollectionSpecimen.AccessionDate, CollectionSpecimen.AccessionDay,  " +
                      "CollectionSpecimen.AccessionMonth, CollectionSpecimen.AccessionYear, CollectionSpecimen.AccessionDateSupplement,  " +
                      "CollDateCategory_Enum.DisplayText AS AccessionDateCategory, CollectionSpecimen.DepositorsName, CollectionSpecimen.DepositorsAgentURI,  " +
                      "CollectionSpecimen.DepositorsAccessionNumber, CollectionSpecimen.LabelTitle, CollLabelType_Enum.DisplayText AS LabelType,  " +
                      "CollLabelTranscriptionState_Enum.DisplayText AS LabelTranscriptionState, CollectionSpecimen.LabelTranscriptionNotes,  " +
                      "CollectionSpecimen.ExsiccataURI, CollectionSpecimen.ExsiccataAbbreviation, CollectionSpecimen.OriginalNotes, CollectionSpecimen.AdditionalNotes,  " +
                      "CollectionSpecimen.InternalNotes, CollectionSpecimen.ReferenceTitle, CollectionSpecimen.ReferenceURI, CollectionSpecimen.Problems,  " +
                      "CollectionSpecimen.DataWithholdingReason, " +
                      "'current version' AS KindOfChange, CollectionSpecimen.LogUpdatedWhen AS DateOfChange,  " +
                      "CollectionSpecimen.LogUpdatedBy AS ResponsibleUser, NULL AS LogID " +
                      "FROM CollectionSpecimen LEFT OUTER JOIN " +
                      "CollLabelTranscriptionState_Enum ON CollectionSpecimen.LabelTranscriptionState = CollLabelTranscriptionState_Enum.Code LEFT OUTER JOIN " +
                      "CollLabelType_Enum ON CollectionSpecimen.LabelType = CollLabelType_Enum.Code LEFT OUTER JOIN " +
                      "CollDateCategory_Enum ON CollectionSpecimen.AccessionDateCategory = CollDateCategory_Enum.Code " +
                      "WHERE (CollectionSpecimen.CollectionSpecimenID = " + this.SpecimenID.ToString() + ") ";
                string SQL = "SELECT CollectionSpecimen_log.Version, CollectionSpecimen_log.AccessionNumber, CollectionSpecimen_log.AccessionDate, CollectionSpecimen_log.AccessionDay,  " +
                      "CollectionSpecimen_log.AccessionMonth, CollectionSpecimen_log.AccessionYear, CollectionSpecimen_log.AccessionDateSupplement,  " +
                      "CollDateCategory_Enum.DisplayText AS AccessionDateCategory, CollectionSpecimen_log.DepositorsName, CollectionSpecimen_log.DepositorsAgentURI,  " +
                      "CollectionSpecimen_log.DepositorsAccessionNumber, CollectionSpecimen_log.LabelTitle, CollLabelType_Enum.DisplayText AS LabelType,  " +
                      "CollLabelTranscriptionState_Enum.DisplayText AS LabelTranscriptionState, CollectionSpecimen_log.LabelTranscriptionNotes,  " +
                      "CollectionSpecimen_log.ExsiccataURI, CollectionSpecimen_log.ExsiccataAbbreviation, CollectionSpecimen_log.OriginalNotes, CollectionSpecimen_log.AdditionalNotes,  " +
                      "CollectionSpecimen_log.InternalNotes, CollectionSpecimen_log.ReferenceTitle, CollectionSpecimen_log.ReferenceURI, CollectionSpecimen_log.Problems,  " +
                      "CollectionSpecimen_log.DataWithholdingReason, " +
                      "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS KindOfChange, LogDate AS DateOfChange, LogUser AS ResponsibleUser, LogID " +
                      "FROM CollectionSpecimen_log LEFT OUTER JOIN " +
                      "CollLabelTranscriptionState_Enum ON CollectionSpecimen_log.LabelTranscriptionState = CollLabelTranscriptionState_Enum.Code LEFT OUTER JOIN " +
                      "CollLabelType_Enum ON CollectionSpecimen_log.LabelType = CollLabelType_Enum.Code LEFT OUTER JOIN " +
                      "CollDateCategory_Enum ON CollectionSpecimen_log.AccessionDateCategory = CollDateCategory_Enum.Code " +
                      "WHERE (CollectionSpecimen_log.CollectionSpecimenID = " + this.SpecimenID.ToString() + ") ORDER BY LogID DESC ";
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionSpecimen_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionSpecimen_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionSpecimen_Log;
            }
        }

        private System.Data.DataTable dtCollectionEventLocalisationHistory
        {
            get
            {
                System.Data.DataTable dtCollectionEventLocalisation_Log = new DataTable("CollectionEventLocalisation");
                string SqlCurrent = "SELECT CollectionEvent.Version, LocalisationSystem.LocalisationSystemName AS [Localisation system],  " +
                    "LocalisationSystem.DisplayTextLocation1 + ': ' + CollectionEventLocalisation.Location1 AS [Value 1],  " +
                    "LocalisationSystem.DisplayTextLocation2 + ': ' + CollectionEventLocalisation.Location2 AS [Value 2], CollectionEventLocalisation.LocationAccuracy AS Accuracy,  " +
                    "CollectionEventLocalisation.LocationNotes AS Notes, CollectionEventLocalisation.DeterminationDate AS [Determination date],  " +
                    "CollectionEventLocalisation.DistanceToLocation AS [Distance to location], CollectionEventLocalisation.DirectionToLocation AS [Direction to location],  " +
                    "CollectionEventLocalisation.ResponsibleName AS [Responsible User], CollectionEventLocalisation.ResponsibleAgentURI AS [Responsible agent URI],  " +
                    "CollectionEventLocalisation.AverageAltitudeCache AS [Altitude cache], CollectionEventLocalisation.AverageLatitudeCache AS [Latitude cache],  " +
                    "CollectionEventLocalisation.AverageLongitudeCache AS [Longitude cache], " +
                    "'current version' AS [Kind of change], CollectionEventLocalisation.LogUpdatedWhen AS [Date of change],  " +
                    "CollectionEventLocalisation.LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM CollectionEventLocalisation INNER JOIN " +
                    "CollectionEvent ON CollectionEventLocalisation.CollectionEventID = CollectionEvent.CollectionEventID INNER JOIN " +
                    "LocalisationSystem ON CollectionEventLocalisation.LocalisationSystemID = LocalisationSystem.LocalisationSystemID " +
                    "WHERE CollectionEventLocalisation.CollectionEventID = " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString();
                string SQL = "SELECT CollectionEventLocalisation_Log.LogVersion AS Version, LocalisationSystem.LocalisationSystemName AS [Localisation system],  " +
                    "LocalisationSystem.DisplayTextLocation1 + ': ' + CollectionEventLocalisation_Log.Location1 AS [Value 1],  " +
                    "LocalisationSystem.DisplayTextLocation2 + ': ' + CollectionEventLocalisation_Log.Location2 AS [Value 2], CollectionEventLocalisation_Log.LocationAccuracy AS Accuracy,  " +
                    "CollectionEventLocalisation_Log.LocationNotes AS Notes, CollectionEventLocalisation_Log.DeterminationDate AS [Determination date],  " +
                    "CollectionEventLocalisation_Log.DistanceToLocation AS [Distance to location], CollectionEventLocalisation_Log.DirectionToLocation AS [Direction to location],  " +
                    "CollectionEventLocalisation_Log.ResponsibleName AS [Responsible User], CollectionEventLocalisation_Log.ResponsibleAgentURI AS [Responsible agent URI],  " +
                    "CollectionEventLocalisation_Log.AverageAltitudeCache AS [Altitude cache], CollectionEventLocalisation_Log.AverageLatitudeCache AS [Latitude cache],  " +
                    "CollectionEventLocalisation_Log.AverageLongitudeCache AS [Longitude cache], " +
                    "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], LogDate AS [Date of change],  " +
                    "LogUser AS [Responsible user], LogID  " +
                    "FROM CollectionEventLocalisation_Log INNER JOIN " +
                    "LocalisationSystem ON CollectionEventLocalisation_Log.LocalisationSystemID = LocalisationSystem.LocalisationSystemID " +
                    "WHERE CollectionEventLocalisation_Log.CollectionEventID = " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString();
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionEventLocalisation_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionEventLocalisation_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionEventLocalisation_Log;
            }
        }

        private System.Data.DataTable dtCollectionAgentHistory
        {
            get
            {
                System.Data.DataTable dtCollectionAgent_Log = new DataTable("CollectionAgent");
                string SqlCurrent = "SELECT CollectionSpecimen.Version, CollectorsName AS Collector, CollectorsAgentURI AS [Collectors URI], " +
                    "CollectorsSequence AS [Sequence], CollectorsNumber AS [Collectors number], Notes, " +
                    "CollectionAgent.DataWithholdingReason AS [Datawithholding reason], " +
                    "'current version' AS [Kind of change], CollectionAgent.LogUpdatedWhen AS [Date of change], " +
                    "CollectionAgent.LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM CollectionAgent INNER JOIN " +
                    "CollectionSpecimen ON CollectionAgent.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
                    "WHERE CollectionAgent.CollectionSpecimenID =  " + this.SpecimenID.ToString();
                string SQL = "SELECT LogVersion AS Version, CollectorsName AS Collector, CollectorsAgentURI AS [Collectors URI], " +
                    "CollectorsSequence AS [Sequence], CollectorsNumber AS [Collectors number], Notes, " +
                    "CollectionAgent_log.DataWithholdingReason AS [Datawithholding reason], " +
                    "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], LogDate AS [Date of change], " +
                    "LogUser AS [Responsible user], LogID  " +
                    "FROM  CollectionAgent_log " +
                    "WHERE CollectionSpecimenID = " + this.SpecimenID.ToString() +
                    " ORDER BY LogVersion DESC, LogID DESC";
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionAgent_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionAgent_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionAgent_Log;
            }
        }

        private System.Data.DataTable dtCollectionEventHistory
        {
            get
            {
                System.Data.DataTable dtCollectionEvent_Log = new DataTable("CollectionEvent");
                string SqlCurrent = "SELECT CollectionEvent.Version, CollectionEventSeries.Description AS CollectionEventSeries, CollectionEvent.CollectorsEventNumber AS [Event number],  " +
                    "CollectionEvent.CollectionDate AS [Collection date], CollectionEvent.CollectionDay AS [Collection day],  " +
                    "CollectionEvent.CollectionMonth AS [Collection month], CollectionEvent.CollectionYear AS [Collection year],  " +
                    "CollectionEvent.CollectionDateSupplement AS [Date supplement], CollectionEvent.CollectionDateCategory AS [Date category],  " +
                    "CollectionEvent.CollectionTime AS [Collection time], CollectionEvent.CollectionTimeSpan AS [Collection time span],  " +
                    "CollectionEvent.LocalityDescription AS [Locality description], CollectionEvent.HabitatDescription AS [Habitat description],  " +
                    "CollectionEvent.ReferenceTitle AS [Reference title], CollectionEvent.ReferenceURI AS [Reference URI],  " +
                    "CollectionEvent.CollectingMethod AS [Collecting method], CollectionEvent.Notes, CollectionEvent.CountryCache AS Country,  " +
                    "CollectionEvent.DataWithholdingReason, " +
                    "'current version' AS [Kind of change], CollectionEvent.LogUpdatedWhen AS [Date of change], CollectionEvent.LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM CollectionEvent LEFT OUTER JOIN " +
                    "CollectionEventSeries ON CollectionEvent.SeriesID = CollectionEventSeries.SeriesID " +
                    "WHERE (CollectionEvent.CollectionEventID = " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString() + ") ";
                string SQL = "SELECT CollectionEvent_log.Version, CollectionEventSeries.Description AS CollectionEventSeries, CollectionEvent_log.CollectorsEventNumber AS [Event number],  " +
                    "CollectionEvent_log.CollectionDate AS [Collection date], CollectionEvent_log.CollectionDay AS [Collection day],  " +
                    "CollectionEvent_log.CollectionMonth AS [Collection month], CollectionEvent_log.CollectionYear AS [Collection year],  " +
                    "CollectionEvent_log.CollectionDateSupplement AS [Date supplement], CollectionEvent_log.CollectionDateCategory AS [Date category],  " +
                    "CollectionEvent_log.CollectionTime AS [Collection time], CollectionEvent_log.CollectionTimeSpan AS [Collection time span],  " +
                    "CollectionEvent_log.LocalityDescription AS [Locality description], CollectionEvent_log.HabitatDescription AS [Habitat description],  " +
                    "CollectionEvent_log.ReferenceTitle AS [Reference title], CollectionEvent_log.ReferenceURI AS [Reference URI],  " +
                    "CollectionEvent_log.CollectingMethod AS [Collecting method], CollectionEvent_log.Notes, CollectionEvent_log.CountryCache AS Country,  " +
                    "CollectionEvent_log.DataWithholdingReason, " +
                    "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END, LogDate, LogUser, LogID  " +
                    "FROM CollectionEvent_log LEFT OUTER JOIN " +
                    "CollectionEventSeries ON CollectionEvent_log.SeriesID = CollectionEventSeries.SeriesID " +
                    "WHERE (CollectionEvent_log.CollectionEventID = " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString() + ") " +
                    " ORDER BY Version DESC, LogID DESC";
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionEvent_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionEvent_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionEvent_Log;
            }
        }

        private System.Data.DataTable dtCollectionEventImageHistory
        {
            get
            {
                System.Data.DataTable dtCollectionEventImage_Log = new DataTable("CollectionEventImage");
                string SqlCurrent = "SELECT URI, ResourceURI AS [Resource URI], ImageType AS [Image type], Notes, " +
                    "'current version' AS [Kind of change], LogUpdatedWhen AS [Date of change], LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM CollectionEventImage WHERE CollectionEventID = " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString() + " ";
                string SQL = "SELECT URI, ResourceURI AS [Resource URI], ImageType AS [Image type], Notes, " +
                    "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], LogDate AS [Date of change], LogUser  AS [Responsible user], LogID  " +
                    "FROM CollectionEventImage_Log WHERE CollectionEventID = " + this._DataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString() + " " +
                    " ORDER BY LogID DESC ";
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionEventImage_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionEventImage_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionEventImage_Log;
            }
        }

        private System.Data.DataTable dtCollectionEventSeriesHistory
        {
            get
            {
                System.Data.DataTable dtCollectionEventSeries_Log = new DataTable("CollectionEventSeries");
                string SqlCurrent = "SELECT Description, SeriesCode, Notes, " +
                    "'current version' AS [Kind of change], LogUpdatedWhen AS [Date of change], LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                    "FROM CollectionEventSeries WHERE SeriesID = " + this._DataSetCollectionSpecimen.CollectionEventSeries.Rows[0]["SeriesID"].ToString() + " ";
                string SQL = "SELECT Description, SeriesCode, Notes, " +
                    "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], LogDate AS [Date of change], LogUser  AS [Responsible user], LogID  " +
                    "FROM CollectionEventSeries_Log WHERE SeriesID = " + this._DataSetCollectionSpecimen.CollectionEventSeries.Rows[0]["SeriesID"].ToString() + " " +
                    " ORDER BY LogID DESC ";
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionEventSeries_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionEventSeries_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionEventSeries_Log;
            }
        }

        private System.Data.DataTable dtCollectionEventSeriesImageHistory
        {
            get
            {
                System.Data.DataTable dtCollectionEventSeriesImage_Log = new DataTable("CollectionEventSeriesImage");
                if (this._DataSetCollectionSpecimen.CollectionEventSeries.Rows.Count == 0)
                {
                    return dtCollectionEventSeriesImage_Log;
                }
                try
                {
                    string SqlCurrent = "SELECT URI, ResourceURI AS [Resource URI], ImageType AS [Image type], Notes, " +
                        "'current version' AS [Kind of change], LogUpdatedWhen AS [Date of change], LogUpdatedBy AS [Responsible user], NULL AS LogID " +
                        "FROM CollectionEventSeriesImage WHERE SeriesID = " + this._DataSetCollectionSpecimen.CollectionEventSeries.Rows[0]["SeriesID"].ToString() + " ";
                    string SQL = "SELECT URI, ResourceURI AS [Resource URI], ImageType AS [Image type], Notes, " +
                        "CASE WHEN LogState = 'U' THEN 'UPDATE' ELSE 'DELETE' END AS [Kind of change], LogDate AS [Date of change], LogUser  AS [Responsible user], LogID  " +
                        "FROM CollectionEventSeriesImage_Log WHERE SeriesID = " + this._DataSetCollectionSpecimen.CollectionEventSeries.Rows[0]["SeriesID"].ToString() + " " +
                        " ORDER BY LogID DESC ";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtCollectionEventSeriesImage_Log);
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtCollectionEventSeriesImage_Log);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtCollectionEventSeriesImage_Log;
            }
        }


    }
}
