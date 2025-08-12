using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    public interface iImportStep
    {
        /// <summary>
        /// The key of the import step, used for sorting
        /// </summary>
        /// <returns></returns>
        string StepKey();

        /// <summary>
        /// The key of the superior step - if present
        /// </summary>
        /// <returns></returns>
        string SuperiorStepKey();

        int StepDetailKey();

        /// <summary>
        /// If the step had been handled complete
        /// </summary>
        /// <returns></returns>
        bool StepOK();

        /// <summary>
        /// The level within the import hierarchy
        /// </summary>
        /// <returns>0 = basic, 1 - n following levels</returns>
        byte StepLevel();

        /// <summary>
        /// the title of the step as shown in the interface
        /// </summary>
        /// <returns></returns>
        string StepTitle();

        /// <summary>
        /// the message of a step e.g. in case of an error
        /// </summary>
        /// <returns></returns>
        string StepMessage();

        /// <summary>
        /// the image of the step as shown in the interface
        /// </summary>
        /// <returns></returns>
        System.Drawing.Image StepImage();

        //System.Windows.Forms.Panel SelectionPanelForDependentSteps();

        /// <summary>
        /// The parallel number, e.g. if several identifications are imported
        /// </summary>
        /// <returns></returns>
        int? StepParallelNumber();

        string TableName();
        string TableAlias();

        /// <summary>
        /// if a step is visible in the interface
        /// </summary>
        /// <returns></returns>
        bool IsVisible();
        /// <summary>
        /// set the visibility of a step in the interface. 
        /// Hiding depend on CanHide
        /// </summary>
        /// <param name="Visible"></param>
        void IsVisible(bool Visible);
        /// <summary>
        /// if a step can be made invisible in the interface
        /// </summary>
        /// <param name="canHide"></param>
        void CanHide(bool canHide);

        /// <summary>
        /// if a step can be made invisible in the interface
        /// </summary>
        /// <returns></returns>
        bool CanHide();

        DiversityCollection.UserControls.iUserControlImportInterface getUserControlImportInterface();
        //System.Windows.Forms.TabPage getParentTabPage();

        ///// <summary>
        ///// The tabpage related to the step
        ///// </summary>
        ///// <returns></returns>
        //System.Windows.Forms.TabPage TabPage();
    }

    /// <summary>
    /// Import steps correspond to a tabpage in the interface and will be listed in the selections and the execution plan
    /// In the interface they are organized via SuperiorImportStep
    /// </summary>
    public class Import_Step : iImportStep
    {
        #region Parameter

        public static readonly string StepKeySeparator = "_";
        public static readonly string StepKeyParallelNumberSeparator = ":";
        
        private string _Title;
        private string _Key;
        private string _TableName;
        private string _TableAlias;
        private ImportStepStatus _ImportStepStatus = ImportStepStatus.Unhandled;
        private int? _ParallelNumber;
        private DiversityCollection.UserControls.iUserControlImportInterface _iUserControlImportInterface;
        private bool _IsVisible = true;
        private bool _CanHide = true;
        private bool _MustImport = false;
        /// <summary>
        /// if data must be imported even if there are no data in the file, e.g. for the project that must be imported as soon as specimen data are imported
        /// </summary>
        public bool MustImport
        {
            get { return _MustImport; }
            set 
            {
                _MustImport = value;
                if (this.SelectionPanel != null)
                {
                }
            }
        }

        DiversityCollection.Import_Step _SuperiorImportStep;

        public DiversityCollection.Import_Step SuperiorImportStep
        {
            get { return _SuperiorImportStep; }
            set { _SuperiorImportStep = value; }
        }

        private string _Error;
        private System.Drawing.Image _StepImage;
        private bool _StepOK;
        /// <summary>
        /// If an import step is only the haeder of a group, e.g. Part 1 for the first part that is stored.
        /// these step will be ignored when walking trough
        /// </summary>
        private bool _IsGroupHaeder;

        private DiversityCollection.UserControls.UserControlImportStep _UserControlImportStep;

        public DiversityCollection.UserControls.UserControlImportStep UserControlImportStep
        {
            get
            {
                if (this._UserControlImportStep == null)
                {
                    this._UserControlImportStep = new DiversityCollection.UserControls.UserControlImportStep(this);
                }
                return _UserControlImportStep;
            }
            set { _UserControlImportStep = value; }
        }

        private System.Windows.Forms.TabPage _TabPage;

        public System.Windows.Forms.TabPage TabPage
        {
            get { return _TabPage; }
            //set { _TabPage = value; }
        }
        private System.Windows.Forms.TabControl _TabControl;

        public System.Windows.Forms.TabControl TabControl
        {
            get { return _TabControl; }
           // set { _TabControl = value; }
        }
        private byte _Level;

        private System.Windows.Forms.Panel _SelectionPanel;

        public System.Windows.Forms.Panel SelectionPanel
        {
            get { return _SelectionPanel; }
           // set { _SelectionPanel = value; }
        }

        public enum ImportStepStatus { Unhandled, Error, OK };

        #endregion

        #region Construction

        private Import_Step(string Title, 
            string Description, 
            string StepPosition, 
            byte Level,
            DiversityCollection.UserControls.iUserControlImportInterface UserControlImportInterface,
            System.Drawing.Image StepImage)
        {
            this._Key = StepPosition;
            this._Title = Title;
            this._Error = "";
            this._iUserControlImportInterface = UserControlImportInterface;
            this._StepImage = StepImage;
            this._Level = Level;
            try
            {
                this._UserControlImportStep = new DiversityCollection.UserControls.UserControlImportStep(this);
            }
            catch (System.Exception ex) { }
        }

        public static Import_Step GetImportStep(string StepPosition)
        {
            if (DiversityCollection.Import.ImportSteps.ContainsKey(StepPosition))
                return DiversityCollection.Import.ImportSteps[StepPosition];
            else return null;
        }

        public static Import_Step GetImportStep(
            string Title, 
            string Description, 
            string StepPosition,
            string TableName,
            int? ParallelNumber,
            Import_Step SuperiorImportStep, 
            byte Level, 
            DiversityCollection.UserControls.iUserControlImportInterface UserControlImportInterface,
            System.Drawing.Image StepImage,
            System.Windows.Forms.Panel SelectionPanel)
        {
            if (DiversityCollection.Import.ImportSteps.ContainsKey(StepPosition))
                return DiversityCollection.Import.ImportSteps[StepPosition];
            else
            {
                DiversityCollection.Import_Step I = new Import_Step(Title, Description, StepPosition, Level, UserControlImportInterface, StepImage);
                try
                {
                if (SuperiorImportStep != null)
                {
                    if (StepPosition.Length > SuperiorImportStep.StepKey().Length)
                        I.SuperiorImportStep = SuperiorImportStep;
                    else
                    {
                        if (SuperiorImportStep.SuperiorImportStep != null)
                            I.SuperiorImportStep = SuperiorImportStep.SuperiorImportStep;
                    }
                }
                I._TableName = TableName;
                I._ParallelNumber = ParallelNumber;
                string Group = I.Title;
                if (I.SuperiorImportStep != null)
                    Group = I.SuperiorImportStep.StepTitle();
                I._SelectionPanel = SelectionPanel;
                if (I._SelectionPanel != null)
                {
                    DiversityCollection.Import.AddImportSelector(Group,
                    I._Title,
                    I,
                    I._SelectionPanel);
                }
                else if (SuperiorImportStep != null)
                {
                    if (SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps() != null)
                        DiversityCollection.Import.AddImportSelector(Group,
                            I._Title,
                            I,
                            SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps());
                }
                    if (!DiversityCollection.Import.ImportSteps.ContainsKey(StepPosition))
                        DiversityCollection.Import.ImportSteps.Add(StepPosition, I);
                }
                catch (System.Exception ex) { }
                return I;
            }
        }

        #endregion

        #region Interface

        public void Reset()
        {
            if (this._SelectionPanel != null && this._SelectionPanel.Controls.Count > 0)
            {
                //if (this._SelectionPanel.Controls.Contains(this._UserControlImportStep))
                //    this._SelectionPanel.Controls.Remove(this._UserControlImportStep);
                this._SelectionPanel.Controls.Clear();
            }
        }

        public string StepTitle() { return this._Title; }

        public string StepKey() { return this._Key; }

        public int StepDetailKey()
        {
            int Detail = -1;
            try
            {
                string[] SepKeys = new string[1];
                SepKeys[0] = DiversityCollection.Import_Step.StepKeySeparator;
                string[] KeyParts = this.StepKey().Split(SepKeys, StringSplitOptions.RemoveEmptyEntries);
                string DetailKey = KeyParts[KeyParts.Length - 1];
                string[] SepNum = new string[1];
                SepNum[0] = DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
                string[] DetailKeyParts = DetailKey.Split(SepNum, StringSplitOptions.RemoveEmptyEntries);
                string Key = DetailKeyParts[0];
                int.TryParse(Key, out Detail);
            }
            catch (System.Exception ex) { }
            return Detail;
        }

        public bool StepOK() { return this._StepOK; }

        public byte StepLevel() { return this._Level; }

        public System.Drawing.Image StepImage() 
        {
            return this.UserControlImportStep.StepImage;
            //return this._StepImage; 
        }

        public int? StepParallelNumber() 
        {
            if (this._ParallelNumber == null)
            {
                try
                {
                    if (this._Key.IndexOf(':') == -1)
                        return null;
                    int Num;
                    string[] SepKeys = new string[1];
                    SepKeys[0] = DiversityCollection.Import_Step.StepKeySeparator;
                    string[] KeyParts = this.StepKey().Split(SepKeys, StringSplitOptions.RemoveEmptyEntries);
                    string DetailKey = KeyParts[KeyParts.Length - 1];
                    string[] SepNum = new string[1];
                    SepNum[0] = DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
                    string[] DetailKeyParts = DetailKey.Split(SepNum, StringSplitOptions.RemoveEmptyEntries);
                    if (DetailKeyParts.Length > 1)
                    {
                        string Key = DetailKeyParts[1];
                        if (int.TryParse(Key, out Num))
                            this._ParallelNumber = Num;
                    }
                }
                catch (System.Exception ex) { }
            }
            return this._ParallelNumber; 
        }

        public DiversityCollection.UserControls.iUserControlImportInterface getUserControlImportInterface()
        { return this._iUserControlImportInterface; }

        public string TableName()
        { return this._TableName; }

        public string TableAlias()
        {
            if (this._TableAlias != null)
                return this._TableAlias;
            string TA = this._TableName;
            if (this.SuperiorImportStep != null && this.SuperiorImportStep._ParallelNumber != null)
            {
                if (this._TableName != this.SuperiorImportStep._TableName)
                    TA += "_" + this.SuperiorImportStep._ParallelNumber + "_";
            }
            if (this._ParallelNumber != null)
                TA += this._ParallelNumber.ToString();
            return TA;
        }

        public void setTableAlias(string TableAlias)
        {
            this._TableAlias = TableAlias;
        }

        public bool IsVisible()
        {
            if (!this._IsVisible && !this._CanHide && DiversityCollection.Import.AttachmentKeyImportColumn == null)
                this._IsVisible = true;
            return this._IsVisible;
        }
        public void IsVisible(bool Visible)
        {
            if (this._CanHide)
            {
                this._IsVisible = Visible;
                if (!Visible)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                    {
                        if (KV.Value.SuperiorImportStep != null && KV.Value.SuperiorImportStep == this)
                            KV.Value.IsVisible(false);
                    }
                }
            }
        }

        public void CanHide(bool canHide)
        {
            this._CanHide = canHide;
        }

        public bool CanHide() { return this._CanHide; }


        #region Error

        public string StepMessage() { return this._Error; }

        public void setStepError(string Error)
        {
            try
            {
                this._Error = Error;
                if (this._Error.Length > 0)
                    this._ImportStepStatus = ImportStepStatus.Error;
                else if (this._ImportStepStatus == ImportStepStatus.Error)
                    this._ImportStepStatus = ImportStepStatus.OK;
                this._UserControlImportStep.setStatus();
            }
            catch (System.Exception ex) { }
        }

        public void setStepError()
        {
            try
            {
                this._Error = "";
                int iRelatedColumns = 0;
                bool CurrentColumnInSourceFileIsMissing = false;
                bool CurrentColumnIsAttachmentKey = false;
                //System.Drawing.Color ColorMissing = System.Drawing.Color.Yellow;
                //System.Drawing.Color ColorAttachmentKey = System.Drawing.Color.LightGreen;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.iImportColumnControl> KV in DiversityCollection.Import.ImportColumnControls)
                {
                    if (KV.Value == null)
                        continue;
                    string Step = this.StepKey();
                    string StepControl = KV.Value.ImportColumn().StepKey;
                    if (KV.Value.ImportColumn().StepKey == this.StepKey())
                    {
                        string E = KV.Value.Error();
                        if (E.Length > 0)
                            E += "\r\n";
                        this._Error += E;

                        if (KV.Value.ImportColumn().TypeOfSource == Import_Column.SourceType.File)
                        {
                            if (KV.Value.ImportColumn().ColumnInSourceFile == null)
                                CurrentColumnInSourceFileIsMissing = true;
                            if (DiversityCollection.Import.AttachmentKeyImportColumn != null
                                && KV.Value.ImportColumn().Table == DiversityCollection.Import.AttachmentKeyImportColumn.Table
                                && KV.Value.ImportColumn().Column == DiversityCollection.Import.AttachmentKeyImportColumn.Column)
                                CurrentColumnIsAttachmentKey = true;
                        }
                        if (KV.Value.ImportColumn().IsSelected)
                            iRelatedColumns++;
                    }
                }
                if (iRelatedColumns == 0)
                    this._ImportStepStatus = ImportStepStatus.Unhandled;
                else
                {
                    if (this._Error.Length == 0)
                        this._ImportStepStatus = ImportStepStatus.OK;
                    else this._ImportStepStatus = ImportStepStatus.Error;
                }
                if (CurrentColumnIsAttachmentKey)
                    this.UserControlImportStep.setStatus(FormImportWizard.ColorForAttachment);
                else if (CurrentColumnInSourceFileIsMissing)
                    this.UserControlImportStep.setStatus(FormImportWizard.ColorForColumQuery);
                else
                    this._UserControlImportStep.setStatus();
            }
            catch (System.Exception ex) { }
        }

        public string getStepError() { return this._Error; }

        #endregion

        public ImportStepStatus getImportStepStatus() 
        {
            if (this._Error.Length > 0)
                this._ImportStepStatus = ImportStepStatus.Error;
            else if (this._ImportStepStatus == ImportStepStatus.Error)
                this._ImportStepStatus = ImportStepStatus.OK;
            return this._ImportStepStatus; 
        }

        public bool IsGroupHaeder
        {
            get
            {
                if (_IsGroupHaeder == null)
                    _IsGroupHaeder = false;
                return _IsGroupHaeder;
            }
            set { _IsGroupHaeder = value; }
        }

        public string SuperiorStepKey()
        {
            if (this.SuperiorImportStep != null)
            {
                string SupStepKey = this.SuperiorImportStep.StepKey();
                if (this.StepKey().Length > SupStepKey.Length)
                {
                    return this.SuperiorImportStep.StepKey();
                }
                else
                {
                }
            }
            string SuperiorKey = "";
            string[] ss = this._Key.Split(new char[]{'_'});
            for (int i = 0; i < ss.Length - 1; i++)
            {
                SuperiorKey += DiversityCollection.Import_Step.StepKeySeparator + ss[i];
            }
            if (SuperiorKey.Replace(DiversityCollection.Import_Step.StepKeySeparator, "").Trim().Length == 0)
            {
                SuperiorKey = "";
                ss = this._Key.Split(new char[] { ':' });
                SuperiorKey = ss[0];
            }
            if (SuperiorKey.Replace(DiversityCollection.Import_Step.StepKeySeparator, "").Trim().Length == 0)
                SuperiorKey = "";
            return SuperiorKey;
        }

        public System.Collections.Generic.List<DiversityCollection.Import_Column> StepColumns
        {
            get
            {
                System.Collections.Generic.List<DiversityCollection.Import_Column> L = new List<Import_Column>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
                {
                    if (KV.Value.IsSelected && KV.Value.StepKey == this._Key)
                        L.Add(KV.Value);
                }
                return L;
            }
        }

        public void DataGridColumnsRelatedToImportStep(ref System.Collections.Generic.List<int> List)
        {
            if (List == null)
                List = new List<int>();
            foreach (DiversityCollection.Import_Column C in this.StepColumns)
            {
                if (C.IsSelected && C.ColumnInSourceFile != null && C.TypeOfSource == Import_Column.SourceType.File)
                    List.Add((int)C.ColumnInSourceFile);
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
            {
                if (KV.Value.SuperiorImportStep != null && KV.Value.SuperiorImportStep == this)
                    DataGridColumnsRelatedToImportStep(ref List);
            }
        }


        //public string Group() 
        //{
        //    if (this._Group == null)
        //        this._Group = "";
        //    return this._Group; 
        //}

        //public string Step() 
        //{
        //    if (this._Step == null)
        //        this._Step = "";
        //    return this._Step; 
        //}

        #region static functions for handling keys and numbers
        
        public static DiversityCollection.Import.ImportStep ImportStepKey(string StepKey)
        {
            int Key = DiversityCollection.Import_Step.StepKeyPart(StepKey, 0);
            return (Import.ImportStep)Key;
        }

        public static DiversityCollection.Import.ImportStepEvent ImportStepEventKey(string StepKey)
        {
            int Key = DiversityCollection.Import_Step.StepKeyPart(StepKey, 1);
            return (Import.ImportStepEvent)Key;
        }

        public static DiversityCollection.Import.ImportStepSpecimen ImportStepSpecimenKey(string StepKey)
        {
            int Key = DiversityCollection.Import_Step.StepKeyPart(StepKey, 1);
            return (Import.ImportStepSpecimen)Key;
        }

        public static DiversityCollection.Import.ImportStepStorage ImportStepStorageKey(string StepKey)
        {
            int Key = DiversityCollection.Import_Step.StepKeyPart(StepKey, 1);
            return (Import.ImportStepStorage)Key;
        }

        public static DiversityCollection.Import.ImportStepUnit ImportStepUnitKey(string StepKey)
        {
            int Key = DiversityCollection.Import_Step.StepKeyPart(StepKey, 1);
            return (Import.ImportStepUnit)Key;
        }

        private static int StepKeyPart(string StepKey, int Position)
        {
            int Key = 0;
            string[] Separators = new string[1];
            Separators[0] = DiversityCollection.Import_Step.StepKeySeparator;
            string[] Parts = StepKey.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            if (Parts.Length > Position)
            {
                string[] SeparatorsPosition = new string[1];
                SeparatorsPosition[0] = DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
                string[] NumberParts = Parts[Position].Split(SeparatorsPosition, StringSplitOptions.None);
                if (NumberParts.Length > 0)
                    int.TryParse(NumberParts[0], out Key);
            }
            return Key;
        }

        /// <summary>
        /// getting the parallel number of a import item as coded in a step key
        /// </summary>
        /// <param name="StepKey">The step key</param>
        /// <param name="Position">the position within the key, e.g. 0 for Part, 1 for Processing</param>
        /// <returns>The parallel number</returns>
        public static int StepKeyPartParallelNumber(string StepKey, int Position)
        {
            int N = 1;
            string[] Separators = new string[1];
            Separators[0] = DiversityCollection.Import_Step.StepKeySeparator;
            string[] Parts = StepKey.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            if (Parts.Length > Position)
            {
                string[] SeparatorsPosition = new string[1];
                SeparatorsPosition[0] = DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
                string[] NumberParts = Parts[Position].Split(SeparatorsPosition, StringSplitOptions.None);
                if (NumberParts.Length > 1)
                    int.TryParse(NumberParts[1], out N);
            }
            return N;
        }

        /// <summary>
        /// get the next step key for parallel imports
        /// </summary>
        /// <param name="iStepKey">the enum for the key</param>
        /// <param name="SuperiorImportStep">the superior import step</param>
        /// <returns></returns>
        public static string getNextImportStepKey(int iStepKey, DiversityCollection.Import_Step SuperiorImportStep)
        {
            string Key = "";
            if (SuperiorImportStep != null)
                Key = SuperiorImportStep.StepKey();
            Key += DiversityCollection.Import_Step.StepKeySeparator;
            if (iStepKey < 10)
                Key += "0";
            Key += iStepKey.ToString();
            Key += DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
            int iLast = DiversityCollection.Import_Step.getNextImportStepNumber(SuperiorImportStep, iStepKey);
            if (iLast < 10)
                Key += "0";
            Key += iLast.ToString();
            return Key;
        }

        public static string getNextImportStepKey(int iStepKey, DiversityCollection.Import_Step SuperiorImportStep, int NextNumber)
        {
            string Key = "";
            if (SuperiorImportStep != null)
                Key = SuperiorImportStep.StepKey();
            Key += DiversityCollection.Import_Step.StepKeySeparator;
            if (iStepKey < 10)
                Key += "0";
            Key += iStepKey.ToString();
            Key += DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
            if (NextNumber < 10)
                Key += "0";
            Key += NextNumber.ToString();
            return Key;
        }

        public static int getNextImportStepNumber(DiversityCollection.Import_Step SuperiorImportStep, int StepEnumKey)
        {
            int iLast = 1;
            string StepKey = "";
            if (SuperiorImportStep != null)
            {
                StepKey = SuperiorImportStep.StepKey();
                try
                {
                    System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> ChildSteps = new SortedList<string, Import_Step>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                    {
                        if (KV.Value.SuperiorImportStep == SuperiorImportStep
                            && KV.Value.StepDetailKey() == StepEnumKey)
                            ChildSteps.Add(KV.Value.StepKey(), KV.Value);
                    }
                    if (ChildSteps.Count > 0 && ChildSteps.Last().Value.StepParallelNumber() != null)
                        iLast = (int)ChildSteps.Last().Value.StepParallelNumber() + 1; //<DiversityCollection.Import_Step>
                }
                catch (System.Exception ex) { }
            }
            else
            {
                System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> ParallelSteps = new SortedList<string, Import_Step>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    if (KV.Value.StepDetailKey() == StepEnumKey && KV.Value.StepParallelNumber() != null)
                        ParallelSteps.Add(KV.Value.StepKey(), KV.Value);
                }
                if (ParallelSteps.Count > 0 && ParallelSteps.Last().Value.StepParallelNumber() != null)
                    iLast = (int)ParallelSteps.Last().Value.StepParallelNumber() + 1; //<DiversityCollection.Import_Step>
            }
            return iLast;
        }

        public static string getImportStepKey(DiversityCollection.Import_Step Import_Step)
        {
            string Key = Import_Step.StepKey();
            if (Import_Step.SuperiorImportStep != null)
                Key = Import_Step.SuperiorImportStep.StepKey() + Key;
            if (Import_Step.StepParallelNumber() != null)
            {
                Key += DiversityCollection.Import_Step.StepKeyParallelNumberSeparator;
            }
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import_Step Import_Step, int ParallelNumber)
        {
            string Key = Import_Step.StepKey();
            if (Import_Step.SuperiorImportStep != null)
                Key = Import_Step.SuperiorImportStep.StepKey() + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStep ImportStep)
        {
            string Key = ((int)ImportStep).ToString();
            if (Key.Length == 1) Key = "0" + Key;
            Key = Import_Step.StepKeySeparator + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStepEvent ImportStep)
        {
            string Key = ((int)ImportStep).ToString();
            if (Key.Length == 1) Key = "0" + Key;// ((int)DiversityCollection.Import.ImportStep.Event).ToString()
            Key = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Event) + Import_Step.StepKeySeparator + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStepSpecimen ImportStep)
        {
            string Key = ((int)ImportStep).ToString();
            if (Key.Length == 1) Key = "0" + Key;
            Key = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Specimen) + Import_Step.StepKeySeparator + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStep ImportStep, int ParallelImportNumber)
        {
            string Key = DiversityCollection.Import.getImportStepKey(ImportStep);
            string P = ParallelImportNumber.ToString();
            if (P.Length == 1) P = "0" + P;
            else if (P.Length > 2) P = P.Substring(0, 2);
            Key += Import_Step.StepKeyParallelNumberSeparator + P;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStepEvent ImportStep, int ParallelImportNumber)
        {
            string Key = ((int)ImportStep).ToString();
            Key = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Event) + Import_Step.StepKeySeparator + Key;
            string P = ParallelImportNumber.ToString();
            if (P.Length == 1) P = "0" + P;
            else if (P.Length > 2) P = P.Substring(0, 2);
            Key += Import_Step.StepKeyParallelNumberSeparator + P;
            return Key;
        }

        public static string getImportStepKey(string StepKey, int ParallelImportNumber)
        {
            string Key = "";

            char[] x = new char[1];
            x[0] = Import_Step.StepKeySeparator.ToString()[0];
            string[] KeyMainParts = StepKey.Split(x);

            char[] y = new char[1];
            y[0] = Import_Step.StepKeyParallelNumberSeparator.ToString()[0];
            string[] CurrentLevelParts = KeyMainParts[KeyMainParts.Length - 1].Split(y);

            string P = ParallelImportNumber.ToString();
            if (P.Length == 1) P = "0" + P;
            else if (P.Length > 2) P = P.Substring(0, 2);
            for (int i = 0; i < KeyMainParts.Length - 1; i++)
            {
                if (KeyMainParts[i].Length > 0)
                    Key += Import_Step.StepKeySeparator + KeyMainParts[i];
            }
            Key += Import_Step.StepKeySeparator + CurrentLevelParts[0] + Import_Step.StepKeyParallelNumberSeparator + P;
            return Key;
        }

        #endregion

        #endregion

        #region Properties

        public byte Level
        {
            get { return _Level; }
            set { _Level = value; }
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public System.Drawing.Image Image
        {
            get { return _StepImage; }
            set { _StepImage = value; }
        }

        public void RemoveRelatedTabpage()
        {
            //this._TabControl.TabPages.Remove(this._TabPage);
        }

        public void ShowControls()
        {
            if (this.getUserControlImportInterface() != null)
                this.getUserControlImportInterface().showStepControls(this);
        }

        public void setImportStepVisibility(bool IsVisible)
        {
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null
                && DiversityCollection.Import.AttachmentKeyImportColumn.Table == this.TableName()
                && this.StepParallelNumber() > 1
                && IsVisible)
            {

            }
            this._UserControlImportStep.Visible = IsVisible;
            if (this._UserControlImportStep.IsCurrent && !IsVisible)
            {
                DiversityCollection.Import.MoveToNextStep();
            }

            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
            {
                if (KV.Value == null)
                    continue;
                if (KV.Value._SuperiorImportStep == null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KVIC in DiversityCollection.Import.ImportColumns)
                    {
                        if (KVIC.Value.StepKey == KV.Key)
                        {
                            KVIC.Value.IsSelected = IsVisible;
                        }
                    }
                }
                if (KV.Value._SuperiorImportStep == this)
                    KV.Value.setImportStepVisibility(IsVisible);
            }
            this._IsVisible = IsVisible;
            if (this.SelectionPanel != null)
            {
                foreach (System.Windows.Forms.Control C in this.SelectionPanel.Controls)
                {
                    if (C.GetType() == typeof(DiversityCollection.UserControls.UserControlImportSelector))
                    {
                        DiversityCollection.UserControls.UserControlImportSelector IS = (DiversityCollection.UserControls.UserControlImportSelector)C;
                        if (IS.ImportSteps().Contains(this))
                            IS.setSelectionDependingOnVisibilityOfSteps();
                    }
                }
            }
            else if (this._SuperiorImportStep != null)
            {
                if (this._SuperiorImportStep.SelectionPanel != null)
                {
                    foreach (System.Windows.Forms.Control C in this._SuperiorImportStep.SelectionPanel.Controls)
                    {
                        if (C.GetType() == typeof(DiversityCollection.UserControls.UserControlImportSelector))
                        {
                            DiversityCollection.UserControls.UserControlImportSelector IS = (DiversityCollection.UserControls.UserControlImportSelector)C;
                            if (IS.ImportSteps().Contains(this))
                                IS.setSelectionDependingOnVisibilityOfSteps();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
