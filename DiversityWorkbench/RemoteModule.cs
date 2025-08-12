using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    // ARiane uncommented for .NET8
    /// <summary>
    /// Interface for the retrieval of a remote item
    /// </summary>
    public interface IRemoteModule
    {
        bool ModuleIsAvailable();
        bool DatabaseIsAvailable();
        void setServerConnection(DiversityWorkbench.ServerConnection ServerConnection);
        DiversityWorkbench.ServerConnection getServerConnection();
        //string getTextString();
        //RemoteValues getRemoteValues();
        //RemoteValues getRemoteValues(int ID);
        //DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns();
        //System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions();
    }

    
    /// <summary>
    /// base class for remote modules providing basic funtionallity to get in access with a remote module
    /// and starting and stopping the remote module
    /// </summary>
    public class RemoteModule : DiversityWorkbench.IRemoteModule
    {
        #region Parameter
        private string _ConnectionState;

        protected static System.Diagnostics.Process _Process;
        protected DiversityWorkbench.ServerConnection _ServerConnection;
        protected string _MainTable;
        #endregion

        #region Construction
        public RemoteModule() 
        {
            //ChannelRegistration.RegisterTcpChannelClient();
        }

        public RemoteModule(DiversityWorkbench.ServerConnection ServerConnection) : this()
        {
            this._ServerConnection = ServerConnection;
        }

        #endregion

        #region Properties
        public virtual string MainTable
        {
            get { return _MainTable; }
            set { _MainTable = value; }
        }

        public string ConnectionState
        {
            get { return _ConnectionState; }
            set { _ConnectionState = value; }
        }

        protected virtual string ModuleName { get { return DiversityWorkbench.WorkbenchSettings.Default.ModuleName; } }

        protected virtual string RemoteManagerName
        {
            get
            {
                string RMN = "RemoteManager" + Settings.ModuleName;
                return RMN;
            }
        }

        protected virtual string URL { get { return ""; } }

        protected virtual int TcpPortRemoting { get { return -1; } }

        protected virtual System.Diagnostics.Process Process
        {
            get { return RemoteModule._Process; }
            set { RemoteModule._Process = value; }
        }
        
        protected string ServerApplicationFileName
        {
            get
            {
                string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ApplicationDirectory() + "\\" + Settings.ModuleName + ".exe";
                return Path;
            }
        }

        #endregion

        #region Interface

        public virtual void setServerConnection(DiversityWorkbench.ServerConnection ServerConnection)
        {
            this._ServerConnection = ServerConnection;
        }

        public virtual DiversityWorkbench.ServerConnection getServerConnection()
        {
            return this._ServerConnection;
        }

        public virtual bool DatabaseIsAvailable()
        {
            bool OK = false;
            DiversityWorkbench.IRemoteModule iRM;
            try
            {
                iRM = (DiversityWorkbench.IRemoteModule)Activator.GetObject(typeof(DiversityWorkbench.IRemoteModule), this.URL);
                OK = iRM.DatabaseIsAvailable();
            }
            catch { }
            return OK;
        }
        
        public bool ModuleIsAvailable()
        {
            bool OK = false;
            DiversityWorkbench.IRemoteModule iRM;
            try
            {
                iRM = (DiversityWorkbench.IRemoteModule)Activator.GetObject(typeof(DiversityWorkbench.IRemoteModule), this.URL);
                OK = iRM.ModuleIsAvailable();
            }
            catch { }
            return OK;  
        }

        public virtual string getTestString()
        {
            System.Windows.Forms.MessageBox.Show("##6 getTestString() RemoteModule");
            string Test = "RemoteModule";
            try
            {
                //DiversityWorkbench.ITestServer I;
                ////string URL = "tcp://localhost:" + DiversityWorkbench.WorkbenchSettings.Default.TcpPortTestServer.ToString() + "/" + this.RemoteManagerName;
                //I = (DiversityWorkbench.ITestServer)Activator.GetObject(typeof(DiversityWorkbench.ITestServer), this.URL);
                //Test = I.getTestString();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "getTestString()", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return Test;
        }

        public virtual RemoteValues getRemoteValues()
        {
            DiversityWorkbench.RemoteValues V = new DiversityWorkbench.RemoteValues();
            return V;
        }

        public virtual RemoteValues getRemoteValues(int ID)
        {
            DiversityWorkbench.RemoteValues V = new DiversityWorkbench.RemoteValues();
            return V;
        }

        //public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        //{
        //    DiversityWorkbench.UserControls.QueryDisplayColumn[] Q;
        //    DiversityWorkbench.IRemoteModule iRM;
        //    try
        //    {
        //        string URL = "tcp://localhost:" + DiversityWorkbench.WorkbenchSettings.Default.TcpPortTestServer.ToString() + "/" + this.RemoteManagerName;
        //        iRM = (DiversityWorkbench.IRemoteModule)Activator.GetObject(typeof(DiversityWorkbench.IRemoteModule), URL);
        //        Q = iRM.QueryDisplayColumns();
        //    }
        //    catch 
        //    {
        //        Q = new QueryDisplayColumn[1];
        //    }
        //    return Q;
        //}

        //public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        //{
        //    System.Collections.Generic.List<DiversityWorkbench.QueryCondition> Q;
        //    DiversityWorkbench.IRemoteModule iRM;
        //    try
        //    {
        //        string URL = "tcp://localhost:" + DiversityWorkbench.WorkbenchSettings.Default.TcpPortTestServer.ToString() + "/" + this.RemoteManagerName;
        //        iRM = (DiversityWorkbench.IRemoteModule)Activator.GetObject(typeof(DiversityWorkbench.IRemoteModule), URL);
        //        Q = iRM.QueryConditions();
        //    }
        //    catch
        //    {
        //        Q = new List<QueryCondition>();
        //    }
        //    return Q;
        //}

        #endregion

        #region Process
        // ARiane uncommented no referneces .NET8
        //public void Start()
        //{
        //    DiversityWorkbench.IRemoteModule iRI;
        //    iRI = (DiversityWorkbench.IRemoteModule)Activator.GetObject(typeof(DiversityWorkbench.IRemoteModule), this.URL);
            
        //    // if process is running and module already available do not start a second one
        //    try { if (iRI.ModuleIsAvailable()) return; }
        //    catch  { }

        //    // module is not available
        //    if (this.Process == null)
        //    {
        //        if (!System.IO.File.Exists(this.ServerApplicationFileName)) throw new ArgumentException("File not found");
        //        try
        //        {
        //            System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
        //            StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //            StartInfo.FileName = this.ServerApplicationFileName;
        //            StartInfo.CreateNoWindow = true;
        //            StartInfo.Arguments = this.TcpPortRemoting.ToString();
        //            if (StartInfo.FileName.Length == 0 || StartInfo.Arguments.Length == 0)
        //                return;
        //            this.Process = new System.Diagnostics.Process();
        //            this.Process.StartInfo = StartInfo;
        //            this.Process.Start();
        //            System.Windows.Forms.MessageBox.Show("##4 Process.Start()");
        //        }
        //        catch (System.Exception ex)
        //        {
        //            throw new ArgumentException("File not found");
        //        }
        //    }
        //    else if (this.Process.HasExited)
        //    {
        //        try
        //        {
        //            this.Process.Start();
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

        public static void KillProcess()
        {
            try
            {
                if (RemoteModule._Process != null && !RemoteModule._Process.HasExited)
                    RemoteModule._Process.Kill();
            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error stopping the process for " + DiversityWorkbench.Settings.ModuleName, System.Windows.Forms.MessageBoxButtons.OK);
            }
        }
        
        #endregion
    }
}
