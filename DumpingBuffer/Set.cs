using Common;

namespace DumpingBufferLib
{
    public class Set
    {
        public Set(Code code, Code oposite)
        {
            Code = code;
            Oposite = oposite;
        }

        public Code Code { get; private set; }
        public Code Oposite { get; private set; }
    }
}
