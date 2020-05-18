using Common;

namespace HistoricalLib
{
    public class HistoricalProperty
    {

        public HistoricalProperty(Code code, Value historicalValue)
        {
            Code = code;
            HistoricalValue = historicalValue;
        }

        public Code Code { get; set; }
        public Value HistoricalValue { get; set; }
    }
}
