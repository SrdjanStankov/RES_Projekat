using HistoricalLib;
using HistoricalLib.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderLib
{
    public class Reader
    {
        public Historical historical = new Historical();

        public Reader()
        {
        }

        public void GetChangesForInterval(DateTime d1, DateTime d2)
        {
            historical.ReadFromDatabase(d1, d2);
        }

        public void WriteDatabase(DateTime d1, DateTime d2)
        {
            List<Model> models = historical.ReadFromDatabase(d1, d2);
            foreach (var item in models)
            {
                if(item.TimeStamp>d1 && item.TimeStamp<d2)
                    Console.Write(item.Id+"\t"+item.Value.IdGeographicArea+"\t"+item.Value.Consumption+"\t"+item.Code+"\t"+item.TimeStamp+"\t"+Environment.NewLine);
            }
        }

    }
}
