using HistoricalLib;
using System.Collections.Generic;

namespace HistoricalLib
{
    public class Description
    {
        public Description(int id, int dataset, List<HistoricalProperty> historicalProperties)
        {
            Id = id;
            Dataset = dataset;
            HistoricalProperties = historicalProperties;
        }

        public int Id { get; set; }
        public int Dataset { get; set; }
        public List<HistoricalProperty> HistoricalProperties { get; set; }
    }
}
