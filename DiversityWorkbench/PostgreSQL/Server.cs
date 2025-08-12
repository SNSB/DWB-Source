using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.PostgreSQL
{
    public class Server
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
        }
        private int _Port;

        public int Port
        {
            get { return _Port; }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> _Roles;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Role> Roles
        {
            get
            {
                if (this._Roles == null)
                    this._Roles = new Dictionary<string, Role>();
                return _Roles;
            }
            set { _Roles = value; }
        }

        public Server(string Name, int Port)
        {
            this._Name = Name;
            this._Port = Port;
            this._Roles = new Dictionary<string, Role>();
        }

    }
}
