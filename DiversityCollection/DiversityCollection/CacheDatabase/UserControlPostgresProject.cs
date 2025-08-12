using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlPostgresProject : UserControl
    {
        private string _Project;
        private int _ProjectID;
        private InterfaceCacheDB _Interface;

        public UserControlPostgresProject(int ProjectID, string Project, InterfaceCacheDB Interface)
        {
            InitializeComponent();
            try
            {
                this._Project = Project;
                this.labelTitle.Text = this._Project;
                this._ProjectID = ProjectID;
                this._Interface = Interface;
                this.initControl();
            }
            catch(System.Exception ex)
            { }
        }

        private void initControl()
        {
            this.UpdateCheckPostgresProject();
        }

        private void tableLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            //DiversityCollection.CacheDatabase.FormCacheDB.PostgresSelectedProject = this._Project;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // check resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
                System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdatePGSchema_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> ReplaceStrings = new Dictionary<string, string>();
                    ReplaceStrings.Add("#project#", this._Project);
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    DiversityWorkbench.Forms.FormUpdateDatabase f =
                        new DiversityWorkbench.Forms.FormUpdateDatabase(
                            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, // .Postgres.PostgresConnection().Database,
                            DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion.ToString(),
                            con,
                            Versions,
                            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(),
                            this._Project,
                            ReplaceStrings);
                    f.ForPostgres = true;
                    f.ShowInTaskbar = true;
                    f.ShowDialog();
                    this._Interface.initPostgresAdminProjectLists();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
            }
            catch (System.Exception ex)
            { }
        }

        public bool PostgresEstablishProject()
        {
            bool Establihed = true;
            try
            {
                string Message = "";
                string SQL = "CREATE SCHEMA  \"" + this._Project + "\"" +
                    "AUTHORIZATION \"CacheAdmin\";";
                // Create the Schema
                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL))
                {
                    if (Message.Contains("42P06"))
                    //if (Message.IndexOf("42P06: ") > -1 && Message.ToLower().IndexOf(" schema") > -1)
                    //    Establihed = true;
                    //else if ((Message.StartsWith("42P06: schema") || Message.StartsWith("42P06: Schema")) && (Message.EndsWith("already exists") || Message.EndsWith("existiert bereits")))
                        Establihed = true;
                    else
                        Establihed = false;
                }
                if (Establihed)
                {
                    SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + this._Project + "\"" +
                        "GRANT EXECUTE ON FUNCTIONS " +
                        "TO \"CacheUser\";";
                    Message = "";
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL))
                    {
                        Establihed = false;
                    }
                    else
                    {
                        SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + this._Project + "\"" +
                            "GRANT SELECT ON TABLES " +
                            "TO \"CacheUser\";";
                        Message = "";
                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) //.Postgres.PostgresExecuteSqlNonQuery(SQL))
                            Establihed = false;
                        else
                        {
                            SQL = "CREATE OR REPLACE FUNCTION \"" + this._Project + "\".version() " +
                                "RETURNS integer AS " +
                                "$BODY$ " +
                                "declare " +
                                "v integer; " +
                                "BEGIN " +
                                "SELECT 0 into v; " +
                                "RETURN v; " +
                                "END; " +
                                "$BODY$ " +
                                "LANGUAGE plpgsql STABLE " +
                                "COST 100; " +
                                "ALTER FUNCTION \"" + this._Project + "\".version() " +
                                "OWNER TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this._Project + "\".version() TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this._Project + "\".version() TO \"CacheUser\"";
                            if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL)) //.Postgres.PostgresExecuteSqlNonQuery(SQL))
                                Establihed = false;

                            SQL = "CREATE OR REPLACE FUNCTION \"" + this._Project + "\".projectid() " +
                                "RETURNS integer AS " +
                                "$BODY$ " +
                                "declare " +
                                "v integer; " +
                                "BEGIN " +
                                "SELECT " + this._ProjectID.ToString() + " into v; " +
                                "RETURN v; " +
                                "END; " +
                                "$BODY$ " +
                                "LANGUAGE plpgsql STABLE " +
                                "COST 100; " +
                                "ALTER FUNCTION \"" + this._Project + "\".projectid() " +
                                "OWNER TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this._Project + "\".projectid() TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this._Project + "\".projectid() TO \"CacheUser\"";
                            if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL)) //.Postgres.PostgresExecuteSqlNonQuery(SQL))
                                Establihed = false;
                        }
                    }
                }
            }
            catch(System.Exception ex)
            { }
            return Establihed;
        }

        public void UpdateCheckPostgresProject()
        {
            string SQL = "select \"" + this._Project + "\".version()";
            try
            {
                int version = int.Parse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL));// .Postgres.PostgresExecuteSqlSkalar(SQL));
                if (version < DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion)
                {
                    this.buttonUpdate.Visible = true;
                    this.buttonUpdate.Text = "Update to v. " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion.ToString();
                }
                else this.buttonUpdate.Visible = false;
                this.labelVersion.Text = "Version " + version.ToString();
            }
            catch (System.Exception ex)
            { }
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {
            this._Interface.setPostgresProject(this._Project, this.buttonUpdate.Visible);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().ClearSchema(this._Project))
            {
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().DeleteSchema(this._Project))
                {
                    this._Interface.initPostgresAdminProjectLists();
                    this._Interface.initOverviewPostgres();
                }
            }
            else System.Windows.Forms.MessageBox.Show("Deleting of project failed");
        }

    }
}
