using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public partial class Chart : Component
    {
        public Chart()
        {
            InitializeComponent();
        }

        public Chart(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void InitChart(int ID, bool ForCollection = true, int Width = 800, int Height = 400)
        {
            _ID = ID;
            _ForCollection = ForCollection;
            _Chart.Titles.Clear();
            _Chart.Series.Clear();
            _Chart.Width = Width;
            _Chart.Height = Height;
        }

        private int _ID;
        private bool _ForCollection;

        private System.Collections.Generic.List<string> ChartTitles()
        {
            System.Collections.Generic.List<string> Titles = new List<string>();
            string SQL = "";
            if (this._ForCollection)
                SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C WHERE C.CollectionID = " + _ID.ToString();
            else
                SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C INNER JOIN CollectionTask T ON T.CollectionID = C.CollectionID AND T.CollectionTaskID = " + _ID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (!this._ForCollection)
            {
                SQL = "SELECT C.TaskDisplayText AS DisplayText FROM [dbo].[CollectionTaskHierarchyAll]() C WHERE C.CollectionTaskID = " + _ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            }
            foreach (System.Data.DataRow R in dt.Rows)
                Titles.Add(R[0].ToString());
            return Titles;
        }

        private string ChartTitle()
        {
            string Title = "";
            foreach (string T in this.ChartTitles())
            {
                if (Title.Length > 0)
                    Title += "_";
                Title += T.Replace(" ", "_").Replace("|", "_").Replace(".", "_");
            }
            while (Title.IndexOf("__") > -1)
                Title = Title.Replace("__", "_");
            return Title;
        }

        public string SaveImage(string Path = "")
        {
            try
            {
                if (Path.Length == 0)
                    Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this.ChartTitle() + "_Export_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png";
                this._Chart.SaveImage(Path, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); ;
            }
            return Path;
        }


    }
}
