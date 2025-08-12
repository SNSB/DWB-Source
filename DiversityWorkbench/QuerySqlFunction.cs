using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    /// <summary>
    /// A class for the retrieval of data via a sql function with parameters
    /// </summary>
    public class QuerySqlFunction
    {

        #region Parameter

        private string _SqlFunction;
        private System.Collections.Generic.List<string> _Parameters;
        private string _QueryColumn;
        
        #endregion

        #region Construction
        
        public QuerySqlFunction()
        { }
        
        #endregion

        public void SetSqlFunctionName(string SqlFunction)
        {
            this._SqlFunction = SqlFunction;
        }

        public void SetSqlParameters(System.Collections.Generic.List<string> Parameters)
        {
            this._Parameters = Parameters;
        }

        public void setQueryColumn(string QueryColumn)
        {
            this._QueryColumn = QueryColumn;
        }

        /// <summary>
        /// Where clause for query
        /// </summary>
        /// <param name="SqlFunction">Name of the SQL function</param>
        /// <param name="QueryColumn">The column extracted from the result of the function</param>
        /// <param name="QueryOperator">The query operator</param>
        /// <param name="Parameters">The list of parameters</param>
        /// <returns></returns>
        public string WhereClause(string SqlFunction, string QueryColumn, string QueryOperator, System.Collections.Generic.List<string> Parameters)
        {
            string SQL = "";
            try
            {
                this.SetSqlFunctionName(SqlFunction);
                this.SetSqlParameters(Parameters);
                this.setQueryColumn(QueryColumn);

                if (SqlFunction.Length == 0 || QueryColumn.Length == 0 || Parameters.Count == 0)
                    return "";

                foreach (string s in this._Parameters)
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += "s";
                }
                SQL = "SELECT F." + this._QueryColumn +
                " FROM dbo." + this._SqlFunction + "(" + SQL + ")";
            }
            catch { }
            return SQL;
        }
    }
}
