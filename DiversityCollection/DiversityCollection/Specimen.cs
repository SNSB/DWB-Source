using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DiversityCollection
{
    public partial class Specimen : Component
    {

        #region Parameter

        private static System.Windows.Forms.ImageList _ImageList;
        private static System.Windows.Forms.ImageList _ImageListUser;
        private static System.Windows.Forms.ImageList _ImageListLocalisationSystem;
        private static System.Windows.Forms.ImageList _ImageListStorage;
        private static System.Windows.Forms.ImageList _ImageListTaxon;
        private static System.Windows.Forms.ImageList _ImageListUnitDescription;

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _ImagesMaterialCategory;
        
        #endregion

        #region Construction

        public Specimen()
        {
            InitializeComponent();
        }

        public Specimen(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #endregion

        #region Image handling

        #region Access functions for images
        public static System.Drawing.Image getImage(OverviewImage Image)
        {
            System.Drawing.Image I = Specimen.ImageList.Images[(int)Image];
            return I;
        }

        public static System.Drawing.Image getImage(OverviewImageStorage Image)
        {
            System.Drawing.Image I = Specimen.ImageListStorage.Images[(int)Image];
            return I;
        }

        public static System.Drawing.Image getImage(OverviewImageLocalisationSystem Image)
        {
            System.Drawing.Image I = Specimen.ImageListStorage.Images[(int)Image];
            return I;
        }

        public static System.Drawing.Image getImage(OverviewImageTaxon Image)
        {
            System.Drawing.Image I = Specimen.ImageListTaxon.Images[(int)Image];
            return I;
        }

        public static System.Drawing.Image getImage(OverviewImageTableOrField Image)
        {
            System.Drawing.Image I = Specimen.ImageListTaxon.Images[(int)Image];
            return I;
        }

        public static System.Drawing.Image Image(string Identifier)
        {
            //int I = DiversityCollection.Specimen.ImageList.Images[Identifier], IsGrey);
            //System.Drawing.Image Image = DiversityCollection.Specimen.ImageListTaxon.Images[I];
            return DiversityCollection.Specimen.ImageList.Images[Identifier];
        }

        public static System.Drawing.Image ImageForAgent(OverviewImageUser User)
        {
            return DiversityCollection.Specimen.ImageListUser.Images[(int)User];
        }

        public static System.Drawing.Image ImageForOverview(OverviewImage Image)
        {
            return DiversityCollection.Specimen.ImageList.Images[(int)Image];
        }

        public static System.Drawing.Image ImageForTableOrField(ImageTableOrField Image, bool IsGray)
        {
            int Position = ((int)Image * 2) - 1;
            if (IsGray) Position++;
            if (Position == -1) Position = 0; // in case of first image Null
            return DiversityCollection.Specimen.ImageListTablesAndFields.Images[Position];
        }

        #endregion

        #region image lists
        public static System.Windows.Forms.ImageList ImageList
        {
            get
            {
                if (DiversityCollection.Specimen._ImageList == null)
                {
                    DiversityCollection.Specimen S = new Specimen();
                    DiversityCollection.Specimen._ImageList = S.imageList;
                }
                return DiversityCollection.Specimen._ImageList;
            }
        }

        public static System.Windows.Forms.ImageList ImageListUser
        {
            get
            {
                if (DiversityCollection.Specimen._ImageListUser == null)
                {
                    DiversityCollection.Specimen S = new Specimen();
                    DiversityCollection.Specimen._ImageListUser = S.imageListUser;
                }
                return DiversityCollection.Specimen._ImageListUser;
            }
        }

        public static System.Windows.Forms.ImageList ImageListLocalisationSystem
        {
            get
            {
                if (DiversityCollection.Specimen._ImageListLocalisationSystem == null)
                {
                    DiversityCollection.Specimen S = new Specimen();
                    DiversityCollection.Specimen._ImageListLocalisationSystem = S.imageListLocalisationSystem;
                }
                return DiversityCollection.Specimen._ImageListLocalisationSystem;
            }
        }

        public static System.Windows.Forms.ImageList ImageListStorage
        {
            get
            {
                if (DiversityCollection.Specimen._ImageListStorage == null)
                {
                    DiversityCollection.Specimen S = new Specimen();
                    DiversityCollection.Specimen._ImageListStorage = S.imageList;
                }
                return DiversityCollection.Specimen._ImageListStorage;
            }
        }

        public static System.Windows.Forms.ImageList ImageListTaxon
        {
            get
            {
                if (DiversityCollection.Specimen._ImageListTaxon == null)
                {
                    DiversityCollection.Specimen S = new Specimen();
                    DiversityCollection.Specimen._ImageListTaxon = S.imageList;
                }
                return DiversityCollection.Specimen._ImageListTaxon;
            }
        }


        public static System.Windows.Forms.ImageList ImageListUnitDescription
        {
            get
            {
                if (DiversityCollection.Specimen._ImageListUnitDescription == null)
                {
                    DiversityCollection.Specimen S = new Specimen();
                    DiversityCollection.Specimen._ImageListUnitDescription = S.imageListUnitDescription;
                }
                return DiversityCollection.Specimen._ImageListUnitDescription;
            }
        }

        #endregion

        public static int ImageIndex(System.Data.DataRow R, bool GreyImage)
        {
            int i = 0;
            string Table = R.Table.TableName;
            switch (Table)
            {
                case "CollectionEventSeries":
                    i = (int)OverviewImage.EventSeries;
                    break;
                case "SamplingPlot":
                    i = (int)OverviewImage.SamplingPlot;
                    break;
                case "CollectionEventList":
                case "CollectionEvent":
                    i = (int)OverviewImage.Event;
                    break;
                case "CollectionEventMethod":
                case "CollectionEventMethodList":
                    i = (int)OverviewImageTableOrField.Tool;
                    break;
                case "CollectionEventParameterValue":
                case "CollectionEventParameterValueList":
                    i = (int)OverviewImageTableOrField.Parameter;
                    break;
                case "CollectionEventImage":
                    i = (int)OverviewImageStorage.Icones;
                    break;
                case "CollectionEventProperty":
                    i = (int)OverviewImage.EventProperty;
                    int PropertyID;
                    if (int.TryParse(R["PropertyID"].ToString(), out PropertyID))
                    {
                        string ParsingMethod = DiversityCollection.LookupTable.ParsingMethodNameProperty(PropertyID);
                        i = Specimen.ImageIndexForCollectionEventProperty(ParsingMethod, GreyImage);
                        if (GreyImage) i--;
                    }
                    break;
                case "CollectionEventLocalisation":
                    i = (int)OverviewImage.Localisation;
                    int LocalisationSystemID;
                    if (int.TryParse(R["LocalisationSystemID"].ToString(), out LocalisationSystemID))
                    {
                        if (LocalisationSystemID > 17 && LocalisationSystemID < 22)
                        {
                            if (LocalisationSystemID == 18)
                                i = (int)OverviewImageTableOrField.Place2;
                            if (LocalisationSystemID == 19)
                                i = (int)OverviewImageTableOrField.Place3;
                            if (LocalisationSystemID == 20)
                                i = (int)OverviewImageTableOrField.Place4;
                            if (LocalisationSystemID == 21)
                                i = (int)OverviewImageTableOrField.Place5;
                        }
                        else
                        {
                            string ParsingMethod = DiversityCollection.LookupTable.ParsingMethodName(LocalisationSystemID);
                            if (ParsingMethod == "Height")
                            {
                                string LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(LocalisationSystemID);
                                i = Specimen.ImageForLocalisationSystem(LocalisationSystem, GreyImage);
                            }
                            else
                                i = Specimen.ImageForLocalisationSystem(ParsingMethod, GreyImage);
                        }
                        if (GreyImage) i--;
                    }
                    break;
                case "Regulation":
                case "CollectionEventRegulation":
                    i = (int)OverviewImageTableOrField.Paragraph;
                    break;
                case "CollectionSpecimenList":
                    i = (int)OverviewImage.Specimen;
                    break;
                case "CollectionSpecimen":
                    i = (int)OverviewImage.Specimen;
                    //try
                    //{
                    //    if (R["AccessionNumber"].Equals(System.DBNull.Value))
                    //        i = (int)OverviewImageStorage.Observation;
                    //}
                    //catch { }
                    break;
                case "CollectionAgent":
                    i = (int)OverviewImage.Agent;
                    break;
                case "Collection":
                    string CollectionType = "";
                    if (R.Table.Columns.Contains("Type"))
                    {
                        CollectionType = R["Type"].ToString().ToLower();
                        if (CollectionType == OverviewImageTableOrField.Box.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Box;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Cupboard.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Cupboard;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Location.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Location;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Container.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Container;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Department.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Department;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.SubdividedContainer.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.SubdividedContainer;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Drawer.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Drawer;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Freezer.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Freezer;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Fridge.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Fridge;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Institution.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Institution;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Radioactive.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Radioactive;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Room.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Room;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Rack.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Rack;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.SteelLocker.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.SteelLocker;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Sensor.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Sensor;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Trap.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Trap;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Area.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Area;
                            break;
                        }
                        else if (CollectionType == OverviewImageTableOrField.Hardware.ToString().ToLower())
                        {
                            i = (int)OverviewImageTableOrField.Hardware;
                            break;
                        }
                    }
                    i = (int)OverviewImage.Collection;
                    break;
                case "Identification":
                    if (R["DependsOnIdentificationSequence"].Equals(System.DBNull.Value))
                        i = (int)OverviewImage.Identification;
                    else
                        i=(int)OverviewImageTableOrField.IdentificationAdd;
                    break;
                case "IdentificationUnitList":
                case "IdentificationUnit":
                    string TaxonomicGroup = R["TaxonomicGroup"].ToString();
                    string UnitDescription = "";
                    if (R.Table.Columns.Contains("UnitDescription"))
                    {
                        try { UnitDescription = R["UnitDescription"].ToString(); }
                        catch { }
                        if (UnitDescription.Length == 0)
                            i = Specimen.TaxonImage(TaxonomicGroup, false);
                        else
                            i = Specimen.TaxonImageIndex(TaxonomicGroup, UnitDescription, false);
                    }
                    else
                        i = Specimen.TaxonImage(TaxonomicGroup, false);
                    break;
                case "IdentificationUnitAnalysis":
                    i = (int)OverviewImage.Analysis;
                    break;
                case "IdentificationUnitGeoAnalysis":
                    i = (int)OverviewImage.GeoAnalysis;
                    break;
                case "CollectionSpecimenProcessing":
                    i = (int)OverviewImage.Processing;
                    break;
                case "CollectionSpecimenPartDescription":
                    i = (int)OverviewImageTableOrField.PartDescription;
                    break;
                case "CollectionSpecimenPartRegulation":
                    i = (int)OverviewImageTableOrField.Paragraph;
                    break;
                case "CollectionSpecimenReference":
                    i = (int)OverviewImageTableOrField.Reference;
                    break;
                case "CollectionSpecimenRelation":
                    bool isInternal = true;
                    bool.TryParse(R["IsInternalRelationCache"].ToString(), out isInternal);
                    if (isInternal) i = (int)OverviewImage.Relation;
                    else if (GreyImage)
                    {
                        i = (int)OverviewImageTableOrField.RelationExternGrey;
                        i--;
                    }
                    else i = (int)OverviewImage.RelationExtern;
                    break;
                case "CollectionSpecimenRelationInternal":
                    i = (int)OverviewImageTableOrField.RelationInvers;
                    break;
                case "CollectionSpecimenRelationExternal":
                    i = (int)OverviewImageTableOrField.RelationExtern;
                    break;
                case "CollectionSpecimenRelationInvers":
                    i = (int)OverviewImage.RelationInversGrey;
                    if (GreyImage) i--;
                    break;
                case "CollectionSpecimenPart":
                    string MaterialCategory = R["MaterialCategory"].ToString();
                    i = Specimen.MaterialCategoryImage(MaterialCategory, false);
                    break;
                case "CollectionSpecimenTransaction":
                    int TransactionID = int.Parse(R["TransactionID"].ToString());
                    string Type = DiversityCollection.LookupTable.TransactionType(TransactionID).ToLower();
                    switch (Type)
                    {
                        case "loan":
                            i = (int)OverviewImage.Loan;
                            break;
                        case "embargo":
                            i = (int)OverviewImageTableOrField.Embargo;
                            break;
                        case "forwarding":
                            i = (int)OverviewImageTableOrField.Forwarding;
                            break;
                        case "permit":
                            i = (int)OverviewImageTableOrField.Permit;
                            break;
                        case "return":
                            i = (int)OverviewImageTableOrField.Return;
                            break;
                        case "borrow":
                            i = (int)OverviewImageTableOrField.Borrow;
                            break;
                        case "gift":
                            i = (int)OverviewImageTableOrField.Gift;
                            break;
                        case "request":
                            i = (int)OverviewImageTableOrField.Request;
                            break;
                        case "purchase":
                            i = (int)OverviewImageTableOrField.Purchase;
                            break;
                        case "inventory":
                            i = (int)OverviewImageTableOrField.Inventory;
                            break;
                        case "exchange":
                            i = (int)OverviewImageTableOrField.Exchange;
                            break;
                        case "transaction group":
                            i = (int)OverviewImageTableOrField.Purchase;
                            break;
                        case "removal":
                            i = (int)OverviewImageTableOrField.Removal;
                            break;
                        case "regulation":
                            i = (int)OverviewImageTableOrField.Paragraph;
                            if (GreyImage) i--;
                            break;
                        case "warning":
                            i = (int)OverviewImageTableOrField.Warning;
                            break;
                        default:
                            i = (int)OverviewImage.Transaction;
                            if (GreyImage) i--;
                            break;
                    }
                    break;
                case "Annotation":
                    string AnnotationType = R["AnnotationType"].ToString();
                    switch (AnnotationType)
                    {
                        case "Reference":
                        case "Annotation":
                            i = (int)OverviewImageTableOrField.Annotation;
                            break;
                            //i = (int)OverviewImageTableOrField.Reference;
                            //break;                            
                        case "Problem":
                            i = (int)OverviewImageTableOrField.Problem;
                            break;

                        default:
                            i = (int)OverviewImageTableOrField.ID;
                            break;
                    }
                    break;
                case "IdentifierEvent":
                case "IdentifierSpecimen":
                case "IdentifierPart":
                case "IdentifierTransaction":
                case "IdentifierUnit":
                case "ExternalIdentifier":
                    i = (int)OverviewImageTableOrField.ID;
                    //string IdentifierType = R["Type"].ToString();
                    //if (DiversityWorkbench.ReferencingTable.Regulations() != null
                    //    && DiversityWorkbench.ReferencingTable.Regulations().Contains(IdentifierType))
                    //    i = (int)OverviewImageTableOrField.Paragraph;
                    //else
                    //    i = (int)OverviewImageTableOrField.ID;
                    break;
                case "SubcollectionContent":
                    string Material = R["MaterialCategory"].ToString();
                    i = Specimen.MaterialCategoryImage(Material, false);
                    break;
                case "Task":
                case "CollectionTask":
                    string TaskType = "task";
                    if (R.Table.Columns.Contains("Type"))
                        TaskType = R["Type"].ToString();
                    else if (R.Table.Columns.Contains("TaskID"))
                        TaskType = DiversityCollection.LookupTable.TaskType(int.Parse(R["TaskID"].ToString())).ToLower();
                    i = Specimen.TaskTypeImage(TaskType, GreyImage);
                    break;
            }
            if (GreyImage) i++;
            return i;
        }

        #region old
        public static int ImageIndex(System.Collections.Generic.List<string> IdentifierList, bool GreyImage)
        {
            int iImage = -1;
            for (int i = 0; i < IdentifierList.Count; i++ )
            {
                switch (IdentifierList[i])
                {
                    case "CollectionEventSeries":
                        iImage = (int)OverviewImage.EventSeries;
                        break;
                    case "SamplingPlot":
                        iImage = (int)OverviewImage.SamplingPlot;
                        break;
                    case "CollectionEventList":
                    case "CollectionEvent":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.Event;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            if (IdentifierList[i + 1].ToLower().IndexOf("reference") > -1)
                            {
                                iImage = (int)OverviewImageTableOrField.Reference;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("responsible") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "CollectionEventProperty":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.EventProperty;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            string Prop = IdentifierList[i + 1];
                            if (Prop.StartsWith("PropertyID_"))
                            {
                                int PropertyID;
                                if(int.TryParse(Prop.Substring("PropertyID_".Length), out PropertyID))
                                {
                                    string ParsingMethod = DiversityCollection.LookupTable.ParsingMethodNameProperty(PropertyID);
                                    iImage = Specimen.ImageIndexForCollectionEventProperty(ParsingMethod, false);
                                    return iImage;
                                }
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("responsible") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "CollectionEventLocalisation":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.Localisation;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            string LocSys = IdentifierList[i + 1];
                            if (LocSys.StartsWith("LocalisationSystemID_"))
                            {
                                int LocalisationSystemID;
                                if (int.TryParse(LocSys.Substring("LocalisationSystemID_".Length), out LocalisationSystemID))
                                {
                                    if (LocalisationSystemID > 17 && LocalisationSystemID < 22)
                                    {
                                        if (LocalisationSystemID == 18)
                                            iImage = (int)OverviewImageTableOrField.Place2;
                                        if (LocalisationSystemID == 19)
                                            iImage = (int)OverviewImageTableOrField.Place3;
                                        if (LocalisationSystemID == 20)
                                            iImage = (int)OverviewImageTableOrField.Place4;
                                        if (LocalisationSystemID == 21)
                                            iImage = (int)OverviewImageTableOrField.Place5;
                                    }
                                    else
                                    {
                                        string ParsingMethod = DiversityCollection.LookupTable.ParsingMethodName(LocalisationSystemID);
                                        if (ParsingMethod == "Height")
                                        {
                                            string LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(LocalisationSystemID);
                                            iImage = Specimen.ImageForLocalisationSystem(LocalisationSystem, GreyImage);
                                        }
                                        else
                                            iImage = Specimen.ImageForLocalisationSystem(ParsingMethod, GreyImage);
                                    }
                                    return iImage;
                                }
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("responsible") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "CollectionSpecimenList":
                        iImage = (int)OverviewImage.Specimen;
                        break;
                    case "CollectionSpecimenPartDescription":
                        iImage = (int)OverviewImageTableOrField.PartDescription;
                        break;
                    case "CollectionSpecimen":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.Specimen;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            if (IdentifierList[i + 1].ToLower().IndexOf("reference") > -1)
                            {
                                iImage = (int)OverviewImageTableOrField.Reference;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("depositor") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("exsiccata") > -1)
                            {
                                iImage = (int)OverviewImage.Specimen;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "CollectionSpecimenImage":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImageStorage.Icones;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            if (IdentifierList[i + 1].ToLower().IndexOf("agenturi") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "CollectionAgent":
                        iImage = (int)OverviewImage.Agent;
                        break;
                    case "CollectionProject":
                        iImage = (int)OverviewImage.Project;
                        break;
                    case "Collection":
                        iImage = (int)OverviewImage.Collection;
                        break;
                    case "Identification":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.Identification;
                            return iImage;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            if (IdentifierList[i + 1].ToLower().IndexOf("reference") > -1)
                            {
                                iImage = (int)OverviewImageTableOrField.Reference;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("responsible") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            //else if (IdentifierList[i + 1].ToLower().IndexOf("nameuri") > -1)
                            //{
                            //    iImage = (int)OverviewImage.t;
                            //    if (GreyImage) iImage++;
                            //    return iImage;
                            //}
                        }
                        break;
                    case "IdentificationUnitList":
                    case "IdentificationUnit":
                        string TaxonomicGroup = "";// R["TaxonomicGroup"].ToString();
                        //string UnitDescription = "";
                        //if (R.Table.Columns.Contains("UnitDescription"))
                        //{
                        //    try { UnitDescription = R["UnitDescription"].ToString(); }
                        //    catch { }
                        //    if (UnitDescription.Length == 0)
                        //        iImage = Specimen.TaxonImage(TaxonomicGroup, false);
                        //    else
                        //        iImage = Specimen.TaxonImageIndex(TaxonomicGroup, UnitDescription, false);
                        //}
                        //else
                        //    iImage = Specimen.TaxonImage(TaxonomicGroup, false);
                        break;
                    case "TaxonomicGroup":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = -1;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            iImage = Specimen.TaxonImage(IdentifierList[i + 1], false);
                            return iImage;
                        }
                        break;
                    case "MaterialCategory":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = -1;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            iImage = Specimen.MaterialCategoryImage(IdentifierList[i + 1], false);
                            return iImage;
                        }
                        break;
                    case "IdentificationUnitAnalysis":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.Analysis;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            if (IdentifierList[i + 1].ToLower().IndexOf("reference") > -1)
                            {
                                iImage = (int)OverviewImageTableOrField.Reference;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("responsible") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "IdentificationUnitGeoAnalysis":
                        iImage = (int)OverviewImage.GeoAnalysis;
                        break;
                    case "CollectionSpecimenProcessing":
                        if (i == IdentifierList.Count - 1)
                        {
                            iImage = (int)OverviewImage.Processing;
                        }
                        else if (i == IdentifierList.Count - 2)
                        {
                            if (IdentifierList[i + 1].ToLower().IndexOf("reference") > -1)
                            {
                                iImage = (int)OverviewImageTableOrField.Reference;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                            else if (IdentifierList[i + 1].ToLower().IndexOf("responsible") > -1)
                            {
                                iImage = (int)OverviewImage.Agent;
                                if (GreyImage) iImage++;
                                return iImage;
                            }
                        }
                        break;
                    case "CollectionSpecimenRelation":
                        bool isInternal = true;
                        //bool.TryParse(R["IsInternalRelationCache"].ToString(), out isInternal);
                        //if (isInternal) iImage = (int)OverviewImage.Relation;
                        //else if (GreyImage)
                        //{
                        //    iImage = (int)OverviewImageTableOrField.RelationExternGrey;
                        //    iImage--;
                        //}
                        //else iImage = (int)OverviewImage.RelationExtern;
                        break;
                    case "CollectionSpecimenRelationInvers":
                        iImage = (int)OverviewImage.RelationInversGrey;
                        if (GreyImage) iImage--;
                        break;
                    case "CollectionSpecimenRelationInternal":
                        iImage = (int)OverviewImageTableOrField.RelationInvers;
                        if (GreyImage) iImage++;
                        break;
                    case "CollectionSpecimenRelationExternal":
                        iImage = (int)OverviewImageTableOrField.RelationExtern;
                        if (GreyImage) iImage++;
                        break;
                    case "CollectionSpecimenPart":
                        //string MaterialCategory = R["MaterialCategory"].ToString();
                        //iImage = Specimen.MaterialCategoryImage(MaterialCategory, false);
                        break;
                    case "CollectionSpecimenTransaction":
                        //int TransactionID = int.Parse(R["TransactionID"].ToString());
                        //string Type = DiversityCollection.LookupTable.TransactionType(TransactionID);
                        //switch (Type)
                        //{
                        //    case "loan":
                        //        iImage = (int)OverviewImage.Loan;
                        //        break;
                        //    case "embargo":
                        //        iImage = (int)OverviewImageTableOrField.Embargo;
                        //        break;
                        //    case "forwarding":
                        //        iImage = (int)OverviewImageTableOrField.Forwarding;
                        //        break;
                        //    case "permit":
                        //        iImage = (int)OverviewImageTableOrField.Permit;
                        //        break;
                        //    case "return":
                        //        iImage = (int)OverviewImageTableOrField.Return;
                        //        break;
                        //    case "borrow":
                        //        iImage = (int)OverviewImageTableOrField.Borrow;
                        //        break;
                        //    case "gift":
                        //        iImage = (int)OverviewImageTableOrField.Gift;
                        //        break;
                        //    case "request":
                        //        iImage = (int)OverviewImageTableOrField.Request;
                        //        break;
                        //    case "purchase":
                        //        iImage = (int)OverviewImageTableOrField.Purchase;
                        //        break;
                        //    case "inventory":
                        //        iImage = (int)OverviewImageTableOrField.Inventory;
                        //        break;
                        //    case "exchange":
                        //        iImage = (int)OverviewImageTableOrField.Exchange;
                        //        break;
                        //    case "transaction group":
                        //        iImage = (int)OverviewImageTableOrField.Purchase;
                        //        break;
                        //    case "removal":
                        //        iImage = (int)OverviewImageTableOrField.Removal;
                        //        break;
                        //    default:
                        //        iImage = (int)OverviewImage.Transaction;
                        //        if (GreyImage) iImage--;
                        //        break;
                        //}
                        break;
                    case "Annotation":
                        //string AnnotationType = R["AnnotationType"].ToString();
                        //switch (AnnotationType)
                        //{
                        //    case "Annotation":
                        //        iImage = (int)OverviewImageTableOrField.Annotation;
                        //        break;
                        //    case "Problem":
                        //        iImage = (int)OverviewImageTableOrField.Problem;
                        //        break;
                        //    case "Reference":
                        //        iImage = (int)OverviewImageTableOrField.Reference;
                        //        break;
                        //    default:
                        //        iImage = (int)OverviewImageTableOrField.ID;
                        //        break;
                        //}
                        break;
                    case "Settings":
                        iImage = (int)OverviewImageTableOrField.Settings;
                        break;
                    case "Database":
                        iImage = (int)OverviewImageTableOrField.Database;
                        break;
                    case "Webservice":
                        iImage = (int)OverviewImageTableOrField.Webservice;
                        break;
                    case "ModuleSource":
                        iImage = (int)OverviewImageTableOrField.Pin;
                        break;
                    case "DiversityMobile":
                        iImage = (int)OverviewImageTableOrField.MobileDevice;
                        break;
                }
                if (GreyImage) iImage++;
            }
            return iImage;
         }

        public static int ImageIndex(string Identifier)
        {
            int i = 0;
            switch (Identifier)
            {
                case "TableColumn":
                    i = (int)OverviewImageTableOrField.TableColumn;
                    break;
                case "DiversityAgents":
                    i = (int)OverviewImageTableOrField.Task;
                    break;
                case "DiversityCollectionCache":
                    i = (int)OverviewImageTableOrField.CacheDB;
                    break;
                case "DiversityTaxonNames":
                    i = (int)OverviewImageTaxon.Animal;
                    break;
                case "DiversityWorkbench":
                    i = (int)OverviewImageTableOrField.DiversityWorkbench;
                    break;
                case "Date":
                case "Date from to":
                case "Time from to":
                    i = (int)OverviewImageTableOrField.Task;
                    break;
                default:
                    break;
            }
            return i;
        }

        #endregion

        #region Collection type

        public static System.Drawing.Image CollectionTypeImage(bool IsGrey, string CollectionType)
        {
            //if (CollectionType == "regulation")
            //    CollectionType = "Paragraph";
            if (Specimen.CollectionType_Images.ContainsKey(CollectionType))
                return Specimen.CollectionType_Images[CollectionType];
            else if (IsGrey && Specimen._CollectionType_ImagesGray.ContainsKey(CollectionType))
                return Specimen._CollectionType_ImagesGray[CollectionType];
            else
            {
                int I = DiversityCollection.Specimen.CollectionTypeImage(CollectionType, IsGrey);
                System.Drawing.Image Image = null;
                try
                {
                    if (DiversityCollection.Specimen.ImageListTaxon.Images.Count >= I + 1)
                        Image = DiversityCollection.Specimen.ImageListTaxon.Images[I];
                    else
                    {
                        if (Specimen.AddedImageIndex.ContainsKey(CollectionType.ToLower() + "|CollectionType|"))
                        {
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
                return Image;
            }
        }

        public static int CollectionTypeImage(string CollectionType, bool Grey)
        {
            int I = (int)OverviewImage.Collection;
            CollectionType = CollectionType.ToLower();
            switch (CollectionType)
            {
                case "box":
                    I = (int)OverviewImageTableOrField.Box;
                    break;
                case "drawer":
                    I = (int)OverviewImageTableOrField.Drawer;
                    break;
                case "freezer":
                    I = (int)OverviewImageTableOrField.Freezer;
                    break;
                case "fridge":
                    I = (int)OverviewImageTableOrField.Fridge;
                    break;
                case "institution":
                    I = (int)OverviewImageTableOrField.Institution;
                    break;
                case "cupboard":
                    I = (int)OverviewImageTableOrField.Cupboard;
                    break;
                case "subdivided container":
                    I = (int)OverviewImageTableOrField.SubdividedContainer;
                    break;
                case "container":
                    I = (int)OverviewImageTableOrField.Container;
                    break;
                case "location":
                    I = (int)OverviewImageTableOrField.Location;
                    break;
                case "department":
                    I = (int)OverviewImageTableOrField.Department;
                    break;
                case "radioactive":
                    I = (int)OverviewImageTableOrField.Radioactive;
                    break;
                case "room":
                    I = (int)OverviewImageTableOrField.Room;
                    break;
                case "rack":
                    I = (int)OverviewImageTableOrField.Rack;
                    break;
                case "steel locker":
                    I = (int)OverviewImageTableOrField.SteelLocker;
                    break;
                case "regulation":
                    I = (int)OverviewImageTableOrField.Paragraph;
                    break;
                case "task":
                    I = (int)OverviewImageTableOrField.Task;
                    break;
                case "sensor":
                    I = (int)OverviewImageTableOrField.Sensor;
                    break;
                case "sensorhum":
                    I = (int)OverviewImageTableOrField.SensorHumidity;
                    break;
                case "sensortemp":
                    I = (int)OverviewImageTableOrField.SensorTemperature;
                    break;
                case "trap":
                    I = (int)OverviewImageTableOrField.Trap;
                    break;
                case "trapped":
                    I = (int)OverviewImageTableOrField.Trapped;
                    break;
                case "trapended":
                    I = (int)OverviewImageTableOrField.TrapEnded;
                    break;
                case "battery":
                    I = (int)OverviewImageTableOrField.Battery;
                    break;
                case "area":
                    I = (int)OverviewImageTableOrField.Area;
                    break;
                case "hardware":
                    I = (int)OverviewImageTableOrField.Hardware;
                    break;
                default:
                    if (Specimen.AddedImageIndex.ContainsKey(CollectionType + "|CollectionType|"))
                        I = Specimen.AddedImageIndex[CollectionType + "|CollectionType|"];
                    else
                        I = (int)OverviewImage.Collection;
                    break;
            }
            if (Grey) I++;
            return I;
        }

        //public static int IndexImageCollectionType(string Type, bool IsGrey)
        //{
        //    int i = 0;
        //    switch (Type)
        //    {
        //        case "embargo":
        //            i = 2;
        //            break;
        //        case "exchange":
        //            i = 6;
        //            break;
        //        case "inventory":
        //            i = 7;
        //            break;
        //        case "loan":
        //            i = 4;
        //            break;
        //        case "request":
        //            i = 8;
        //            break;
        //        case "forwarding":
        //            i = 9;
        //            break;
        //        case "permit":
        //            i = 10;
        //            break;
        //        case "return":
        //            i = 11;
        //            break;
        //        case "gift":
        //            i = 12;
        //            break;
        //        case "borrow":
        //            i = 13;
        //            break;
        //        case "purchase":
        //            i = 14;
        //            break;
        //        case "transaction group":
        //            i = 15;
        //            break;
        //        case "removal":
        //            i = 16;
        //            break;
        //        case "permanent loan":
        //            i = 17;
        //            break;
        //    }
        //    if (IsInactive != null && (bool)IsInactive)
        //        i++;
        //    return i;
        //}

        #endregion

        #region Taxa
        
        public static System.Drawing.Image TaxonImage(bool IsGrey, string TaxonomicGroup)
        {
            if (Specimen.TaxonomicGroup_Images.ContainsKey(TaxonomicGroup))
                return Specimen.TaxonomicGroup_Images[TaxonomicGroup];
            else if (IsGrey && Specimen._TaxonomicGroup_ImagesGray.ContainsKey(TaxonomicGroup))
                return Specimen._TaxonomicGroup_ImagesGray[TaxonomicGroup];
            else
            {
                int I = DiversityCollection.Specimen.TaxonImage(TaxonomicGroup, IsGrey);
                System.Drawing.Image Image = null;
                try
                {
                    if (DiversityCollection.Specimen.ImageListTaxon.Images.Count >= I + 1)
                        Image = DiversityCollection.Specimen.ImageListTaxon.Images[I];
                    else 
                    {
                        if (Specimen.AddedImageIndex.ContainsKey(TaxonomicGroup.ToLower() + "|TaxonomicGroup|"))
                        {
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
                return Image;
            }
        }

        public static int TaxonImage(string TaxonomicGroup, bool Grey)
        {
            int I = (int)OverviewImageTaxon.Other;
            TaxonomicGroup = TaxonomicGroup.ToLower();
            switch (TaxonomicGroup)
            {
                case "other":
                    I = (int)OverviewImageTableOrField.Other2;
                    break;
                case "organism":
                    I = (int)OverviewImageTableOrField.Organism; // #115
                    break;
                case "virus":
                    I = (int)OverviewImageTaxon.Virus;
                    break;
                case "bacterium":
                    I = (int)OverviewImageTaxon.Bacterium;
                    break;
                case "fungus":
                    I = (int)OverviewImageTaxon.Fungus;
                    break;
                case "lichen":
                    I = (int)OverviewImageTaxon.Lichen;
                    break;
                case "alga":
                    I = (int)OverviewImageTaxon.Alga;
                    break;
                case "bryophyte":
                    I = (int)OverviewImageTaxon.Bryophyt;
                    break;
                case "plant":
                    I = (int)OverviewImageTaxon.Plant;
                    break;
                case "myxomycete":
                    I = (int)OverviewImageTaxon.Myxomycete;
                    break;
                case "insect":
                    I = (int)OverviewImageTaxon.Insect;
                    break;
                case "coleoptera":
                    I = (int)OverviewImageTableOrField.Coleoptera;
                    break;
                case "diptera":
                    I = (int)OverviewImageTableOrField.Diptera;
                    break;
                case "heteroptera":
                    I = (int)OverviewImageTableOrField.Heteroptera;
                    break;
                case "hymenoptera":
                    I = (int)OverviewImageTableOrField.Hymenoptera;
                    break;
                case "lepidoptera":
                    I = (int)OverviewImageTableOrField.Lepidoptera;
                    break;
                case "mammal":
                    I = (int)OverviewImageTaxon.Mammal;
                    break;
                case "mollusc":
                    I = (int)OverviewImageTaxon.Mollusc;
                    break;
                case "bird":
                    I = (int)OverviewImageTaxon.Bird;
                    break;
                case "fish":
                    I = (int)OverviewImageTaxon.Fish;
                    break;
                case "animal":
                    I = (int)OverviewImageTaxon.Animal;
                    break;
                case "arthropod":
                case "arthropode":
                    I = (int)OverviewImageTaxon.Arthropod;
                    break;
                case "echinoderm":
                    I = (int)OverviewImageTaxon.Echinoderm;
                    break;
                case "vertebrate":
                    I = (int)OverviewImageTaxon.Vertebrate;
                    break;
                case "amphibian":
                    I = (int)OverviewImageTaxon.Amphibian;
                    break;
                case "reptile":
                    I = (int)OverviewImageTaxon.Reptile;
                    break;
                case "evertebrate":
                    I = (int)OverviewImageTaxon.Evertebrate;
                    break;
                case "cnidaria":
                    I = (int)OverviewImageTaxon.Cnidaria;
                    break;
                case "soil":
                    I = (int)OverviewImageTaxon.Soil;
                    break;
                case "soil horizon":
                    I = (int)OverviewImageTableOrField.SoilHorizon;
                    break;
                case "rock":
                    I = (int)OverviewImageTableOrField.Rock;
                    break;
                case "mineral":
                    I = (int)OverviewImageTableOrField.Mineral;
                    break;
                case "artefact":
                    I = (int)OverviewImageTableOrField.Model;
                    break;
                case "herbivore":
                    I = (int)OverviewImageTaxon.Herbivore;
                    break;
                case "gall":
                    I = (int)OverviewImageUnitPart.Gall;
                    break;
                case "archaea":
                    I = (int)OverviewImageTableOrField.Archaea;
                    break;
                case "chromista":
                    I = (int)OverviewImageTableOrField.Chromista;
                    break;
                case "protozoa":
                    I = (int)OverviewImageTableOrField.Protozoa;
                    break;
                case "spider":
                    I = (int)OverviewImageTableOrField.Spider;
                    break;
                case "human":
                    I = (int)OverviewImageTableOrField.Human;
                    break;
                default:
                    if (Specimen.AddedImageIndex.ContainsKey(TaxonomicGroup + "|TaxonomicGroup|"))
                        I = Specimen.AddedImageIndex[TaxonomicGroup + "|TaxonomicGroup|"];
                    else
                        I = (int)OverviewImageTableOrField.Other2;
                    break;
            }
            if (Grey) I++;
            return I;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TaxonomicGroup">the taxonomic group</param>
        /// <param name="UnitDescription">the part of the unit, e.g. branch of leaf of a tree</param>
        /// <param name="Grey">if the grey variant should be taken</param>
        /// <returns>the index of the image corresponding to the taxonomic group</returns>
        public static int TaxonImageIndex(string TaxonomicGroup, string UnitDescription, bool Grey)
        {
            int I = (int)OverviewImageTableOrField.Other2;
            TaxonomicGroup = TaxonomicGroup.ToLower();
            UnitDescription = UnitDescription.ToLower();
            I = DiversityCollection.Specimen.TaxonImage(TaxonomicGroup, Grey);
            switch (TaxonomicGroup)
            {
                case "plant":
                    if (DiversityCollection.Specimen.UnitPartList(TaxonomicGroup).ContainsKey(UnitDescription))
                        I = DiversityCollection.Specimen.UnitPartList(TaxonomicGroup)[UnitDescription];
                    break;
                case "bird":
                    if (DiversityCollection.Specimen.UnitPartList(TaxonomicGroup).ContainsKey(UnitDescription))
                        I = DiversityCollection.Specimen.UnitPartList(TaxonomicGroup)[UnitDescription];
                    break;
            }
            return I;
        }

        public static System.Collections.Generic.SortedDictionary<string, int> UnitPartList(string TaxonomicGroup)
        {
            System.Collections.Generic.SortedDictionary<string, int> Dict = new SortedDictionary<string, int>();
            switch (TaxonomicGroup)
            {
                case "plant":
                    Dict.Add("tree", (int)OverviewImageUnitPart.Tree);
                    Dict.Add("baum", (int)OverviewImageUnitPart.Tree);
                    Dict.Add("strauch", (int)OverviewImageUnitPart.Tree);

                    Dict.Add("branch", (int)OverviewImageUnitPart.Branch);
                    Dict.Add("twig", (int)OverviewImageUnitPart.Branch);
                    Dict.Add("ast", (int)OverviewImageUnitPart.Branch);
                    Dict.Add("zweig", (int)OverviewImageUnitPart.Branch);

                    Dict.Add("leaf", (int)OverviewImageUnitPart.Leaf);
                    Dict.Add("blatt", (int)OverviewImageUnitPart.Leaf);

                    Dict.Add("root", (int)OverviewImageUnitPart.Root);
                    Dict.Add("wurzel", (int)OverviewImageUnitPart.Root);

                    Dict.Add("gall", (int)OverviewImageUnitPart.Gall);
                    Dict.Add("galle", (int)OverviewImageUnitPart.Gall);

                    break;
                    //case "bird":
                    //    Dict.Add("gesang", (int)OverviewImageUnitPart.Sound);
                    //    Dict.Add("song", (int)OverviewImageUnitPart.Sound);
                    //    Dict.Add("voice", (int)OverviewImageUnitPart.Sound);

                    break;
            }
            return Dict;
        }

        #endregion

        #region Material
        
        public static System.Drawing.Image MaterialCategoryImage(bool IsGrey, string MaterialCategory)
        {
            try
            {
                if (Specimen.MaterialCategory_Images.ContainsKey(MaterialCategory))
                    return Specimen._MaterialCategory_Images[MaterialCategory];
                else if (IsGrey && Specimen._MaterialCategory_ImagesGray.ContainsKey(MaterialCategory))
                    return Specimen._MaterialCategory_ImagesGray[MaterialCategory];
                else
                {
                    int I = DiversityCollection.Specimen.MaterialCategoryImage(MaterialCategory, IsGrey);
                    System.Drawing.Image Image = null;
                    try
                    {
                        if (DiversityCollection.Specimen.ImageListTaxon.Images.Count >= I + 1)
                            Image = DiversityCollection.Specimen.ImageListStorage.Images[I];
                        else if (DiversityCollection.Specimen.AddedImageIndex.ContainsKey(MaterialCategory + "|MaterialCategory|"))
                        {
                            if (DiversityCollection.Specimen._ImageList.Images.ContainsKey(MaterialCategory) && !IsGrey)
                                Image = DiversityCollection.Specimen._ImageList.Images[MaterialCategory];
                            else if (DiversityCollection.Specimen._ImageList.Images.ContainsKey(MaterialCategory + "_grey") && IsGrey)
                                Image = DiversityCollection.Specimen._ImageList.Images[MaterialCategory + "_grey"];
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    return Image;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return DiversityCollection.Resource.NULL;
        }

        public static System.Drawing.Image MaterialCategoryImage(DiversityCollection.Specimen.OverviewImageState State, string MaterialCategory)
        {
            int I = DiversityCollection.Specimen.MaterialCategoryImage(MaterialCategory, false);
            if (State == OverviewImageState.Grey) I++;
            else if (State == OverviewImageState.Hierarchy) I = I + 2;
            System.Drawing.Image Image = DiversityCollection.Specimen.ImageListStorage.Images[I];
            return Image;
        }

        public static int MaterialCategoryImage(string MaterialCategory, bool Grey)
        {
            int I = (int)OverviewImageStorage.Specimen;
            MaterialCategory = MaterialCategory.ToLower().Trim();
            switch (MaterialCategory)
            {
                case "skull":
                case "bones":
                case "fossil bones":
                case "fossil skull":
                    I = (int)OverviewImageStorage.Bones;
                    break;
                case "single bones":
                case "fossil single bones":
                    I = (int)OverviewImageStorage.Bone;
                    break;
                case "complete skeleton":
                case "fossil complete skeleton":
                    I = (int)OverviewImageStorage.Skeleton;
                    break;
                case "postcranial skeleton":
                case "fossil postcranial skeleton":
                    I = (int)OverviewImageStorage.Postcranial;
                    break;
                case "incomplete skeleton":
                case "fossil incomplete skeleton":
                    I = (int)OverviewImageStorage.Incomplete;
                    break;
                case "cultures":
                    I = (int)OverviewImageStorage.Culture;
                    break;
                case "drawing":
                    I = (int)OverviewImageStorage.Drawing;
                    break;
                case "dna sample":
                    I = (int)OverviewImageStorage.DNA;
                    break;
                case "dna lyophilised":
                    I = (int)OverviewImageTableOrField.DnaLyophilised;
                    break;
                case "herbarium sheets":
                    I = (int)OverviewImageStorage.HerbariumSheet;
                    break;
                case "icones":
                    I = (int)OverviewImageStorage.Icones;
                    break;
                case "photogr. print":
                    I = (int)OverviewImageStorage.Photo;
                    break;
                case "sem table":
                    I = (int)OverviewImageStorage.SemTable;
                    break;
                case "micr. slide":
                    I = (int)OverviewImageStorage.Slide;
                    break;
                case "specimen":
                    I = (int)OverviewImageStorage.Specimen;
                    break;
                case "pelt":
                    I = (int)OverviewImageStorage.Pelt;
                    break;
                case "photogr. slide":
                    I = (int)OverviewImageStorage.Dia;
                    break;
                case "tem specimen":
                    I = (int)OverviewImageStorage.TemGrid;
                    break;
                case "vial":
                    I = (int)OverviewImageStorage.Vial;
                    break;
                case "drawing or photograph":
                case "medium":
                    I = (int)OverviewImageStorage.Medium;
                    break;
                case "fossile specimen":
                    I = (int)OverviewImageStorage.Fossil;
                    break;
                case "human observation":
                case "machine observation":
                case "observation":
                    I = (int)OverviewImageStorage.Observation;
                    break;
                case "thin section":
                    I = (int)OverviewImageStorage.Thinsection;
                    break;
                case "trace":
                case "trace fossil":
                    I = (int)OverviewImageStorage.Trace;
                    break;
                case "fossil specimen":
                case "fossil shell":
                    I = (int)OverviewImageStorage.Fossil;
                    break;
                case "sound":
                    I = (int)OverviewImageStorage.Sound;
                    break;
                case "shell":
                    I = (int)OverviewImageTaxon.Evertebrate;
                    break;
                case "tooth":
                case "fossil tooth":
                    I = (int)OverviewImageStorage.Tooth;
                    break;
                case "otolith":
                case "fossil otolith":
                    I = (int)OverviewImageStorage.Otolith;
                    break;
                case "fossil scales":
                    I = (int)OverviewImageStorage.Scales;
                    break;
                case "mould":
                    I = (int)OverviewImageStorage.Mould;
                    break;
                case "tissue sample":
                    I = (int)OverviewImageStorage.Tissue;
                    break;
                case "nest":
                    I = (int)OverviewImageStorage.Nest;
                    break;
                case "egg":
                    I = (int)OverviewImageStorage.Egg;
                    break;
                case "genitalia":
                    I = (int)OverviewImageTableOrField.Genitalia;
                    break;
                case "literature":
                    I = (int)OverviewImageTableOrField.Literature;
                    break;
                case "subcollection":
                    I = (int)OverviewImageTableOrField.Subcollection;
                    break;
                case "cultural specimens":
                    I = (int)OverviewImageTableOrField.Model;
                    break;
                case "mineral specimen":
                    I = (int)OverviewImageTableOrField.MineralSpecimen;
                    break;
                case "earth science specimen":
                    I = (int)OverviewImageTableOrField.Geology;
                    break;
                case "core sample":
                case "core":
                    I = (int)OverviewImageTableOrField.Core;
                    break;
                case "material sample":
                    I = (int)OverviewImageTaxon.Other;
                    break;
                case "pinned specimen":
                    I = (int)OverviewImageTableOrField.Pinned;
                    break;
                default:
                    if (Specimen.AddedImageIndex.ContainsKey(MaterialCategory + "|MaterialCategory|"))
                        I = Specimen.AddedImageIndex[MaterialCategory + "|MaterialCategory|"];
                    else
                        I = (int)(int)OverviewImageStorage.Specimen;
                    break;
                //default:
                //    I = (int)OverviewImageStorage.Specimen;
                //    break;
            }
            if (Grey) I++;
            return I;
        }

        public static System.Collections.Generic.Dictionary<string, System.Drawing.Image> MaterialCategoryImages()
        {
            if (DiversityCollection.Specimen._ImagesMaterialCategory == null)
            {
                foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtMaterialCategories.Rows)
                {
                    System.Drawing.Image I = DiversityCollection.Specimen.MaterialCategoryImage(false, R["Code"].ToString());
                    if (I != null)
                        DiversityCollection.Specimen._ImagesMaterialCategory.Add(R["Code"].ToString(), I);
                }
            }
            return DiversityCollection.Specimen._ImagesMaterialCategory;
        }

        #endregion

        #region Enums

        public enum OverviewImage
        {
            /*00-09*/ Null, EventSeriesHierarchy, EventSeriesHierarchyGrey, EventSeries, EventSeriesGrey, SamplingPlotHierachy, SamplingPlotHierarchyGray, SamplingPlot, SamplingPlotGray, Event, 
            /*10-19*/ EventGrey, Localisation, LocalisationGrey, EventProperty, EventPropertyGrey, Specimen, SpecimenGrey, Collection, CollectionGrey, Identification, 
            /*20-29*/ IdentificationGrey, Analysis, AnalysisGrey, AnalysisHierarchy, GeoAnalysis, GeoAnalysisGrey, Processing, ProcessingGrey, ProcessingHierarchy, Loan, 
            /*30-40*/ LoanGrey, Transaction, TransactionGrey, Agent, AgentGrey, Relation, RelationGrey, RelationExtern, RelationInversGrey, Project, ProjectGrey
        }

        public enum OverviewImageStorage
        {
            /*41-44*/ Specimen = 41, SpecimenGrey, HerbariumSheet, HerbariumSheetGrey, 
            /*45-49*/ Icones, IconesGrey, Drawing, DrawingGrey, Photo, 
            /*50-54*/ PhotoGrey, Dia, DiaGrey, Slide, SlideGrey, 
            /*55-59*/ Thinsection, ThinsectionGrey, Culture, CultureGrey, Vial,  
            /*60-64*/ VialGrey, SemTable, SemTableGrey, TemGrid, TemGridGrey, 
            /*65-69*/ Bones, BonesGrey, Skeleton, SkeletonGrey, Postcranial, 
            /*70-74*/ PostcranialGrey, Incomplete, IncompleteGrey, Bone, BoneGrey, 
            /*75-79*/ Tooth, ToothGrey, Pelt, PeltGrey, DNA, 
            /*80-84*/ DNAGrey, Observation, ObservationGrey, Fossil, FossilGrey, 
            /*85-89*/ Medium, MediumGrey, Trace, TraceGrey, Sound, 
            /*90-94*/ SoundGrey, Otolith, OtolithGrey, Scales, ScalesGrey,
            /*95-99*/ Mould, MouldGrey, Tissue, TissueGrey, Nest, 
            /*100-102*/ NestGrey, Egg, EggGrey
        }

        public enum OverviewImageTaxon
        {
            /*103-104*/ Other = 103, OtherGrey,  
            /*105-109*/ Virus, VirusGrey, Bacterium, BacteriumGrey, Fungus, 
            /*110-114*/ FungusGrey, Lichen, LichenGrey, Alga, AlgaGrey,
            /*115-119*/ Bryophyt, BryophytGrey, Plant, PlantGrey, Myxomycete, 
            /*120-124*/ MyxomyceteGrey, Insect, InsectGrey, Mammal, MammalGrey, 
            /*125-129*/ Mollusc, MolluscGrey, Bird, BirdGrey, Fish, 
            /*130-134*/ FishGrey, Arthropod, ArthropodGrey, Echinoderm, EchinodermGrey, 
            /*135-139*/ Vertebrate, VertebrateGrey, Reptile, ReptileGrey, Evertebrate,
            /*140-144*/ EvertebrateGrey, Cnidaria, CnidariaGrey, Soil, SoilGrey,
            /*145-148*/ Herbivore, HerbivoreGrey, Animal, AnimalGrey, Amphibian, 
            /*150*/AmphibianGrey
        }

        public enum OverviewImageUnitPart
        {
            /*151-154*/ Tree = 151, TreeGrey, Branch, BranchGrey, Leaf, 
            /*155-162*/  LeafGrey, Root, RootGrey, Gall, GallGrey, Flower, FlowerGrey 
        }

        public enum OverviewImageTableOrField
        {
            /*163-164*/ Label = 163, LabelGrey, 
            /*165-169*/ Annotation, AnnotationAdd, Genitalia, GenitaliaGrey, Altitide, 
            /*170-174*/ AltitideGrey, Depth, DepthGrey, Exposition, ExpositionGrey, 
            /*175-179*/ GaussKrueger, GaussKruegerGrey, Height, HeightGrey, MTB,
            /*180-184*/ MTBGrey, Place, PlaceGrey, Slope, SlopeGrey, 
            /*185-189*/ Stratigraphy, StratigraphyGrey, Embargo, EmbargoGrey, RelationExtern, 
            /*190-194*/ RelationExternGrey, Parent, ParentGrey, Tool, ToolGrey,
            /*195-199*/ Parameter, ParameterGrey, Document, DocumentGrey, Hierarchy,
            /*200-204*/ HierarchyGrey, UnitInPart, UnitInPartGrey, Forwarding, ForwardingGrey,
            /*205-209*/ Permit, PermitGrey, Return, ReturnGrey, Borrow,
            /*210-214*/ BorrowGrey, Gift, GiftGrey, Request, RequestGrey,
            /*215-219*/ Purchase, PurchaseGrey, Inventory, InventoryGrey, Exchange,
            /*220-224*/ ExchangeGrey, ImageArea, ImageAreaGrey, Removal, RemovalGrey,
            /*225-229*/ Archaea, ArchaeaGrey, Chromista, ChromistaGrey, Protozoa,
            /*230-234*/ ProtozoaGrey, Problem, ProblemGrey, Reference, ReferenceGrey, 
            /*235-239*/ Place2, Place2Grey, Place3, Place3Grey, Place4,
            /*240-244*/ Place4Grey, Place5, Place5Grey, ID, IDGrey, 
            /*245-249*/ Literature, LiteratureGrey, Spider, SpiderGray, Subcollection,
            /*250-254*/ SubcollectionGray, Payment, PaymentGray, Paragraph, ParagraphGray,
            /*255-259*/ DnaLyophilised, DnaLyophilisedGray, PartDescription, PartDescriptionGray, Mineral, 
            /*260-264*/ MineralGray, Rock, RockGray, Database, DatabaseGray,
            /*265-269*/ Webservice, WebserviceGray, Human, HumanGray, Model, 
            /*270-274*/ ModelGray, Geology, GeologyGray, MobileDevice, MobileDeviceGray, 
            /*275-279*/ Pin, PinGrey, Settings, SettingsGrey, Box, 
            /*280-284*/ BoxGrey, Cupboard, CupboardGrey, Drawer, DrawerGrey,
            /*285-289*/ Institution, InstitutionGrey, Radioactive, RadioactiveGrey, Room,
            /*290-294*/ RoomGrey, SteelLocker, SteelLockerGrey, Freezer, FreezerGrey,
            /*295-299*/ Fridge, FridgeGrey, MineralSpecimen, MineralSpecimenGrey, Other2,
            /*300-304*/ Other2Grey, TableColumn, TableColumnGray, CacheDB, CacheDBGray,
            /*305-309*/ DiversityWorkbench, DiversityWorkbenchGray, SoilHorizon, SoilHorizonGrey, IdentificationAdd,
            /*310-314*/ IdentificationAddGrey, Section, SectionGrey, Task, TaskGray, 
            /*315-319*/ Trap, TrapGrey, Gas, GasGrey, Poison,
            /*320-324*/ PoisonGrey, Radiation, RadiationGrey, Bug, BugGrey,
            /*325-329*/ Date, DateGray, Terminology, TerminologyGrey, Descriptions,
            /*330-334*/ DescriptionsGrey, DiversityAgents, DiversityAgentsGrey, DiversityCollection, DiversityCollectionGrey,
            DiversityGazetteer, DiversityGazetteerGrey, DiversityProjects, DiversityProjectsGrey, DiversityReferences,
            DiversityReferencesGrey, DiversitySamplingPlots, DiversitySamplingPlotsGrey, DiversityScientificTerms, DiversityScientificTermsGrey,
            DiversityTaxonNames, DiversityTaxonNamesGrey, Gazetteer, GazetteerGrey, Inspection,
            InspectionGrey, Query, QueryGrey, Graph, GraphGrey,
            Repair, RepairGrey, Damage, DamageGrey, Cleaning,
            CleaningGrey, CollectionTask, CollectionTaskGrey, Sensor, SensorGrey,
            Warning, WarningGrey, SubdividedContainer, SubdividedContainerGray, Trapped,
            TrapEnded, SensorHumidity, SensorHumidityGrey, SensorTemperature, SensorTemperatureGrey,
            WetCollection, WetCollectionGrey, Container, ContainerGrey, Department,
            DepartmentGrey, Location, LocationGrey, Battery, BatteryGrey,
            Diptera, DipteraGrey, Lepidoptera, LepidopteraGrey, Heteroptera,
            HeteropteraGrey, Hymenoptera, HymenopteraGrey, Coleoptera, ColeopteraGrey,
            Exhibition, ExhibitionGrey, Area, AreaGrey, Pinned,
            PinnedGrey, UTM, UTMgrey, Hardware, HardwareGrey,
            RelationInvers, RelationInversGray, Rack, RackGrey, KeyBlue, 
            KeyBlueGray, Organism, OrganismGrey, Core, CoreGray // #115
        }

        public enum OverviewImageState { Plain, Grey, Hierarchy }

        public enum OverviewImageUser { Agent, Manager, Requester, User, CollectionManager, Depositor }

        public enum OverviewImageLocalisationSystem { Localisation, Place, Altitide, Country, Depth, Exposition, GaussKrueger, Height, MTB, Slope, SamplingPlot, Place2, Place3, Place4, Place5, UTM }

        #endregion 

        #region Table or field

        public static System.Collections.Generic.List<string> DisplayGroups
        {
            get
            {
                System.Collections.Generic.List<string> Groups = new List<string>();
                Groups.Add("Coordinates WGS84");
                Groups.Add("Stratigraphy");
                Groups.Add("Lithostratigraphy");
                Groups.Add("Chronostratigraphy");
                Groups.Add("Biostratigraphy");
                Groups.Add("Sampling plot");
                Groups.Add("Altitude");
                Groups.Add("Depth");
                Groups.Add("MTB");
                Groups.Add("TK25");
                Groups.Add("Place");
                Groups.Add("Place 2");
                Groups.Add("Place 3");
                Groups.Add("Place 4");
                Groups.Add("Place 5");
                Groups.Add("UTM");
                return Groups;
            }
        }
        
        public static int TableImage(string TableName, bool Grey)
        {
            int I = (int)OverviewImage.Specimen;
            if (TableName.EndsWith("_Core"))
                TableName = TableName.Substring(0, TableName.Length - 5);
            if (TableName.EndsWith("_Core2"))
                TableName = TableName.Substring(0, TableName.Length - 6);
            TableName = TableName.ToLower();
            switch (TableName)
            {
                case "collection":
                    I = (int)OverviewImage.Collection;
                    break;
                case "collectiontask":
                    I = (int)OverviewImageTableOrField.CollectionTask;
                    break;
                case "task":
                    I = (int)OverviewImageTableOrField.Task;
                    break;
                case "collectionagent":
                case "collectionmanager":
                    I = (int)OverviewImage.Agent;
                    break;
                case "collectionspecimenpart":
                    I = (int)OverviewImageStorage.Specimen;
                    break;
                case "processing":
                case "collectionspecimenprocessing":
                    I = (int)OverviewImage.Processing;
                    break;
                case "collectionspecimenpartdescription":
                    I = (int)OverviewImageTableOrField.PartDescription;
                    break;
                case "collectionspecimenrelation":
                    I = (int)OverviewImage.Relation;
                    break;
                case "collectionspecimenrelationinvers":
                    I = (int)OverviewImage.RelationInversGrey;
                    break;
                case "collectionspecimenrelationinternal":
                    I = (int)OverviewImageTableOrField.RelationInvers;
                    break;
                case "collectionspecimenrelationexternal":
                    I = (int)OverviewImageTableOrField.RelationExtern;
                    break;
                case "collectionevent":
                    I = (int)OverviewImage.Event;
                    break;
                case "method":
                case "collectioneventmethod":
                    I = (int)OverviewImageTableOrField.Tool;
                    break;
                case "parameter":
                case "collectioneventparametervalue":
                    I = (int)OverviewImageTableOrField.Parameter;
                    break;
                case "collectioneventproperty":
                    I = (int)OverviewImage.EventProperty;
                    break;
                case "collectioneventlocalisation":
                    I = (int)OverviewImage.Localisation;
                    break;
                case "collectionspecimenimage":
                case "collectioneventimage":
                case "collectioneventseriesimage":
                    I = (int)OverviewImageStorage.Icones;
                    break;
                case "collectioneventseries":
                    I = (int)OverviewImage.EventSeries;
                    break;
                case "collectioneventseriesdescriptor":
                    I = (int)OverviewImageTableOrField.KeyBlue;
                    break;
                case "collectioneventregulation":
                case "collectionspecimenpartregulation":
                    I = (int)OverviewImageTableOrField.Paragraph;
                    break;
                case "collectionspecimen":
                    I = (int)OverviewImage.Specimen;
                    break;
                case "collectionproject":
                    I = (int)OverviewImage.Project;
                    break;
                case "transaction":
                case "collectionspecimentransaction":
                    I = (int)OverviewImage.Transaction;
                    break;
                case "collectionspecimenreference":
                    I = (int)OverviewImageTableOrField.Reference;
                    break;
                case "identificationunit":
                    I = (int)OverviewImageTaxon.Plant;
                    break;
                case "identificationunitinpart":
                    I = (int)OverviewImageTableOrField.UnitInPart;
                    break;
                case "analysis":
                case "identificationunitanalysis":
                    I = (int)OverviewImage.Analysis;
                    break;
                case "identificationunitgeoanalysis":
                    I = (int)OverviewImage.GeoAnalysis;
                    break;
                case "identificationunitanalysismethod":
                case "collectionspecimenprocessingmethod":
                    I = (int)OverviewImageTableOrField.Tool;
                    break;
                case "identificationunitanalysismethodparameter":
                case "collectionspecimenprocessingmethodparameter":
                    I = (int)OverviewImageTableOrField.Parameter;
                    break;
                case "identification":
                    I = (int)OverviewImage.Identification;
                    break;
                case "samplingplot":
                    I = (int)OverviewImage.SamplingPlot;
                    break;
                case "label":
                    I = (int)OverviewImageTableOrField.Label;
                    break;
                case "parent":
                    I = (int)OverviewImageTableOrField.Parent;
                    break;
                case "identifierevent":
                case "identifierspecimen":
                case "identifierpart":
                case "identifiertransaction":
                case "identifierunit":
                case "externalidentifier":
                    I = (int)OverviewImageTableOrField.ID;
                    break;
                case "annotation":
                    I = (int)OverviewImageTableOrField.Annotation;
                    break;
                case "transactiondocument":
                    I = (int)OverviewImageTableOrField.Document;
                    break;
                default:
                    I = (int)OverviewImage.Null;
                    break;
            }
            if (Grey) I++;
            return I;
        }

        public static int TableOrGroupImage(string TableOrGroupName, bool Grey)
        {
            int I = (int)OverviewImage.Specimen;
            if (TableOrGroupName.EndsWith("_Core"))
                TableOrGroupName = TableOrGroupName.Substring(0, TableOrGroupName.Length - 5);
            TableOrGroupName = TableOrGroupName.ToLower();
            I = DiversityCollection.Specimen.TableImage(TableOrGroupName, Grey);
            switch (TableOrGroupName)
            {
                case "lithostratigraphy":
                case "chronostratigraphy":
                case "biostratigraphy":
                case "stratigraphy":
                    I = (int)OverviewImageTableOrField.Stratigraphy;
                    if (Grey) I++;
                    break;
                case "coordinates wgs84":
                    I = (int)OverviewImage.Localisation;
                    if (Grey) I++;
                    break;
                case "sampling plot":
                    I = (int)OverviewImage.SamplingPlot;
                    if (Grey) I++;
                    break;
                case "height":
                    I = (int)OverviewImageTableOrField.Height;
                    if (Grey) I++;
                    break;
                case "place":
                    I = (int)OverviewImageTableOrField.Place;
                    if (Grey) I++;
                    break;
                case "place 2":
                    I = (int)OverviewImageTableOrField.Place2;
                    if (Grey) I++;
                    break;
                case "place 3":
                    I = (int)OverviewImageTableOrField.Place3;
                    if (Grey) I++;
                    break;
                case "place 4":
                    I = (int)OverviewImageTableOrField.Place4;
                    if (Grey) I++;
                    break;
                case "place 5":
                    I = (int)OverviewImageTableOrField.Place5;
                    if (Grey) I++;
                    break;
                case "altitude":
                    I = (int)OverviewImageTableOrField.Altitide;
                    if (Grey) I++;
                    break;
                case "depth":
                    I = (int)OverviewImageTableOrField.Depth;
                    if (Grey) I++;
                    break;
                case "label":
                    I = (int)OverviewImageTableOrField.Label;
                    if (Grey) I++;
                    break;
                case "tk25":
                case "mtb":
                    I = (int)OverviewImageTableOrField.MTB;
                    if (Grey) I++;
                    break;
                case "utm":
                    I = (int)OverviewImageTableOrField.UTM;
                    if (Grey) I++;
                    break;
            }
            return I;
        }

        public enum ImageTableOrField
        {
            Null, EventSeries, Event, DateTime, Localisation, Habitat, Specimen, Depositor, Label, Unit,
            Identification, Analysis, Storage, Processing, Transaction, Agent, Relation, Project, Images,
            Database, Dictionary, Reference, Embargo, Tool, Parameter, Logging, Document, Hierarchy, Parent, UnitInPart, 
            Forwarding, Permit, Return, Borrow, Gift, Request, Purchase, Inventory, Exchange, ImageArea, Archaea, Chromista, Protozoa
        }

        public static int ImageTableOrFieldIndex(ImageTableOrField Index, bool Grey)
        {
            if (Grey) return (int)Index * 2;
            else return (int)Index * 2 - 1;
        }

        public static System.Windows.Forms.ImageList ImageListTablesAndFields
        {
            get
            {
                DiversityCollection.Specimen S = new Specimen();
                return S.imageListTablesAndFields;
            }
        }

        public static System.Drawing.Image ImageForTable(string TableName, bool IsGrey)
        {
            try
            {
                int i = 0;
                TableName = TableName.ToLower();
                switch (TableName)
                {
                    case "collection":
                        i = (int)OverviewImage.Collection;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "collectiontask":
                        i = (int)OverviewImageTableOrField.CollectionTask;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "task":
                        i = (int)OverviewImageTableOrField.Task;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "taskmodule":
                        i = (int)OverviewImageTableOrField.DiversityWorkbench;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "taskresult":
                        i = (int)OverviewImageTableOrField.TableColumn;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "projectuser":
                    case "userproxy":
                        i = (int)OverviewImageUser.User;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListUser.Images[i];
                    case "transactionagent":
                    case "collectionagent":
                        i = (int)OverviewImage.Agent;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionmanager":
                        i = (int)OverviewImageUser.CollectionManager;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListUser.Images[i];
                        break;
                    case "externalrequestcredentials ":
                    case "collectionrequester":
                        i = (int)OverviewImageUser.Requester;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListUser.Images[i];
                        break;
                    case "collectionspecimenpart":
                        i = (int)OverviewImageStorage.Specimen;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListStorage.Images[i];
                        break;
                    case "collectioneventregulation":
                    case "collectionspecimenpartregulation":
                        i = (int)OverviewImageTableOrField.Paragraph;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListStorage.Images[i];
                        break;
                    case "processing":
                    case "projectprocessing":
                    case "processingmaterialcategory":
                    case "collectionspecimenprocessing":
                    case "methodforprocessing":
                        i = (int)OverviewImage.Processing;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionspecimenrelation":
                        i = (int)OverviewImage.Relation;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionspecimenpartdescription":
                        i = (int)OverviewImageTableOrField.PartDescription;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionspecimenreference":
                        i = (int)OverviewImageTableOrField.Reference;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionevent":
                        i = (int)OverviewImage.Event;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectioneventmethod":
                        i = (int)OverviewImageTableOrField.Tool;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectioneventparametervalue":
                        i = (int)OverviewImageTableOrField.Parameter;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectioneventproperty":
                        i = (int)OverviewImage.EventProperty;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectioneventlocalisation":
                        i = (int)OverviewImage.Localisation;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionexternaldatasource":
                        i = DiversityCollection.Specimen.ImageTableOrFieldIndex(ImageTableOrField.Database, IsGrey);
                        return DiversityCollection.Specimen.ImageListTablesAndFields.Images[i];
                        break;
                    case "entity":
                    case "entityrepresentation":
                    case "entityusage":
                        i = DiversityCollection.Specimen.ImageTableOrFieldIndex(ImageTableOrField.Dictionary, IsGrey);
                        return DiversityCollection.Specimen.ImageListTablesAndFields.Images[i];
                        break;
                    case "collectionimage":
                    case "collectionspecimenimage":
                    case "collectioneventimage":
                    case "collectioneventseriesimage":
                    case "collectioneventseriesimages":
                    case "collectiontaskimage":
                        i = (int)OverviewImageStorage.Icones;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListStorage.Images[i];
                        break;
                    case "collectionspecimenimageproperty":
                        i = (int)DiversityCollection.Specimen.OverviewImageTableOrField.ImageArea;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        //i = DiversityCollection.Specimen.ImageTableOrFieldIndex(ImageTableOrField.ImageArea, IsGrey);
                        //return DiversityCollection.Specimen.ImageListTablesAndFields.Images[i];
                        break;
                    case "collectioneventseries":
                        i = (int)OverviewImage.EventSeries;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectioneventseriesdescriptor":
                        i = (int)DiversityCollection.Specimen.OverviewImageTableOrField.KeyBlue;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionspecimen":
                        i = (int)OverviewImage.Specimen;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "projectproxy":
                    case "collectionproject":
                    //case "projectanalysis":
                    //case "projectprocessing":
                        i = (int)OverviewImage.Project;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "transaction":
                    case "collectionspecimentransaction":
                        i = (int)OverviewImage.Transaction;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "identificationunit":
                        i = (int)OverviewImageTaxon.Plant;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListTaxon.Images[i];
                        break;
                    case "identificationunitinpart":
                        i = (int)OverviewImageTableOrField.UnitInPart;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageListTaxon.Images[i];
                        break;
                    case "analysis":
                    case "analysisresult":
                    case "projectanalysis":
                    case "analysistaxonomicgroup":
                    case "identificationunitanalysis":
                    case "methodforanalysis":
                        i = (int)OverviewImage.Analysis;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "identificationunitgeoanalysis":
                        i = (int)OverviewImage.GeoAnalysis;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "identification":
                        i = (int)OverviewImage.Identification;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "samplingplot":
                        i = (int)OverviewImage.SamplingPlot;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "label":
                        i = DiversityCollection.Specimen.ImageTableOrFieldIndex(ImageTableOrField.Label, IsGrey);
                        return DiversityCollection.Specimen.ImageListTablesAndFields.Images[i];
                        break;
                    case "parent":
                        i = DiversityCollection.Specimen.ImageTableOrFieldIndex(ImageTableOrField.Parent, IsGrey);
                        return DiversityCollection.Specimen.ImageListTablesAndFields.Images[i];
                        break;
                    case "identifierevent":
                    case "identifierspecimen":
                    case "identifierpart":
                    case "identifiertransaction":
                    case "identifierunit":
                    case "externalidentifier":
                        i = (int)OverviewImageTableOrField.ID;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "annotation":
                        i = (int)OverviewImageTableOrField.Annotation;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "transactiondocument":
                        i = (int)OverviewImageTableOrField.Document;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "externalrequestcredentials":
                        i = (int)OverviewImage.Transaction;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                    case "collectionspecimenprocessingmethod":
                    case "identificationunitanalysismethod":
                    case "method":
                        i = (int)OverviewImageTableOrField.Tool;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "collectionspecimenprocessingmethodparameter":
                    case "identificationunitanalysismethodparameter":
                    case "parameter":
                    case "parametervalue_enum":
                    case "collectiontaskmetric":
                        i = (int)OverviewImageTableOrField.Parameter;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    case "transactionpayment":
                        i = (int)OverviewImageTableOrField.Payment;
                        if (IsGrey) i++;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                    default:
                        i = (int)OverviewImage.Null;
                        return DiversityCollection.Specimen.ImageList.Images[i];
                        break;
                }
            }
            catch (System.Exception ex) { }
            return DiversityCollection.Specimen.ImageListTablesAndFields.Images[0];
        }
        
        #endregion

        #region LocalisationSystem and CollectionEventProperty

        public static System.Drawing.Image ImageForCollectionEventProperty(bool IsGrey, string ParsingMethod)
        {
            int I = DiversityCollection.Specimen.ImageIndexForCollectionEventProperty(ParsingMethod, IsGrey);
            System.Drawing.Image Image = DiversityCollection.Specimen.ImageList.Images[I];
            return Image;
        }

        public static System.Drawing.Image ImageForCollectionEventProperty(string Property)
        {
            string SQL = "SELECT ParsingMethodName FROM Property WHERE ";
            int ID;
            if (int.TryParse(Property, out ID))
                SQL += "PropertyID = " + Property;
            else
                SQL += "PropertyName = '" + Property + "'";
            string ParsingMethod = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            int I = DiversityCollection.Specimen.ImageIndexForCollectionEventProperty(ParsingMethod, false);
            System.Drawing.Image Image = DiversityCollection.Specimen.ImageList.Images[I];
            return Image;
        }


        public static int ImageIndexForCollectionEventProperty(string ParsingMethod, bool IsGrey)
        {
            /*
            Stratigraphy
            Vegetation             
             * * */
            int i = (int)OverviewImage.EventProperty;
            try
            {
                switch (ParsingMethod.ToLower())
                {
                    case "lithostratigraphy":
                    case "chronostratigraphy":
                    case "biostratigraphy":
                    case "stratigraphy":
                        i = (int)OverviewImageTableOrField.Stratigraphy;
                        if (IsGrey) i++;
                        break;
                    default:
                        i = (int)OverviewImage.EventProperty;
                        if (IsGrey) i++;
                        break;
                }
                return i;
            }
            catch (System.Exception ex) { }
            return i;
        }

        public static System.Drawing.Image ImageForCollectionEventProperty(int PropertyID)
        {
            /*
            Stratigraphy
            Vegetation             
             * * */
            int i = (int)OverviewImage.EventProperty;
            switch (PropertyID)
            {
                case 20:
                case 30:
                case 60:
                    i = (int)DiversityCollection.Specimen.OverviewImageTableOrField.Stratigraphy;
                    break;
            }
            return DiversityCollection.Specimen.ImageList.Images[(int)i];
        }

        public static System.Drawing.Image ImageForLocalisationSystem(bool IsGrey, string ParsingMethod)
        {
            int I = DiversityCollection.Specimen.ImageForLocalisationSystem(ParsingMethod, IsGrey);
            System.Drawing.Image Image = DiversityCollection.Specimen.ImageList.Images[I];
            return Image;
        }

        public static int ImageForLocalisationSystem(string ParsingMethod, bool IsGrey)
        {
            /*
            Altitude
            Coordinates
            Depth
            Exposition
            Gazetteer
            GK
            Height
            MTB
            RD
            SamplingPlot
            Slope
             * */
            int i = (int)OverviewImage.Localisation;
            try
            {
                switch (ParsingMethod)
                {
                    case "RD":
                    case "GK":
                        i = (int)OverviewImageTableOrField.GaussKrueger;
                        if (IsGrey) i++;
                        break;
                    case "MTB":
                        i = (int)OverviewImageTableOrField.MTB;
                        if (IsGrey) i++;
                        break;
                    case "Altitude":
                        i = (int)OverviewImageTableOrField.Altitide;
                        if (IsGrey) i++;
                        break;
                    case "Gazetteer":
                        i = (int)OverviewImageTableOrField.Place;
                        if (IsGrey) i++;
                        break;
                    case "Exposition":
                        i = (int)OverviewImageTableOrField.Exposition;
                        if (IsGrey) i++;
                        break;
                    case "Slope":
                        i = (int)OverviewImageTableOrField.Slope;
                        if (IsGrey) i++;
                        break;
                    case "SamplingPlot":
                        i = (int)OverviewImage.SamplingPlot;
                        if (IsGrey) i++;
                        break;
                    case "Depth":
                        i = (int)OverviewImageTableOrField.Depth;
                        if (IsGrey) i++;
                        break;
                    case "Height":
                        i = (int)OverviewImageTableOrField.Height;
                        if (IsGrey) i++;
                        break;
                    case "UTM":
                        i = (int)OverviewImageTableOrField.UTM;
                        if (IsGrey) i++;
                        break;
                    default:
                        i = (int)OverviewImage.Localisation;
                        if (IsGrey) i++;
                        break;
                }
                return i;
            }
            catch (System.Exception ex) { }
            return i;
        }

        public static System.Drawing.Image ImageForLocalisationSystem(OverviewImageLocalisationSystem LocalisationSystem)
        {
            return DiversityCollection.Specimen.ImageListLocalisationSystem.Images[(int)LocalisationSystem];
        }

        public static System.Drawing.Image ImageForLocalisationSystem(int LocalisationSystemID)
        {
            /*
             * LocalisationSystemID	LocalisationSystemName
            1	Top50 (deutsche Landesvermessung)
            2	Gauss-Krüger coordinates
            3	MTB (A, CH, D)
            4	Altitude (mNN)
            5	mNN (barometric)
            6	Greenwich Coordinates
            7	Named area (DiversityGazetteer)
            8	Coordinates WGS84
            9	Coordinates
            10	Exposition
            11	Slope
            12	Coordinates PD
            13	Sampling plot
            14	Depth
            15	Height
            16	Dutch RD coordinates
            18  Named area 2 (DiversityGazetteer) 
            19  Named area 2 (DiversityGazetteer) 
            20  Named area 2 (DiversityGazetteer) 
            21  Named area 2 (DiversityGazetteer) 
             * */
            DiversityCollection.Specimen.OverviewImageLocalisationSystem L = OverviewImageLocalisationSystem.Localisation;
            switch (LocalisationSystemID)
            {
                case 2:
                    L = OverviewImageLocalisationSystem.GaussKrueger;
                    break;
                case 3:
                    L = OverviewImageLocalisationSystem.MTB;
                    break;
                case 4:
                    L = OverviewImageLocalisationSystem.Altitide;
                    break;
                case 7:
                    L = OverviewImageLocalisationSystem.Place;
                    break;
                case 10:
                    L = OverviewImageLocalisationSystem.Exposition;
                    break;
                case 11:
                    L = OverviewImageLocalisationSystem.Slope;
                    break;
                case 13:
                    L = OverviewImageLocalisationSystem.SamplingPlot;
                    break;
                case 14:
                    L = OverviewImageLocalisationSystem.Depth;
                    break;
                case 15:
                    L = OverviewImageLocalisationSystem.Height;
                    break;
                case 16:
                    L = OverviewImageLocalisationSystem.GaussKrueger;
                    break;
                case 17:
                    L = OverviewImageLocalisationSystem.MTB;
                    break;
                case 18:
                    L = OverviewImageLocalisationSystem.Place2;
                    break;
                case 19:
                    L = OverviewImageLocalisationSystem.Place3;
                    break;
                case 20:
                    L = OverviewImageLocalisationSystem.Place4;
                    break;
                case 21:
                    L = OverviewImageLocalisationSystem.Place5;
                    break;
                case 22:
                    L = OverviewImageLocalisationSystem.UTM;
                    break;
            }
            return DiversityCollection.Specimen.ImageListLocalisationSystem.Images[(int)L];
        }
        
        #endregion

        public static System.Drawing.Image ImageForModule(string Module)
        {
            switch(Module.ToLower())
            {
                case "diversityagents":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityAgents);
                case "diversitycollection":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityCollection);
                //case "diversitydescriptions":
                //    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityGazetteer);
                case "diversitygazetteer":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityGazetteer);
                case "diversityprojects":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityProjects);
                case "diversityreferences":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityReferences);
                case "diversitysamplingplots":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversitySamplingPlots);
                case "diversityscientificterms":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityScientificTerms);
                case "diversitytaxonnames":
                    return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityTaxonNames);
            }
            return DiversityCollection.Specimen.getImage(OverviewImageTableOrField.DiversityWorkbench);
        }

        private static System.Windows.Forms.ImageList _ImageListTransactionType;
        public static System.Windows.Forms.ImageList ImageListTransactionType()
        {
            if (DiversityCollection.Specimen._ImageListTransactionType == null)
            {
                DiversityCollection.Specimen S = new Specimen();
                DiversityCollection.Specimen._ImageListTransactionType = S.imageListTransaction;
            }
            return DiversityCollection.Specimen._ImageListTransactionType;
        }

        /// <summary>
        /// Index of the image according to the type of a transaction as defined in table CollTransactionType_Enum
        /// </summary>
        /// <param name="TransactionType">type of a transaction as defined in table CollTransactionType_Enum</param>
        /// <param name="IsInactive">If the transaction is not active any more, e.g. occured in the past or is expired</param>
        /// <returns>The index of the image in the list</returns>
        public static int IndexImageTransactionType(string TransactionType, bool? IsInactive)
        {
            int i = 0;
            switch (TransactionType.ToLower())
            {
                    /*borrow
                    embargo
                    exchange
                    gift
                    inventory
                    loan
                    purchase
                    request*/

                case "embargo":
                    i = 2;
                    break;
                case "exchange":
                    i = 6;
                    break;
                case "inventory":
                    i = 7;
                    break;
                case "loan":
                    i = 4;
                    break;
                case "request":
                    i = 8;
                    break;
                case "forwarding":
                    i = 9;
                    break;
                case "permit":
                    i = 10;
                    break;
                case "return":
                    i = 11;
                    break;
                case "gift":
                    i = 12;
                    break;
                case "borrow":
                    i = 13;
                    break;
                case "purchase":
                    i = 14;
                    break;
                case "transaction group":
                    i = 15;
                    break;
                case "removal":
                    i = 16;
                    break;
                case "permanent loan":
                    i = 17;
                    break;
                case "regulation":
                    i = 18;
                    break;
                case "warning":
                    i = 19;
                    break;
                case "task":
                    i = 20;
                    break;
                case "ipm":
                    i = 21;
                    break;
            }
            if (IsInactive != null && (bool)IsInactive)
                i++;
            return i;
        }

        private static System.Windows.Forms.ImageList _ImageListRetrievalType;
        public static System.Windows.Forms.ImageList ImageListRetrievalType()
        {
            if (DiversityCollection.Specimen._ImageListRetrievalType == null)
            {
                DiversityCollection.Specimen S = new Specimen();
                DiversityCollection.Specimen._ImageListRetrievalType = S.imageListRetrievalType;
            }
            return DiversityCollection.Specimen._ImageListRetrievalType;
        }

        public static System.Drawing.Image ImageForTransactionType(string TransactionType)
        {
            int Index = IndexImageTransactionType(TransactionType, false);
            return Specimen.ImageListTransactionType().Images[Index];
        }


        public static System.Drawing.Image ImageForRetrievalType(string RetrievalType)
        {
            switch (RetrievalType.ToLower())
            {
                case "literature":
                    return DiversityCollection.Specimen.ImageListRetrievalType().Images[0];
                case "observation":
                case "human observation":
                case "machine observation":
                    return DiversityCollection.Specimen.ImageListRetrievalType().Images[1];
                case "sound recording":
                case "audio recording":
                    return DiversityCollection.Specimen.ImageListRetrievalType().Images[2];
            }
            return null;
        }

        #endregion    

        #region Defaults

        public static bool DefaultUseCurrentUserAsDefault
        {
            get
            {
                return DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleCurrent;
            }
        }

        public static bool DefaultUseIdentificationResponsible
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 0
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[0] == '0')
                    return false;
                else return true;
            }
        }

        public static bool DefaultUseCollector
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 1
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[1] == '0')
                    return false;
                else return true;
            }
        }

        public static bool DefaultUseLocalisationResponsible
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 2
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[2] == '0')
                    return false;
                else return true;
            }
        }

        public static bool DefaultUseEventPropertiyResponsible
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 3
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[3] == '0')
                    return false;
                else return true;
            }
        }

        public static bool DefaultUseAnalyisisResponsible
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 4
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[4] == '0')
                    return false;
                else return true;
            }
        }

        public static bool DefaultUseProcessingResponsible
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 5
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[5] == '0')
                    return false;
                else return true;
            }
        }

        public static bool DefaultUseTaskResponsible
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length > 6
                    && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[6] == '0')
                    return false;
                else return true;
            }
        }

        public static string DefaultResponsibleName
        { get { return DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsible; } }

        public static string DefaultResponsibleURI
        { get { return DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleURI; } }
        
        #endregion

        #region Images provided by the user

        #region Taxonomic Group

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _TaxonomicGroup_Images;
        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _TaxonomicGroup_ImagesGray;

        public static System.Collections.Generic.Dictionary<string, System.Drawing.Image> TaxonomicGroup_Images
        {
            get
            {
                if (Specimen._TaxonomicGroup_Images == null)
                {
                    Specimen._TaxonomicGroup_Images = new Dictionary<string, System.Drawing.Image>();
                    Specimen._TaxonomicGroup_ImagesGray = new Dictionary<string, System.Drawing.Image>();

                    Specimen._TaxonomicGroup_Images.Add("", Specimen.getImage(OverviewImage.Null));
                    Specimen._TaxonomicGroup_ImagesGray.Add("", Specimen.getImage(OverviewImage.Null));

                    foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtTaxonomicGroups.Rows)
                    {
                        if (!R["Icon"].Equals(System.DBNull.Value) &&
                            !AddedImageIndex.ContainsKey(R["Code"].ToString().ToLower() + "|TaxonomicGroup|"))
                        {
                            try
                            {
                                System.Byte[] B = (System.Byte[])R["Icon"];
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                                System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                                Specimen._TaxonomicGroup_Images.Add(R["Code"].ToString(), I);
                                AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|TaxonomicGroup|", Specimen._ImageList.Images.Count);
                                Specimen._ImageList.Images.Add(R["Code"].ToString().ToLower(), I);
                                // Gray image
                                System.Drawing.Bitmap BM = new System.Drawing.Bitmap(I, 16, 16);
                                System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BM);
                                System.Drawing.Image IG = (System.Drawing.Image)BG;
                                Specimen._TaxonomicGroup_ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                                AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|TaxonomicGroup|Gray", Specimen._ImageList.Images.Count);
                                Specimen._ImageList.Images.Add(R["Code"].ToString().ToLower() + "_grey", IG);
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else if (!Specimen._TaxonomicGroup_Images.ContainsKey(R["Code"].ToString().ToLower()))
                        {
                            try
                            {
                                System.Drawing.Image I = DiversityCollection.Specimen.TaxonImage(false, R["Code"].ToString());
                                Specimen._TaxonomicGroup_Images.Add(R["Code"].ToString().ToLower(), I);
                                System.Drawing.Image IG = DiversityCollection.Specimen.TaxonImage(true, R["Code"].ToString());
                                Specimen._TaxonomicGroup_ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                        }

                    }
                }
                return Specimen._TaxonomicGroup_Images;
            }
            set { Specimen._TaxonomicGroup_Images = value; }
        }

        public static void ResetTaxonomicGroup_Images()
        {
            Specimen._TaxonomicGroup_Images = null;
            Specimen._AddedImageIndex = null;
        }
        
        #endregion  
 
        #region MaterialCategory

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _MaterialCategory_Images;
        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _MaterialCategory_ImagesGray;

        public static System.Collections.Generic.Dictionary<string, System.Drawing.Image> MaterialCategory_Images
        {
            get
            {
                if (Specimen._MaterialCategory_Images == null)
                {
                    Specimen._MaterialCategory_Images = new Dictionary<string, System.Drawing.Image>();
                    Specimen._MaterialCategory_ImagesGray = new Dictionary<string, System.Drawing.Image>();

                    Specimen._MaterialCategory_Images.Add("", Specimen.getImage(OverviewImage.Null));
                    Specimen._MaterialCategory_ImagesGray.Add("", Specimen.getImage(OverviewImage.Null));

                    foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtMaterialCategories.Rows)
                    {
                        try
                        {
                            string Key = R["Code"].ToString();
                            string KeyLower = Key.ToLower();
                            if (R.Table.Columns.Contains("Icon") &&
                                !R["Icon"].Equals(System.DBNull.Value) &&
                                (!AddedImageIndex.ContainsKey(Key + "|MaterialCategory|") && !AddedImageIndex.ContainsKey(KeyLower + "|MaterialCategory|")))
                            {
                                try
                                {
                                    System.Byte[] B = (System.Byte[])R["Icon"];
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                                    System.Drawing.Bitmap BC = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(ms);
                                    BC.MakeTransparent();
                                    System.Drawing.Image I = (System.Drawing.Image)BC;// System.Drawing.Image.FromStream(ms);
                                    Specimen._MaterialCategory_Images.Add(R["Code"].ToString(), I);
                                    AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|MaterialCategory|", Specimen._ImageList.Images.Count);
                                    Specimen._ImageList.Images.Add(R["Code"].ToString().ToLower(), I);
                                    // Gray image
                                    //System.Drawing.Bitmap BM = new System.Drawing.Bitmap(I, 16, 16);
                                    System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BC);
                                    BG.MakeTransparent();
                                    System.Drawing.Image IG = (System.Drawing.Image)BG;
                                    Specimen._MaterialCategory_ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                                    AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|MaterialCategory|Gray", Specimen._ImageList.Images.Count);
                                    Specimen._ImageList.Images.Add(R["Code"].ToString().ToLower() + "_grey", IG);
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                            else if (!Specimen._MaterialCategory_Images.ContainsKey(KeyLower) && !Specimen._MaterialCategory_Images.ContainsKey(Key))
                            {
                                try
                                {
                                    System.Drawing.Image I = DiversityCollection.Specimen.MaterialCategoryImage(false, R["Code"].ToString());
                                    Specimen._MaterialCategory_Images.Add(R["Code"].ToString().ToLower(), I);
                                    System.Drawing.Image IG = DiversityCollection.Specimen.MaterialCategoryImage(true, R["Code"].ToString());
                                    Specimen._MaterialCategory_ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                            }
                            else
                            {
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                return Specimen._MaterialCategory_Images;
            }
            set { Specimen._MaterialCategory_Images = value; }
        }

        public static void Reset_MaterialCategory_Images()
        {
            Specimen._MaterialCategory_Images = null;
        }

        #endregion  

        #region Collection type

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _CollectionType_Images;
        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _CollectionType_ImagesGray;

        public static System.Collections.Generic.Dictionary<string, System.Drawing.Image> CollectionType_Images
        {
            get
            {
                if (Specimen._CollectionType_Images == null)
                {
                    Specimen._CollectionType_Images = new Dictionary<string, System.Drawing.Image>();
                    Specimen._CollectionType_ImagesGray = new Dictionary<string, System.Drawing.Image>();

                    Specimen._CollectionType_Images.Add("", Specimen.getImage(OverviewImage.Null));
                    Specimen._CollectionType_ImagesGray.Add("", Specimen.getImage(OverviewImage.Null));

                    foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollectionTypes.Rows)
                    {
                        if (!R["Icon"].Equals(System.DBNull.Value) &&
                            !AddedImageIndex.ContainsKey(R["Code"].ToString().ToLower() + "|CollectionType|"))
                        {
                            try
                            {
                                System.Byte[] B = (System.Byte[])R["Icon"];
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                                System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                                Specimen._CollectionType_Images.Add(R["Code"].ToString(), I);
                                AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|CollectionType|", Specimen._ImageList.Images.Count);
                                Specimen._ImageList.Images.Add(R["Code"].ToString().ToLower(), I);
                                // Gray image
                                System.Drawing.Bitmap BM = new System.Drawing.Bitmap(I, 16, 16);
                                System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BM);
                                System.Drawing.Image IG = (System.Drawing.Image)BG;
                                Specimen._CollectionType_ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                                AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|CollectionType|Gray", Specimen._ImageList.Images.Count);
                                Specimen._ImageList.Images.Add(R["Code"].ToString().ToLower() + "_grey", IG);
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else if (!Specimen._CollectionType_Images.ContainsKey(R["Code"].ToString().ToLower()))
                        {
                            try
                            {
                                System.Drawing.Image I = DiversityCollection.Specimen.CollectionTypeImage(false, R["Code"].ToString());
                                Specimen._CollectionType_Images.Add(R["Code"].ToString().ToLower(), I);
                                System.Drawing.Image IG = DiversityCollection.Specimen.CollectionTypeImage(true, R["Code"].ToString());
                                Specimen._CollectionType_ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                        }

                    }
                }
                return Specimen._CollectionType_Images;
            }
            set { Specimen._CollectionType_Images = value; }
        }

        public static void ResetCollectionType_Images()
        {
            Specimen._CollectionType_Images = null;
            Specimen._AddedImageIndex = null;
        }

        #endregion


        #region Task type

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _TaskType_Images;
        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _TaskType_ImagesGray;

        public static System.Collections.Generic.Dictionary<string, System.Drawing.Image> TaskType_Images
        {
            get
            {
                if (Specimen._TaskType_Images == null)
                {
                    Specimen.InitUser_Images(ref Specimen._TaskType_Images, ref Specimen._TaskType_ImagesGray, DiversityCollection.LookupTable.DtTaskTypes, "TaskType", false);
                }
                return Specimen._TaskType_Images;
            }
            set { Specimen._TaskType_Images = value; }
        }

        public static System.Drawing.Image TaskTypeImage(bool IsGrey, string TaskType)
        {
            if (Specimen.TaskType_Images.ContainsKey(TaskType))
                return Specimen.TaskType_Images[TaskType];
            else if (IsGrey && Specimen._TaskType_ImagesGray.ContainsKey(TaskType))
                return Specimen._TaskType_ImagesGray[TaskType];
            else
            {
                return Specimen.ImageList.Images[Specimen.TaskTypeImage(TaskType, false)];
                //return Specimen.ImageList.Images[Specimen.TaskTypeImage("Task", false)];
            }
        }

        public static int TaskTypeImage(string TaskType, bool Grey)
        {
            int I = (int)OverviewImageTableOrField.Task;
            TaskType = TaskType.ToLower();

            // has been used for separating time and date - deprecated
            //if (TaskType.IndexOf(" ") > -1)
            //    TaskType = TaskType.Substring(0, TaskType.IndexOf(" ")).Trim();

            switch (TaskType)
            {
                // defalut
                case "task":
                    I = (int)OverviewImageTableOrField.Task;
                    break;

                // Types
                case "analysis":
                    I = (int)OverviewImage.Analysis;
                    break;
                case "cleaning":
                    I = (int)OverviewImageTableOrField.Cleaning;
                    break;
                case "damage":
                    I = (int)OverviewImageTableOrField.Damage;
                    break;
                case "document":
                    I = (int)OverviewImageTableOrField.Document;
                    break;
                case "evaluation":
                    I = (int)OverviewImageTableOrField.Section;
                    break;
                case "freezing":
                    I = (int)OverviewImageTableOrField.Freezer;
                    break;
                case "gas":
                    I = (int)OverviewImageTableOrField.Gas;
                    break;
                case "inspection":
                    I = (int)OverviewImageTableOrField.Inspection;
                    break;
                case "irradiation":
                    I = (int)OverviewImageTableOrField.Radioactive;
                    break;
                case "legislation":
                    I = (int)OverviewImageTableOrField.Paragraph;
                    break;
                case "metric":
                case "monitoring":
                    I = (int)OverviewImageTableOrField.Graph;
                    break;
                case "part":
                    I = (int)OverviewImageStorage.Specimen;
                    break;
                case "payment":
                    I = (int)OverviewImageTableOrField.Payment;
                    break;
                case "poison":
                    I = (int)OverviewImageTableOrField.Poison;
                    break;
                case "exhibition":
                    I = (int)OverviewImageTableOrField.Exhibition;
                    break;
                case "problem":
                    I = (int)OverviewImageTableOrField.Problem;
                    break;
                case "processing":
                    I = (int)OverviewImage.Processing;
                    break;
                case "query":
                    I = (int)OverviewImageTableOrField.Query;
                    break;
                case "radiation":
                    I = (int)OverviewImageTableOrField.Radiation;
                    break;
                case "repair":
                    I = (int)OverviewImageTableOrField.Repair;
                    break;
                case "search":
                    I = (int)OverviewImageStorage.Observation;
                    break;
                case "sensor":
                    I = (int)OverviewImageTableOrField.Sensor;
                    break;
                case "humidity":
                    I = (int)OverviewImageTableOrField.SensorHumidity;
                    break;
                case "temperature":
                    I = (int)OverviewImageTableOrField.SensorTemperature;
                    break;
                case "trap":
                    I = (int)OverviewImageTableOrField.Trap;
                    break;
                case "hardware":
                    I = (int)OverviewImageTableOrField.Hardware;
                    break;
                case "ipm":
                    I = (int)OverviewImageTableOrField.Task;
                    break;
                case "pest":
                    I = (int)OverviewImageTableOrField.Bug;
                    break;
                case "beneficial organism":
                    I = (int)OverviewImageTaxon.Animal;
                    break;
                case "battery":
                    I = (int)OverviewImageTableOrField.Battery;
                    break;
                case "bycatch":
                    I = (int)OverviewImageTableOrField.Spider;
                    break;
                //case "wet collection":
                //    I = (int)OverviewImageTableOrField.WetCollection;
                //    break;
                //case "dry collection":
                //    I = (int)OverviewImageStorage.Specimen;
                //    break;
                //case "bones":
                //    I = (int)OverviewImageStorage.Bones;
                //    break;
                //case "skins":
                //    I = (int)OverviewImageStorage.Pelt;
                //    break;
                //case "department":
                //    I = (int)OverviewImageTableOrField.Room;
                //    break;


                // Module related
                // dwb
                case "diversityworkbench":
                    I = (int)OverviewImageTableOrField.DiversityWorkbench;
                    break;

                // agent
                case "diversityagents":
                case "agent":
                    I = (int)OverviewImageTableOrField.DiversityAgents;
                    break;
                    I = (int)OverviewImage.Agent;
                    break;

                // Collection
                case "diversitycollection":
                case "collection":
                    I = (int)OverviewImageTableOrField.DiversityCollection;
                    break;
                    I = (int)OverviewImage.Collection;
                    break;


                //Description
                case "diversitydescriptions":
                    I = (int)OverviewImageTableOrField.Descriptions;
                    break;
                case "description":
                    I = (int)OverviewImageTableOrField.Descriptions;
                    break;

                // Gazetteer
                case "diversitygazetteer":
                case "gazetteer":
                    I = (int)OverviewImageTableOrField.DiversityGazetteer;
                    break;
                    I = (int)OverviewImageTableOrField.Gazetteer;
                    break;

                //Project
                case "diversityprojects":
                case "project":
                    I = (int)OverviewImageTableOrField.DiversityProjects;
                    break;
                    I = (int)OverviewImage.Project;
                    break;

                //Plot
                case "diversitysamplingplots":
                case "samplingplot":
                case "sampling plot":
                    I = (int)OverviewImageTableOrField.DiversitySamplingPlots;
                    break;
                    I = (int)OverviewImage.SamplingPlot;
                    break;
                    I = (int)OverviewImage.SamplingPlot;
                    break;

                //Term
                case "diversityscientificterms":
                case "scientificterm":
                case "scientific term":
                    I = (int)OverviewImageTableOrField.DiversityScientificTerms;
                    break;
                    I = (int)OverviewImageTableOrField.Terminology;
                    break;
                    I = (int)OverviewImageTableOrField.Terminology;
                    break;

                //Name
                case "diversitytaxonnames":
                case "taxonname":
                case "taxon name":
                    I = (int)OverviewImageTableOrField.DiversityTaxonNames;
                    break;
                    I = (int)OverviewImageTableOrField.Bug;
                    break;
                    I = (int)OverviewImageTableOrField.Bug;
                    break;

                // date and time
                case "date":
                    I = (int)OverviewImageTableOrField.Date;
                    break;
                case "time":
                    I = (int)ImageTableOrField.DateTime;
                    break;

                // result type
                case "list":
                    I = (int)OverviewImageTableOrField.TableColumn;
                    break;

                default:
                    if (Specimen.AddedImageIndex.ContainsKey(TaskType + "|TaskType|"))
                        I = Specimen.AddedImageIndex[TaskType + "|TaskType|"];
                    else
                        I = (int)OverviewImageTableOrField.Task;
                    break;
            }
            if (Grey) I++;
            return I;
        }


        public static void ResetTaskType_Images()
        {
            Specimen._TaskType_Images = null;
            Specimen._AddedImageIndex = null;
        }

        #endregion

        public static void InitUser_Images(
            ref System.Collections.Generic.Dictionary<string, System.Drawing.Image> Images,
            ref System.Collections.Generic.Dictionary<string, System.Drawing.Image> ImagesGray,
            System.Data.DataTable LookupTable,
            string ImageType,
            bool IncludeNullImage = true)
        {
            if (Images == null)
            {
                Images = new Dictionary<string, System.Drawing.Image>();
                ImagesGray = new Dictionary<string, System.Drawing.Image>();

                //if (IncludeNullImage)
                {
                    Images.Add("", Specimen.getImage(OverviewImage.Null));
                    ImagesGray.Add("", Specimen.getImage(OverviewImage.Null));
                }

                foreach (System.Data.DataRow R in LookupTable.Rows)
                {
                    if (!R["Icon"].Equals(System.DBNull.Value) &&
                        !AddedImageIndex.ContainsKey(R["Code"].ToString().ToLower() + "|" + ImageType  + "|"))
                    {
                        try
                        {
                            System.Byte[] B = (System.Byte[])R["Icon"];
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                            System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                            Images.Add(R["Code"].ToString(), I);
                            AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|TaskType|", Specimen._ImageList.Images.Count);
                            Specimen._ImageList.Images.Add(I);
                            // Gray image
                            System.Drawing.Bitmap BM = new System.Drawing.Bitmap(I, 16, 16);
                            System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BM);
                            System.Drawing.Image IG = (System.Drawing.Image)BG;
                            ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                            AddedImageIndex.Add(R["Code"].ToString().ToLower() + "|" + ImageType + "|Gray", Specimen._ImageList.Images.Count);
                            Specimen._ImageList.Images.Add(IG);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    else if (!Images.ContainsKey(R["Code"].ToString().ToLower()))
                    {
                        try
                        {
                            System.Drawing.Image I = null;
                            System.Drawing.Image IG = null;
                            switch (ImageType)
                            {
                                case "TaskType":
                                    I = DiversityCollection.Specimen.TaskTypeImage(false, R["Code"].ToString());
                                    IG = DiversityCollection.Specimen.TaskTypeImage(true, R["Code"].ToString());
                                    break;
                            }
                            if (I != null && IG != null)
                            {
                                Images.Add(R["Code"].ToString().ToLower(), I);
                                ImagesGray.Add(R["Code"].ToString().ToLower(), IG);
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    else
                    {
                    }

                }
            }
        }


        private static System.Collections.Generic.Dictionary<string, int> _AddedImageIndex;

        public static System.Collections.Generic.Dictionary<string, int> AddedImageIndex
        {
            get
            {
                if (Specimen._AddedImageIndex == null)
                {
                    Specimen._AddedImageIndex = new Dictionary<string, int>();
                }
                return Specimen._AddedImageIndex;
            }
            set { Specimen._AddedImageIndex = value; }
        }

        #endregion


    }
}
