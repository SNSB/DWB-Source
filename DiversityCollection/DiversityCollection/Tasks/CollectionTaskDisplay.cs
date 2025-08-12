using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    class CollectionTaskDisplay
    {
        #region static
        public static string NodeText(System.Data.DataRow R, bool IncludeIDinTreeview = false)
        {
            string NodeText = "";
            try
            {
                string CollectionName = "";
                string Task = "";
                string Type = "";
                DiversityCollection.Task.TaskModuleType TaskModuleType = DiversityCollection.Task.TaskModuleType.None;
                DiversityCollection.Task.TaskDateType TaskDateType = DiversityCollection.Task.TaskDateType.None;
                string DisplayText = "";
                string Result = "";
                string ID = "";
                string Spacer = " ";
                string Start = "";
                string End = "";
                string SQL = "";
                string Parent = "";
                string NumberValue = "";

                // DisplayText
                DisplayText = R["DisplayText"].ToString();

                // Task
                if (!R["TaskID"].Equals(System.DBNull.Value))
                {
                    string SqlTask = "SELECT DisplayText FROM Task WHERE TaskID = " + R["TaskID"].ToString();
                    Task = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTask);
                    SqlTask = "SELECT [Type] FROM Task WHERE TaskID = " + R["TaskID"].ToString();
                    Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTask);
                    TaskModuleType = DiversityCollection.Task.TypeOfTaskModule(Type);
                    string SqlParent = "SELECT P.DisplayText From Task P INNER JOIN Task T ON T.TaskParentID = P.TaskID AND NOT T.TaskParentID IS NULL AND T.TaskID = " + R["TaskID"].ToString();
                    string Message = "";
                    Parent = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlParent, ref Message);
                }

                // ID
                if (IncludeIDinTreeview)
                    ID = "[" + R["CollectionTaskID"].ToString() + "]   ";

                // Separator
                SQL = "select dbo.TaskCollectionHierarchySeparator()";
                string TaskCollectionHierarchySeparator = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                // Collection
                if (R["CollectionTaskParentID"].Equals(System.DBNull.Value))
                {
                    if (!R["CollectionID"].Equals(System.DBNull.Value))
                    {
                        SQL = "SELECT CollectionName FROM Collection WHERE CollectionID = " + R["CollectionID"].ToString();
                        CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) + TaskCollectionHierarchySeparator;
                    }
                }
                else
                {
                    SQL = "SELECT CASE WHEN T.CollectionID = P.CollectionID THEN '' ELSE C.CollectionName END AS CollectionName " +
                        "FROM CollectionTask AS T INNER JOIN " +
                        "Collection AS C ON T.CollectionID = C.CollectionID INNER JOIN " +
                        "CollectionTask AS P ON T.CollectionTaskParentID = P.CollectionTaskID " +
                        "WHERE T.CollectionTaskID = " + R["CollectionTaskID"].ToString();
                    CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (CollectionName.Length > 0)
                        CollectionName += TaskCollectionHierarchySeparator;
                }

                // Start
                string DateType = DiversityCollection.LookupTable.TaskDateType(int.Parse(R["TaskID"].ToString()));
                if (DateType.Length > 0)
                {
                    TaskDateType = DiversityCollection.Task.TypeOfTaskDate(DateType);
                }

                if (!R["TaskStart"].Equals(System.DBNull.Value) && R["TaskStart"].ToString().Length > 0)
                {
                    System.DateTime DT;
                    if (System.DateTime.TryParse(R["TaskStart"].ToString(), out DT))
                    {
                        switch (TaskDateType)
                        {
                            case DiversityCollection.Task.TaskDateType.DateFromTo:
                            case DiversityCollection.Task.TaskDateType.Date:
                                Start = DT.ToString("yyyy-MM-dd");
                                break;
                            case DiversityCollection.Task.TaskDateType.TimeFromTo:
                            case DiversityCollection.Task.TaskDateType.Time:
                                Start = DT.ToString("HH:mm");
                                break;
                            default:
                                Start = DT.ToString("yyyy-MM-dd HH:mm");
                                break;
                        }
                    }
                }

                // End
                if (!R["TaskEnd"].Equals(System.DBNull.Value) && R["TaskEnd"].ToString().Length > 0)
                {
                    System.DateTime DT;
                    if (System.DateTime.TryParse(R["TaskEnd"].ToString(), out DT))
                    {
                        switch (TaskDateType)
                        {
                            case DiversityCollection.Task.TaskDateType.DateFromTo:
                            case DiversityCollection.Task.TaskDateType.Date:
                                End = DT.ToString("yyyy-MM-dd");
                                break;
                            case DiversityCollection.Task.TaskDateType.TimeFromTo:
                            case DiversityCollection.Task.TaskDateType.Time:
                                End = DT.ToString("HH:mm");
                                break;
                            default:
                                End = DT.ToString("yyyy-MM-dd HH:mm");
                                break;
                        }
                    }
                }


                // Result
                if (!R["Result"].Equals(System.DBNull.Value) && R["Result"].ToString().Length > 0)
                {
                    Result = R["Result"].ToString();
                    if (Result.Length > 50)
                        Result = Result.Substring(0, 50) + "...";
                }

                // NumberValue
                if (!R["NumberValue"].Equals(System.DBNull.Value) && R["NumberValue"].ToString().Length > 0)
                {
                    NumberValue = R["NumberValue"].ToString();
                }


                NodeText = ID + CollectionName; // + Task;
                if (DisplayText != Task)
                {
                    if (DisplayText.Length > 0)
                        NodeText += DisplayText + " (" + Task + ")";
                    else if (Parent.Length > 0)
                        NodeText += Task + " (" + Parent + ")";
                    else
                        NodeText += Task;
                }
                else
                {
                    NodeText += DisplayText;
                }

                if (Start.Length > 0)
                {
                    NodeText += " " + Start;
                    if (End.Length > 0)
                        NodeText += " - " + End;
                }
                else if (End.Length > 0)
                    NodeText += " " + End;

                if (Result.Length > 0)
                    NodeText += ": " + Result;

                if (NumberValue.Length > 0)
                {
                    int i;
                    if (int.TryParse(NumberValue, out i))
                        NodeText = NumberValue + "x " + NodeText;
                }

                // Spacer
                if (R["CollectionTaskParentID"].Equals(System.DBNull.Value))
                {
                    for (int i = 0; i < (int)NodeText.Length / 2; i++)
                        Spacer += " ";
                    NodeText += Spacer;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return NodeText;
        }

        #endregion
    }
}
