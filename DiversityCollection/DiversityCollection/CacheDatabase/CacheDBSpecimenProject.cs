using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DiversityCollection.CacheDatabase
{
    public class CacheDBSpecimenProject
    {

        #region Parameter

        private int _ProjectID;
        private System.Data.DataTable _DtSpecimen;
        private System.Data.DataTable _DtUnit;
        private string _DataType;

        #endregion

        #region Construction

        public CacheDBSpecimenProject(int ProjectID, string DataType)
        {
            this._ProjectID = ProjectID;
            this._DataType = DataType;
        }

        #endregion

        #region Refreshing the data

        public bool RefreshData()
        {
            if (!this.RefreshDataSpecimen())
                return false;
            if (!this.RefreshDataUnit())
                return false;
            if (!this.RefreshDataIdentification())
                return false;
            return true;
        }

        public bool RefreshDataSpecimen()
        {
            try
            {
                string SQL = "SELECT RTRIM(Value) FROM " + DiversityCollection.CacheDatabase.CacheDB.ProjectsDatabase + ".[dbo].[ProjectSettings_ABCD] (" + this._ProjectID.ToString() + ") WHERE ProjectSetting = 'RecordURI'";
                string RecordURI = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                SQL = "DELETE S FROM CollectionSpecimenCache AS S INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE  P.ProjectID = " + this._ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                SQL = "INSERT INTO CollectionSpecimenCache " +
                    "(CollectionSpecimenID, Version, AccessionNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, LocalityDescription,  " +
                    "CountryCache, LabelTranscriptionNotes, ExsiccataAbbreviation, OriginalNotes, CollectorsEventNumber, Chronostratigraphy, Lithostratigraphy, RecordURI) " +
                    "SELECT S.CollectionSpecimenID, Version, AccessionNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, LocalityDescription,  " +
                    "CountryCache, LabelTranscriptionNotes, ExsiccataAbbreviation, OriginalNotes, CollectorsEventNumber, Chronostratigraphy, Lithostratigraphy, ";
                if (RecordURI.Length > 0)
                    SQL += "'" + RecordURI + "' + CAST(S.CollectionSpecimenID as varchar) ";
                else SQL += " NULL ";
                SQL += "FROM CollectionProject P, ";
                switch (this._DataType)
                {
                    case "Observations":
                    case "Specimen":
                        SQL += "CollectionSpecimen";
                        break;
                    case "Parts":
                        SQL += "CollectionSpecimen_Part";
                        break;
                }
                SQL += " AS S WHERE S.CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM CollectionSpecimenCache) AND P.CollectionSpecimenID = S.CollectionSpecimenID AND P.ProjectID = " + this._ProjectID.ToString();
                switch (this._DataType)
                {
                    case "Specimen":
                        SQL += " AND (S.AccessionNumber <> '') ";
                        break;
                    case "Observations":
                    case "Parts":
                        break;
                }
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                SQL = "EXECUTE [dbo].[procRefreshCollectionSpecimenCacheCoordinates]  " + this._ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                this.FillDtSpecimen();

                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool RefreshDataUnit()
        {
            try
            {
                string SQL = "DELETE U " +
                    "FROM IdentificationUnitCache AS U INNER JOIN " +
                    "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool RefreshDataIdentification()
        {
            try
            {
                string SQL = "DELETE I " +
                    "FROM IdentificationCache AS U INNER JOIN " +
                    "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        public void RequerySpecimen()
        {
            this.FillDtSpecimen();
        }

        public System.Data.DataTable DtSpecimen
        {
            get
            {
                if (this._DtSpecimen == null)
                {
                    this._DtSpecimen = new DataTable();
                    this.FillDtSpecimen();
                }
                return this._DtSpecimen;
            }
        }

        private void FillDtSpecimen()
        {
            //this._DtSpecimen.Clear();
            //string SQL = "SELECT S.CollectionSpecimenID, S.Version, S.AccessionNumber, S.CollectionDate, S.CollectionDay, S.CollectionMonth, S.CollectionYear, S.CollectionDateSupplement, " +
            //   "S.LocalityDescription, S.CountryCache, S.LabelTranscriptionNotes, S.ExsiccataAbbreviation, S.OriginalNotes, S.Latitude, S.Longitude, S.LocationAccuracy, " +
            //   "S.DistanceToLocation, S.DirectionToLocation, S.CollectorsEventNumber, S.Chronostratigraphy, S.Lithostratigraphy, S.RecordURI, S.LogInsertedWhen " +
            //   "FROM CollectionSpecimenCache AS S INNER JOIN " +
            //   "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
            //   "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
            //   "ORDER BY S.CollectionSpecimenID";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDB.ConnectionStringCacheDB);
            //ad.Fill(this._DtSpecimen);
        }

        public System.Data.DataTable DtUnit
        {
            get
            {
                if (this._DtUnit == null)
                {
                    string SQL = "SELECT U.CollectionSpecimenID, U.IdentificationUnitID, U.SpecimenPartID, U.LastIdentificationCache, U.TaxonomicGroup, U.SubstrateID, U.ExsiccataNumber, U.DisplayOrder, " +
                        "U.ColonisedSubstratePart, U.IdentificationQualifier, U.VernacularTerm, U.TaxonomicName, U.Notes, U.TypeStatus, U.SynonymName, U.AcceptedName, " +
                        "U.AcceptedNameURI, U.SynNameURI, U.TaxonomicRank, U.GenusOrSupragenericName, U.TaxonNameSinAuthor, U.IdentificationSequenceFirstEntry, " +
                        "U.TaxonomicNameFirstEntry, U.LastValidIdentificationSequence, U.LastValidTaxonName, U.LastValidTaxonWithQualifier, U.LastValidTaxonSinAuthor, " +
                        "U.LastValidTaxonIndex, U.LastValidTaxonListOrder, U.FamilyCache, U.OrderCache, U.LifeStage, U.Gender, U.Relation, " +
                        "U.UnitAssociation_AssociatedUnitSourceInstitutionCode, U.UnitAssociation_AssociatedUnitSourceName, U.UnitAssociation_AssociatedUnitID, " +
                        "U.UnitAssociation_AssociationType, U.UnitAssociation_Comment, U.UnitID, U.UnitGUID, U.AccNrUnitID, U.NomenclaturalTypeDesignation_TypifiedName, " +
                        "U.NomenclaturalTypeDesignation_TypeStatus, U.MaterialCategory, U.LogInsertedWhen " +
                        "FROM IdentificationUnitCache AS U INNER JOIN " +
                        "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                        "WHERE P.ProjectID = " + this._ProjectID.ToString();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._DtUnit = new DataTable();
                    //ad.Fill(this._DtUnit);
                }
                return this._DtUnit;
            }
        }

    }

}
