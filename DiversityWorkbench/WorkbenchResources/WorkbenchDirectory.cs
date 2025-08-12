using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DiversityWorkbench.WorkbenchResources
{
    public class WorkbenchDirectory
    {

        private bool _CompareVersions = false;
        /// <summary>
        /// A directory containing subdirectories and files that should be transferred to the homedirectory of the user
        /// </summary>
        /// <param name="Name">Name of the directory</param>
        public WorkbenchDirectory(string Name, bool CompareVersions = false)
        {
            this._CompareVersions = CompareVersions;
        }

        /// <summary>
        /// Initializes directory with resources files according to list of folders
        /// </summary>
        /// <param name="FoldersForTransfer">List of folders in application directory that should be copied into workbench directory</param>
        //public static void InitWorkbenchDirectory(System.Collections.Generic.List<string> FoldersForTransfer)
        //{
        //    try
        //    {
        //        System.IO.DirectoryInfo Dapp = new System.IO.DirectoryInfo(....Windows.Forms.Application.StartupPath);
        //        System.IO.DirectoryInfo DWorkbench = new System.IO.DirectoryInfo(WorkbenchDirectoryModule());
        //        System.Collections.Generic.List<System.IO.DirectoryInfo> Missing = new List<System.IO.DirectoryInfo>();
        //        foreach (string F in FoldersForTransfer)
        //        {
        //            System.IO.DirectoryInfo[] DD = Dapp.GetDirectories(F, System.IO.SearchOption.TopDirectoryOnly);
        //            if (DD.Length == 1)
        //            {
        //                System.IO.DirectoryInfo[] WW = DWorkbench.GetDirectories(F, System.IO.SearchOption.TopDirectoryOnly);
        //                if (WW.Length == 0)
        //                {
        //                    DD[0].MoveTo(WorkbenchDirectoryModule() + "\\" + F);
        //                }
        //                else if (WW.Length == 1)
        //                {
        //                    if (FileOrDirectoryMissing(DD[0], WW[0]))
        //                    {
        //                        Missing.Add(DD[0]);
        //                    }
        //                }
        //            }
        //        }
        //        if (Missing.Count > 0)
        //        {
        //            DiversityWorkbench.WorkbenchResources.FormResources f = new FormResources(Missing);
        //            f.ShowDialog();
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        public static void CopyWorkbenchDirectory(System.Collections.Generic.List<string> FoldersForTransfer)
        {
            // Markus 9.8.24: Optional copy of files
            if (DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HowToCopyAppToUserDirectoryOption == CopyAppToUserDirectoryOption.None)
                return;

            try
            {
                System.IO.DirectoryInfo Dapp = new System.IO.DirectoryInfo(ApplicationDirectory());
                System.IO.DirectoryInfo DWorkbench = new System.IO.DirectoryInfo(WorkbenchDirectoryModule());
                //System.Collections.Generic.List<System.IO.DirectoryInfo> Missing = new List<System.IO.DirectoryInfo>();
                foreach (string F in FoldersForTransfer)
                {
                    System.IO.DirectoryInfo[] DD = Dapp.GetDirectories(F, System.IO.SearchOption.TopDirectoryOnly);
                    if (DD.Length > 0)
                    {
                        foreach (System.IO.DirectoryInfo D in DD)
                        {
                            System.IO.DirectoryInfo T = new System.IO.DirectoryInfo(DWorkbench.FullName + "\\" + D.Name);
                            CopyContent(D, T);
                        }
                    }
                    //else
                    //{
                    //    System.IO.DirectoryInfo S = new System.IO.DirectoryInfo(Dapp.FullName + "\\" + F);
                    //    System.IO.DirectoryInfo T = new System.IO.DirectoryInfo(DWorkbench.FullName + "\\" + F);
                    //    if (Dapp.Exists)
                    //        CopyContent(S, T);
                    //}
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void CopyContent(System.IO.DirectoryInfo Source, System.IO.DirectoryInfo Target)
        {
            try
            {
                System.IO.DirectoryInfo[] SS = Source.GetDirectories();
                if (!Target.Exists)
                    Target.Create();
                System.Security.AccessControl.DirectorySecurity dSecurity = new System.Security.AccessControl.DirectorySecurity();
                dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(System.Environment.UserName,
                                                            System.Security.AccessControl.FileSystemRights.FullControl,
                                                            System.Security.AccessControl.AccessControlType.Allow));
                System.IO.FileSystemAclExtensions.SetAccessControl(Target, dSecurity);
                System.IO.DirectoryInfo[] TT = Target.GetDirectories();

                // Copy subdirectories
                foreach (System.IO.DirectoryInfo S in SS)
                {
                    System.IO.DirectoryInfo T = new System.IO.DirectoryInfo(Target.FullName + "\\" + S.Name);
                    if (!T.Exists)
                    {
                        T.Create();
                        System.IO.FileSystemAclExtensions.SetAccessControl(T, dSecurity);
                        
                    }
                    CopyContent(S, T);
                }

                // Copy files
                System.IO.FileInfo[] ss = Source.GetFiles();
                System.IO.FileInfo[] tt = Target.GetFiles();
                foreach (System.IO.FileInfo s in ss)
                {
                    // Markus 9.8.24: Optional copy of files
                    System.IO.FileInfo t = new System.IO.FileInfo(Target.FullName + "\\" + s.Name);
                    if (DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HowToCopyAppToUserDirectoryOption == CopyAppToUserDirectoryOption.Copy)
                    {
                        s.CopyTo(t.FullName, true);
                    }
                    else if (DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HowToCopyAppToUserDirectoryOption == CopyAppToUserDirectoryOption.Missing)
                    {
                        if (!t.Exists)
                        {
                            s.CopyTo(t.FullName, true);
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static bool FileOrDirectoryMissing(System.IO.DirectoryInfo Source, System.IO.DirectoryInfo Target)
        {
            if (MissingSubDirectories(Source, Target).Count == 0 &&
                MissingFiles(Source, Target).Count == 0)
                return false;
            else
                return true;




            bool NothingMissing = true;

            // Check Directories
            System.IO.DirectoryInfo[] SS = Source.GetDirectories();
            System.IO.DirectoryInfo[] TT = Target.GetDirectories();
            foreach (System.IO.DirectoryInfo S in SS)
            {
                bool ContainsFolder = false;
                foreach(System.IO.DirectoryInfo T in TT)
                {
                    if (T.Name == S.Name)
                    {
                        ContainsFolder = true;
                        break;
                    }
                }
                if (!ContainsFolder)
                {
                    NothingMissing = false;
                    break;
                }
            }

            // Check Files
            System.IO.FileInfo[] ss = Source.GetFiles();
            System.IO.FileInfo[] tt = Target.GetFiles();
            foreach (System.IO.FileInfo s in ss)
            {
                bool ContainsFile = false;
                foreach (System.IO.FileInfo t in tt)
                {
                    if (t.Name == s.Name)
                    {
                        ContainsFile = true;
                        break;
                    }
                }
                if (!ContainsFile)
                {
                    NothingMissing = false;
                    break;
                }
            }

            return NothingMissing;
        }

        public static System.Collections.Generic.List<System.IO.DirectoryInfo> MissingSubDirectories(System.IO.DirectoryInfo Source, System.IO.DirectoryInfo Target, bool CheckSubdirectories = false)
        {
            System.Collections.Generic.List<System.IO.DirectoryInfo> DD = new List<System.IO.DirectoryInfo>();
            System.IO.DirectoryInfo[] SS = Source.GetDirectories();
            System.IO.DirectoryInfo[] TT = Target.GetDirectories();
            foreach (System.IO.DirectoryInfo S in SS)
            {
                System.Collections.Generic.List<System.IO.DirectoryInfo> tt = new List<System.IO.DirectoryInfo>(); 
                bool ContainsFolder = false;
                foreach (System.IO.DirectoryInfo T in TT)
                {
                    if (T.Name == S.Name)
                    {
                        ContainsFolder = true;
                        tt.Add(T);
                        break;
                    }
                }
                if (!ContainsFolder)
                {
                    DD.Add(S);
                    break;
                }
                else if (CheckSubdirectories)
                {
                    foreach (System.IO.DirectoryInfo T in tt)
                    {
                        System.Collections.Generic.List<System.IO.DirectoryInfo> ss = MissingSubDirectories(S, T);
                        foreach (System.IO.DirectoryInfo s in ss)
                            DD.Add(s);
                    }
                }
            }
            return DD;
        }

        public static System.Collections.Generic.List<System.IO.FileInfo> MissingFiles(System.IO.DirectoryInfo Source, System.IO.DirectoryInfo Target)
        {
            System.Collections.Generic.List<System.IO.FileInfo> DD = new List<System.IO.FileInfo>();
            System.IO.FileInfo[] SS = Source.GetFiles();
            System.IO.FileInfo[] TT = Target.GetFiles();
            foreach (System.IO.FileInfo S in SS)
            {
                bool ContainsFile = false;
                foreach (System.IO.FileInfo T in TT)
                {
                    if (T.Name == S.Name)
                    {
                        ContainsFile = true;
                        break;
                    }
                }
                if (!ContainsFile)
                {
                    DD.Add(S);
                    break;
                }
            }
            return DD;
        }

        public static string WorkbenchDirectoryModule()
        {
            System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryPath() + "\\" + DiversityWorkbench.WorkbenchSettings.Default.ModuleName);
            if (!Dir.Exists)
            {
                Dir.Create();
                System.Security.AccessControl.DirectorySecurity dSecurity = Dir.GetAccessControl();
                dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(System.Environment.UserName,
                                                            System.Security.AccessControl.FileSystemRights.FullControl,
                                                            System.Security.AccessControl.AccessControlType.Allow));
                Dir.SetAccessControl(dSecurity);
            }
            return Dir.FullName;
        }

        /// <summary>
        /// Folder within the WorkbenchDirectoryModule
        /// </summary>
        /// <param name="Folder">Name of the folder</param>
        /// <returns>The path of the folder</returns>
        public static string WorkbenchDirectoryModule(string Folder)
        {
            System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(WorkbenchDirectoryModule() + "\\" + Folder);
            if (!Dir.Exists)
            {
                Dir.Create();
                System.Security.AccessControl.DirectorySecurity dSecurity = Dir.GetAccessControl();
                dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(System.Environment.UserName,
                                                            System.Security.AccessControl.FileSystemRights.FullControl,
                                                            System.Security.AccessControl.AccessControlType.Allow));
                Dir.SetAccessControl(dSecurity);
            }
            return Dir.FullName;
        }

        public enum WorkbenchDirectoryType { Home, MyDocuments, UserDefined }// Application }

        // is kept in Settings
        //public static string ResourcesDirectory
        //{
        //    get
        //    {
        //        return DiversityWorkbench.WorkbenchSettings.Default.ResourcesDirectory;
        //    }
        //    set
        //    {
        //        DiversityWorkbench.WorkbenchSettings.Default.ResourcesDirectory = value;
        //        DiversityWorkbench.WorkbenchSettings.Default.Save();
        //    }
        //}

        public static WorkbenchDirectoryType CurrentDirectoryType()
        {
            switch (DiversityWorkbench.Settings.ResourcesDirectory.ToLower())
            {
                case "home":
                    return WorkbenchDirectoryType.Home;
                case "mydocuments":
                    return WorkbenchDirectoryType.MyDocuments;
                default:
                    return WorkbenchDirectoryType.UserDefined;
            }
            //return WorkbenchDirectoryType.Home;
        }

        public static string WorkbenchDirectoryPath()
        {
            string FolderPath = ApplicationDirectory(); 

            // The home directory for the user
            if (DiversityWorkbench.Settings.ResourcesDirectory.ToUpper() == DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType.Home.ToString().ToUpper())
            {
                FolderPath = HomeDirectory();
            }

            // the myDocuments directory
            else if (DiversityWorkbench.Settings.ResourcesDirectory == DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments.ToString())
            {
                System.IO.DirectoryInfo MyDocumentsDirectory = new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                if (MyDocumentsDirectory.Exists)
                    FolderPath = MyDocumentsDirectory.FullName + "\\DiversityWorkbench";
            }

            // a path entered by the user
            else
            {
                System.IO.DirectoryInfo Directory = new System.IO.DirectoryInfo(DiversityWorkbench.Settings.ResourcesDirectory);
                if (Directory.Exists)
                {
                    FolderPath = Directory.FullName;
                    if (!FolderPath.EndsWith("\\DiversityWorkbench"))
                        FolderPath += "\\DiversityWorkbench";
                }
            }

            System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(FolderPath);
            if (!Dir.Exists)
            {
                Dir.Create();
                System.Security.AccessControl.DirectorySecurity dSecurity = Dir.GetAccessControl();
                dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(System.Environment.UserName,
                                                            System.Security.AccessControl.FileSystemRights.FullControl,
                                                            System.Security.AccessControl.AccessControlType.Allow));
                Dir.SetAccessControl(dSecurity);
            }
            return FolderPath;
        }

        public static string ApplicationDirectory()
        {
            return System.Windows.Forms.Application.StartupPath;
            //System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(....Windows.Forms.Application.StartupPath);
            //string FolderPath = DI.Parent.FullName;// +"\\DiversityWorkbench";
            //return FolderPath;
        }

        private static string HomeDirectory()
        {
            string FolderPath = "";
            try
            {
                var homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
                if (homeDrive != null)
                {
                    var homePath = Environment.GetEnvironmentVariable("HOMEPATH");
                    if (homePath != null)
                    {
                        var fullHomePath = homeDrive + System.IO.Path.DirectorySeparatorChar + homePath;
                        FolderPath = System.IO.Path.Combine(fullHomePath, "DiversityWorkbench");
                    }
                    else
                    {
                        throw new Exception("Environment variable error, there is no 'HOMEPATH'");
                    }
                }
                else
                {
                    throw new Exception("Environment variable error, there is no 'HOMEDRIVE'");
                }
            }
            catch (System.Exception ex)
            {
            }
            if (FolderPath.Length > 0)
            {
                System.IO.DirectoryInfo HomeDirectory = new System.IO.DirectoryInfo(FolderPath);
                if (HomeDirectory.Exists)
                    FolderPath = HomeDirectory.FullName;
            }
            return FolderPath;
        }

        public static bool setWorkbenchDirectory()
        {
            DiversityWorkbench.WorkbenchResources.FormResourcesDirectory f = new FormResourcesDirectory();
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                switch (f.Type())
                {
                    case WorkbenchDirectoryType.MyDocuments:
                    case WorkbenchDirectoryType.Home:
                        DiversityWorkbench.Settings.ResourcesDirectory = f.Type().ToString();
                        break;
                    default:
                        DiversityWorkbench.Settings.ResourcesDirectory = f.UserDefinedDirectory();
                        break;
                }
                DiversityWorkbench.Settings.SaveWorkbenchSettings();
                return true;
            }
            else
                return false;




            //System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
            //D.Add(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType.Application.ToString(), "Same directory as APPLICATION directory");
            //D.Add(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType.Home.ToString(), "HOME directory of the user");
            //D.Add(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments.ToString(), "MY DOCUMENTS directory of the user");
            //if (DiversityWorkbench.Settings.ResourcesDirectory.Length > 0 && !D.ContainsKey(DiversityWorkbench.Settings.ResourcesDirectory))
            //    D.Add(DiversityWorkbench.Settings.ResourcesDirectory, DiversityWorkbench.Settings.ResourcesDirectory);
            //System.IO.DirectoryInfo MyDocuments = new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            //string MyDocumentsPath = MyDocuments.FullName + "\\DiversityWorkbench"; // MyDocumentsDirectory();// MyDocuments.FullName;
            //string Explanation = "Please select or enter the place for the resources for the application\r\n\r\n" +
            //    "APPLICATION directory: " + ApplicationDirectory() + "\r\n" +
            //    "HOME directory: " + HomeDirectory() + "\r\n" +
            //    "MY DOCUMENTS: " + MyDocumentsPath;
            //if (DiversityWorkbench.Settings.ResourcesDirectory != null && DiversityWorkbench.Settings.ResourcesDirectory.Length > 0)
            //    Explanation += "\r\n\r\nCurrent directory: " + DiversityWorkbench.Settings.ResourcesDirectory;

            //DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(D, "Resources directory", Explanation, false);
            //f.PresetSelection(DiversityWorkbench.Settings.ResourcesDirectory);
            //f.Width = 600;
            //f.ShowDialog();
            //if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            //{
            //    string Dir = "";
            //    if (f.SelectedValue != null && f.SelectedValue != "NULL")
            //        DiversityWorkbench.Settings.ResourcesDirectory = f.SelectedValue;
            //    else if (f.SelectedString != null)
            //    {
            //        Dir = f.SelectedString;
            //        System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Dir);
            //        if (DI.Exists)
            //            DiversityWorkbench.Settings.ResourcesDirectory = Dir;
            //        else
            //            System.Windows.Forms.MessageBox.Show(Dir + " is not a valid directory");
            //    }
            //}
        }

        public static string HelpProviderNameSpace()
        {
            string NameSpace = ApplicationDirectory() + "\\" + System.Windows.Forms.Application.ProductName.ToString() + ".chm";
            return NameSpace;
        }

        public enum FolderType { Archive, Backup, Documentation, Export, Import, Query, Settings, Spreadsheet, Tools, Updates, Checklist }        
        public static string Folder(FolderType Type)
        {
            string Dir = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\";
            switch(Type)
            {
                default:
                    Dir += Type.ToString() + "\\";
                    break;
            }

            System.IO.DirectoryInfo Directory = new System.IO.DirectoryInfo(Dir);
            if (!Directory.Exists)
            {
                Directory.Create();
                System.Security.AccessControl.DirectorySecurity dSecurity = Directory.GetAccessControl();
                dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(System.Environment.UserName,
                                                            System.Security.AccessControl.FileSystemRights.FullControl,
                                                            System.Security.AccessControl.AccessControlType.Allow));
                Directory.SetAccessControl(dSecurity);
                switch (Type)
                {
                    case FolderType.Query:
                    case FolderType.Settings:
                    case FolderType.Spreadsheet:
                        Directory.Attributes = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Hidden;
                        break;
                }
            }

            return Dir;
        }

        public static string ErrorLogFile(string Module) { return DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\" + Module + "Error.log"; }

        public static string ErrorLogFile()
        {
            //return System.Windows.Forms.Application.StartupPath + "\\" + DiversityWorkbench.Settings.ModuleName + "Error.log";
            return DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\" + DiversityWorkbench.Settings.ModuleName + "Error.log";
        }


        public static string HowToCopyAppToUserDirectory
        {
            get
            {
                return DiversityWorkbench.WorkbenchSettings.Default.HowToCopyAppToUserDirectory;
            }
            set
            {
                DiversityWorkbench.WorkbenchSettings.Default.HowToCopyAppToUserDirectory = value;
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
        }

        public static System.Collections.Generic.Dictionary<string, string> HowToCopyAppToUserDirectoryOptions
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> strings = new Dictionary<string, string>();
                strings.Add("Copy files at program start", "Copy");
                strings.Add("Add missing files at program start", "Missing");
                strings.Add("Do not copy", "None");
                return strings;
            }
        }

        public static CopyAppToUserDirectoryOption HowToCopyAppToUserDirectoryOption
        {
            get
            {
                CopyAppToUserDirectoryOption option = CopyAppToUserDirectoryOption.Copy;
                switch (DiversityWorkbench.WorkbenchSettings.Default.HowToCopyAppToUserDirectory)
                {
                    case "Missing":
                        option = CopyAppToUserDirectoryOption.Missing;
                        break;
                    case "None":
                        option = CopyAppToUserDirectoryOption.None;
                        break;
                }
                return option;
            }
        }

        public enum CopyAppToUserDirectoryOption { Copy, Missing, None}


    }
}
