using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{

    /// <summary>
    /// Additional bindings of columns, not the default display column of a remote link, e.g. FamilyCache for a link to TaxonNames
    /// </summary>
    public struct RemoteColumnBinding
    {
        /// <summary>
        /// MW 4.5.2015: The update mode for a column
        /// Allways: default
        /// OnlyEmpty: The new values will only be inserted if the current data are empty
        /// IfNotEmptyAskUser: In this case the user will be asked if a filled value should be changed
        /// </summary>
        public enum UpdateMode { Allways, OnlyEmpty, IfNotEmptyAskUser }
        public UpdateMode ModeOfUpdate;
        /// <summary>
        /// The column in a table bound to the remote values of the link, e.g. CountryCache in table CollectionEvent bound to a gazzetteer
        /// </summary>
        public DataColumn Column;
        /// <summary>
        /// The parameter returned by the remote service, e.g. the country
        /// </summary>
        public string RemoteParameter;
    }

    /// <summary>
    /// Remote link to a DiversityWorkbench module. A column in a may be linked to none, one or several modules, 
    /// e.g. the column Location2 in the table CollectionEventLocalisation. This column has 2 RemoteLinks (Gazetteer, SamplingPlots)
    /// every Remote link defines the bindings of additional columns (=RemoteColumnBinding) in a Dictionary for the columns and the bindings
    /// </summary>
    public class RemoteLink
    {
        /// <summary>
        /// The Modules a remote link can be pointing to
        /// </summary>
        public enum LinkedModule { None, DiversityAgents, DiversityExsiccatae, DiversityGazetteer, DiversityProjects, DiversitySamplingPlots, DiversityScientificTerms, DiversityReferences, DiversityTaxonNames }
        private LinkedModule _LinkedToModule = LinkedModule.None;

        public LinkedModule LinkedToModule
        {
            get { return _LinkedToModule; }
            set { _LinkedToModule = value; }
        }

        /// <summary>
        /// the list of values in the decision column that leed to this link option, e.g. 13 as LocalisationSystemID for SamplingPlot
        /// </summary>
        private System.Collections.Generic.List<string> _DecisionColumnValues;

        public System.Collections.Generic.List<string> DecisionColumnValues
        {
            get { return _DecisionColumnValues; }
            //set { _DecisionColumnValues = value; }
        }
        /// <summary>
        /// The bindings of a column, e.g. NameURI
        /// </summary>
        private System.Collections.Generic.List<RemoteColumnBinding> _RemoteColumnBindings;

        public System.Collections.Generic.List<RemoteColumnBinding> RemoteColumnBindings
        {
            get { return _RemoteColumnBindings; }
            //set { _RemoteColumnBindings = value; }
        }

        public RemoteLink()
        {
        }

        public RemoteLink(
            LinkedModule Module,
            string[] DecisionColumnValues,
            System.Collections.Generic.List<RemoteColumnBinding> RemoteColumnBindings)
        {
            //this._IsOptional = IsOptonal;
            this._LinkedToModule = Module;
            this._DecisionColumnValues = new List<string>();
            if (DecisionColumnValues != null)
            {
                for (int i = 0; i < DecisionColumnValues.Length; i++)
                {
                    this._DecisionColumnValues.Add(DecisionColumnValues[i]);
                }
            }
            this._RemoteColumnBindings = RemoteColumnBindings;
        }

        public RemoteLink(LinkedModule Module)
        {
            this._LinkedToModule = Module;
            this._DecisionColumnValues = new List<string>();
            this._RemoteColumnBindings = new List<RemoteColumnBinding>();
        }

    }
}
