using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public class PackageAddOn
    {
        #region Parameter
        /// <summary>
        /// The pack as defined in the parent package
        /// </summary>
        private Package _Package;
        public enum AddOn { ABCD_BayernFlora }
        /// <summary>
        ///  exclusive = no other add-on can be installed and updates are restricted to the add-on
        /// </summary>
        public enum AddOnType { exclusive }
        private AddOn _AddOn;

        //private bool _IsExclusive;
        /// <summary>
        /// the minimal version of the parent package before the add on can be installed
        /// </summary>
        //private int _MinVersionPackage = 0;
        
        #endregion

        #region Construction

        public PackageAddOn(Package ParentPackage, AddOn AddOn)
        {
            this._Package = ParentPackage;
            if (PackageAddOns().ContainsKey(this._Package.PackagePack) && PackageAddOns()[this._Package.PackagePack].ContainsKey(AddOn))
                this._AddOn = AddOn;
        }

        public PackageAddOn(Package ParentPackage, string AddOn)
        {
            this._Package = ParentPackage;
            foreach (System.Collections.Generic.KeyValuePair<PackageAddOn.AddOn, string> KV in PackageAddOn.PackageAddOns()[this._Package.PackagePack])
            {
                if (KV.Key.ToString() == AddOn)
                {
                    this._AddOn = KV.Key;
                }
            }
        }

        #endregion

        #region Interface

        public Package AddOnPackage() { return this._Package; }
        public AddOn AddOnOfPackage() { return this._AddOn; }
        public AddOnType TypeOfAddOn() { return PackageAddOn.AddOnTypes()[this._AddOn]; }
        
        #endregion

        #region static infos
        
        private static System.Collections.Generic.Dictionary<Package.Pack, System.Collections.Generic.Dictionary<AddOn, string>> _PackageAddOns;
        /// <summary>
        /// packages and their related Add-ons
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<Package.Pack, System.Collections.Generic.Dictionary<AddOn, string>> PackageAddOns()
        {
            if (PackageAddOn._PackageAddOns == null)
            {
                PackageAddOn._PackageAddOns = new Dictionary<Package.Pack, Dictionary<AddOn, string>>();
                Dictionary<AddOn, string> BFL = new Dictionary<AddOn, string>();
                string ABCD_BayernFloraDescription = "Add on for ABCD for project BayernFlora (http://daten.bayernflora.de/de/index.php): " +
                    "(1) For endangered species (on the basis of a list of TaxonIDs including their synonyms) the locality is set to ''. " +
                    "(2) Providing additional information concerning the source of the taxonomic names. " +
                    "(3) Observations with an identifications qualified with cf...  are not published. " +
                    "(4) Inclusion of a site containing an explanation for the analysis. " +
                    "(5) Observations with a status like X (on the basis of a list of values) and their associations are not published.";
                BFL.Add(AddOn.ABCD_BayernFlora, ABCD_BayernFloraDescription);
                PackageAddOn._PackageAddOns.Add(DiversityCollection.CacheDatabase.Package.Pack.ABCD, BFL);
            }
            return PackageAddOn._PackageAddOns;
        }

        private static System.Collections.Generic.Dictionary<AddOn, int> _CompatibleVersions;
        /// <summary>
        /// The compatible version of the package where the add-on can be installed
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<AddOn, int> PackageCompatibleVersions()
        {
            if (PackageAddOn._CompatibleVersions == null)
            {
                PackageAddOn._CompatibleVersions = new Dictionary<AddOn, int>();
                PackageAddOn._CompatibleVersions.Add(AddOn.ABCD_BayernFlora, 12); // TODO Ariane: can we outsource the version numbers somewhere so that we only have to change it at one place?
            }
            return PackageAddOn._CompatibleVersions;
        }

        private static System.Collections.Generic.Dictionary<AddOn, AddOnType> _AddOnTypes;
        /// <summary>
        /// the types of the the add-ons
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<AddOn, AddOnType> AddOnTypes()
        {
            if (PackageAddOn._AddOnTypes == null)
            {
                PackageAddOn._AddOnTypes = new Dictionary<AddOn, AddOnType>();
                PackageAddOn._AddOnTypes.Add(AddOn.ABCD_BayernFlora, AddOnType.exclusive);
            }
            return PackageAddOn._AddOnTypes;
        }

        public static int Version(AddOn AddOn)
        {
            switch (AddOn)
            {
                case PackageAddOn.AddOn.ABCD_BayernFlora:
                    return 6;
                default:
                    return 1;
            }
        }

        //public static int CompatiblePackageVersion(AddOn AddOn)
        //{
        //    switch (AddOn)
        //    {
        //        case PackageAddOn.AddOn.ABCD_BayernFlora:
        //            return 2;
        //        default:
        //            return 1;
        //    }
        //}

        #endregion
    
    }
}
