using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class LinkedServerDatabase
    {
        private string _DatabaseName;

        private DiversityWorkbench.LinkedServer _LinkedServer;
        public string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }

        private string _DiversityWorkbenchModule;
        public string DiversityWorkbenchModule
        {
            get { return _DiversityWorkbenchModule; }
            set { _DiversityWorkbenchModule = value; }
        }

        private string _BaseURL;
        public string BaseURL
        {
            get { return _BaseURL; }
            set { _BaseURL = value; }
        }

        public LinkedServerDatabase(DiversityWorkbench.LinkedServer LinkedServer, string DatabaseName)
        {
            this._DatabaseName = DatabaseName;
            this._LinkedServer = LinkedServer;
        }

    }
}
