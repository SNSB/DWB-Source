using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Data
{
    public class Role
    {

        #region Parameter

        private string _Name;
        private System.Data.DataTable _dtPermissions;
        private static readonly string _SqlRolesList = "SELECT name FROM sysusers WHERE (issqlrole = 1) AND (name <> N'public') AND (name NOT LIKE 'db_%') ORDER BY name ";

        public enum Permission { SELECT, INSERT, UPDATE, DELETE, EXECUTE }

        #endregion

        #region Construction

        public Role(string Name)
        {
            this._Name = Name;
        }
        
        #endregion

        public System.Data.DataTable Permissions()
        {
            if (this._dtPermissions == null)
            {
                this._dtPermissions = new System.Data.DataTable();
                try
                {
                    // the permissions
                    string SQL = "select sys.schemas.name as [Schema], sys.objects.name AS [Object in database], sys.database_permissions.permission_name AS Permission, sys.database_permissions.state_desc as State " +
                        ", sys.objects.type_desc AS [Type of object], sys.objects.create_date AS [Created], sys.objects.modify_date AS [Modified] " +
                        "from sys.database_permissions , sys.database_principals , sys.objects, sys.schemas " +
                        "where sys.database_principals.principal_id = sys.database_permissions.grantee_principal_id " +
                        "and sys.database_permissions.major_id = sys.objects.object_id " +
                        "and sys.database_principals.name = '" + this._Name + "' and sys.schemas.schema_id = sys.objects.schema_id " +
                        "order by sys.objects.name, sys.database_permissions.permission_name, sys.database_permissions.state_desc";
                    string Message = "";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtPermissions, ref Message);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return this._dtPermissions;
        }

        public static System.Collections.Generic.List<string> DatabaseRoles()
        {
            System.Collections.Generic.List<string> Roles = new List<string>();
            try
            {
                //string SQL = "SELECT name FROM sysusers WHERE (issqlrole = 1) AND (name <> N'public') AND (name NOT LIKE 'db_%') " +
                //    " ORDER BY name ";
                string Message = "";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(DiversityWorkbench.Data.Role._SqlRolesList, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                    Roles.Add(R[0].ToString());
            }
            catch(System.Exception ex)
            { }
            return Roles;
        }

        #region Included roles

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _IncludedRoles;

        public static void ResetIncludedRoles()
        {
            _IncludedRoles = null;
        }

        public static System.Collections.Generic.List<string> IncludedRoles(string Role)
        {
            if (_IncludedRoles == null)
                _IncludedRoles = new Dictionary<string, List<string>>();
            if (_IncludedRoles.ContainsKey(Role))
                return _IncludedRoles[Role];
            string SQL = "select M.name AS SuperiorRole " +
                "from sys.database_principals P, sys.database_role_members R, sys.database_principals M " +
                "where P.type = 'R' " +
                "and M.type = 'R' " +
                "and P.is_fixed_role = 0 " +
                "and P.principal_id = R.role_principal_id " +
                "and M.principal_id = R.member_principal_id " +
                "and P.Name = '" + Role + "'";
            System.Collections.Generic.List<string> Roles = new List<string>();
            try
            {
                string Message = "";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                    Roles.Add(R[0].ToString());
                if (!_IncludedRoles.ContainsKey(Role))
                    _IncludedRoles.Add(Role, Roles);
            }
            catch (System.Exception ex)
            { }
            return Roles;
        }
        
        #endregion

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _InheritingFromRoles;

        public static void ResetInheritingFromRoles()
        {
            _InheritingFromRoles = null;
        }

        public static System.Collections.Generic.List<string> InheritingFromRoles(string Role)
        {
            if (_InheritingFromRoles == null)
                _InheritingFromRoles = new Dictionary<string, List<string>>();
            if (_InheritingFromRoles.ContainsKey(Role))
                return _InheritingFromRoles[Role];

            string SQL = "select P.name AS ContainedRole " +
                "from sys.database_principals P, sys.database_role_members R, sys.database_principals M " +
                "where P.type = 'R' " +
                "and M.type = 'R' " +
                "and P.is_fixed_role = 0 " +
                "and P.principal_id = R.role_principal_id " +
                "and M.principal_id = R.member_principal_id " +
                "and M.Name = '" + Role + "'";
            System.Collections.Generic.List<string> Roles = new List<string>();
            try
            {
                string Message = "";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                    Roles.Add(R[0].ToString());
                _InheritingFromRoles.Add(Role, Roles);
            }
            catch (System.Exception ex)
            { }
            return Roles;
        }

        public static System.Data.DataTable Permissions(string Role, System.Collections.Generic.List<string> Objects)
        {
            System.Data.DataTable DtPermissions = new System.Data.DataTable();
            string SQL = "SELECT sys.objects.name AS [Object in database] " +
            ",max(case when PS.permission_name = 'SELECT' THEN 'S' ELSE '' END) AS [Select] " +
            ",max(case when PIn.permission_name = 'INSERT' THEN 'I' ELSE '' END) AS [Insert] " +
            ",max(case when PUp.permission_name = 'UPDATE' THEN 'U' ELSE '' END) AS [Update] " +
            ",max(case when PDe.permission_name = 'DELETE' THEN 'D' ELSE '' END) AS [Delete] " +
            ",max(case when PEx.permission_name = 'EXECUTE' THEN 'E' ELSE '' END) AS [Execute] " +
            ", sys.objects.type_desc AS [Type of object] " +
            "FROM sys.database_permissions  " +
            "INNER JOIN sys.database_principals ON sys.database_permissions.grantee_principal_id = sys.database_principals.principal_id  " +
            "INNER JOIN sys.objects ON sys.database_permissions.major_id = sys.objects.object_id " +
            "LEFT OUTER JOIN sys.database_permissions PS ON sys.database_permissions.major_id = sys.objects.object_id AND sys.database_permissions.permission_name = 'SELECT' " +
            "LEFT OUTER JOIN sys.database_permissions PIn ON sys.database_permissions.major_id = sys.objects.object_id AND sys.database_permissions.permission_name = 'INSERT' " +
            "LEFT OUTER JOIN sys.database_permissions PUp ON sys.database_permissions.major_id = sys.objects.object_id AND sys.database_permissions.permission_name = 'UPDATE' " +
            "LEFT OUTER JOIN sys.database_permissions PDe ON sys.database_permissions.major_id = sys.objects.object_id AND sys.database_permissions.permission_name = 'DELETE' " +
            "LEFT OUTER JOIN sys.database_permissions PEx ON sys.database_permissions.major_id = sys.objects.object_id AND sys.database_permissions.permission_name = 'EXECUTE' " +
            "WHERE        (sys.database_principals.name = '" + Role+ "') AND (sys.database_permissions.state_desc = 'GRANT') ";
            if (Objects.Count > 0)
            {
                SQL += "AND sys.objects.name IN (";
                foreach(string s in Objects)
                {
                    if (!SQL.EndsWith("(")) SQL += ", ";
                    SQL += "'" + s + "'";
                }
                SQL += ")";
            }
            SQL += "GROUP BY sys.objects.name, sys.objects.type_desc " +
                "ORDER BY [Type of object], [Object in database]";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref DtPermissions, ref Message);
            return DtPermissions;
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Role.Permission, string>>> _AllPermissions;

        public static void ResetAllPermissions()
        {
            _AllPermissions = null;
        }

        /// <summary>
        /// Return a dictionary containing all permissions of role included those inherited from other roles
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Role.Permission, string>> AllPermissions(string Role)
        {
            if (_AllPermissions == null)
                _AllPermissions = new Dictionary<string, Dictionary<string, Dictionary<Permission, string>>>();
            if (_AllPermissions.ContainsKey(Role))
                return _AllPermissions[Role];
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Securables())
                {
                    System.Collections.Generic.Dictionary<Role.Permission, string> P = new Dictionary<Permission, string>();
                    P.Add(Permission.SELECT, "");
                    P.Add(Permission.INSERT, "");
                    P.Add(Permission.UPDATE, "");
                    P.Add(Permission.DELETE, "");
                    P.Add(Permission.EXECUTE, "");
                    if (_AllPermissions.ContainsKey(Role))
                    {
                        _AllPermissions[Role].Add(KV.Key, P);
                    }
                    else
                    {
                        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Role.Permission, string>> OP = new Dictionary<string, Dictionary<Permission, string>>();
                        OP.Add(KV.Key, P);
                        _AllPermissions.Add(Role, OP);
                    }
                }
                string SQL = "SELECT O.name AS [Object] , " +
                    "P.permission_name AS [Permission], O.type_desc AS [Type]  " +
                    ",case when O.type = 'U' then 1 else case when O.type = 'V' then 2 else case when o.type = 'FN' then 3 else case when o.type = 'TF' then 4 else 5 end end end end as OrderSequence " +
                    "FROM sys.database_permissions P  " +
                    "INNER JOIN sys.database_principals R ON P.grantee_principal_id = R.principal_id   " +
                    "INNER JOIN sys.objects O ON P.major_id = O.object_id  " +
                    "WHERE (R.name = '" + Role + "')  " +
                    "AND (P.state_desc = 'GRANT') " +
                    "ORDER BY OrderSequence, [Object]";
                System.Data.DataTable dt = new System.Data.DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    switch (R["Permission"].ToString().ToUpper())
                    {
                        case "SELECT":
                            if (_AllPermissions[Role][R["Object"].ToString()][Permission.SELECT].Length == 0)
                                _AllPermissions[Role][R["Object"].ToString()][Permission.SELECT] = Role;
                            break;
                        case "INSERT":
                            if (_AllPermissions[Role][R["Object"].ToString()][Permission.INSERT].Length == 0)
                                _AllPermissions[Role][R["Object"].ToString()][Permission.INSERT] = Role;
                            break;
                        case "UPDATE":
                            if (_AllPermissions[Role][R["Object"].ToString()][Permission.UPDATE].Length == 0)
                                _AllPermissions[Role][R["Object"].ToString()][Permission.UPDATE] = Role;
                            break;
                        case "DELETE":
                            if (_AllPermissions[Role][R["Object"].ToString()][Permission.DELETE].Length == 0)
                                _AllPermissions[Role][R["Object"].ToString()][Permission.DELETE] = Role;
                            break;
                        case "EXECUTE":
                            if (_AllPermissions[Role][R["Object"].ToString()][Permission.EXECUTE].Length == 0)
                                _AllPermissions[Role][R["Object"].ToString()][Permission.EXECUTE] = Role;
                            break;
                    }
                }

                // inherited permissons
                foreach (string IR in InheritingFromRoles(Role))
                {
                    AllPermissions(IR);
                    AllPermissionsInheritedFrom(Role, IR);
                }
            }
            catch (System.Exception ex)
            {
            }
            return _AllPermissions[Role];
        }

        private static void AllPermissionsInheritedFrom(string Role, string InheritFromRole)
        {
            System.Collections.Generic.List<Permission> PP = new List<Permission>();
            PP.Add(Permission.SELECT);
            PP.Add(Permission.INSERT);
            PP.Add(Permission.UPDATE);
            PP.Add(Permission.DELETE);
            PP.Add(Permission.EXECUTE);
            if (_AllPermissions.ContainsKey(Role) &&
                _AllPermissions.ContainsKey(InheritFromRole))
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<Permission, string>> KVdict in _AllPermissions[Role])
                {
                    foreach (Permission P in PP)
                    {
                        if (_AllPermissions[Role][KVdict.Key][P].Length == 0 &&
                            _AllPermissions[InheritFromRole][KVdict.Key][P].Length > 0)
                            _AllPermissions[Role][KVdict.Key][P] = _AllPermissions[InheritFromRole][KVdict.Key][P];
                    }
                }
            }
        }

        private static System.Collections.Generic.Dictionary<string, string> _Securables;

        public static System.Collections.Generic.Dictionary<string, string> Securables()
        {
            if (_Securables == null)
            {
                _Securables = new Dictionary<string, string>();
                System.Data.DataTable dt = new System.Data.DataTable();
                string Message = "";
                string SQL = "select T.TABLE_NAME AS Name, T.TABLE_TYPE AS [Type], case when T.TABLE_TYPE = 'BASE TABLE' then 1 else 2 end AS OrderColumn " +
                    "from INFORMATION_SCHEMA.TABLES T " + 
                    "union " +
                    "select R.SPECIFIC_NAME AS Name, R.ROUTINE_TYPE AS [Type], case when R.ROUTINE_TYPE = 'FUNCTION' then 3 else 4 end AS OrderColumn " +
                    "from INFORMATION_SCHEMA.ROUTINES R " + 
                    "order by OrderColumn, Name";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    _Securables.Add(R[0].ToString(), R[1].ToString());
                }
            }
            return _Securables;
        }

    }
}
