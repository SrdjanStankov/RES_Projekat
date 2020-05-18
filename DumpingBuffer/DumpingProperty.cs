using Common;

namespace DumpingBufferLib
{
    public class DumpingProperty
    {

        public DumpingProperty(Code code, Value dumpingValue)
        {
            Code = code;
            DumpingValue = dumpingValue;
        }

        public Code Code { get; set; }
        public Value DumpingValue { get; set; }

    }
}
