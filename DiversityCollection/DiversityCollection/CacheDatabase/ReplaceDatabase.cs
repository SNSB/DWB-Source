using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.CacheDatabase
{
    public struct CongruenceObject
    {
        public string ObjectName;
        public ReplaceDatabase.CongruenceStatus Status;
        public int VersionOld;
        public int VersionNew;
        public System.Collections.Generic.List<CongruenceCount> CongruenceCounts;
        public string Message;
        public System.Collections.Generic.Dictionary<string, CongruenceObject> ContainedObjects;
        public CongruenceObject(string O, ReplaceDatabase.CongruenceStatus S)
        {
            ObjectName = O;
            Status = S;
            VersionOld = 0;
            VersionNew = 0;
            CongruenceCounts = new List<CongruenceCount>();
            Message = "";
            ContainedObjects = new Dictionary<string, CongruenceObject>();
        }
        public CongruenceObject(string O, System.Collections.Generic.List<CongruenceCount> Counts)
        {
            ObjectName = O;
            VersionOld = 0;
            VersionNew = 0;
            Status = ReplaceDatabase.CongruenceStatus.Added;
            CongruenceCounts = Counts;
            foreach(CongruenceCount CC in CongruenceCounts)
            {
                if (CC.Status > Status)
                    Status = CC.Status;
            }
            Message = "";
            ContainedObjects = new Dictionary<string, CongruenceObject>();
        }
    }

    public struct CongruenceCount
    {
        public string SourceName;
        public int? CountOld;
        public int? CountNew;
        public ReplaceDatabase.CongruenceStatus Status;
        public CongruenceCount(string Source)
        {
            SourceName = Source;
            CountNew = null;
            CountOld = null;
            Status = ReplaceDatabase.CongruenceStatus.Congruent;
        }

        public CongruenceCount(string Source, int? Old, int? New)
        {
            SourceName = Source;
            CountNew = New;
            CountOld = Old;
            if (Old == null && New == null)
            {
                Status = ReplaceDatabase.CongruenceStatus.Missing;
            }
            else if (Old != null && New == null)
            {
                Status = ReplaceDatabase.CongruenceStatus.Missing;
            }
            else if (Old == null && New != null)
            {
                Status = ReplaceDatabase.CongruenceStatus.Added;
            }
            else
            {
                if (Old == New)
                    Status = ReplaceDatabase.CongruenceStatus.Congruent;
                else if (Old == 0 && New > 0)
                    Status = ReplaceDatabase.CongruenceStatus.Added;
                else if (Old > 0 && New == 0)
                    Status = ReplaceDatabase.CongruenceStatus.DataAreMissing;
                else
                    Status = ReplaceDatabase.CongruenceStatus.DataCountDifferent;
            }
        }

    }


    public partial class ReplaceDatabase : Component
    {
        private string _OldReplacedDatabase = "";
        private string _NewReplacementDatabase = "";
        public enum CongruenceStatus { Added, Congruent, DataCountDifferent, DataAreMissing, UpdateNeeded, Missing }

        public System.Drawing.Image ImageCongruenceStatus(CongruenceStatus status)
        {
            switch (status)
            {
                case CongruenceStatus.Added:
                    return this.imageList.Images[0];
                case CongruenceStatus.Congruent:
                    return this.imageList.Images[1];
                case CongruenceStatus.DataCountDifferent:
                    return this.imageList.Images[2];
                case CongruenceStatus.DataAreMissing:
                    return this.imageList.Images[3];
                case CongruenceStatus.UpdateNeeded:
                    return this.imageList.Images[4];
                default:
                    return this.imageList.Images[5];
            }
        }

        public int ImageIndexCongruenceStatus(CongruenceStatus status)
        {
            switch (status)
            {
                case CongruenceStatus.Added:
                    return 0;
                case CongruenceStatus.Congruent:
                    return 1;
                case CongruenceStatus.DataCountDifferent:
                    return 2;
                case CongruenceStatus.DataAreMissing:
                    return 3;
                case CongruenceStatus.UpdateNeeded:
                    return 4;
                default:
                    return 5;
            }
        }


        public bool StatusIsCongruent(CongruenceStatus Status)
        {
            bool Congruent = false;
            switch (Status)
            {
                case CongruenceStatus.Added:
                case CongruenceStatus.Congruent:
                case CongruenceStatus.DataCountDifferent:
                    Congruent = true;
                    break;
            }
            return Congruent;
        }


        public ReplaceDatabase()
        {
            InitializeComponent();
        }

        public ReplaceDatabase(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
