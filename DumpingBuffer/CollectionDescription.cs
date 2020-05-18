using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpingBufferLib
{
    public class CollectionDescription
    {
        public int Id { get; set; }

        public int DataSet { get; set; }

        public List<DumpingProperty> DumpingPropertyCollection { get; set; }

        public CollectionDescription(int id, int dataSet, List<DumpingProperty> dumpingPropertyCollection)
        {
            Id = id;
            DataSet = dataSet;
            DumpingPropertyCollection = new List<DumpingProperty>(dumpingPropertyCollection);
        }

        public CollectionDescription()
        {
            DumpingPropertyCollection = new List<DumpingProperty>();
        }
    }
}
