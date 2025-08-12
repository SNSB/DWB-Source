using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.CacheDB.ReplaceDB
{
    public partial class UserControlReplacement : UserControl
    {

        private ReplacedPart _ReplacedPart;
        private Replacement _Replacement;

        public UserControlReplacement()
        {
            InitializeComponent();
        }

        public UserControlReplacement(ReplacedPart ReplacedPart, ref Replacement Replacement)
        {
            InitializeComponent();
            this._ReplacedPart = ReplacedPart;
            this._Replacement = Replacement;
            this.initControl();
        }

        private void initControl()
        {
            try
            {
                //this.groupBoxPackages.Visible = false;
                this.splitContainerContent.Panel2Collapsed = true;
                this.groupBox.Text = this._ReplacedPart.Name;
                if (_ReplacedPart.Counts().Count > 0)
                {
                    this.InitCount(this.panelContent);
                }
                if (this._ReplacedPart.ContainedParts != null && this._ReplacedPart.ContainedParts.Count > 0)
                {
                    //Replacement.Status status = Replacement.Status._None;
                    foreach (System.Collections.Generic.KeyValuePair<string, ReplacedPart> co in this._ReplacedPart.ContainedParts)
                    {
                        //if (this._Replacement.RestrictToCritical && this._Replacement.CanReplaceDB(co.Value.Status()))
                        //    continue;
                        DiversityWorkbench.CacheDB.ReplaceDB.UserControlReplacement U = new UserControlReplacement(co.Value, ref this._Replacement);
                        this.panelPackages.Controls.Add(U);
                        U.Dock = DockStyle.Top;
                        U.BringToFront();
                        //status = this._Replacement.StatusWorst(status, co.Value.Status());
                    }
                    //if (status != Replacement.Status._None)
                    //{
                    //    this.buttonInfoPackages.Image = this._Replacement.ImageStatus(status);
                    //    this.toolTip.SetToolTip(this.buttonInfoPackages, this._Replacement.StatusDescription(status));
                    //    status = this._ReplacedPart.Status();
                    //}
                    //else 
                    if (this._Replacement.RestrictToCritical)
                    {

                        if (this._ReplacedPart.ContainedParts.ContainsKey(this._Replacement.Sources()[this._ReplacedPart.Type].MainTable))
                        {
                            ReplacedPart RP = this._ReplacedPart.ContainedParts[this._Replacement.Sources()[this._ReplacedPart.Type].MainTable];
                            DiversityWorkbench.CacheDB.ReplaceDB.UserControlReplacement U = new UserControlReplacement(RP, ref this._Replacement);
                            this.panelPackages.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.BringToFront();
                            //status = this._Replacement.StatusWorst(status, RP.Status());
                        }
                    }
                    this.groupBoxPackages.Visible = true;
                    this.splitContainerContent.Panel2Collapsed = false;
                }
                //this.SetStatus();
                //this.buttonInfo.Click += Info_Click;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void InitCount(System.Windows.Forms.Panel Panel)
        {
            try
            {
                int iCount = 0;
                Panel.Controls.Clear();
                Panel.Visible = true;
                //Replacement.Status statusCount = Replacement.Status._None;
                System.Collections.Generic.Stack<ReplaceCount> replaceCounts = new Stack<ReplaceCount>();
                foreach (System.Collections.Generic.KeyValuePair<string, ReplaceCount> CC in _ReplacedPart.Counts())
                {
                    if (this._Replacement.RestrictToCritical && this._Replacement.CanReplaceDB(CC.Value.Status))
                        continue;
                    replaceCounts.Push(CC.Value);
                    iCount++;
                }
                if (this._Replacement.RestrictToCritical && replaceCounts.Count == 0)
                {
                    if (this._Replacement.Sources().ContainsKey(this._ReplacedPart.Type)
                        && this._ReplacedPart.Counts().ContainsKey(this._Replacement.Sources()[this._ReplacedPart.Type].MainTable))
                    {
                        ReplaceCount RP = this._ReplacedPart.Counts()[this._Replacement.Sources()[this._ReplacedPart.Type].MainTable];
                        replaceCounts.Push(RP);
                        iCount++;
                    }
                    else if (this._ReplacedPart.Type == Replacement.Type.Project_Package)
                    {
                        if (this._Replacement.Sources().ContainsKey(Replacement.Type.Project))
                        {
                            if (this._Replacement.Sources()[Replacement.Type.Project].ContainedParts.Count > 0)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, ReplacedPart> KV in this._Replacement.Sources()[Replacement.Type.Project].ContainedParts)
                                {
                                    if (this._ReplacedPart.Counts().ContainsKey(KV.Value.MainTable))
                                    {
                                        ReplaceCount RP = this._ReplacedPart.Counts()[KV.Value.MainTable];
                                        replaceCounts.Push(RP);
                                        iCount++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                while (replaceCounts.Count > 0)
                {
                    bool IsHeader = false;
                    ReplaceCount RC = replaceCounts.Pop();
                    if (replaceCounts.Count == 0)
                        IsHeader = true;
                    this.AddCount(RC, Panel, IsHeader);
                }
                //foreach (System.Collections.Generic.KeyValuePair<string, ReplaceCount> CC in _ReplacedPart.Counts())
                //{
                //    if ((int)CC.Value.Status > 1)
                //    {
                //        this.AddCount(CC.Value, Panel);
                //        iCount++;
                //    }
                //    statusCount = this._Replacement.StatusWorst(statusCount, CC.Value.Status);
                //}
                if (iCount == 0)
                    groupBox.Visible = false;
                else
                {
                    groupBox.Visible = true;
                    if (this._ReplacedPart.Type != Replacement.Type.Project)
                    {
                        int H = 60 + (18 * (iCount));// - 1));
                        if (H > 170)
                            H = 170;
                        else
                        {

                        }
                        this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(H);
                        //if (iCount > 1 && this.Height < 80)
                        //    this.Height = 36 + (18 * (iCount - 1));
                    }
                }
                //if ((int)statusCount > (int)Replacement.Status.DataCountDifferent)
                //    this.groupBoxContent.BackColor = System.Drawing.Color.Pink;
                //else
                //    this.groupBoxContent.BackColor = System.Drawing.SystemColors.Control;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void AddCount(ReplaceCount CC, System.Windows.Forms.Panel Panel, bool IsHeader)
        {
            //bool IsHeader = false;
            //if (Panel.Controls.Count == 0)
            //    IsHeader = true;
            UserControlCount UCC = new UserControlCount(CC, this._Replacement, IsHeader);
            UCC.Dock = DockStyle.Top;
            //UCC.SendToBack();
            //if (IsHeader)
            //    UCC.BringToFront();
            //UCC.SendToBack();
            //else
            //    UCC.BringToFront();
            Panel.Controls.Add(UCC);
            //// Panel
            //System.Windows.Forms.Panel P = new Panel();
            //P.Height = 18;
            //P.Dock = DockStyle.Top;
            //P.BringToFront();
            //Panel.Controls.Add(P);
            //// Label
            //System.Windows.Forms.Label L = new Label();
            //L.Text = CC.SourceName;
            //L.Dock = DockStyle.Fill;
            //L.TextAlign = ContentAlignment.MiddleRight;
            //L.BringToFront();
            //P.Controls.Add(L);
            //// Picture
            //System.Windows.Forms.PictureBox Pi = new PictureBox();
            //Pi.Image = this._Replacement.ImageStatus(CC.Status);
            //Pi.Height = 16;
            //Pi.Width = 16;
            //Pi.Dock = DockStyle.Right;
            //Pi.Click += Info_Click;
            //Pi.Tag = CC;
            //Pi.BringToFront();
            //P.Controls.Add(Pi);
        }

        //private void Info_Click(object sender, EventArgs e)
        //{
        //    string Message = "Old\t->\tNew\tName of source\r\n";
        //    if (sender.GetType() == typeof(System.Windows.Forms.PictureBox))
        //    {
        //        System.Windows.Forms.PictureBox P = (System.Windows.Forms.PictureBox)sender;
        //        if (P.Tag != null && P.Tag.GetType() == typeof(ReplaceCount))
        //        {
        //            ReplaceCount RC = (ReplaceCount)P.Tag;

        //            string CountOld = RC.CountOld.ToString();
        //            if (CountOld.Length == 0) CountOld = "-";

        //            string CountNew = RC.CountNew.ToString();
        //            if (CountNew.Length == 0) CountNew = "-";

        //            Message += "\r\n" + CountOld + "\t->\t" + CountNew + "\t" + RC.SourceName;

        //            Message += "\r\n\r\n" + this._Replacement.StatusDescription(RC.Status);
        //        }
        //    }
        //    else
        //    {
        //        foreach (System.Collections.Generic.KeyValuePair<string, ReplaceCount> CC in this._ReplacedPart.Counts())
        //        {
        //            string CountOld = CC.Value.CountOld.ToString();
        //            if (CountOld.Length == 0) CountOld = "-";

        //            string CountNew = CC.Value.CountNew.ToString();
        //            if (CountNew.Length == 0) CountNew = "-";

        //            Message += "\r\n" + CountOld + "\t->\t" + CountNew + "\t" + CC.Value.SourceName;
        //        }
        //    }
        //    System.Windows.Forms.MessageBox.Show(Message, "Content of " + this._ReplacedPart.Name);
        //}

        //private void SetStatus()
        //{
        //    try
        //    {
        //        Replacement.Status S = this._ReplacedPart.Status();
        //        this.buttonInfo.Image = this._Replacement.ImageStatus(S);
        //        this.toolTip.SetToolTip(this.buttonInfo, this._Replacement.StatusDescription(S));
        //        //if ((int)S > (int)Replacement.Status.DataCountDifferent)
        //        //    this.BackColor = System.Drawing.Color.Pink;
        //        //else
        //        //    this.BackColor = System.Drawing.SystemColors.Control;
        //        //this.labelMessage.Text = this._Replacement.StatusDescription(Status);
        //        //this.pictureBoxCongruenceStatus.Image = this._Replacement.ImageStatus(Status);

        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

    }
}
