using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiversityProjects
{
    public partial class Setting : Component
    {

        #region Construction
        
        public Setting()
        {
            InitializeComponent();
        }

        public Setting(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        
        #endregion

        #region Parameter and properties

        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterSetting;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                {
                    System.Windows.Forms.Form F = new System.Windows.Forms.Form();
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(F, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                }
                return this._FormFunctions;
            }
        }
        
        #endregion

        public System.Data.DataSet DsSetting()
        {
            if (this.dataSetSetting.Setting.Rows.Count == 0)
            {
                string SQL = "SELECT SettingID, ParentSettingID, DisplayText, DisplayOrder, Description FROM Setting ORDER BY DisplayText";
                this._sqlDataAdapterSetting = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterSetting, DiversityProjects.Project.SqlSetting, this.dataSetSetting.Setting);
                this.dataSetSetting.Setting.Clear();
                this._sqlDataAdapterSetting.Fill(this.dataSetSetting.Setting);
            }
            return this.dataSetSetting;
        }

        public bool AddSetting(ref int? SettingID, string DisplayText, int? ParentSettingID)
        {
            int _SettingID = -1;
            string Display = DisplayText;
            if (Display.Length > 50)
                Display = Display.Substring(0, 50);
            string SQL = "insert into Setting (DisplayText, [Description], ParentSettingID) " +
                "values ('" + Display + "', '" + DisplayText + "', ";
            if (ParentSettingID == null)
                SQL += "NULL";
            else SQL += ParentSettingID.ToString();
            SQL += "); SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out _SettingID))
            {
                SettingID = _SettingID;
                return true;
            }
            else return false;
        }

        public bool RemoveSetting(int SettingID)
        {
            bool OK = true;
            System.Collections.Generic.Stack<int> IDsToRemove = new Stack<int>();
            IDsToRemove.Push(SettingID);
            this.GetDependingSettings(SettingID, ref IDsToRemove);
            while( IDsToRemove.Count > 0)
            {
                int ID = IDsToRemove.Pop();
                string SQL = "DELETE S FROM Setting S WHERE S.SettingID = " + ID.ToString();
                string Message = "";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    OK = true;
                else
                {
                    OK = false;
                    SQL = "SELECT P.Project " +
                        " FROM ProjectSetting AS S INNER JOIN " +
                        " Project AS P ON S.ProjectID = P.ProjectID " +
                        " WHERE S.SettingID = " + ID.ToString() +
                        " ORDER BY P.Project";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Message = "This setting is used in the following projects:";
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            Message += "\r\n" + R[0].ToString();
                        }
                    }
                    System.Windows.Forms.MessageBox.Show(Message, "Deleting failed");
                    return false;
                }
            }
            return OK; 
        }

        private void GetDependingSettings(int SettingID, ref System.Collections.Generic.Stack<int> IDsToRemove)
        {
            string SQL = "SELECT SettingID FROM Setting AS S WHERE ParentSettingID = " + SettingID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                int i;
                if (int.TryParse(R[0].ToString(), out i))
                {
                    IDsToRemove.Push(i);
                    GetDependingSettings(i, ref IDsToRemove);
                }
            }
        }

        public bool UpdateSettingDisplayText(int SettingID, string DisplayText)
        {
            string SQL = "UPDATE S SET DisplayText = '" + DisplayText + "' FROM Setting S WHERE S.SettingID = " + SettingID.ToString();
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                return true;
            else
                return false;
        }

        public bool UpdateSettingDisplayOrder(int SettingID, string DisplayOrder)
        {
            int i;
            if (int.TryParse(DisplayOrder, out i))
            {
                string SQL = "UPDATE S SET DisplayOrder = " + DisplayOrder + " FROM Setting S WHERE S.SettingID = " + SettingID.ToString();
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool SetSettingParentID(int SettingID, int? ParentSettingID)
        {
            string SQL = "UPDATE S SET ParentSettingID = ";
            if (ParentSettingID == null)
                SQL += " NULL ";
            else
                SQL += ParentSettingID.ToString();
            SQL += " FROM Setting S WHERE S.SettingID = " + SettingID.ToString();
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                return true;
            else
                return false;
        }

        #region Handling the hierarchy
        
        private System.Data.DataTable _DtSettingHierarchy;
        /// <summary>
        /// The hierarchy of the setting
        /// </summary>
        /// <returns>A table containing the settings including the hierarchy</returns>
        public System.Data.DataTable DtSettingHierarchy()
        {
            if (this._DtSettingHierarchy == null)
            {
                this._DtSettingHierarchy = new System.Data.DataTable();
                string SQL = "SELECT SettingID, ParentSettingID, DisplayText, DisplayOrder, Description, Hierarchy " +
                    "FROM dbo.SettingHierarchyAll() AS H " +
                    "ORDER BY Hierarchy, DisplayOrder ";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DtSettingHierarchy);
            }
            return this._DtSettingHierarchy;
        }


        public void ResetSettingHierarchy()
        { this._DtSettingHierarchy = null; }

        /// <summary>
        /// A table containing the settings exclusive the children of this setting
        /// </summary>
        /// <param name="SettingID"></param>
        /// <returns></returns>
        public System.Data.DataTable DtSettingHierarchyPotentialParents(int SettingID)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            this._DtSettingHierarchy = new System.Data.DataTable();
            string SQL = "SELECT SettingID, ParentSettingID, DisplayText, DisplayOrder, Description, Hierarchy " +
                "FROM dbo.SettingHierarchyAll() AS H " +
                "WHERE H.SettingID NOT IN (SELECT SettingID FROM dbo.SettingHierarchyChildren(" + SettingID + "))" +
                "ORDER BY Hierarchy, DisplayOrder";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            return dt;
        }



        /// <summary>
        /// Building the hierarchy of the settings
        /// </summary>
        /// <param name="Tree">The tree where the hierarchy should be created in</param>
        /// <param name="ProjectID">an optional project ID
        /// if this is given, the settings are related to the project ID</param>
        /// <param name="ForSelection">If the settings should be choosen for selection for a project</param>
        public void BuildHierarchyTree(ref System.Windows.Forms.TreeView Tree, int? ProjectID, bool ForSelection)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            if (ProjectID != null && ForSelection)
                dt = this.DtProjectMissingSettings((int)ProjectID);
            else if (ProjectID != null)
                dt = this.DtProjectSettings((int)ProjectID);
            else dt = this.DtSettingHierarchy();
            System.Data.DataRow[] RR = dt.Select("ParentSettingID IS NULL", "DisplayOrder, DisplayText");
            foreach (System.Data.DataRow R in RR)
            {
                string DisplayText = R["DisplayText"].ToString();
                if (ProjectID != null && !ForSelection && !R["Value"].Equals(System.DBNull.Value))
                    DisplayText += ": " + R["Value"].ToString();
                System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode(DisplayText);
                N.Tag = R;
                Tree.Nodes.Add(N);
                this.AddHierarchyChildren(N, ProjectID, ref dt);
            }
        }

        private void AddHierarchyChildren(System.Windows.Forms.TreeNode NP, int? ProjectID, ref System.Data.DataTable DT)
        {
            System.Data.DataRow RP = (System.Data.DataRow)NP.Tag;
            System.Data.DataRow[] RR = DT.Select("ParentSettingID = " + RP["SettingID"].ToString(), "DisplayOrder, DisplayText");
            foreach (System.Data.DataRow RC in RR)
            {
                if (RC["ParentSettingID"].ToString() == RP["SettingID"].ToString())
                {
                    string DisplayText = RC["DisplayText"].ToString();
                    if (ProjectID != null && RC.Table.Columns.Contains("Value") && !RC["Value"].Equals(System.DBNull.Value))
                        DisplayText += ": " + RC["Value"].ToString();
                    System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode(DisplayText);
                    N.Tag = RC;
                    NP.Nodes.Add(N);
                    this.AddHierarchyChildren(N, ProjectID, ref DT);
                }
            }
        }

        /// <summary>
        /// The settings used for a project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        private System.Data.DataTable DtProjectSettings(int ProjectID)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy, Value " +
                "FROM DBO.SettingHierarchyProjectAll(" + ProjectID + ") ORDER BY DisplayOrder";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            return dt;
        }

        /// <summary>
        /// returns a table that contains all settings that are so far not found for a project
        /// </summary>
        /// <param name="ProjectID">The project for which the setting should be found</param>
        /// <returns>A table containing missing settings</returns>
        private System.Data.DataTable DtProjectMissingSettings(int ProjectID)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy " +
                "FROM DBO.SettingHierarchyNonProject(" + ProjectID.ToString() + ") ORDER BY DisplayOrder";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            return dt;
        }

        #region Hierarchy including parents

        public void BuildHierarchyTreeWithParents(ref System.Windows.Forms.TreeView Tree, int ProjectID)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = this.DtProjectSettingsWithParents(ProjectID);
                System.Data.DataRow[] RR = dt.Select("ParentSettingID IS NULL", "DisplayText");
                foreach (System.Data.DataRow R in RR)
                {
                    string DisplayText = R["DisplayText"].ToString();
                    if (ProjectID != null && !R["Value"].Equals(System.DBNull.Value))
                        DisplayText += ": " + R["Value"].ToString();
                    System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode(DisplayText);
                    if (R["ProjectID"].ToString() != ProjectID.ToString())
                    {
                        if (!R["Value"].Equals(System.DBNull.Value))
                        {
                            N.Text += "    [ " + R["Project"].ToString() + " ]";
                            N.ForeColor = System.Drawing.Color.Chocolate;
                        }
                        else
                            N.ForeColor = System.Drawing.Color.Gray;
                    }
                    N.Tag = R;
                    Tree.Nodes.Add(N);
                    this.AddHierarchyChildrenWithParents(N, ProjectID, ref dt);
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void AddHierarchyChildrenWithParents(System.Windows.Forms.TreeNode NP, int? ProjectID, ref System.Data.DataTable DT)
        {
            System.Data.DataRow RP = (System.Data.DataRow)NP.Tag;
            foreach (System.Data.DataRow RC in DT.Rows)
            {
                if (RC["ParentSettingID"].ToString() == RP["SettingID"].ToString())
                {
                    string DisplayText = RC["DisplayText"].ToString();
                    if (ProjectID != null && !RC["Value"].Equals(System.DBNull.Value))
                        DisplayText += ": " + RC["Value"].ToString();
                    System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode(DisplayText);
                    if (RC["ProjectID"].ToString() != ProjectID.ToString())
                    {
                        if (!RC["Value"].Equals(System.DBNull.Value))
                        {
                            N.Text += "    [ " + RC["Project"].ToString() + " ]";
                            N.ForeColor = System.Drawing.Color.Chocolate;
                        }
                        else
                            N.ForeColor = System.Drawing.Color.Gray;
                    }
                    N.Tag = RC;
                    NP.Nodes.Add(N);
                    this.AddHierarchyChildrenWithParents(N, ProjectID, ref DT);
                }
            }
        }

        private System.Data.DataTable DtProjectSettingsWithParents(int ProjectID)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                string SQL = "SELECT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy, Value, ProjectID, Project " +
                    "FROM DBO.SettingHierarchyProjectAll(" + ProjectID + ") ORDER BY DisplayOrder";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return dt;
        }

        #endregion
        
        #endregion
    }
}
