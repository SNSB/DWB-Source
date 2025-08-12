using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection
{
    /// <summary>
    /// Provides the paths for the directories
    /// </summary>
    public static class Folder
    {

        #region Transaction

        public enum TransactionFolder { Schemas, Statistics }
        public static string Transaction()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\";
            EnsureFolderExists(Path);
            return Path;
        }

        public static string Transaction(TransactionFolder F)
        {
            string Path = Transaction();
            switch (F)
            {
                case TransactionFolder.Statistics:
                    Path += "Schemas\\" + F.ToString() + "\\";
                    break;
                default:
                    Path += F.ToString() + "\\";
                    break;
            }
            EnsureFolderExists(Path);
            return Path;
        }
        
        #endregion

        #region CacheDB

        public enum CacheDBFolder { Reports, Export, Diagnostics }
        public static string CacheDB()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\CacheDB\\";
            EnsureFolderExists(Path);
            return Path;
        }

        public static string CacheDB(CacheDBFolder F)
        {
            string Path = CacheDB();
            switch (F)
            {
                default:
                    Path += F.ToString() + "\\";
                    break;
            }
            EnsureFolderExists(Path);
            return Path;
        }
        
        #endregion

        #region Statistics

        public static string Statistics()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Statistics\\";
            EnsureFolderExists(Path);
            return Path;
        }
        
        #endregion

        #region Export

        public static string Export()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Export\\";
            EnsureFolderExists(Path);
            return Path;
        }
        
        #endregion

        #region Import
        
        public static string Import()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Import\\";
            EnsureFolderExists(Path);
            return Path;
        }

        #endregion

        #region Maps

        public static string Maps()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Maps\\";
            EnsureFolderExists(Path);
            return Path;
        }

        #endregion

        #region Report

        public enum ReportFolder { Task, TaskSchema, TaskImg, TaskSchemaCollection }

        public static string Report()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Report\\";
            EnsureFolderExists(Path);
            return Path;
        }

        public static string Report(ReportFolder F)
        {
            string Path = Report();
            switch (F)
            {
                case ReportFolder.TaskSchema:
                    Path += "Task\\Schema\\";
                    break;
                case ReportFolder.TaskImg:
                    Path += "Task\\img\\";
                    break;
                case ReportFolder.TaskSchemaCollection:
                    Path += "Task\\SchemaCollection\\";
                    break;
                default:
                    Path += F.ToString() + "\\";
                    break;
            }
            EnsureFolderExists(Path);
            return Path;
        }


        #endregion

        #region LabelPrinting

        public enum LabelPrintingFolder { Schemas, img, Collection, CollectionSchemas, CollectionImg }
        
        public static string LabelPrinting()
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\LabelPrinting\\";
            EnsureFolderExists(Path);
            return Path;
        }

        public static string LabelPrinting(LabelPrintingFolder F)
        {
            string Path = LabelPrinting();
            switch (F)
            {
                case LabelPrintingFolder.CollectionImg:
                    Path += "Collection\\img\\";
                    break;
                case LabelPrintingFolder.CollectionSchemas:
                    Path += "Collection\\Schemas\\";
                    break;
                default:
                    Path += F.ToString() + "\\";
                    break;
            }
            EnsureFolderExists(Path);
            return Path;
        }

        #endregion

        #region Common
        
        private static void EnsureFolderExists(string Path)
        {
            try
            {
                if (!System.IO.File.Exists(Path))
                {
                    System.IO.Directory.CreateDirectory(Path);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion
    }
}
