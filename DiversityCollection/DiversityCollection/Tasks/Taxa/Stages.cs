using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{
    public class Stages
    {
        public static string Stage(int AnalysisID)
        {
            if (Database.StageList.ContainsKey(AnalysisID)) return Database.StageList[AnalysisID];
            return "";
        }

        public static System.Collections.Generic.List<int> StageIDs
        {
            get
            {
                System.Collections.Generic.List<int> l = new List<int>();
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Database.StageList)
                    l.Add(KV.Key);
                return l;
            }
        }

        private static System.Collections.Generic.Dictionary<int, int> _StageSorting;
        public static System.Collections.Generic.Dictionary<int, int> StageSorting
        {
            get
            {
                if ( _StageSorting == null)
                {
                    _StageSorting = Database.StageSorting;
                }
                return _StageSorting;
            }
        }


    }
}
