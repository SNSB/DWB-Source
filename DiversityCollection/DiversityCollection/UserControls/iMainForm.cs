using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.UserControls
{
    public interface iMainForm
    {
        DiversityCollection.Datasets.DataSetCollectionSpecimen DataSetCollectionSpecimen();
        DiversityCollection.Datasets.DataSetCollectionEventSeries DataSetCollectionEventSeries();

        int? ProjectID();
        System.Collections.Generic.List<int> SelectedIDs();

        int FormWidth();
        int FormHeight();

        void SelectAll();

        int? EventSeriesID();

        int? CollectionEventID();
        int ID_Specimen();
        void saveSpecimen();
        void setSpecimen();
        bool setSpecimen(int ID);
        bool setSpecimenPart(int ID);
        int? getSpecimenPartID();
        System.Windows.Forms.TreeNode SelectedUnitHierarchyNode();
        System.Windows.Forms.TreeNode SelectedPartHierarchyNode();
        void SaveImagesSpecimen();
        //void ShowAllSpecimenImages();
        System.Windows.Forms.ImageList ImageListDataWithholding();
        void CustomizeDisplay(DiversityCollection.Forms.FormCustomizeDisplay.Customization Customize);
        void RestrictImagesToUnit(int IdentificationUnitID);
        void RestrictImagesToPart(int SpecimenPartID);
        void ReleaseImageRestriction();
        bool ClientUpToDate();
        bool DataAvailable();
        // #84
        CollectionSpecimen.AvailabilityState Availability();

        bool ReadOnly();
        void SelectNode(System.Data.DataRow Row, DiversityCollection.Forms.FormCollectionSpecimen.Tree Tree);
        //DiversityCollection.CollectionSpecimen.AvailabilityState AvailabilityState();
    }
}
