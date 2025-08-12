#define ShowEntityInfos
#undef ShowEntityInfos

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace DiversityWorkbench
{
    public partial class Entity : Component
    {
        #region Parameter
        public enum Accessibility { inapplicable, read_only, no_restrictions }
        public enum Determination { calculated, user_defined, service_link }
        public enum Visibility { hidden, optional, visible }
        //public enum EntityUsage { preset, read_only, hidden, inapplicable }
        public enum EntityInformationField { DoesExist, DisplayText, Abbreviation, Description, DisplayTextOK, AbbreviationOK, DescriptionOK }
        private string _ConnectionString;
        
        #endregion

        #region Construction

        public Entity()
        {
            InitializeComponent();
        }

        public Entity(string ConnectionString)
        {
            InitializeComponent();
            this._ConnectionString = ConnectionString;
        }

        public Entity(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Info

        private static bool? _EntityTablesExist;

        /// <summary>
        /// In case of DiversityWorkbench.Settings.UseEntity = true check in database contains the tables for entity description
        /// </summary>
        public static bool EntityTablesExist
        {
            get
            {
                try
                {
                    if (!DiversityWorkbench.Settings.UseEntity) return false;

                    if (_EntityTablesExist == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        _EntityTablesExist = EntityTablesExistInDatabase;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                if (_EntityTablesExist != null)
                    return (bool)_EntityTablesExist;
                else return false;
            }
        }

        public bool EntityTablesExist_O
        {
            get
            {
                if (!DiversityWorkbench.Settings.UseEntity) return false;

                string SQL = "SELECT CASE WHEN COUNT(*) = 6 THEN 1 ELSE 0 END AS EntityTablesExist " +
                    "FROM INFORMATION_SCHEMA.TABLES AS T " +
                    "WHERE T.TABLE_NAME IN ('Entity', 'EntityContext_Enum', 'EntityLanguageCode_Enum', " +
                    "'EntityRepresentation', 'EntityUsage', 'EntityAccessibility_Enum', 'EntityDetermination_Enum', 'EntityVisibility_Enum')";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this._ConnectionString).ToString() == "1") return true;
                else return false;
            }
        }

        private static bool? _EntityTablesExistInDatabase;

        /// <summary>
        /// Check in database contains the tables for entity description
        /// </summary>
        public static bool EntityTablesExistInDatabase
        {
            get
            {
                if (_EntityTablesExistInDatabase == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                    {
                        con.Open();
                        string SQL = "SELECT CASE WHEN COUNT(*) = 8 THEN 1 ELSE 0 END AS EntityTablesExist " +
                        "FROM INFORMATION_SCHEMA.TABLES AS T " +
                        "WHERE T.TABLE_NAME IN ('Entity', 'EntityContext_Enum', 'EntityLanguageCode_Enum', " +
                        "'EntityRepresentation', 'EntityUsage', 'EntityAccessibility_Enum', 'EntityDetermination_Enum', 'EntityVisibility_Enum')";
                        if (DiversityWorkbench.Settings.ConnectionString != null && DiversityWorkbench.Settings.ConnectionString.Length > 0 &&
                            DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, con).ToString() == "1") _EntityTablesExistInDatabase = true;
                        else
                        {
                            _EntityTablesExistInDatabase = false;
                        }
                        con.Close();
                    }
#if xDEBUG
                    // Testing disposal to ensure existing connections
                    DiversityWorkbench.Forms.FormFunctions.Connection().Close();
                    DiversityWorkbench.Forms.FormFunctions.Connection().Dispose();
#endif

                }
                if (_EntityTablesExistInDatabase != null)
                    return (bool)_EntityTablesExistInDatabase;
                else return false;
            }
        }


        public static System.Collections.Generic.Dictionary<string, string> EntityInformation(string Entity, string Context, string LanguageCode)
        {
            System.Collections.Generic.Dictionary<string, string> DictEntity = new Dictionary<string, string>();
            string SQL = "select * from [dbo].[EntityInformation_2] ('" + Entity + "', '" + LanguageCode + "', '" + Context + "')";
            System.Data.DataTable dtEntity = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dtEntity);
            }
            catch { }
            if (dtEntity.Rows.Count > 0)
            {
                foreach (System.Data.DataColumn C in dtEntity.Columns)
                {
                    DictEntity.Add(C.ColumnName, dtEntity.Rows[0][C].ToString());
                }
            }
            return DictEntity;
        }

        public static bool IsDefined(string Entity, string Context, string LanguageCode)
        {
            if (!DiversityWorkbench.Settings.UseEntity) return false;

            bool Defined = false;
            string SQL = "SELECT COUNT(*) " +
                "FROM EntityRepresentation AS R " +
                "WHERE (Entity = '" + Entity + "') AND (EntityContext = '" + Context + "') AND (LanguageCode = '" + LanguageCode + "')";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result != "0")
                Defined = true;
            return Defined;
        }

        public static System.Collections.Generic.Dictionary<string, string> EntityInformation(string Entity, bool GetInfos = false)
        {
            // Markus 6.8.24: Prüfung ob Tabellen vorhanden sind
            if (!EntityTablesExistInDatabase)
                return new Dictionary<string, string>();

#if ShowEntityInfos
            GetInfos = true;
#endif
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (DiversityWorkbench.Entity._EntityDictionary != null 
                && DiversityWorkbench.Entity._EntityDictionary.ContainsKey(Entity))
            {
                if (DiversityWorkbench.Entity._EntityDictionary[Entity].ContainsKey(DiversityWorkbench.Settings.Language))
                {
                    if (DiversityWorkbench.Entity._EntityDictionary[Entity][DiversityWorkbench.Settings.Language].ContainsKey(DiversityWorkbench.Settings.Context))
                        return DiversityWorkbench.Entity._EntityDictionary[Entity][DiversityWorkbench.Settings.Language][DiversityWorkbench.Settings.Context];
                }
            }
            System.Collections.Generic.Dictionary<string, string> DictEntity = new Dictionary<string, string>();
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                string SQL = "select * from [dbo].[EntityInformation_2] ('" + Entity + "', '" + DiversityWorkbench.Settings.Language + "', '" + DiversityWorkbench.Settings.Context + "')";
                System.Data.DataTable dtEntity = new System.Data.DataTable();
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(dtEntity);
                    }
                    catch(System.Exception ex) { }
                    if (dtEntity.Rows.Count > 0)
                    {
                        foreach (System.Data.DataColumn C in dtEntity.Columns)
                        {
                            DictEntity.Add(C.ColumnName, dtEntity.Rows[0][C].ToString());
                        }
                    }
                }
                DiversityWorkbench.Entity.AddEntityToDictionary(DictEntity);
            }
            watch.Stop();
            if(GetInfos)
            {
                _ExecutionMilliseconds += watch.ElapsedMilliseconds;
                if (_EntityInfos == null) _EntityInfos = new Dictionary<string, string>();
                if (!_EntityInfos.ContainsKey(Entity)) _EntityInfos.Add(Entity, "ms: " + watch.ElapsedMilliseconds.ToString());
            }
            return DictEntity;
        }

        public static void ShowInfos()
        {
            if (_EntityInfos != null && _EntityInfos.Count > 0)
            {
                string Message = _ExecutionMilliseconds.ToString() + " ms whole\r\n\r\n";
                string Code = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _EntityInfos)
                {
                    Message += KV.Key + ": " + KV.Value + "\r\n";
                    Code += "EntityFields.Add(\"" + KV.Key + "\");\r\n";
                }
#if (DEBUG && ShowEntityInfos)
                Message += "\r\n\r\nCode:\r\n\r\n" + Code;
                System.Windows.Forms.MessageBox.Show(Message);
                DiversityWorkbench.FormRichEdit f = new FormRichEdit("Entity", Message);
                f.ShowDialog();
#endif
            }
#if DEBUG
            else
            {
                //System.Windows.Forms.MessageBox.Show("No content in entities");
            }
#endif
        }

        private static long _ExecutionMilliseconds = 0;
        private static System.Collections.Generic.List<string> _Entities;
        private static System.Collections.Generic.Dictionary<string, string> _EntityInfos;

//        private static async void GetEntityInformationAsync()
//        {
//            foreach (System.Collections.Generic.KeyValuePair<string, string> E in _EntityInfos)
//            {
//                System.Collections.Generic.Dictionary<string, string> DictEntity = await System.Threading.Tasks.Task.Run(() => EntityInformation(E.Key));
//                DiversityWorkbench.Entity.AddEntityToDictionary(DictEntity);
//            }
//        }

//        public static async void InitEntityInformationAsync(System.Collections.Generic.List<string> Entities)
//        {
//            long Milliseconds = 0;
//            var watch = System.Diagnostics.Stopwatch.StartNew();
//            foreach (string E in Entities)
//            {
//                System.Collections.Generic.Dictionary<string, string> DictEntity = await System.Threading.Tasks.Task.Run(() => EntityInformation(E));
//                DiversityWorkbench.Entity.AddEntityToDictionary(DictEntity);
//            }
//            watch.Stop();
//            Milliseconds = watch.ElapsedMilliseconds;
//#if DEBUG
//            System.Windows.Forms.MessageBox.Show("public static async void InitEntityInformationAsync(System.Collections.Generic.List<string> Entities)\r\n" + 
//                "for " + Entities.Count.ToString() + " Entities:\r\n" +
//                Milliseconds.ToString() + " milliseconds");
//#endif
//        }

        public System.Collections.Generic.Dictionary<string, string> EntityInformation_O(string Entity)
        {
            if (DiversityWorkbench.Entity._EntityDictionary != null
                && DiversityWorkbench.Entity._EntityDictionary.ContainsKey(Entity))
            {
                if (DiversityWorkbench.Entity._EntityDictionary[Entity].ContainsKey(DiversityWorkbench.Settings.Language))
                {
                    if (DiversityWorkbench.Entity._EntityDictionary[Entity][DiversityWorkbench.Settings.Language].ContainsKey(DiversityWorkbench.Settings.Context))
                        return DiversityWorkbench.Entity._EntityDictionary[Entity][DiversityWorkbench.Settings.Language][DiversityWorkbench.Settings.Context];
                }
            }
            System.Collections.Generic.Dictionary<string, string> DictEntity = new Dictionary<string, string>();
            if (this._ConnectionString.Length > 0)
            {
                string SQL = "select * from [dbo].[EntityInformation_2] ('" + Entity + "', '" + DiversityWorkbench.Settings.Language + "', '" + DiversityWorkbench.Settings.Context + "')";
                System.Data.DataTable dtEntity = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
                try
                {
                    ad.Fill(dtEntity);
                }
                catch { }
                if (dtEntity.Rows.Count > 0)
                {
                    foreach (System.Data.DataColumn C in dtEntity.Columns)
                    {
                        DictEntity.Add(C.ColumnName, dtEntity.Rows[0][C].ToString());
                    }
                }
                DiversityWorkbench.Entity.AddEntityToDictionary(DictEntity);
            }
            return DictEntity;
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>>> _EntityDictionary;

        private static void AddEntityToDictionary(System.Collections.Generic.Dictionary<string, string> Dict)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> DictContext = new Dictionary<string,Dictionary<string,string>>();
            DictContext.Add(DiversityWorkbench.Settings.Context, Dict);
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>> DictLanguage = new Dictionary<string,Dictionary<string,Dictionary<string,string>>>();
            DictLanguage.Add(DiversityWorkbench.Settings.Language, DictContext);

            if (Entity._EntityDictionary == null)
                Entity._EntityDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
            try
            {
                if (!DiversityWorkbench.Entity._EntityDictionary.ContainsKey(Dict["Entity"]))
                    DiversityWorkbench.Entity._EntityDictionary.Add(Dict["Entity"], DictLanguage);
                else
                {
                    if (!DiversityWorkbench.Entity._EntityDictionary[Dict["Entity"]].ContainsKey(DiversityWorkbench.Settings.Language))
                    {
                        DiversityWorkbench.Entity._EntityDictionary[Dict["Entity"]].Add(DiversityWorkbench.Settings.Language, DictContext);
                    }
                    else
                    {
                        if (!DiversityWorkbench.Entity._EntityDictionary[Dict["Entity"]][DiversityWorkbench.Settings.Language].ContainsKey(DiversityWorkbench.Settings.Context))
                            DiversityWorkbench.Entity._EntityDictionary[Dict["Entity"]][DiversityWorkbench.Settings.Language].Add(DiversityWorkbench.Settings.Context, Dict);
                    }
                }
            }
            catch { }
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>>> EntityDictionary
        {
            get { return Entity._EntityDictionary; }
        }

        public static void EntityReset() { Entity._EntityDictionary = null; }

        public static string EntityContent(EntityInformationField Field, System.Collections.Generic.Dictionary<string, string> Entity)
        {
            string Content = "";
            if (Entity.ContainsKey(Field.ToString()))
            {
                switch (Field)
                {
                    case EntityInformationField.Abbreviation:
                        if (Entity.ContainsKey(EntityInformationField.Abbreviation.ToString()) && Entity.ContainsKey(EntityInformationField.AbbreviationOK.ToString()))
                        {
                            if (Entity[EntityInformationField.AbbreviationOK.ToString()].ToLower() == "true")
                                Content = Entity[EntityInformationField.Abbreviation.ToString()];
                        }
                        break;
                    case EntityInformationField.Description:
                        if (Entity.ContainsKey(EntityInformationField.Description.ToString()) && Entity.ContainsKey(EntityInformationField.DescriptionOK.ToString()))
                        {
                            if (Entity[EntityInformationField.DescriptionOK.ToString()].ToLower() == "true")
                                Content = Entity[EntityInformationField.Description.ToString()];
                        }
                        break;
                    case EntityInformationField.DisplayText:
                        if (Entity.ContainsKey(EntityInformationField.DisplayText.ToString()) && Entity.ContainsKey(EntityInformationField.DisplayTextOK.ToString()))
                        {
                            if (Entity[EntityInformationField.DisplayTextOK.ToString()].ToLower() == "true")
                                Content = Entity[EntityInformationField.DisplayText.ToString()];
                        }
                        break;
                }
            }
            return Content;
        }

        public static string EntityText(string Entity, EntityInformationField PreferedField)
        {
            System.Collections.Generic.Dictionary<string, string> DictEntity = DiversityWorkbench.Entity.EntityInformation(Entity);
            string DisplayText = DiversityWorkbench.Entity.EntityContent(PreferedField, DictEntity);
            if (DisplayText.Length == 0)
            {
                switch (PreferedField)
                {
                    case EntityInformationField.Abbreviation:
                        if (DictEntity.ContainsKey(EntityInformationField.Abbreviation.ToString()) && DictEntity.ContainsKey(EntityInformationField.AbbreviationOK.ToString()))
                        {
                            DisplayText = DictEntity[EntityInformationField.Abbreviation.ToString()];
                        }
                        break;
                    case EntityInformationField.Description:
                        if (DictEntity.ContainsKey(EntityInformationField.Description.ToString()) && DictEntity.ContainsKey(EntityInformationField.DescriptionOK.ToString()))
                        {
                            DisplayText = DictEntity[EntityInformationField.Description.ToString()];
                        }
                        break;
                    case EntityInformationField.DisplayText:
                        if (DictEntity.ContainsKey(EntityInformationField.DisplayText.ToString()) && DictEntity.ContainsKey(EntityInformationField.DisplayTextOK.ToString()))
                        {
                            DisplayText = DictEntity[EntityInformationField.DisplayText.ToString()];
                        }
                        break;
                }
            }
            if (DisplayText.Length == 0)
            {
                DisplayText = Entity;
            }
            return DisplayText;
        }

        //public string EntityContent_O(EntityInformationField Field, System.Collections.Generic.Dictionary<string, string> Entity)
        //{
        //    string Content = "";
        //    if (Entity.ContainsKey(Field.ToString()))
        //    {
        //        switch (Field)
        //        {
        //            case EntityInformationField.Abbreviation:
        //                if (Entity.ContainsKey(EntityInformationField.Abbreviation.ToString()) && Entity.ContainsKey(EntityInformationField.AbbreviationOK.ToString()))
        //                {
        //                    if (Entity[EntityInformationField.AbbreviationOK.ToString()].ToLower() == "true")
        //                        Content = Entity[EntityInformationField.Abbreviation.ToString()];
        //                }
        //                break;
        //            case EntityInformationField.Description:
        //                if (Entity.ContainsKey(EntityInformationField.Description.ToString()) && Entity.ContainsKey(EntityInformationField.DescriptionOK.ToString()))
        //                {
        //                    if (Entity[EntityInformationField.DescriptionOK.ToString()].ToLower() == "true")
        //                        Content = Entity[EntityInformationField.Description.ToString()];
        //                }
        //                break;
        //            case EntityInformationField.DisplayText:
        //                if (Entity.ContainsKey(EntityInformationField.DisplayText.ToString()) && Entity.ContainsKey(EntityInformationField.DisplayTextOK.ToString()))
        //                {
        //                    if (Entity[EntityInformationField.DisplayTextOK.ToString()].ToLower() == "true")
        //                        Content = Entity[EntityInformationField.DisplayText.ToString()];
        //                }
        //                break;
        //        }
        //    }
        //    return Content;
        //}

        public static void setEntityRepresentation(string Entity, string Representation, EntityInformationField PreferedField, bool OnlyIfMissing = false)
        {
            if (PreferedField == EntityInformationField.AbbreviationOK ||
                PreferedField == EntityInformationField.DescriptionOK ||
                PreferedField == EntityInformationField.DisplayTextOK ||
                PreferedField == EntityInformationField.DoesExist)
                return;
            try
            {
                string SQL = "SELECT COUNT(*) FROM Entity E WHERE E.Entity = '" + Entity + "'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "0")
                {
                    SQL = "INSERT INTO [dbo].[Entity] ([Entity]) VALUES ('" + Entity + "')";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
                SQL = "SELECT count(*) FROM EntityRepresentation " +
                    " WHERE Entity = '" + Entity + "' " +
                    " AND LanguageCode = N'" + DiversityWorkbench.Settings.Language + "'" +
                    " AND EntityContext = N'" + DiversityWorkbench.Settings.Context + "'";
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "0")
                {
                    SQL = "INSERT INTO [dbo].[EntityRepresentation] ([Entity], LanguageCode, EntityContext, " + PreferedField.ToString() + ") " +
                        " VALUES ('" + Entity + "', " +
                        "'" + DiversityWorkbench.Settings.Language + "', " +
                        "'" + DiversityWorkbench.Settings.Context + "', " +
                        "'" + Representation.Replace("'", "''") + "')";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
                else if (!OnlyIfMissing)
                {
                    SQL = "UPDATE E SET " + PreferedField.ToString() + " = '" + Representation.Replace("'", "''") + "' " +
                        " FROM [dbo].[EntityRepresentation] E " +
                        " WHERE [Entity] = '" + Entity + "' " +
                        " AND LanguageCode = '" + DiversityWorkbench.Settings.Language + "' " +
                        " AND EntityContext =  '" + DiversityWorkbench.Settings.Context + "'";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

#endregion

#region Setting the form controls

        public static void setEntity(System.Windows.Forms.Form Form, System.Windows.Forms.ToolTip ToolTip)
        {
            foreach (System.Windows.Forms.Control C in Form.Controls)
            {
                Entity.setEntity(C, ToolTip);
            }
            //if (Form.MainMenuStrip != null)
            //    Entity.setEnitityMenu(Form, Form.MainMenuStrip);
        }

        //private static void setEntitySync(System.Windows.Forms.Form Form, System.Windows.Forms.ToolTip ToolTip)
        //{
        //    foreach (System.Windows.Forms.Control C in Form.Controls)
        //    {
        //        Entity.setEntity(C, ToolTip);
        //    }
        //}

        //private static void setEntityAsync(System.Windows.Forms.Form Form, System.Windows.Forms.ToolTip ToolTip)
        //{
        //    System.Collections.Generic.Dictionary<System.Windows.Forms.Control, System.ComponentModel.ComponentResourceManager> Controls = new Dictionary<System.Windows.Forms.Control, ComponentResourceManager>();
        //    foreach (System.Windows.Forms.Control C in Form.Controls)
        //    {
        //        Entity.getControls(C, ref Controls);
        //    }
        //}

        private static void getControls(System.Windows.Forms.Control Control, ref System.Collections.Generic.Dictionary<System.Windows.Forms.Control, System.ComponentModel.ComponentResourceManager> Controls)
        {
            string Title = "";
            string TextFromResource = "";
            if (Control.AccessibleName != null)
            {
                System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>(); ;
                if (Control.AccessibleName != null
                    && Control.AccessibleName.ToString().Length > 0
                    && TextFromResource != null
                    && TextFromResource.Length > 0)
                {
                    // LABEL
                    if (Control.GetType() == typeof(System.Windows.Forms.Label))
                    {
                        //if (!Controls.ContainsKey(Control))
                        //Controls.Add(Control, )
                    }

                    // CheckBox
                    if (Control.GetType() == typeof(System.Windows.Forms.CheckBox))
                    {

                    }

                    // BUTTON
                    if (Control.GetType() == typeof(System.Windows.Forms.Button))
                    {

                    }

                    // GROUPBOX
                    if (Control.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                    }

                    // Radiobutton
                    if (Control.GetType() == typeof(System.Windows.Forms.RadioButton))
                    {
                    }
                }
            }

            if (Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                && Control.GetType() != typeof(System.Windows.Forms.Button)
                && Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
            {
                foreach (System.Windows.Forms.Control C in Control.Controls)
                {
                    Entity.getControls(C, ref Controls);
                }
            }
        }

        public static void setEntity(System.Windows.Forms.Form Form, System.ComponentModel.ComponentResourceManager resource)
        {
            foreach (System.Windows.Forms.Control C in Form.Controls)
            {
                Entity.setEntity(C, resource);
            }
        }

        public static void setEntity(System.Windows.Forms.Control Control, System.ComponentModel.ComponentResourceManager resource)
        {
            string Title = "";
            string TextFromResource = "";
            if (Control.AccessibleName != null)
            {
                try { TextFromResource = resource.GetString(Control.AccessibleName.ToString()); }
                catch { }
                System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>(); ;
                if (Control.AccessibleName != null
                    && Control.AccessibleName.ToString().Length > 0
                    && TextFromResource != null
                    && TextFromResource.Length > 0)
                {
                    Title = resource.GetString(Control.AccessibleName.ToString());
                }
                if (Title.Length > 0)
                {
                    // LABEL
                    if (Control.GetType() == typeof(System.Windows.Forms.Label))
                    {
                        System.Windows.Forms.Label L = (System.Windows.Forms.Label)Control;
                        // save original Text
                        if (L.AccessibleDescription == null || L.AccessibleDescription.Length == 0)
                            L.AccessibleDescription = L.Text;
                        L.Text = Title;
                    }

                    // CheckBox
                    if (Control.GetType() == typeof(System.Windows.Forms.CheckBox))
                    {
                        System.Windows.Forms.CheckBox C = (System.Windows.Forms.CheckBox)Control;
                        if (C.AccessibleDescription == null || C.AccessibleDescription.Length == 0)
                            C.AccessibleDescription = C.Text;
                        C.Text = Title;
                    }

                    // BUTTON
                    if (Control.GetType() == typeof(System.Windows.Forms.Button))
                    {
                        System.Windows.Forms.Button B = (System.Windows.Forms.Button)Control;
                        if (B.AccessibleDescription == null || B.AccessibleDescription.Length == 0)
                            B.AccessibleDescription = B.Text;
                        B.Text = Title;
                    }


                    // GROUPBOX
                    if (Control.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        System.Windows.Forms.GroupBox G = (System.Windows.Forms.GroupBox)Control;
                        if (G.AccessibleDescription == null || G.AccessibleDescription.Length == 0)
                            G.AccessibleDescription = G.Text;
                        G.Text = Title;
                    }

                    // Radiobutton
                    if (Control.GetType() == typeof(System.Windows.Forms.RadioButton))
                    {
                        System.Windows.Forms.RadioButton G = (System.Windows.Forms.RadioButton)Control;
                        if (G.AccessibleDescription == null || G.AccessibleDescription.Length == 0)
                            G.AccessibleDescription = G.Text;
                        G.Text = Title;
                    }
                }
            }

            if (Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                && Control.GetType() != typeof(System.Windows.Forms.Button)
                && Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
            {
                foreach (System.Windows.Forms.Control C in Control.Controls)
                {
                    Entity.setEntity(C, resource);
                }
            }
        }

        public static void setEntity(System.Windows.Forms.Control Control, System.Windows.Forms.ToolTip ToolTip)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            try
            {
                string TableColumn = "";
                if (Control.AccessibleName != null
                    && Control.AccessibleName.ToString().Length > 0)
                {
                    //Dict = DiversityWorkbench.Entity.EntityInformation(Control.AccessibleName.ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    Dict = DiversityWorkbench.Entity.EntityInformation(Control.AccessibleName.ToString());
                }
                else
                {
                    string Entity = DiversityWorkbench.Entity.getEntityOfControlViaDatabinding(Control);
                    Dict = DiversityWorkbench.Entity.EntityInformation(Entity);
                    TableColumn = Entity;
                }
                if (Dict.Count > 0 && Dict["Entity"].Length > 0 && Dict["DoesExist"] == "True")
                {
                    if ((Dict["Visibility"] == Visibility.hidden.ToString())
                        && Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                    {
                        //Control.Visible = false;
                    }
                    else
                    {
                        //Control.Visible = true;

                        // TEXTBOX
                        if (Control.GetType() == typeof(System.Windows.Forms.TextBox))
                        {
                            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)Control;
                            if (Dict["Description"].Length > 0 && Dict["DescriptionOK"] == "True")
                                ToolTip.SetToolTip(T, Dict["Description"]);
                            if (Dict["Accessibility"] == Accessibility.read_only.ToString())
                                T.ReadOnly = true;
                            //else T.ReadOnly = false;
                        }

                        // LABEL
                        if (Control.GetType() == typeof(System.Windows.Forms.Label)
                            && Control.AccessibleName != Dict["Abbreviation"])
                        //&& !Control.AccessibleName.EndsWith(Dict["Abbreviation"]))
                        {
                            System.Windows.Forms.Label L = (System.Windows.Forms.Label)Control;
                            if (Dict["Abbreviation"].Length > 0 && Dict["AbbreviationOK"] == "True")
                            {
                                // save original Text
                                if (L.AccessibleDescription == null || L.AccessibleDescription.Length == 0)
                                    L.AccessibleDescription = L.Text;
                                string Trailer = "";
                                System.Collections.Generic.List<string> TrailerList = new List<string>();
                                TrailerList.Add(" :");
                                TrailerList.Add(":");
                                TrailerList.Add(" ...");
                                foreach (string T in TrailerList)
                                {
                                    if (L.Text.EndsWith(T))
                                    {
                                        Trailer = T;
                                        break;
                                    }
                                }
                                L.Text = Dict["Abbreviation"] + Trailer;
                            }
                            else if (L.AccessibleDescription != null && L.AccessibleDescription.Length > 0)
                                L.Text = L.AccessibleDescription;
                        }

                        // CheckBox
                        if (Control.GetType() == typeof(System.Windows.Forms.CheckBox))
                        {
                            System.Windows.Forms.CheckBox C = (System.Windows.Forms.CheckBox)Control;
                            if (Dict["Description"].Length > 0 && Dict["DescriptionOK"] == "True")
                                ToolTip.SetToolTip(C, Dict["Description"]);
                            if (Dict["Accessibility"] == Accessibility.read_only.ToString())
                                C.Enabled = false;
                            //else C.Enabled = true;
                            if (Dict["Abbreviation"].Length > 0 && Dict["AbbreviationOK"] == "True")
                                C.Text = Dict["Abbreviation"];
                        }

                        // BUTTON
                        if (Control.GetType() == typeof(System.Windows.Forms.Button))
                        {
                            System.Windows.Forms.Button B = (System.Windows.Forms.Button)Control;
                            if (B.Text.Length == 0)
                            {
                                ToolTip.SetToolTip(B, Dict["Description"]);
                            }
                            else
                            {
                                B.Text = Dict["DisplayText"];
                                ToolTip.SetToolTip(B, Dict["Description"]);
                            }
                        }

                        // RADIOBUTTON
                        if (Control.GetType() == typeof(System.Windows.Forms.RadioButton))
                        {
                            System.Windows.Forms.RadioButton B = (System.Windows.Forms.RadioButton)Control;
                            if (B.Text.Length == 0)
                            {
                                ToolTip.SetToolTip(B, Dict["Description"]);
                            }
                            else
                            {
                                B.Text = Dict["DisplayText"];
                                ToolTip.SetToolTip(B, Dict["Description"]);
                            }
                        }


                        // GROUPBOX
                        if (Control.GetType() == typeof(System.Windows.Forms.GroupBox)
                            && Control.AccessibleName != Dict["DisplayText"])
                        //&& !Control.AccessibleName.EndsWith(Dict["DisplayText"]))
                        {
                            System.Windows.Forms.GroupBox G = (System.Windows.Forms.GroupBox)Control;
                            if (Dict["DisplayText"].Length > 0 && Dict["DisplayTextOK"] == "True")
                            {
                                if (G.AccessibleDescription == null || G.AccessibleDescription.Length > 0)
                                    G.AccessibleDescription = G.Text;
                                G.Text = Dict["DisplayText"];
                            }
                            else if (G.AccessibleDescription != null && G.AccessibleDescription.Length > 0)
                                G.Text = G.AccessibleDescription;
                            if (Dict["Description"].Length > 0 && Dict["DescriptionOK"] == "True")
                                ToolTip.SetToolTip(G, Dict["Description"]);
                        }

                        // USER CONTROL MODULE RELATED ENTRY
                        if (Control.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry))
                        {
                            DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)Control;
                            UC.setEntity();
                        }

                        // USER CONTROL QUERY LIST
                        if (Control.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryList))
                        {
                            DiversityWorkbench.UserControls.UserControlQueryList UC = (DiversityWorkbench.UserControls.UserControlQueryList)Control;
                            UC.setEntity();
                        }

                        //// USER CONTROL QUERY CONDITION
                        if (Control.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                        {
                            DiversityWorkbench.UserControls.UserControlQueryCondition UC = (DiversityWorkbench.UserControls.UserControlQueryCondition)Control;
                            UC.setEntity();
                        }

                    }
                }
                else if (TableColumn.Length > 0)
                {
                    string Description = DiversityWorkbench.EntityDescription.TableColumnDescription(TableColumn);
                    ToolTip.SetToolTip(Control, Description);
                }
                if (Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    && Control.GetType() != typeof(System.Windows.Forms.Button)
                    && Control.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                {
                    foreach (System.Windows.Forms.Control C in Control.Controls)
                    {
                        Entity.setEntity(C, ToolTip);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static string getEntityOfControlViaDatabinding(System.Windows.Forms.Control Control)
        {
            //System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>(); ;
            string Entity = "";
            try
            {
                if (Control.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)Control;
                    if (T.DataBindings.Count > 0)
                    {
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)T.DataBindings[0].DataSource;
                        return BS.DataMember + "." + T.DataBindings[0].BindingMemberInfo.BindingMember;
                    }
                }

                if (Control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox T = (System.Windows.Forms.ComboBox)Control;
                    if (T.DataBindings.Count > 0)
                    {
                        if (T.DataBindings[0].DataSource.GetType() == typeof(System.Windows.Forms.BindingSource))
                        {
                            System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)T.DataBindings[0].DataSource;
                            Entity = BS.DataMember + "." + T.DataBindings[0].BindingMemberInfo.BindingMember;
                            return Entity;
                        }
                        else return "";
                    }
                }

                if (Control.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    System.Windows.Forms.CheckBox T = (System.Windows.Forms.CheckBox)Control;
                    if (T.DataBindings.Count > 0)
                    {
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)T.DataBindings[0].DataSource;
                        Entity = BS.DataMember + "." + T.DataBindings[0].BindingMemberInfo.BindingMember;
                        return Entity;
                    }
                }

                if (Control.GetType() == typeof(System.Windows.Forms.MaskedTextBox))
                {
                    System.Windows.Forms.MaskedTextBox T = (System.Windows.Forms.MaskedTextBox)Control;
                    if (T.DataBindings.Count > 0)
                    {
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)T.DataBindings[0].DataSource;
                        Entity = BS.DataMember + "." + T.DataBindings[0].BindingMemberInfo.BindingMember;
                        return Entity;
                    }
                }

                if (Entity.Length == 0)
                {
                    object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                    Control.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        System.Windows.Forms.BindingMemberInfo bmi = Control.DataBindings[0].BindingMemberInfo;
                        string Column = bmi.BindingField;
                        string Table = bmi.BindingPath;
                        if (Table.Length == 0)
                        {
                            System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                            if (B.DataSource.GetType() == typeof(System.Windows.Forms.BindingSource))
                            {
                                System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                                Table = BS.DataMember.ToString();
                                Column = B.BindingMemberInfo.BindingField;
                            }
                            else if (B.DataSource.GetType().BaseType == typeof(System.Data.DataTable))
                            {
                                Table = B.DataSource.ToString();
                                Column = B.BindingMemberInfo.BindingField;
                            }
                        }
                        Entity = Table + "." + Column;
                    }
                }
            }
            catch { }
            return Entity;
        }
        
#endregion

#region Language and Context
        
        public static void Reset()
        {
            _Context = null;
            _dtContext = null;
            _dtLanguage = null;
            _LanguageCodes = null;
            _EntityDictionary = null;
        }

#region Language

        public static System.Drawing.Image LanguageImage
        {
            get
            {
                DiversityWorkbench.Entity E = new Entity();
                System.Drawing.Image I = E.imageListLanguage.Images[0];
                switch (DiversityWorkbench.Settings.Language)
                {
                    case "en-US":
                        I = E.imageListLanguage.Images[2];//[0];  MW 2019-11-12 - wieder auf GB-Flagge zurueckgesetzt
                        break;
                    case "de-DE":
                        I = E.imageListLanguage.Images[1];
                        break;
                    case "en-GB":
                        I = E.imageListLanguage.Images[2];
                        break;
                }
                return I;
            }
        }

        private static System.Collections.Generic.Dictionary<string, string> _LanguageCodes;

        public static System.Collections.Generic.Dictionary<string, string> LanguageCodes
        {
            get
            {
                if (Entity._LanguageCodes == null)
                {
                    foreach (System.Data.DataRow R in DiversityWorkbench.Entity.DtLanguage.Rows)
                    {
                        _LanguageCodes.Add(R["Code"].ToString(), R["DisplayText"].ToString());
                    }
                }
                return Entity._LanguageCodes;
            }
        }

        private static System.Data.DataTable _dtLanguage;

        public static System.Data.DataTable DtLanguage
        {
            get
            {
                if (_dtLanguage == null)
                {
                    _dtLanguage = new System.Data.DataTable();
                    string SQL = "SELECT Code, DisplayText FROM EntityLanguageCode_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(_dtLanguage);
                }
                return _dtLanguage;
            }
        }

#endregion

#region Context

        private static System.Collections.Generic.Dictionary<string, string> _Context;

        public static System.Collections.Generic.Dictionary<string, string> Context
        {
            get
            {
                if (Entity._Context == null)
                {
                    foreach (System.Data.DataRow R in DiversityWorkbench.Entity.DtContext.Rows)
                    {
                        _Context.Add(R["Code"].ToString(), R["DisplayText"].ToString());
                    }
                }
                return Entity._Context;
            }
        }

        private static System.Data.DataTable _dtContext;

        public static System.Data.DataTable DtContext
        {
            get
            {
                if (_dtContext == null)
                {
                    _dtContext = new System.Data.DataTable();
                    string SQL = "SELECT DISTINCT EntityContext AS Code, DisplayText " +
                        "FROM EntityRepresentation " +
                        "E where E.Entity like 'EntityContext_Enum.Code.%' " +
                        "and E.LanguageCode = '" + DiversityWorkbench.Settings.Language + "'";

                    //string SQL = "SELECT DISTINCT EntityContext AS Code, EntityContext AS DisplayText FROM dbo.EntityRepresentation " +
                    //    " UNION SELECT DISTINCT EntityContext AS Code, EntityContext AS DisplayText FROM EntityUsage ORDER BY EntityContext";
                    try
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(_dtContext);
                    }
                    catch { }
                    if (_dtContext.Rows.Count == 0 && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        SQL = "SELECT 'General' AS Code, 'General' AS DisplayText ";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(_dtContext);
                    }

                    //string SQL = "SELECT Code, DisplayText FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText";
                    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    //ad.Fill(_dtContext);
                }
                return _dtContext;
            }
        }

        public static void ResetContextTable()
        {
            try
            {
                _dtContext = null;
            }
            catch { }
        }

#endregion

        public static string Message(string MessageIdentifier)
        {
            string Message = "";
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
            Message = resources.GetString(MessageIdentifier);
            return Message;
        }
        
#endregion

    }
}
