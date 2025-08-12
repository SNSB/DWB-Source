using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.WorkbenchResources
{
    public partial class FormResources : Form
    {

        System.IO.DirectoryInfo _SourceDirectory;
        System.IO.DirectoryInfo _TargetDirectory;
        System.Windows.Forms.TreeNode _SourceNode;
        System.Windows.Forms.TreeNode _TargetNode;

        public FormResources()
        {
            InitializeComponent();
            this.initForm();
        }

        public FormResources(System.Collections.Generic.List<System.IO.DirectoryInfo> Missing)
        {
            InitializeComponent();
            this.initForm();
            this.DisableDirectoryChange();
            this._SourceDirectory = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath);
            this._TargetDirectory = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule());
            this._TargetNode = this.initDirectoryTree(this._TargetDirectory, this.treeViewCurrent, Missing);
            this._SourceNode = this.initDirectoryTree(this._SourceDirectory, this.treeViewNew, Missing);
        }

        private void initForm()
        {
            switch(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.CurrentDirectoryType())
            {
                case WorkbenchDirectory.WorkbenchDirectoryType.Home:
                    this.radioButtonHome.Checked = true;
                    break;
                //case WorkbenchDirectory.WorkbenchDirectoryType.Application:
                //    this.radioButtonApplication.Checked = true;
                //    break;
                case WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments:
                    this.radioButtonMyDocuments.Checked = true;
                    break;
            }
        }

        private void DisableDirectoryChange()
        {
            this.radioButtonMyDocuments.Enabled = false;
            this.radioButtonHome.Enabled = false;
            this.radioButtonApplication.Enabled = false;
        }

        private void MarkMissingContent()
        {
            foreach(System.Windows.Forms.TreeNode N in this._SourceNode.Nodes)
            {

            }
        }

        private System.Windows.Forms.TreeNode initDirectoryTree(System.IO.DirectoryInfo Directory, System.Windows.Forms.TreeView Tree, System.Collections.Generic.List<System.IO.DirectoryInfo> Missing)
        {
            System.Windows.Forms.TreeNode N = new TreeNode(Directory.Name);
            N.ImageIndex = 0;
            System.Windows.Forms.TreeNode P = initParentDirectory(Directory, Tree);
            if (P != null)
            {
                P.Nodes.Add(N);
                N.ImageIndex = 1;
                N.Expand();
            }
            else
            {
                Tree.Nodes.Add(N);
            }
            this.addDirectoryContent(Directory, N, false, Missing);
            if (Missing !=null)
            {
                foreach(System.Windows.Forms.TreeNode n in N.Nodes)
                {
                    foreach(System.IO.DirectoryInfo D in Missing)
                    {
                        if (D.Name == n.Text)
                        {
                            n.BackColor = System.Drawing.Color.Yellow;
                            n.ExpandAll();
                            break;
                        }
                    }
                }
            }
            return N;
        }

        private System.Windows.Forms.TreeNode initParentDirectory(System.IO.DirectoryInfo Directory, System.Windows.Forms.TreeView Tree)
        {
            System.Windows.Forms.TreeNode N = null;
            string[] Path = Directory.Parent.FullName.Split(new char[] { '\\' });
            foreach (string P in Path)
            {
                System.Windows.Forms.TreeNode n = new TreeNode(P);
                n.ImageIndex = 1;
                n.Expand();
                if (N !=null)
                    N.Nodes.Add(n);
                else
                    Tree.Nodes.Add(n);
                N = n;
                N.ImageIndex = 1;
                N.Expand();
            }
            return N;
        }

        private void addDirectoryContent(System.IO.DirectoryInfo Directory, System.Windows.Forms.TreeNode Node, bool IsHidden = false, System.Collections.Generic.List<System.IO.DirectoryInfo> Missing = null, bool AddFiles = false)
        {
            if (AddFiles)
            {
                System.IO.FileInfo[] FF = Directory.GetFiles();
                foreach (System.IO.FileInfo F in FF)
                {
                    System.Windows.Forms.TreeNode Nfile = new TreeNode(F.Name);
                    Node.Nodes.Add(Nfile);
                    if (IsHidden)
                    {
                        Nfile.ForeColor = System.Drawing.Color.Gray;
                        Nfile.ImageIndex = 0;
                    }
                    else
                    { }
                }

            }

            System.IO.DirectoryInfo[] DD = Directory.GetDirectories();
            foreach(System.IO.DirectoryInfo D in DD)
            {
                if (Missing != null)
                {
                    bool FolderMissing = false;
                    foreach(System.IO.DirectoryInfo M in Missing)
                    {
                        if (M.Name == D.Name)
                            FolderMissing = true;
                    }
                    if (!FolderMissing)
                        continue;
                }

                System.Windows.Forms.TreeNode N = new TreeNode(D.Name);
                Node.Nodes.Add(N);
                bool Hidden = IsHidden;
                if (!Hidden && (D.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                    Hidden = true;
                if (Hidden)
                {
                    N.ForeColor = System.Drawing.Color.Gray;
                    N.ImageIndex = 2;
                }
                else
                    N.ImageIndex = 1;
                this.addDirectoryContent(D, N, Hidden, Missing, true);
            }

        }

    }
}
